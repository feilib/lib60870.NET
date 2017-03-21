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
    /// 召唤限定词
    /// </summary>
	public class QualifierOfInterrogation
    {
        /// <summary>
        /// 全站召唤
        /// </summary>
        public static byte STATION = 20;
        /// <summary>
        /// 召唤第 1 组信息 （遥信信息）
        /// </summary>
		public static byte GROUP_1 = 21;
        /// <summary>
        /// 召唤第 2 组信息 （遥信信息）
        /// </summary>
		public static byte GROUP_2 = 22;
        /// <summary>
        /// 召唤第 3 组信息 （遥信信息）
        /// </summary>
		public static byte GROUP_3 = 23;
        /// <summary>
        /// 召唤第 4 组信息 （遥信信息）
        /// </summary>
		public static byte GROUP_4 = 24;
        /// <summary>
        /// 召唤第 5 组信息 （遥信信息）
        /// </summary>
		public static byte GROUP_5 = 25;
        /// <summary>
        /// 召唤第 6 组信息 （遥信信息）
        /// </summary>
		public static byte GROUP_6 = 26;
        /// <summary>
        /// 召唤第 7 组信息 （遥信信息）
        /// </summary>
		public static byte GROUP_7 = 27;
        /// <summary>
        /// 召唤第 8 组信息 （遥信信息）
        /// </summary>
		public static byte GROUP_8 = 28;
        /// <summary>
        /// 召唤第 9 组信息 （遥测信息）
        /// </summary>
        public static byte GROUP_9 = 29;
        /// <summary>
        /// 召唤第 10 组信息 （遥测信息）
        /// </summary>
		public static byte GROUP_10 = 30;
        /// <summary>
        /// 召唤第 11 组信息 （遥测信息）
        /// </summary>
		public static byte GROUP_11 = 31;
        /// <summary>
        /// 召唤第 12 组信息 （遥测信息）
        /// </summary>
		public static byte GROUP_12 = 32;
        /// <summary>
        /// 召唤第 13 组信息 （遥测信息）
        /// </summary>
		public static byte GROUP_13 = 33;
        /// <summary>
        /// 召唤第 14 组信息 （遥测信息）
        /// </summary>
		public static byte GROUP_14 = 34;
        /// <summary>
        /// 召唤第 15 组信息 （档位信息）
        /// </summary>
        public static byte GROUP_15 = 35;
        /// <summary>
        /// 召唤第 16 组信息 （远动终端状态信息）
        /// </summary>
        public static byte GROUP_16 = 36;
    }
    /// <summary>
    /// 站召唤命令
    /// </summary>
	public class InterrogationCommand : InformationObject
    {
        override public TypeID Type
        {
            get
            {
                return TypeID.C_IC_NA_1;
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

        /// <summary>
        /// 召唤限定词
        /// </summary>
        byte qoi;

        /// <summary>
        /// 召唤限定词
        /// </summary>
        public byte QOI
        {
            get
            {
                return this.qoi;
            }
            set
            {
                qoi = value;
            }
        }

        /// <summary>
        /// 使用信息地址和召唤限定词初始化对象
        /// </summary>
        /// <param name="ioa">信息体地址</param>
        /// <param name="qoi">召唤限定词</param>
        public InterrogationCommand(int ioa, byte qoi) : base(ioa)
        {
            this.qoi = qoi;
        }

        /// <summary>
        /// 解析站召唤
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        internal InterrogationCommand(ConnectionParameters parameters, byte[] msg, int startIndex) :
            base(parameters, msg, startIndex, false)
        {
            //ioa已经基类里解析过了。。
            startIndex += parameters.SizeOfIOA; /* skip IOA */

            qoi = msg[startIndex++];
        }

        /// <summary>
        /// 将站召唤编码进去
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="parameters"></param>
        /// <param name="isSequence"></param>
        internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence)
        {
            base.Encode(frame, parameters, isSequence);

            frame.SetNextByte(qoi);
        }

    }

    /// <summary>
    /// 累计量的站召唤
    /// </summary>
    public class CounterInterrogationCommand : InformationObject
    {
        override public TypeID Type
        {
            get
            {
                return TypeID.C_CI_NA_1;
            }
        }

        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        byte qcc;

        /// <summary>
        /// Gets or sets the QCC (Qualifier of counter interrogation).
        /// 
        /// 召唤限定词
        /// </summary>
        /// <value>The QCC</value>
        public byte QCC
        {
            get
            {
                return this.qcc;
            }
            set
            {
                qcc = value;
            }
        }

        /// <summary>
        /// 使用信息地址和召唤限定词初始化对象
        /// </summary>
        /// <param name="ioa">信息体地址</param>
        /// <param name="qoi">召唤限定词</param>
        public CounterInterrogationCommand(int ioa, byte qoi) : base(ioa)
        {
            this.qcc = qoi;
        }

        /// <summary>
        /// 解析累计量的站召唤
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        internal CounterInterrogationCommand(ConnectionParameters parameters, byte[] msg, int startIndex) :
            base(parameters, msg, startIndex, false)
        {
            startIndex += parameters.SizeOfIOA; /* skip IOA */

            qcc = msg[startIndex++];
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

            frame.SetNextByte(qcc);
        }

    }

    /// <summary>
    /// 读命令
    /// </summary>
    public class ReadCommand : InformationObject
    {
        override public TypeID Type
        {
            get
            {
                return TypeID.C_RD_NA_1;
            }
        }

        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 构造读命令，主要就是个地址
        /// </summary>
        /// <param name="ioa"></param>
        public ReadCommand(int ioa) : base(ioa)
        {
        }

        /// <summary>
        /// 解析读命令，把地址解析出来就好了
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        internal ReadCommand(ConnectionParameters parameters, byte[] msg, int startIndex) :
            base(parameters, msg, startIndex, false)
        {
        }

    }

    /// <summary>
    /// 时钟同步命令
    /// </summary>
    public class ClockSynchronizationCommand : InformationObject
    {
        override public TypeID Type
        {
            get
            {
                return TypeID.C_CS_NA_1;
            }
        }

        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 新时间
        /// </summary>
        private CP56Time2a newTime;

        /// <summary>
        /// 新时间
        /// </summary>
        public CP56Time2a NewTime
        {
            get
            {
                return this.newTime;
            }
            set
            {
                newTime = value;
            }
        }

        /// <summary>
        /// 使用地址和事件构造同步命令
        /// </summary>
        /// <param name="ioa"></param>
        /// <param name="newTime"></param>
        public ClockSynchronizationCommand(int ioa, CP56Time2a newTime) : base(ioa)
        {
            this.newTime = newTime;
        }

        /// <summary>
        /// 解析时钟同步命令
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        internal ClockSynchronizationCommand(ConnectionParameters parameters, byte[] msg, int startIndex) :
            base(parameters, msg, startIndex, false)
        {
            startIndex += parameters.SizeOfIOA; /* skip IOA */

            /* parse CP56Time2a (time stamp) */
            newTime = new CP56Time2a(msg, startIndex);
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

            frame.AppendBytes(newTime.GetEncodedValue());
        }
    }

    /// <summary>
    /// 复位命令
    /// </summary>
    public class ResetProcessCommand : InformationObject
    {
        override public TypeID Type
        {
            get
            {
                return TypeID.C_RP_NA_1;
            }
        }

        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 复位限定词
        /// <para>0 ： 未使用</para>
        /// <para>1 ： 进程总复位</para>
        /// <para>2 ： 复位时间缓冲区对待处理的带时标信息</para>
        /// <para>其他值 ： 未定义</para>
        /// </summary>
        byte qrp;

        /// <summary>
        /// Gets or sets the QRP (Qualifier of reset process command).
        /// 复位限定词
        /// <para>0 ： 未使用</para>
        /// <para>1 ： 进程总复位</para>
        /// <para>2 ： 复位时间缓冲区对待处理的带时标信息</para>
        /// <para>其他值 ： 未定义</para>
        /// </summary>
        /// <value>The QRP</value>
        public byte QRP
        {
            get
            {
                return this.qrp;
            }
            set
            {
                qrp = value;
            }
        }

        /// <summary>
        /// 使用公共地址和复位限定词初始化对象
        /// </summary>
        /// <param name="ioa"></param>
        /// <param name="qrp"></param>
        public ResetProcessCommand(int ioa, byte qrp) : base(ioa)
        {
            this.qrp = qrp;
        }

        /// <summary>
        /// 解析复位命令
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        internal ResetProcessCommand(ConnectionParameters parameters, byte[] msg, int startIndex) :
            base(parameters, msg, startIndex, false)
        {
            startIndex += parameters.SizeOfIOA; /* skip IOA */

            qrp = msg[startIndex++];
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

            frame.SetNextByte(qrp);
        }

    }

    /// <summary>
    /// 延时获得命令
    /// </summary>
    public class DelayAcquisitionCommand : InformationObject
    {
        override public TypeID Type
        {
            get
            {
                return TypeID.C_CD_NA_1;
            }
        }

        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        private CP16Time2a delay;

        public CP16Time2a Delay
        {
            get
            {
                return this.delay;
            }
            set
            {
                delay = value;
            }
        }

        public DelayAcquisitionCommand(int ioa, CP16Time2a delay) : base(ioa)
        {
            this.delay = delay;
        }

        internal DelayAcquisitionCommand(ConnectionParameters parameters, byte[] msg, int startIndex) :
            base(parameters, msg, startIndex, false)
        {
            startIndex += parameters.SizeOfIOA; /* skip IOA */

            /* parse CP16Time2a (time stamp) */
            delay = new CP16Time2a(msg, startIndex);
        }

        internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence)
        {
            base.Encode(frame, parameters, isSequence);

            frame.AppendBytes(delay.GetEncodedValue());
        }
    }

}

