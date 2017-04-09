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
using System.Text;
using System.Threading;

namespace lib102
{
    /// <summary>
    /// 102֡���ݣ�������Ҫ�Ǳ�ͷ���ټӸ�У��ͣ�ʣ�µ����ݿ�һ��һ��append��ȥ�ġ���
    /// </summary>
    public class T102Frame : Frame
    {
        byte[] buffer;

        /// <summary>
        /// ��·������
        /// </summary>
        LinkControl lc;
        /// <summary>
        /// ��·��ַ��ռ2�ֽ�
        /// </summary>
        int LinkAddress;
        /// <summary>
        /// ֻ������·������1)����·��ַ��2��������ASDU�ĳ���
        ///   ��ȥǰ4���ֽ�ͷ������У��ͣ�1������β��־��1��֮��ĳ���
        /// </summary>
        int msgSize;

        private byte checksum;

        /// <summary>
        /// ����Ĭ��֡
        /// </summary>
        public T102Frame()
        {
            buffer = new byte[256];

            //������ʵ��־
            buffer[0] = 0x68;
            buffer[3] = 0x68;

            //���ȣ�������ͷ4���ֽڣ����ǰ������������·��ַ����3��д�õĵ�ַ
            msgSize = 3;
        }

        /// <summary>
        /// ʹ����·�����򴴽�һ��֡
        /// </summary>
        /// <param name="lc"></param>
        public T102Frame(LinkControl lc)
            : this()
        {
            SetLinkControl(lc);
        }

        /// <summary>
        /// ʹ����·���������·��ַ����һ��֡
        /// </summary>
        /// <param name="lc"></param>
        /// <param name="linkAddr"></param>
        public T102Frame(LinkControl lc, int linkAddr)
            : this(lc)
        {
            SetLinkAddress(linkAddr);
        }

        /// <summary>
        /// ʹ����·����������Ӳ�������һ��֡�����Ӳ���������·��ַ��
        /// </summary>
        /// <param name="lc"></param>
        /// <param name="para"></param>
        public T102Frame(LinkControl lc, ConnectionParameters para)
            : this(lc, para.LinkAddress)
        { }

        /// <summary>
        /// ������·������
        /// </summary>
        /// <param name="lc"></param>
        public void SetLinkControl(LinkControl lc)
        {
            this.lc = lc;
            buffer[4] = lc.getValue();
        }

        /// <summary>
        /// ������·��ַ
        /// </summary>
        /// <param name="addr"></param>
        public void SetLinkAddress(int addr)
        {
            LinkAddress = addr;
            buffer[5] = (byte)(LinkAddress % 0x100);
            buffer[6] = (byte)(LinkAddress / 0x100);
        }

        /// <summary>
        /// ����ǰ���ã����±��ĳ��ȣ�����У��ͣ����Ľ�����־������
        /// </summary>
        public override void PrepareToSend()
        {
            //���ĳ���-�����ֽ�����ͬ��
            buffer[1] = (byte)(msgSize & 0xFF);
            buffer[2] = (byte)(msgSize & 0xFF);

            UpdateCheckSum();
            //У���
            buffer[4 + msgSize] = checksum;
            //��β��־
            buffer[4 + msgSize + 1] = 0x16;
        }

        /// <summary>
        /// ����ǰ���ã����±��ĳ��ȣ�����У��ͣ����Ľ�����־��������·������
        /// <para>����û��������·��ַ�������ʼ����ʱ��û�㶨�Ļ��������ٵ����������֮ǰ�ֶ�����һ��</para>
        /// </summary>
        /// <param name="lc"></param>
        public override void PrepareToSend(LinkControl lc)
        {

            SetLinkControl(lc);
            PrepareToSend();

        }

        /// <summary>
        /// ����ǰ���ã����±��ĳ��ȣ�����У��ͣ����Ľ�����־��������·���������·��ַ
        /// </summary>
        /// <param name="lc"></param>
        /// <param name="linkAddr"></param>
        public override void PrepareToSend(LinkControl lc, int linkAddr)
        {
            SetLinkAddress(linkAddr);
            PrepareToSend(lc);
        }

        /// <summary>
        /// ����У���
        /// </summary>
        private void UpdateCheckSum()
        {
            checksum = 0;
            for (int i = 0; i < msgSize; i++)
            {
                checksum += buffer[4 + i];
            }
        }

        /// <summary>
        /// ��λ֡
        /// </summary>
        public override void ResetFrame()
        {
            msgSize = 3;
        }

        /// <summary>
        /// ����һ����Ҫ���͵��ֽ�
        /// </summary>
        /// <param name="value"></param>
        public override void SetNextByte(byte value)
        {
            buffer[4 + msgSize++] = value;
        }

        /// <summary>
        /// ����һ����Ҫ���͵��ֽ�
        /// </summary>
        /// <param name="bytes"></param>
        public override void AppendBytes(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                buffer[4 + msgSize++] = bytes[i];
            }
        }

        /// <summary>
        /// ��ȡ��Ҫ���͵��ֽ���
        /// </summary>
        /// <returns></returns>
        public override int GetMsgSize()
        {
            return msgSize;
        }

        /// <summary>
        /// ��ȡ��Ҫ���͵�buffer
        /// </summary>
        /// <returns></returns>
        public override byte[] GetBuffer()
        {
            return buffer;
        }

        /// <summary>
        /// ��ȡ���ֽڹ̶�֡  0xE5
        /// </summary>
        /// <returns></returns>
        public static byte[] GetBufferSingleByte()
        {
            return new byte[] { 0xE5 };
        }

        /// <summary>
        /// ʹ�����տ��������·��ַ����ȡ�̶�����֡
        /// </summary>
        /// <param name="lc"></param>
        /// <param name="linkAddr"></param>
        /// <returns></returns>
        public static byte[] GetBufferFix(LinkControl lc, int linkAddr)
        {
            byte[] buf = new byte[6];
            buf[0] = 0x10;
            buf[5] = 0x16;

            //��·���������·��ַ��ֵ
            buf[1] = lc.getValue();
            buf[2] = (byte)(linkAddr % 0x100);
            buf[3] = (byte)(linkAddr / 0x100);

            //����У���
            byte checksum = 0;
            checksum += buf[1];
            checksum += buf[2];
            checksum += buf[3];

            buf[4] = checksum;

            return buf;
        }
    }
}
