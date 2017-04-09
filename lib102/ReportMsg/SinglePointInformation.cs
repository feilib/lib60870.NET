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

namespace lib102
{
    /// <summary>
    /// 单点信息-TypeID.M_SP_TA_2;--102中只有这一个，直接带品质描述和时标56位 
    /// <para>1. 单点信息是指影响电能累计量的有效性事件，作为历史日志储存起来</para>
    /// <para>2. 用来监视电能累计量子站数据终端设备制造厂定义的单点信息是短暂的还是永久的</para>
    /// <para>3. 带地址(SPA)和限定词(SPQ)还有信息状态(SPI)，占2个字节</para>
    /// <para>特别说明：</para>
    /// <para>1. 单点信息在被控设备内可悲当地所认可，此时SPI设置可被设置为0</para>
    /// <para>2. 单点信息常常按瞬变信息传输，此信息具有两种状态SPI=0或1</para>
    /// <para>3. 单点信息可以突发传送，也可以被请求传输</para>
    /// <para>4. 由当地认可时产生的单点信息可以不传送</para>
    /// <para></para>
    /// </summary>
	public class SinglePointInformation : InformationObject
    {
        /// <summary>
        /// TypeID.M_SP_TA_2;
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.M_SP_TA_2;
            }
        }

        /// <summary>
        /// 不支持连续，每个信息的时标和地址都有单独含义
        /// </summary>
        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 实际单点值，bit1
        /// </summary>
        private bool value;

        /// <summary>
        /// 实际点值
        /// </summary>
        public bool Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// 单点地址信息，第一个字节(SPA)-
        /// <para>不同的地址代表不同的含义，具体查表，常用的有：</para>
        /// <para>  1  系统重新启动</para>
        /// <para>  3  电源故障</para>
        /// <para>  7  时间偏移</para>
        /// <para>  13 电能累计量的不允许差额</para>
        /// <para>  15 参数改变</para>
        /// <para>  17 人工输入</para>
        /// <para>  18 警告报文</para>
        /// <para>  19 差错信号</para>
        /// </summary>
        private int objectAddress;

        /// <summary>
        /// 单点地址信息，第一个字节(SPA)--
        /// <para>不同的地址代表不同的含义，具体查表，常用的有：</para>
        /// <para>  1  系统重新启动</para>
        /// <para>  3  电源故障</para>
        /// <para>  7  时间偏移</para>
        /// <para>  13 电能累计量的不允许差额</para>
        /// <para>  15 参数改变</para>
        /// <para>  17 人工输入</para>
        /// <para>  18 警告报文</para>
        /// <para>  19 差错信号</para>
        /// </summary>
        /// 其实基类里有，这里就是专门更新一下说明文件
        public new int ObjectAddress
        {
            get
            {
                return this.objectAddress;
            }
        }

        /// <summary>
        /// 品质描述词(SPQ)
        /// <para>配合SPA使用，可以代表不同的含义，具体查表</para>
        /// </summary>
        private int quality;

        /// <summary>
        /// 品质描述词(SPQ)
        /// <para>配合SPA使用，可以代表不同的含义，具体查表</para>
        /// </summary>
        public int Quality
        {
            get
            {
                return this.quality;
            }
        }

        /// <summary>
        /// 时标
        /// </summary>
        private CP56Time2b timestamp;

        /// <summary>
        /// 时标
        /// </summary>
        public CP56Time2b Timestamp
        {
            get
            {
                return this.timestamp;
            }
        }

        /// <summary>
        /// 通过信息内容解析报文
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        /// <param name="isSequence"></param>
        internal SinglePointInformation(byte[] msg, int startIndex, bool isSequence) :
            base(msg, startIndex, isSequence)
        {
            if (!isSequence)
                startIndex += 1; /* skip IOA 跳过信息体地址(在基类中解析过了) */

            /* parse SPI (single point information with qualitiy)  解析实际值 */
            byte spi = msg[startIndex++];

            //获得值 bit1
            value = ((spi & 0x01) == 0x01);

            //SPQ 信息限定词，品质描述词 bit2-8
            quality = (byte)(spi & 0xFE);

            /* parse CP56Time2a (time stamp) */
            timestamp = new CP56Time2b(msg, startIndex);
        }

        /// <summary>
        /// 构造一个单点信息，使用信息地址、值、品质描述词
        /// </summary>
        /// <param name="objectAddress">信息体地址</param>
        /// <param name="value">单点值</param>
        /// <param name="quality">品质描述词</param>
        /// <param name="timestamp">时标</param>
        public SinglePointInformation(int objectAddress, bool value, int quality, CP56Time2b timestamp) :
            base(objectAddress)
        {
            this.value = value;
            this.quality = quality;
            this.timestamp = timestamp;
        }

        /// <summary>
        /// 编码进入frame
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="isSequence">不支持连续，强制写false了</param>
        internal override void Encode(Frame frame, bool isSequence = false)
        {
            //直接写死把，这个情况不支持连续
            base.Encode(frame, false);

            //quality 需要左移一位
            byte val = (byte)((quality * 2) & 0xFF);

            //设置值
            if (value)
            {
                val = (byte)(val | 0x01);
            }
            else
            {
                val = (byte)(val & 0xFE);
            }

            frame.SetNextByte(val);

            //之后写时间
            frame.AppendBytes(timestamp.GetEncodedValue());
        }

    }

}

