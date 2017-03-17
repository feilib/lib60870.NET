/*
 *  InformationObject.cs
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

namespace lib60870
{
    /// <summary>
    /// 信息体对象-这是个抽象类，大家要继承他。。。
    /// </summary>
	public abstract class InformationObject
    {
        /// <summary>
        /// <para>信息体对象地址</para>
        /// <para>  长度为1-3个字节，在ConnectionParameters中设置</para>
        /// <para>  连续传输方式下，从第二个信息体开始，地址不再出现，默认为上一个地址+1</para>
        /// <para>  离散传输，每个信息体都必须有地址</para>
        /// <para>  没有明确的对象地址时，用0代替</para>
        /// </summary>
		private int objectAddress;

        /// <summary>
        /// 解析信息体对象地址，根据ConnectionParameters中设置的地址长度进行解析
        /// </summary>
        /// <param name="parameters">连接参数</param>
        /// <param name="msg">信息体</param>
        /// <param name="startIndex">开始字节</param>
        /// <returns>解析返回信息对象地址</returns>
		internal static int ParseInformationObjectAddress(ConnectionParameters parameters, byte[] msg, int startIndex)
        {
            int ioa = msg[startIndex];

            if (parameters.SizeOfIOA > 1)
                ioa += (msg[startIndex + 1] * 0x100);

            if (parameters.SizeOfIOA > 2)
                ioa += (msg[startIndex + 2] * 0x10000);

            return ioa;
        }

        /// <summary>
        /// 构造函数，通过信息内容
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        /// <param name="isSequence"></param>
        internal InformationObject(ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence)
        {
            //非连续状态下，才挨着解析地址。。。
            //连续状态时，第二个信息体开始就不出现地址了，默认为上一个地址+1
            if (!isSequence)
                objectAddress = ParseInformationObjectAddress(parameters, msg, startIndex);
        }

        /// <summary>
        /// 使用信息体地址初始化信息体对象
        /// </summary>
        /// <param name="objectAddress"></param>
        public InformationObject(int objectAddress)
        {
            this.objectAddress = objectAddress;
        }

        /// <summary>
        /// <para>信息体对象地址</para>
        /// <para>  长度为1-3个字节，在ConnectionParameters中设置</para>
        /// <para>  连续传输方式下，从第二个信息体开始，地址不再出现，默认为上一个地址+1</para>
        /// <para>  离散传输，每个信息体都必须有地址</para>
        /// <para>  没有明确的对象地址时，用0代替</para>
        /// </summary>
        public int ObjectAddress
        {
            get
            {
                return this.objectAddress;
            }
            internal set
            {
                objectAddress = value;
            }
        }

        /// <summary>
        /// 是否支持连续？子类中实现
        /// </summary>
        public abstract bool SupportsSequence
        {
            get;
        }

        /// <summary>
        /// 获取信息体对象的类型，在子类中实现
        /// </summary>
        public abstract TypeID Type
        {
            get;
        }

        /// <summary>
        /// 编码？这个什么功能目前还不知道，需要等用到了再过来看--子类中实现
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="parameters"></param>
        /// <param name="isSequence"></param>
        internal virtual void Encode(Frame frame, ConnectionParameters parameters, bool isSequence)
        {
            if (!isSequence)
            {
                frame.SetNextByte((byte)(objectAddress & 0xff));

                if (parameters.SizeOfIOA > 1)
                    frame.SetNextByte((byte)((objectAddress / 0x100) & 0xff));

                if (parameters.SizeOfIOA > 2)
                    frame.SetNextByte((byte)((objectAddress / 0x10000) & 0xff));
            }
        }


    }
}

