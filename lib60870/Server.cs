/*
 *  Copyright 2016 MZ Automation GmbH
 *
 *  This file is part of lib60870.NET
 *
 *  lib60870.NET is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  lib60870.NET is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with lib60870.NET.  If not, see <http://www.gnu.org/licenses/>.
 *
 *  See COPYING file for the complete license text.
 */

using System;

using lib60870;
using System.Net;
using System.Net.Sockets;
using System.Threading;
//using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace lib60870
{
    /// <summary>
    /// ���ٴ����� Handler for interrogation command (C_IC_NA_1 - 100).
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="connection"></param>
    /// <param name="asdu"></param>
    /// <param name="qoi">�ٻ��޶���</param>
    /// <returns></returns>
    public delegate bool InterrogationHandler(object parameter, ServerConnection connection, ASDU asdu, byte qoi);

    /// <summary>
    /// �ۼ������ٴ����� Handler for counter interrogation command (C_CI_NA_1 - 101).
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="connection"></param>
    /// <param name="asdu"></param>
    /// <param name="qoi">�ٻ��޶���</param>
    /// <returns></returns>
    public delegate bool CounterInterrogationHandler(object parameter, ServerConnection connection, ASDU asdu, byte qoi);

    /// <summary>
    /// ��������� Handler for read command (C_RD_NA_1 - 102)
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="connection"></param>
    /// <param name="asdu"></param>
    /// <param name="ioa">��Ϣ�����ַ</param>
    /// <returns></returns>
    public delegate bool ReadHandler(object parameter, ServerConnection connection, ASDU asdu, int ioa);

    /// <summary>
    /// ʱ��ͬ��������� Handler for clock synchronization command (C_CS_NA_1 - 103)
    /// </summary>
    public delegate bool ClockSynchronizationHandler(object parameter, ServerConnection connection, ASDU asdu, CP56Time2a newTime);

    /// <summary>
    /// ��λ������  Handler for reset process command (C_RP_NA_1 - 105)
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="connection"></param>
    /// <param name="asdu"></param>
    /// <param name="qrp">��λ�޶���</param>
    /// <returns></returns>
    public delegate bool ResetProcessHandler(object parameter, ServerConnection connection, ASDU asdu, byte qrp);

    /// <summary>
    /// Handler for delay acquisition command (C_CD_NA:1 - 106)
    /// </summary>
    public delegate bool DelayAcquisitionHandler(object parameter, ServerConnection connection, ASDU asdu, CP16Time2a delayTime);


    /// <summary>
    /// ���������� Handler for ASDUs that are not handled by other handlers (default handler)
    /// </summary>
    public delegate bool ASDUHandler(object parameter, ServerConnection connection, ASDU asdu);

    /// <summary>
    /// This class represents a single IEC 60870-5 server (slave or controlled station). It is also the
    /// main access to the server API.
    /// <para>����Ƿ���ˣ�������ǿ��Խ��������վ���</para>
    /// <para>��ʹ�������������start</para>
    /// <para>ÿ��һ��վ�����ӣ����½�һ��ServerConnection������allOpenConnections</para>
    /// <para>����Ƿ���ˣ�������ǿ��Խ��������վ���</para>
    /// </summary>
    public class Server
    {

        private string localHostname = "0.0.0.0";
        private int localPort = 2404;

        private bool running = false;

        private Socket listeningSocket;

        /// <summary>
        /// ����г���
        /// </summary>
        private int maxQueueSize = 1000;

        public int MaxQueueSize
        {
            get
            {
                return this.maxQueueSize;
            }
            set
            {
                maxQueueSize = value;
            }
        }

        private ConnectionParameters parameters = null;

        // List of all open connections
        private List<ServerConnection> allOpenConnections = new List<ServerConnection>();

        // Queue for messages (ASDUs)
        private Queue<ASDU> enqueuedASDUs = null;

        /// <summary>
        /// Create a new server using default connection parameters
        /// ʹ��Ĭ�ϲ�������һ��server
        /// </summary>
        public Server()
        {
            this.parameters = new ConnectionParameters();
        }

        /// <summary>
        /// Create a new server using the provided connection parameters.
        /// ʹ�ø�����������һ��server
        /// </summary>
        /// <param name="parameters">Connection parameters</param>
        public Server(ConnectionParameters parameters)
        {
            this.parameters = parameters;
        }

        /// <summary>
        /// վ�ٻ�������
        /// </summary>
        public InterrogationHandler interrogationHandler = null;
        public object InterrogationHandlerParameter = null;

        /// <summary>
        /// �ۼ����ٻ�������
        /// </summary>
        public CounterInterrogationHandler counterInterrogationHandler = null;
        public object counterInterrogationHandlerParameter = null;

        /// <summary>
        /// �����������
        /// </summary>
        public ReadHandler readHandler = null;
        public object readHandlerParameter = null;

        /// <summary>
        /// ʱ��ͬ��������
        /// </summary>
        public ClockSynchronizationHandler clockSynchronizationHandler = null;
        public object clockSynchronizationHandlerParameter = null;

        /// <summary>
        /// ��λ������
        /// </summary>
        public ResetProcessHandler resetProcessHandler = null;
        public object resetProcessHandlerParameter = null;

        /// <summary>
        /// �ӳٻ�����������
        /// </summary>
        public DelayAcquisitionHandler delayAcquisitionHandler = null;
        public object delayAcquisitionHandlerParameter = null;

        /// <summary>
        /// Sets a callback for interrogaton requests.
        /// <para>�������ٴ���ص�����</para>
        /// </summary>
        /// <param name="handler">The interrogation request handler callback function</param>
        /// <param name="parameter">user provided parameter that is passed to the callback</param>
        public void SetInterrogationHandler(InterrogationHandler handler, object parameter)
        {
            this.interrogationHandler = handler;
            this.InterrogationHandlerParameter = parameter;
        }

        /// <summary>
        /// Sets a callback for counter interrogaton requests.
        /// <para>�����ۼ����ٻ�����ص�</para>
        /// </summary>
        /// <param name="handler">The counter interrogation request handler callback function</param>
        /// <param name="parameter">user provided parameter that is passed to the callback</param>
        public void SetCounterInterrogationHandler(CounterInterrogationHandler handler, object parameter)
        {
            this.counterInterrogationHandler = handler;
            this.counterInterrogationHandlerParameter = parameter;
        }

        /// <summary>
        /// Sets a callback for read requests.
        /// <para>���ö�����ص�</para>
        /// </summary>
        /// <param name="handler">The read request handler callback function</param>
        /// <param name="parameter">user provided parameter that is passed to the callback</param>
        public void SetReadHandler(ReadHandler handler, object parameter)
        {
            this.readHandler = handler;
            this.readHandlerParameter = parameter;
        }

        /// <summary>
        /// Sets a callback for the clock synchronization request.
        /// <para>����ʱ��ͬ���ص�</para>
        /// </summary>
        /// <param name="handler">The clock synchronization request handler callback function</param>
        /// <param name="parameter">user provided parameter that is passed to the callback</param>
        public void SetClockSynchronizationHandler(ClockSynchronizationHandler handler, object parameter)
        {
            this.clockSynchronizationHandler = handler;
            this.clockSynchronizationHandlerParameter = parameter;
        }

        public void SetResetProcessHandler(ResetProcessHandler handler, object parameter)
        {
            this.resetProcessHandler = handler;
            this.resetProcessHandlerParameter = parameter;
        }

        public void SetDelayAcquisitionHandler(DelayAcquisitionHandler handler, object parameter)
        {
            this.delayAcquisitionHandler = handler;
            this.delayAcquisitionHandlerParameter = parameter;
        }

        public ASDUHandler asduHandler = null;
        public object asduHandlerParameter = null;

        /// <summary>
        /// Sets a callback to handle ASDUs (commands, requests) form clients. This callback can be used when
        /// no other callback handles the message from the client/master.
        /// <para>��������ASDU�ص�</para>
        /// </summary>
        /// <param name="handler">The ASDU callback function</param>
        /// <param name="parameter">user provided parameter that is passed to the callback</param>
        public void SetASDUHandler(ASDUHandler handler, object parameter)
        {
            this.asduHandler = handler;
            this.asduHandlerParameter = parameter;
        }

        /// <summary>
        /// Gets the number of connected master/client stations.
        /// ��ȡ���ӵ�վ��
        /// </summary>
        /// <value>The number of open connections.</value>
        public int ActiveConnections
        {
            get
            {
                return this.allOpenConnections.Count;
            }
        }

        private void ServerAcceptThread()
        {
            running = true;

            Console.WriteLine("Waiting for connections...");

            while (running)
            {

                try
                {

                    Socket newSocket = listeningSocket.Accept();

                    if (newSocket != null)
                    {
                        Console.WriteLine("Connected");

                        allOpenConnections.Add(
                            new ServerConnection(newSocket, parameters, this));
                    }

                }
                catch (Exception)
                {
                    running = false;
                }


            }
        }

        internal void Remove(ServerConnection connection)
        {
            allOpenConnections.Remove(connection);
        }

        /// <summary>
        /// Sets the local IP address to bind the server. Default is "0.0.0.0" for
        /// all interfaces
        /// </summary>
        /// <param name="localAddress">Local IP address or hostname to bind.</param>
        public void SetLocalAddress(string localAddress)
        {
            this.localHostname = localAddress;
        }

        /// <summary>
        /// Sets the local TCP port to bind to. Default is 2404.
        /// </summary>
        /// <param name="tcpPort">Local TCP port to bind.</param>
        public void SetLocalPort(int tcpPort)
        {
            this.localPort = tcpPort;
        }

        /// <summary>
        /// Start the server. Listen to client connections.
        /// <para>��������������������</para>
        /// </summary>
        public void Start()
        {
            IPAddress ipAddress = IPAddress.Parse(localHostname);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, localPort);

            // Create a TCP/IP  socket.
            listeningSocket = new Socket(AddressFamily.InterNetwork,
                                       SocketType.Stream, ProtocolType.Tcp);

            listeningSocket.Bind(remoteEP);

            listeningSocket.Listen(100);

            Thread acceptThread = new Thread(ServerAcceptThread);

            acceptThread.Start();

        }

        /// <summary>
        /// Stop the server. Close all open client connections.
        /// <para>�ر���������վ��</para>
        /// </summary>
        public void Stop()
        {
            running = false;

            try
            {
                listeningSocket.Close();

                // close all open connection
                foreach (ServerConnection connection in allOpenConnections)
                {
                    connection.Close();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            listeningSocket.Close();
        }

        /// <summary>
        /// Enqueues the ASDU to the transmission queue.
        /// ��ASDU������У���ͬʱ���͵���ǰ�����������
        /// </summary>
        /// If an active connection exists the ASDU will be sent to the active client immediately. Otherwhise
        /// the ASDU will be added to the transmission queue for later transmission.
        /// <param name="asdu">ASDU to be sent</param>
        public void EnqueueASDU(ASDU asdu)
        {
            if (enqueuedASDUs == null)
            {
                enqueuedASDUs = new Queue<ASDU>();
            }

            if (enqueuedASDUs.Count == maxQueueSize)
                enqueuedASDUs.Dequeue();

            enqueuedASDUs.Enqueue(asdu);

            Console.WriteLine("Queue contains " + enqueuedASDUs.Count + " messages");

            foreach (ServerConnection connection in allOpenConnections)
            {
                //֪ͨ��ǰ��������ӣ��Ͻ���ȡASDU
                if (connection.IsActive)
                    connection.ASDUReadyToSend();
            }
        }

        //ȡһ��ASDU
        internal ASDU DequeueASDU()
        {
            if (enqueuedASDUs == null)
                return null;

            if (enqueuedASDUs.Count > 0)
                return enqueuedASDUs.Dequeue();
            else
                return null;
        }

        //��������һ�����ӣ�
        internal void Activated(ServerConnection activeConnection)
        {
            // deactivate all other connections

            foreach (ServerConnection connection in allOpenConnections)
            {
                if (connection != activeConnection)
                    connection.IsActive = false;
            }
        }
    }

}
