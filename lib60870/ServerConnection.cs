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

namespace lib60870
{
    public class ServerConnection
    {
        /// <summary>
        /// <para>U帧，开启确认</para>
        /// <para>  由被控站回答“开启确认”</para>
        /// <para>  只用于回答控制站的开启命令</para>
        /// <para>  格式和内容固定</para>
        /// </summary>
        static byte[] STARTDT_CON_MSG = new byte[] { 0x68, 0x04, 0x0b, 0x00, 0x00, 0x00 };

        /// <summary>
        /// <para>U帧，停止确认</para>
        /// <para>  由被控站回答的停止确认</para>
        /// <para>  只用于回答控制站的停止命令</para>
        /// <para>  格式和内容固定</para>
        /// </summary>
        static byte[] STOPDT_CON_MSG = new byte[] { 0x68, 0x04, 0x23, 0x00, 0x00, 0x00 };

        /// <summary>
        /// <para>U帧，停止确认</para>
        /// <para>  控制站和被控站都可以发送测试确认命令</para>
        /// <para>  一旦其中一方已经发出了测试命令，另一方必须回答，且不需要在发送测试命令</para>
        /// <para>  用于测试链路是否完好</para>
        /// <para>  格式和内容固定</para>
        /// </summary>
        static byte[] TESTFR_CON_MSG = new byte[] { 0x68, 0x04, 0x83, 0x00, 0x00, 0x00 };

        private int sendCount = 0;
        private int receiveCount = 0;

        private int unconfirmedMessages = 0; /* number of unconfirmed messages received */
        private long lastConfirmationTime = System.Int64.MaxValue; /* timestamp when the last confirmation message was sent */

        private ConnectionParameters parameters;
        private Server server;

        public ServerConnection(Socket socket, ConnectionParameters parameters, Server server)
        {
            this.socket = socket;
            this.parameters = parameters;
            this.server = server;

            Thread workerThread = new Thread(HandleConnection);

            workerThread.Start();
        }

        /// <summary>
        /// Flag indicating that this connection is the active connection.
        /// The active connection is the only connection that is answering
        /// application layer requests and sends cyclic, and spontaneous messages.
        /// </summary>
        private bool isActive = false;

        /// <summary>
        /// Gets or sets a value indicating whether this connection is active.
        /// The active connection is the only connection that is answering
        /// application layer requests and sends cyclic, and spontaneous messages.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                isActive = value;

                if (debugOutput)
                {
                    if (isActive)
                        Console.WriteLine(this.ToString() + " is active");
                    else
                        Console.WriteLine(this.ToString() + " is not active");
                }
            }
        }

        private Socket socket;

        private bool running = false;

        private bool debugOutput = true;

        private int receiveMessage(Socket socket, byte[] buffer)
        {
            if (socket.Poll(100, SelectMode.SelectRead))
            {

                if (socket.Available == 0)
                    throw new SocketException();

                // wait for first byte
                if (socket.Receive(buffer, 0, 1, SocketFlags.None) != 1)
                    return 0;

                if (buffer[0] != 0x68)
                {
                    if (debugOutput)
                        Console.WriteLine("Missing SOF indicator!");
                    return 0;
                }

                // read length byte
                if (socket.Receive(buffer, 1, 1, SocketFlags.None) != 1)
                    return 0;

                int length = buffer[1];

                // read remaining frame
                if (socket.Receive(buffer, 2, length, SocketFlags.None) != length)
                {
                    if (debugOutput)
                        Console.WriteLine("Failed to read complete frame!");
                    return 0;
                }

                return length + 2;
            }
            else
                return 0;
        }

        /// <summary>
        /// 发送I报文
        /// </summary>
        /// <param name="frame"></param>
        private void sendIMessage(Frame frame)
        {
            frame.PrepareToSend(sendCount, receiveCount);
            socket.Send(frame.GetBuffer(), frame.GetMsgSize(), SocketFlags.None);
            sendCount++;
        }

        /// <summary>
        /// 发送S报文
        /// </summary>
        private void sendSMessage()
        {
            byte[] msg = new byte[6];

            msg[0] = 0x68;
            msg[1] = 0x04;
            msg[2] = 0x01;
            msg[3] = 0;
            msg[4] = (byte)((receiveCount % 128) * 2);
            msg[5] = (byte)(receiveCount / 128);

            socket.Send(msg);
        }

        /// <summary>
        /// 发送ASDU
        /// </summary>
        /// <param name="asdu"></param>
        public void SendASDU(ASDU asdu)
        {
            Frame frame = new T104Frame();
            asdu.Encode(frame, parameters);

            //发送I帧
            sendIMessage(frame);
        }

        /// <summary>
        /// 发送激活确认报文（镜像报文）
        /// </summary>
        /// <param name="asdu">原报文</param>
        /// <param name="negative"></param>
        public void SendACT_CON(ASDU asdu, bool negative)
        {
            //传送原因改下
            asdu.Cot = CauseOfTransmission.ACTIVATION_CON;
            //肯定/否定激活改进去
            asdu.IsNegative = negative;

            SendASDU(asdu);
        }

        /// <summary>
        /// 发送激活终止报文（镜像报文）
        /// </summary>
        /// <param name="asdu"></param>
        public void SendACT_TERM(ASDU asdu)
        {
            //传送原因改进去
            asdu.Cot = CauseOfTransmission.ACTIVATION_TERMINATION;
            //直接否认了
            asdu.IsNegative = false;

            SendASDU(asdu);
        }

        /// <summary>
        /// 检查是否t2超时
        /// </summary>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        private bool checkConfirmTimeout(long currentTime)
        {
            if ((currentTime - lastConfirmationTime) >= (parameters.T2 * 1000))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 检查超时、超数等情况
        /// </summary>
        private void SendSMessageIfRequired()
        {

            if (unconfirmedMessages > 0)
            {

                long currentTime = SystemUtils.currentTimeMillis();

                if ((unconfirmedMessages > parameters.W) || checkConfirmTimeout(currentTime))
                {

                    if (debugOutput)
                        Console.WriteLine("Send S message");

                    //TODO check timeout condition /* t2 */
                    lastConfirmationTime = currentTime;

                    unconfirmedMessages = 0;
                    sendSMessage();
                }
            }
        }

        /// <summary>
        /// 增加接受计数器
        /// </summary>
        private void IncreaseReceivedMessageCounters()
        {
            receiveCount++;
            unconfirmedMessages++;

            if (unconfirmedMessages == 1)
            {
                lastConfirmationTime = SystemUtils.currentTimeMillis();
            }
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="msgSize"></param>
        /// <returns></returns>
        private bool HandleMessage(Socket socket, byte[] buffer, int msgSize)
        {

            if ((buffer[2] & 1) == 0)
            {

                if (debugOutput)
                    Console.WriteLine("Received I frame");

                if (msgSize < 7)
                {

                    if (debugOutput)
                        Console.WriteLine("I msg too small!");

                    return false;
                }


                IncreaseReceivedMessageCounters();

                if (isActive)
                {

                    bool messageHandled = false;

                    ASDU asdu = new ASDU(parameters, buffer, msgSize);
                    
                    //根据类型来分别处理
                    switch (asdu.TypeId)
                    {

                        case TypeID.C_IC_NA_1: /* 100 - interrogation command  总召*/

                            if (debugOutput)
                                Console.WriteLine("Rcvd interrogation command C_IC_NA_1");

                            if ((asdu.Cot == CauseOfTransmission.ACTIVATION) || (asdu.Cot == CauseOfTransmission.DEACTIVATION))
                            {
                                if (server.interrogationHandler != null)
                                {

                                    InterrogationCommand irc = (InterrogationCommand)asdu.GetElement(0);

                                    if (server.interrogationHandler(server.InterrogationHandlerParameter, this, asdu, irc.QOI))
                                        messageHandled = true;
                                }
                            }
                            else
                            {
                                asdu.Cot = CauseOfTransmission.UNKNOWN_CAUSE_OF_TRANSMISSION;
                                this.SendASDU(asdu);
                            }

                            break;

                        case TypeID.C_CI_NA_1: /* 101 - counter interrogation command 累计量总召*/

                            if (debugOutput)
                                Console.WriteLine("Rcvd counter interrogation command C_CI_NA_1");

                            if ((asdu.Cot == CauseOfTransmission.ACTIVATION) || (asdu.Cot == CauseOfTransmission.DEACTIVATION))
                            {
                                if (server.counterInterrogationHandler != null)
                                {

                                    CounterInterrogationCommand cic = (CounterInterrogationCommand)asdu.GetElement(0);

                                    if (server.counterInterrogationHandler(server.counterInterrogationHandlerParameter, this, asdu, cic.QCC))
                                        messageHandled = true;
                                }
                            }
                            else
                            {
                                asdu.Cot = CauseOfTransmission.UNKNOWN_CAUSE_OF_TRANSMISSION;
                                this.SendASDU(asdu);
                            }

                            break;

                        case TypeID.C_RD_NA_1: /* 102 - read command  读命令*/

                            if (debugOutput)
                                Console.WriteLine("Rcvd read command C_RD_NA_1");

                            if (asdu.Cot == CauseOfTransmission.REQUEST)
                            {

                                if (debugOutput)
                                    Console.WriteLine("Read request for object: " + asdu.Ca);

                                if (server.readHandler != null)
                                {
                                    ReadCommand rc = (ReadCommand)asdu.GetElement(0);

                                    if (server.readHandler(server.readHandlerParameter, this, asdu, rc.ObjectAddress))
                                        messageHandled = true;

                                }

                            }
                            else
                            {
                                asdu.Cot = CauseOfTransmission.UNKNOWN_CAUSE_OF_TRANSMISSION;
                                this.SendASDU(asdu);
                            }

                            break;

                        case TypeID.C_CS_NA_1: /* 103 - Clock synchronization command 时钟同步 */

                            if (debugOutput)
                                Console.WriteLine("Rcvd clock sync command C_CS_NA_1");

                            if (asdu.Cot == CauseOfTransmission.ACTIVATION)
                            {

                                if (server.clockSynchronizationHandler != null)
                                {

                                    ClockSynchronizationCommand csc = (ClockSynchronizationCommand)asdu.GetElement(0);

                                    if (server.clockSynchronizationHandler(server.clockSynchronizationHandlerParameter,
                                            this, asdu, csc.NewTime))
                                        messageHandled = true;
                                }

                            }
                            else
                            {
                                asdu.Cot = CauseOfTransmission.UNKNOWN_CAUSE_OF_TRANSMISSION;
                                this.SendASDU(asdu);
                            }

                            break;

                        case TypeID.C_TS_NA_1: /* 104 - test command */

                            if (debugOutput)
                                Console.WriteLine("Rcvd test command C_TS_NA_1");

                            //直接处理
                            if (asdu.Cot != CauseOfTransmission.ACTIVATION)
                                asdu.Cot = CauseOfTransmission.UNKNOWN_CAUSE_OF_TRANSMISSION;
                            else
                                asdu.Cot = CauseOfTransmission.ACTIVATION_CON;

                            this.SendASDU(asdu);

                            messageHandled = true;

                            break;

                        case TypeID.C_RP_NA_1: /* 105 - Reset process command  复位命令*/

                            if (debugOutput)
                                Console.WriteLine("Rcvd reset process command C_RP_NA_1");

                            if (asdu.Cot == CauseOfTransmission.ACTIVATION)
                            {

                                if (server.resetProcessHandler != null)
                                {

                                    ResetProcessCommand rpc = (ResetProcessCommand)asdu.GetElement(0);

                                    if (server.resetProcessHandler(server.resetProcessHandlerParameter,
                                        this, asdu, rpc.QRP))
                                        messageHandled = true;
                                }

                            }
                            else
                            {
                                asdu.Cot = CauseOfTransmission.UNKNOWN_CAUSE_OF_TRANSMISSION;
                                this.SendASDU(asdu);
                            }


                            break;

                        case TypeID.C_CD_NA_1: /* 106 - Delay acquisition command 延时获得*/

                            if (debugOutput)
                                Console.WriteLine("Rcvd delay acquisition command C_CD_NA_1");

                            if ((asdu.Cot == CauseOfTransmission.ACTIVATION) || (asdu.Cot == CauseOfTransmission.SPONTANEOUS))
                            {
                                if (server.delayAcquisitionHandler != null)
                                {

                                    DelayAcquisitionCommand dac = (DelayAcquisitionCommand)asdu.GetElement(0);

                                    if (server.delayAcquisitionHandler(server.delayAcquisitionHandlerParameter,
                                            this, asdu, dac.Delay))
                                        messageHandled = true;
                                }
                            }
                            else
                            {
                                asdu.Cot = CauseOfTransmission.UNKNOWN_CAUSE_OF_TRANSMISSION;
                                this.SendASDU(asdu);
                            }

                            break;

                    }

                    //都没处理，就直接当通用ASDU处理掉了
                    if ((messageHandled == false) && (server.asduHandler != null))
                        if (server.asduHandler(server.asduHandlerParameter, this, asdu))
                            messageHandled = true;

                    //依旧没处理，那就返回未知
                    if (messageHandled == false)
                    {
                        asdu.Cot = CauseOfTransmission.UNKNOWN_TYPE_ID;
                        this.SendASDU(asdu);
                    }

                }
                else
                {
                    // connection not activated --> skip message
                    if (debugOutput)
                        Console.WriteLine("Message not activated. Skip I message");
                }


                return true;
            }

            // Check for TESTFR_ACT message
            else if ((buffer[2] & 0x43) == 0x43)
            {

                if (debugOutput)
                    Console.WriteLine("Send TESTFR_CON");

                socket.Send(TESTFR_CON_MSG);
            }

            // Check for STARTDT_ACT message
            else if ((buffer[2] & 0x07) == 0x07)
            {

                if (debugOutput)
                    Console.WriteLine("Send STARTDT_CON");

                this.isActive = true;

                socket.Send(STARTDT_CON_MSG);
            }

            // Check for STOPDT_ACT message
            else if ((buffer[2] & 0x13) == 0x13)
            {

                if (debugOutput)
                    Console.WriteLine("Send STOPDT_CON");

                this.isActive = false;

                socket.Send(STOPDT_CON_MSG);
            }

            // S-message  S帧（用于确认对方的接收序号）
            else if (buffer[2] == 0x01)
            {

                int messageCount = (buffer[4] + buffer[5] * 0x100) / 2;

                Console.WriteLine("Recv S(" + messageCount + ") (own sendcounter = " + sendCount + ")");
            }
            else
            {

                Console.WriteLine("Unknown message");

                return true;
            }

            return true;
        }

        /// <summary>
        /// 检查服务端的ASDU并发送？？
        /// </summary>
        private void checkServerQueue()
        {
            ASDU asdu = server.DequeueASDU();

            if (asdu != null)
                SendASDU(asdu);
        }

        private void HandleConnection()
        {

            byte[] bytes = new byte[300];

            try
            {

                try
                {

                    running = true;

                    while (running)
                    {

                        try
                        {
                            // Receive the response from the remote device.
                            int bytesRec = receiveMessage(socket, bytes);

                            if (bytesRec != 0)
                            {

                                if (debugOutput)
                                    Console.WriteLine(
                                        BitConverter.ToString(bytes, 0, bytesRec));

                                if (HandleMessage(socket, bytes, bytesRec) == false)
                                {
                                    /* close connection on error */
                                    running = false;
                                }
                            }

                            if (isActive)
                                checkServerQueue();

                            SendSMessageIfRequired();

                            Thread.Sleep(1);
                        }
                        catch (SocketException)
                        {
                            running = false;
                        }
                    }

                    Console.WriteLine("CLOSE CONNECTION!");

                    // Release the socket.
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            server.Remove(this);
        }


        public void Close()
        {
            running = false;
        }

        /// <summary>
        /// 准备发送ASDU，为啥这里要发送一次？难道是镜像报文？还需要再读读。。。
        /// </summary>
        public void ASDUReadyToSend()
        {
            if (isActive)
            {
                ASDU asdu = server.DequeueASDU();

                if (asdu != null)
                    SendASDU(asdu);
            }
        }

    }

}
