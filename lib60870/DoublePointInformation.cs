/*
 *  DoublePointInformation.cs
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
    /// 双点遥信值
    /// </summary>
    public enum DoublePointValue
    {
        /// <summary>
        /// 不确定，或者中间状态
        /// </summary>
        INTERMEDIATE = 0,
        /// <summary>
        /// 确定状态开
        /// </summary>
        OFF = 1,
        /// <summary>
        /// 确定状态合
        /// </summary>
        ON = 2,
        /// <summary>
        /// 不确定活中间状态
        /// </summary>
        INDETERMINATE = 3
    }

    /// <summary>
    /// 双点遥信
    /// </summary>
	public class DoublePointInformation : InformationObject
    {
        override public TypeID Type
        {
            get
            {
                return TypeID.M_DP_NA_1;
            }
        }

        /// <summary>
        /// 支持连续
        /// </summary>
        override public bool SupportsSequence
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 值
        /// </summary>
        private DoublePointValue value;

        public DoublePointValue Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// 品质描述词，低2位将被DPI（value）占据
        /// </summary>
        private QualityDescriptor quality;

        public QualityDescriptor Quality
        {
            get
            {
                return this.quality;
            }
        }

        /// <summary>
        /// 使用地址、值、品质描述词创建双点信息
        /// </summary>
        /// <param name="ioa"></param>
        /// <param name="value"></param>
        /// <param name="quality"></param>
        public DoublePointInformation(int ioa, DoublePointValue value, QualityDescriptor quality)
            : base(ioa)
        {
            this.value = value;
            this.quality = quality;
        }

        /// <summary>
        /// 解析报文
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        /// <param name="isSequence"></param>
        internal DoublePointInformation(ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
            base(parameters, msg, startIndex, isSequence)
        {
            if (!isSequence)
                startIndex += parameters.SizeOfIOA; /* skip IOA */

            /* parse DIQ (double point information with qualitiy) */
            byte diq = msg[startIndex++];

            value = (DoublePointValue)(diq & 0x03);

            quality = new QualityDescriptor((byte)(diq & 0xf0));
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

            byte val = quality.EncodedValue;

            val += (byte)value;

            frame.SetNextByte(val);
        }
    }

    /// <summary>
    /// 带短时标的双点信息
    /// </summary>
    public class DoublePointWithCP24Time2a : DoublePointInformation
    {
        override public TypeID Type
        {
            get
            {
                return TypeID.M_DP_TA_1;
            }
        }

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

        public DoublePointWithCP24Time2a(int ioa, DoublePointValue value, QualityDescriptor quality, CP24Time2a timestamp)
            : base(ioa, value, quality)
        {
            this.timestamp = timestamp;
        }

        internal DoublePointWithCP24Time2a(ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
        base(parameters, msg, startIndex, isSequence)
        {
            if (!isSequence)
                startIndex += parameters.SizeOfIOA; /* skip IOA */

            startIndex += 1; /* skip DIQ */

            /* parse CP24Time2a (time stamp) */
            timestamp = new CP24Time2a(msg, startIndex);
        }

        internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence)
        {
            base.Encode(frame, parameters, isSequence);

            frame.AppendBytes(timestamp.GetEncodedValue());
        }
    }

    /// <summary>
    /// 带长时标的双点信息
    /// </summary>
    public class DoublePointWithCP56Time2a : DoublePointInformation
    {
        override public TypeID Type
        {
            get
            {
                return TypeID.M_DP_TB_1;
            }
        }

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
        /// 创建
        /// </summary>
        /// <param name="ioa"></param>
        /// <param name="value"></param>
        /// <param name="quality"></param>
        /// <param name="timestamp"></param>
        public DoublePointWithCP56Time2a(int ioa, DoublePointValue value, QualityDescriptor quality, CP56Time2a timestamp)
            : base(ioa, value, quality)
        {
            this.timestamp = timestamp;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        /// <param name="isSequence"></param>
        internal DoublePointWithCP56Time2a(ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence) :
            base(parameters, msg, startIndex, isSequence)
        {
            if (!isSequence)
                startIndex += parameters.SizeOfIOA; /* skip IOA */

            startIndex += 1; /* skip DIQ */

            /* parse CP56Time2a (time stamp) */
            timestamp = new CP56Time2a(msg, startIndex);
        }

        internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence)
        {
            base.Encode(frame, parameters, isSequence);

            frame.AppendBytes(timestamp.GetEncodedValue());
        }
    }

}

