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

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace lib60870
{
    /// <summary>
    /// <para>104帧内容，这里主要是报头，叫ACPI，剩下的内容靠一点一点append进去的。。</para>
    /// <para>ACPI的六个字节是这边确定，主要包括起始字符，长度，还有4个控制域</para>
    /// <para>I帧的4个控制域分别是发送序号和接受序号，</para>
    /// <para>S帧只有接收序号，发送序号是0</para>
    /// <para>U帧是固定的，不在这里发送。。。</para>
    /// </summary>
    public class T104Frame : Frame
    {
        byte[] buffer;

        int msgSize;

        public T104Frame()
        {
            buffer = new byte[256];

            buffer[0] = 0x68;

            msgSize = 6;
        }

        /// <summary>
        /// 发送前调用，更新报文长度，发送计数器和接收计数器。。。
        /// </summary>
        /// <param name="sendCounter">发送计数器</param>
        /// <param name="receiveCounter">接收计数器</param>
        public override void PrepareToSend(int sendCounter, int receiveCounter)
        {
            /* set size field */
            buffer[1] = (byte)(msgSize - 2);

            buffer[2] = (byte)((sendCounter % 128) * 2);
            buffer[3] = (byte)(sendCounter / 128);

            buffer[4] = (byte)((receiveCounter % 128) * 2);
            buffer[5] = (byte)(receiveCounter / 128);
        }

        /// <summary>
        /// 复位帧
        /// </summary>
        public override void ResetFrame()
        {
            msgSize = 6;
        }

        /// <summary>
        /// 增加一个需要发送的字节
        /// </summary>
        /// <param name="value"></param>
        public override void SetNextByte(byte value)
        {
            buffer[msgSize++] = value;
        }

        /// <summary>
        /// 增加一组需要发送的字节
        /// </summary>
        /// <param name="bytes"></param>
        public override void AppendBytes(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                buffer[msgSize++] = bytes[i];
            }
        }

        /// <summary>
        /// 获取需要发送的字节数
        /// </summary>
        /// <returns></returns>
        public override int GetMsgSize()
        {
            return msgSize;
        }

        /// <summary>
        /// 获取需要发送的buffer
        /// </summary>
        /// <returns></returns>
        public override byte[] GetBuffer()
        {
            return buffer;
        }
    }

}
