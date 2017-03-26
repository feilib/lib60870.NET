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
    /// 102帧内容，这里主要是报头，再加个校验和，剩下的内容靠一点一点append进去的。。
    /// </summary>
    public class T102Frame : Frame
    {
        byte[] buffer;

        /// <summary>
        /// 链路控制域
        /// </summary>
        LinkControl lc;
        /// <summary>
        /// 链路地址，占2字节
        /// </summary>
        int LinkAddress;
        /// <summary>
        /// 只包括链路控制域（1)，链路地址（2），还有ASDU的长度
        ///   除去前4个字节头，还有校验和（1），结尾标志（1）之后的长度
        /// </summary>
        int msgSize;

        private byte checksum;

        /// <summary>
        /// 构造默认帧
        /// </summary>
        public T102Frame()
        {
            buffer = new byte[256];

            //两个其实标志
            buffer[0] = 0x68;
            buffer[3] = 0x68;

            //长度，不包括头4个字节，但是包括控制域和链路地址，共3个写好的地址
            msgSize = 3;
        }

        /// <summary>
        /// 使用链路控制域创建一个帧
        /// </summary>
        /// <param name="lc"></param>
        public T102Frame(LinkControl lc)
            : this()
        {
            SetLinkControl(lc);
        }

        /// <summary>
        /// 使用链路控制域和链路地址设置一个帧
        /// </summary>
        /// <param name="lc"></param>
        /// <param name="linkAddr"></param>
        public T102Frame(LinkControl lc, int linkAddr)
            : this(lc)
        {
            SetLinkAddress(linkAddr);
        }

        /// <summary>
        /// 使用链路控制域和连接参数设置一个帧（连接参数中有链路地址）
        /// </summary>
        /// <param name="lc"></param>
        /// <param name="para"></param>
        public T102Frame(LinkControl lc, ConnectionParameters para)
            : this(lc, para.LinkAddress)
        { }

        /// <summary>
        /// 设置链路控制域
        /// </summary>
        /// <param name="lc"></param>
        public void SetLinkControl(LinkControl lc)
        {
            this.lc = lc;
            buffer[4] = lc.getValue();
        }

        /// <summary>
        /// 设置链路地址
        /// </summary>
        /// <param name="addr"></param>
        public void SetLinkAddress(int addr)
        {
            LinkAddress = addr;
            buffer[5] = (byte)(LinkAddress % 0x100);
            buffer[6] = (byte)(LinkAddress / 0x100);
        }

        /// <summary>
        /// 发送前调用，更新报文长度，还有校验和，报文结束标志。。。
        /// </summary>
        public override void PrepareToSend()
        {
            //报文长度-两个字节是相同的
            buffer[1] = (byte)(msgSize & 0xFF);
            buffer[2] = (byte)(msgSize & 0xFF);

            UpdateCheckSum();
            //校验和
            buffer[4 + msgSize] = checksum;
            //结尾标志
            buffer[4 + msgSize + 1] = 0x16;
        }

        /// <summary>
        /// 发送前调用，更新报文长度，还有校验和，报文结束标志，还有链路控制域
        /// <para>这里没有设置链路地址，如果初始化的时候没搞定的话，必须再调用这个函数之前手动处理一下</para>
        /// </summary>
        /// <param name="lc"></param>
        public override void PrepareToSend(LinkControl lc)
        {

            SetLinkControl(lc);
            PrepareToSend();

        }

        /// <summary>
        /// 发送前调用，更新报文长度，还有校验和，报文结束标志，还有链路控制域和链路地址
        /// </summary>
        /// <param name="lc"></param>
        /// <param name="linkAddr"></param>
        public override void PrepareToSend(LinkControl lc, int linkAddr)
        {
            SetLinkAddress(linkAddr);
            PrepareToSend(lc);
        }

        /// <summary>
        /// 计算校验和
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
        /// 复位帧
        /// </summary>
        public override void ResetFrame()
        {
            msgSize = 3;
        }

        /// <summary>
        /// 增加一个需要发送的字节
        /// </summary>
        /// <param name="value"></param>
        public override void SetNextByte(byte value)
        {
            buffer[4 + msgSize++] = value;
        }

        /// <summary>
        /// 增加一组需要发送的字节
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

        /// <summary>
        /// 获取单字节固定帧  0xE5
        /// </summary>
        /// <returns></returns>
        public static byte[] GetBufferSingleByte()
        {
            return new byte[] { 0xE5 };
        }

        /// <summary>
        /// 使用恋空控制域和链路地址，获取固定长度帧
        /// </summary>
        /// <param name="lc"></param>
        /// <param name="linkAddr"></param>
        /// <returns></returns>
        public static byte[] GetBufferFix(LinkControl lc, int linkAddr)
        {
            byte[] buf = new byte[6];
            buf[0] = 0x10;
            buf[5] = 0x16;

            //链路控制域和链路地址赋值
            buf[1] = lc.getValue();
            buf[2] = (byte)(linkAddr % 0x100);
            buf[3] = (byte)(linkAddr / 0x100);

            //计算校验和
            byte checksum = 0;
            checksum += buf[1];
            checksum += buf[2];
            checksum += buf[3];

            buf[4] = checksum;

            return buf;
        }
    }
}
