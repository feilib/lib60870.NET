/*
 *  ASDU.cs
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
using System.Collections.Generic;

namespace lib102
{
    /// <summary>
    /// ASDU 异常
    /// </summary>
    public class ASDUParsingException : Exception
    {
        public ASDUParsingException(string message) : base(message)
        {

        }
    }

    /// <summary>
    /// This class represents an application layer message. It contains some generic message information and
    /// one or more InformationObject instances of the same type. It is used to send and receive messages.
    /// </summary>
    public class ASDU
    {
        /// <summary>
        /// 连接参数
        /// </summary>
        private ConnectionParameters parameters;

        /// <summary>
        /// 类型标识
        /// </summary>
        private TypeID typeId;
        private bool hasTypeId;

        /// <summary>
        /// <para> 可变结构限定词，根据添加实时变化  SQ+number</para>
        /// <para> SQ 离散或者顺序，0：离散，1连续</para>
        /// <para> number，信息对象数目</para>
        /// </summary>
        private byte vsq; /* variable structure qualifier */

        #region 传送原因结构--1个字节
        /// <summary>
        /// 传送原因
        /// </summary>
        private CauseOfTransmission cot; /* cause */
        /// <summary>
        /// 试验标志（传送原因bit8）
        /// </summary>
        private bool isTest; /* is message a test message */
        /// <summary>
        /// 确认标志（传送原因bit7） 
        /// </summary>
        private bool isNegative; /* is message a negative confirmation */
        #endregion

        /// <summary>
        /// 电能量数据终端设备地址（默认2个字节，也有一个字节的情况，根据参数设置）
        /// </summary>
        private int ca; /* Common address */

        /// <summary>
        /// 记录地址
        /// </summary>
        private RecordAddress ra;

        /// <summary>
        /// 其余信息
        /// </summary>
        private byte[] payload = null;

        /// <summary>
        /// 信息体
        /// </summary>
        private List<InformationObject> informationObjects = null;

        /// <summary>
        /// 初始化ASDU，使用一大堆参数（无类型标识的初始化）
        /// </summary>
        /// <param name="cot">传送原因</param>
        /// <param name="isTest">是否测试</param>
        /// <param name="isNegative">肯定/否定回答</param>
        /// <param name="ca"> 应用服务数据单元公共地址 </param>
        /// <param name="ra">记录地址</param>
        /// <param name="isSequence">是否连续单元（用于构造VSQ），数量根据添加来变</param>

        public ASDU(CauseOfTransmission cot, bool isTest, bool isNegative, int ca, RecordAddress ra, bool isSequence)
            : this(TypeID.M_SP_TA_2, cot, isTest, isNegative, ca, ra, isSequence)
        {
            //这里标志一下，没有类型标识，也就是说在初始化ASDU的时候不加类型标识
            //当在增加信息对象的时候，类型标识根据信息对象的标识来确定。
            this.hasTypeId = false;
        }

        /// <summary>
        /// 初始化ASDU，使用一大堆参数
        /// </summary>
        /// <param name="typeId">类型标识</param>
        /// <param name="cot">传送原因</param>
        /// <param name="isTest">是否测试</param>
        /// <param name="isNegative">肯定/否定回答</param>
        /// <param name="ca"> 应用服务数据单元公共地址 </param>
        /// <param name="ra">记录地址</param>
        /// <param name="isSequence">是否连续单元（用于构造VSQ），数量根据添加来变</param>
        public ASDU(TypeID typeId, CauseOfTransmission cot, bool isTest, bool isNegative, int ca, RecordAddress ra, bool isSequence)
        {
            this.typeId = typeId;
            this.cot = cot;
            this.isTest = isTest;
            this.isNegative = isNegative;
            this.ca = ca;

            if (isSequence)
                this.vsq = 0x80;
            else
                this.vsq = 0;

            this.hasTypeId = true;
        }

        /// <summary>
        /// 添加一个信息体
        /// </summary>
        /// <param name="io">信息体对象</param>
        public void AddInformationObject(InformationObject io)
        {
            if (informationObjects == null)
                informationObjects = new List<InformationObject>();

            if (hasTypeId)
            {
                //类型标识不同的，直接异常，不予以添加
                if (io.Type != typeId)
                    throw new ArgumentException("Invalid information object type: expected " + typeId.ToString() + " was " + io.Type.ToString());
            }
            else
            {
                //初始化的时候没有设置类型标识，则在赋值的时候确认类型标识
                typeId = io.Type;
                hasTypeId = true;
            }

            //增加对象
            informationObjects.Add(io);

            //修改数量
            vsq = (byte)((vsq & 0x80) | informationObjects.Count);
        }

        /// <summary>
        /// 使用buff初始化一个ASDU，相当于解析
        /// </summary>
        /// <param name="parameters">链接参数</param>
        /// <param name="msg">接收到的消息内容</param>
        /// <param name="msgLength">消息长度（在报文头里面有）</param>
        public ASDU(ConnectionParameters parameters, byte[] msg, int msgLength)
        {
            //保存链接参数
            this.parameters = parameters.clone();

            //跳过报文头，68H+长度（2）+68H + 链路控制域（1） + 链路地址（2）
            int bufPos = 7;

            typeId = (TypeID)msg[bufPos++];     //类型标识

            vsq = msg[bufPos++];                //可变结构限定词

            this.hasTypeId = true;

            byte cotByte = msg[bufPos++];       //传送原因

            //测试标志--0 未试验（false），1  试验（true）
            if ((cotByte & 0x80) != 0)
                isTest = true;
            else
                isTest = false;

            //确认标志，0  肯定确认（false） ，1 否定确认（true）
            if ((cotByte & 0x40) != 0)
                isNegative = true;
            else
                isNegative = false;

            //传送原因
            cot = (CauseOfTransmission)(cotByte & 0x3f);

            //电能量数据终端设备地址(2字节）
            ca = msg[bufPos++];
            if (parameters.SizeOfCA > 1)
                ca += (msg[bufPos++] * 0x100);

            //记录地址
            ra = (RecordAddress)msg[bufPos++];     //类型标识

            int payloadSize = msgLength - bufPos - 2;//把最后的和校验和结束符去掉

            payload = new byte[payloadSize];

            /* save payload */
            Buffer.BlockCopy(msg, bufPos, payload, 0, payloadSize);
        }

        /// <summary>
        /// 将ASDU内容编码入frame中。。
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="parameters"></param>
        public void Encode(Frame frame, ConnectionParameters parameters)
        {
            //先编码类型标识
            frame.SetNextByte((byte)typeId);
            //然后就是可变结构限定词
            frame.SetNextByte(vsq);

            #region 传送原因
            byte cotByte = (byte)cot;

            //测试标志
            if (isTest)
                cotByte = (byte)(cotByte | 0x80);
            //确认标志
            if (isNegative)
                cotByte = (byte)(cotByte | 0x40);

            frame.SetNextByte(cotByte);
            #endregion

            #region 电能量数据终端设备地址(2字节）
            frame.SetNextByte((byte)(ca % 256));

            if (parameters.SizeOfCA > 1)
                frame.SetNextByte((byte)(ca / 256));
            #endregion

            //记录地址
            frame.SetNextByte((byte)ra);

            //接下来编码内容
            if (payload != null)
            {
                //解析的情况下，直接把内容搞进去就好了
                frame.AppendBytes(payload);
            }
            else
            {
                //非解析的情况下，，挨着写把。。。
                bool isFirst = true;

                foreach (InformationObject io in informationObjects)
                {

                    if (isFirst)
                    {
                        //第一条，必须编码地址进去，sequence这个参数传递false就好了
                        io.Encode(frame, parameters, false);
                        isFirst = false;
                    }
                    else
                    {
                        //后面的，根据是否为连续，决定是否编码地址
                        if (IsSquence)
                            io.Encode(frame, parameters, true);
                        else
                            io.Encode(frame, parameters, false);
                    }

                }
            }

        }

        /// <summary>
        /// 类型标识
        /// </summary>
        public TypeID TypeId
        {
            get
            {
                return this.typeId;
            }
        }

        /// <summary>
        /// 传送原因
        /// </summary>
        public CauseOfTransmission Cot
        {
            get
            {
                return this.cot;
            }
            set
            {
                this.cot = value;
            }
        }

        /// <summary>
        /// 试验标志
        /// </summary>
        public bool IsTest
        {
            get
            {
                return this.isTest;
            }
        }

        /// <summary>
        /// 确认标志
        /// </summary>
        public bool IsNegative
        {
            get
            {
                return this.isNegative;
            }
            set
            {
                isNegative = value;
            }
        }



        /// <summary>
        /// 应用服务数据单元公共地址（2个字节）
        /// </summary>
        public int Ca
        {
            get
            {
                return this.ca;
            }
        }

        /// <summary>
        /// 记录地址
        /// </summary>
        public RecordAddress Ra
        {
            get
            {
                return this.ra;
            }
            set
            {
                this.ra = value;
            }
        }



        /// <summary>
        /// 离散或者连续，在可变结构限定词里的的第一位中标识SQ
        /// </summary>
        public bool IsSquence
        {
            get
            {
                if ((vsq & 0x80) != 0)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 信息对象数目，可变结构限定词的后七位
        /// </summary>
        public int NumberOfElements
        {
            get
            {
                return (vsq & 0x7f);
            }
        }

        /// <summary>
        /// 获取第n个信息对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public InformationObject GetElement(int index)
        {
            InformationObject retVal = null;

            int elementSize;

            switch (typeId)
            {
                #region 根据不同的类型标识，做不同的解析
                case TypeID.M_SP_TA_2: /* 1 单位遥信 */
                    //1个字节地址+ 1个字节内容+ 7个字节的时标
                    elementSize = 1 + 1 + 7;
                    retVal = new SinglePointInformation(parameters, payload, index * elementSize, false);
                    break;

                case TypeID.M_IT_TA_2: /* 2 */

                    elementSize = 4;

                    break;
                case TypeID.M_IT_TD_2: /* 5 */

                    elementSize = 4;

                    break;
                case TypeID.M_IT_TG_2: /* 8 */

                    elementSize = 4;

                    break;
                case TypeID.M_IT_TK_2: /* 11 */

                    elementSize = 4;

                    break;
                case TypeID.M_EI_NA_2: /* 70 */

                    elementSize = 4;

                    break;
                case TypeID.P_MP_NA_2: /* 71 */

                    elementSize = 4;

                    break;
                case TypeID.M_TI_TA_2: /* 72 */

                    elementSize = 4;

                    break;
                case TypeID.C_RD_NA_2: /* 100 */

                    elementSize = 4;

                    break;
                case TypeID.C_SP_NA_2: /* 101*/

                    elementSize = 4;

                    break;
                case TypeID.C_SP_NB_2: /* 102 */

                    elementSize = 4;

                    break;
                case TypeID.C_TI_NA_2: /* 103 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NA_2: /* 104 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NB_2: /* 105 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NC_2: /* 106 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_ND_2: /* 107 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NE_2: /* 108 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NF_2: /* 109 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NG_2: /* 110 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NH_2: /* 111 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NI_2: /* 112 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NK_2: /* 113 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NL_2: /* 114 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NM_2: /* 115 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NN_2: /* 116 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NO_2: /* 117 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NP_2: /* 118 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NQ_2: /* 119 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NR_2: /* 120 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NS_2: /* 121 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NT_2: /* 122 */

                    elementSize = 4;

                    break;
                case TypeID.C_CI_NU_2: /* 123 */

                    elementSize = 4;

                    break;
                case TypeID.C_SYN_TA_2: /* 128 */

                    elementSize = 4;

                    break;

                #endregion

                default:
                    throw new ASDUParsingException("Unknown ASDU type id:" + typeId);
            }

            return retVal;
        }


        public override string ToString()
        {
            string ret;

            ret = "TypeID: " + typeId.ToString() + " COT: " + cot.ToString();

            if (isTest)
                ret += " [TEST]";

            if (isNegative)
                ret += " [NEG]";

            if (IsSquence)
                ret += " [SEQ]";

            ret += " elements: " + NumberOfElements;

            ret += " CA: " + ca;

            ret += " RA: " + ra.ToString();

            return ret;
        }
    }
}

