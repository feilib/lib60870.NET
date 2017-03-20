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

namespace lib60870
{
    /// <summary>
    /// 单点信息
    /// </summary>
	public class SinglePointInformation : InformationObject
    {
        /// <summary>
        /// TypeID.M_SP_NA_1;
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.M_SP_NA_1;
            }
        }

        override public bool SupportsSequence
        {
            get
            {
                return true;
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
        /// 品质描述词
        /// </summary>
        private QualityDescriptor quality;

        /// <summary>
        /// 品质描述词
        /// </summary>
        public QualityDescriptor Quality
        {
            get
            {
                return this.quality;
            }
        }

        /// <summary>
        /// 通过信息内容解析报文
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        /// <param name="isSequence"></param>
        internal SinglePointInformation(ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
            base(parameters, msg, startIndex, isSequence)
        {
            if (!isSequence)
                startIndex += parameters.SizeOfIOA; /* skip IOA 跳过信息体地址 */

            /* parse SIQ (single point information with qualitiy)  解析实际值 */
            byte siq = msg[startIndex++];

            //获得值 bit1
            value = ((siq & 0x01) == 0x01);

            //品质描述词，只要高4位，低位没用 bit5-8
            quality = new QualityDescriptor((byte)(siq & 0xf0));
        }

        /// <summary>
        /// 构造一个单点信息，使用信息地址、值、品质描述词
        /// </summary>
        /// <param name="objectAddress">信息体地址</param>
        /// <param name="value">单点值</param>
        /// <param name="quality">品质描述词（OV无用，直接被覆盖了）</param>
        public SinglePointInformation(int objectAddress, bool value, QualityDescriptor quality) :
            base(objectAddress)
        {
            this.value = value;
            this.quality = quality;

        }

        /// <summary>
        /// 编码进入frame
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="parameters"></param>
        /// <param name="isSequence"></param>
        internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence)
        {
            base.Encode(frame, parameters, isSequence);

            byte val = quality.EncodedValue;

            //设置值，如果有值为true，则ov反相  todo，这个算法需要验证。。。。或者需要直接覆盖
            if (value)
                val++;

            frame.SetNextByte(val);
        }

    }

    /// <summary>
    /// 带时标的单点信息（用的少）
    /// </summary>
    public class SinglePointWithCP24Time2a : SinglePointInformation
    {
        override public TypeID Type
        {
            get
            {
                return TypeID.M_SP_TA_1;
            }
        }

        /// <summary>
        /// 带时标的就不支持连续了？
        /// </summary>
        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        private CP24Time2a timestamp;

        public CP24Time2a Timestamp
        {
            get
            {
                return this.timestamp;
            }
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        /// <param name="isSequence"></param>
        internal SinglePointWithCP24Time2a(ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
        base(parameters, msg, startIndex, isSequence)
        {
            if (!isSequence)
                startIndex += parameters.SizeOfIOA; /* skip IOA */

            startIndex += 1; /* skip SIQ */

            /* parse CP24Time2a (time stamp) */
            timestamp = new CP24Time2a(msg, startIndex);
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="objectAddress"></param>
        /// <param name="value"></param>
        /// <param name="quality"></param>
        /// <param name="timestamp"></param>
        public SinglePointWithCP24Time2a(int objectAddress, bool value, QualityDescriptor quality, CP24Time2a timestamp) :
            base(objectAddress, value, quality)
        {
            this.timestamp = timestamp;
        }

        internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence)
        {
            base.Encode(frame, parameters, isSequence);

            frame.AppendBytes(timestamp.GetEncodedValue());
        }
    }

    /// <summary>
    /// Single point with CP56Time2a timestamp (M_SP_TB_1)
    /// 带长时标的单点信息
    /// </summary>
    public class SinglePointWithCP56Time2a : SinglePointInformation
    {
        override public TypeID Type
        {
            get
            {
                return TypeID.M_SP_TB_1;
            }
        }

        /// <summary>
        /// 不支持连续
        /// </summary>
        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        private CP56Time2a timestamp;

        public CP56Time2a Timestamp
        {
            get
            {
                return this.timestamp;
            }
        }

        /// <summary>
        /// 通过信息体解析
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        /// <param name="isSequence"></param>
        internal SinglePointWithCP56Time2a(ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
        base(parameters, msg, startIndex, isSequence)
        {
            if (!isSequence)
                startIndex += parameters.SizeOfIOA; /* skip IOA */

            startIndex += 1; /* skip SIQ */

            /* parse CP56Time2a (time stamp) */
            timestamp = new CP56Time2a(msg, startIndex);
        }

        /// <summary>
        /// 创建一个
        /// </summary>
        /// <param name="objectAddress"></param>
        /// <param name="value"></param>
        /// <param name="quality"></param>
        /// <param name="timestamp"></param>
        public SinglePointWithCP56Time2a(int objectAddress, bool value, QualityDescriptor quality, CP56Time2a timestamp) :
        base(objectAddress, value, quality)
        {
            this.timestamp = timestamp;
        }

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="parameters"></param>
        /// <param name="isSequence"></param>
        internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence)
        {
            base.Encode(frame, parameters, isSequence);

            frame.AppendBytes(timestamp.GetEncodedValue());
        }
    }

    /// <summary>
    /// 带变位检出单点信息（用的少）
    /// </summary>
    public class PackedSinglePointWithSCD : InformationObject
    {
        override public TypeID Type
        {
            get
            {
                return TypeID.M_PS_NA_1;
            }
        }

        override public bool SupportsSequence
        {
            get
            {
                return true;
            }
        }


        private StatusAndStatusChangeDetection scd;

        private QualityDescriptor qds;

        public StatusAndStatusChangeDetection SCD
        {
            get
            {
                return this.scd;
            }
            set
            {
                scd = value;
            }
        }

        public QualityDescriptor QDS
        {
            get
            {
                return this.qds;
            }
            set
            {
                qds = value;
            }
        }

        internal PackedSinglePointWithSCD(ConnectionParameters parameters, byte[] msg, int startIndex, bool isSquence) :
            base(parameters, msg, startIndex, isSquence)
        {
            if (!isSquence)
                startIndex += parameters.SizeOfIOA; /* skip IOA */

            scd = new StatusAndStatusChangeDetection(msg, startIndex);
            startIndex += 4;

            qds = new QualityDescriptor(msg[startIndex++]);
        }

        public PackedSinglePointWithSCD(int objectAddress, StatusAndStatusChangeDetection scd, QualityDescriptor quality)
            : base(objectAddress)
        {
            this.scd = scd;
            this.qds = quality;
        }

        internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence)
        {
            base.Encode(frame, parameters, isSequence);

            frame.AppendBytes(scd.GetEncodedValue());

            frame.SetNextByte(qds.EncodedValue);
        }
    }

}

