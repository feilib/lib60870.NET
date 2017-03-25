/*
 *  Frame.cs
 *
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

namespace lib102
{
    public abstract class Frame
    {
        /// <summary>
        /// 发送前调用，更新报文长度，还有校验和，报文结束标志。。。
        /// <para>这里没有设置链路控制域和链路地址，如果初始化的时候没搞定的话，必须再调用这个函数之前手动处理一下</para>
        /// </summary>
		public abstract void PrepareToSend();

        /// <summary>
        /// 发送前调用，更新报文长度，还有校验和，报文结束标志，还有链路控制域
        /// <para>这里没有设置链路地址，如果初始化的时候没搞定的话，必须再调用这个函数之前手动处理一下</para>
        /// </summary>
        /// <param name="lc"></param>
        public abstract void PrepareToSend(LinkControl lc);

        /// <summary>
        /// 发送前调用，更新报文长度，还有校验和，报文结束标志，还有链路控制域和链路地址
        /// </summary>
        /// <param name="lc"></param>
        /// <param name="linkAddr"></param>
        public abstract void PrepareToSend(LinkControl lc,int linkAddr);


        /// <summary>
        /// 复位帧（初始化帧）
        /// </summary>
		public abstract void ResetFrame();

        /// <summary>
        /// 增加一个需要发送的字节
        /// </summary>
        /// <param name="value"></param>
        public abstract void SetNextByte(byte value);

        /// <summary>
        /// 增加一组需要发送的字节
        /// </summary>
        /// <param name="bytes"></param>
        public abstract void AppendBytes(byte[] bytes);

        /// <summary>
        /// 获取总共字节数
        /// </summary>
        /// <returns></returns>
		public abstract int GetMsgSize();

        /// <summary>
        /// 获取需要发送的缓冲区
        /// </summary>
        /// <returns></returns>
		public abstract byte[] GetBuffer();

    }
}
