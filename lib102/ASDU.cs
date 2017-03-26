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
    /// ASDU �쳣
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
        /// ���Ӳ���
        /// </summary>
        private ConnectionParameters parameters;

        /// <summary>
        /// ���ͱ�ʶ
        /// </summary>
        private TypeID typeId;
        private bool hasTypeId;

        /// <summary>
        /// <para> �ɱ�ṹ�޶��ʣ��������ʵʱ�仯  SQ+number</para>
        /// <para> SQ ��ɢ����˳��0����ɢ��1����</para>
        /// <para> number����Ϣ������Ŀ</para>
        /// </summary>
        private byte vsq; /* variable structure qualifier */

        #region ����ԭ��ṹ--1���ֽ�
        /// <summary>
        /// ����ԭ��
        /// </summary>
        private CauseOfTransmission cot; /* cause */
        /// <summary>
        /// �����־������ԭ��bit8��
        /// </summary>
        private bool isTest; /* is message a test message */
        /// <summary>
        /// ȷ�ϱ�־������ԭ��bit7�� 
        /// </summary>
        private bool isNegative; /* is message a negative confirmation */
        #endregion

        /// <summary>
        /// �����������ն��豸��ַ��Ĭ��2���ֽڣ�Ҳ��һ���ֽڵ���������ݲ������ã�
        /// </summary>
        private int ca; /* Common address */

        /// <summary>
        /// ��¼��ַ
        /// </summary>
        private RecordAddress ra;

        /// <summary>
        /// ������Ϣ
        /// </summary>
        private byte[] payload = null;

        /// <summary>
        /// ��Ϣ��
        /// </summary>
        private List<InformationObject> informationObjects = null;

        /// <summary>
        /// ��ʼ��ASDU��ʹ��һ��Ѳ����������ͱ�ʶ�ĳ�ʼ����
        /// </summary>
        /// <param name="cot">����ԭ��</param>
        /// <param name="isTest">�Ƿ����</param>
        /// <param name="isNegative">�϶�/�񶨻ش�</param>
        /// <param name="ca"> Ӧ�÷������ݵ�Ԫ������ַ </param>
        /// <param name="ra">��¼��ַ</param>
        /// <param name="isSequence">�Ƿ�������Ԫ�����ڹ���VSQ�������������������</param>

        public ASDU(CauseOfTransmission cot, bool isTest, bool isNegative, int ca, RecordAddress ra, bool isSequence)
            : this(TypeID.M_SP_TA_2, cot, isTest, isNegative, ca, ra, isSequence)
        {
            //�����־һ�£�û�����ͱ�ʶ��Ҳ����˵�ڳ�ʼ��ASDU��ʱ�򲻼����ͱ�ʶ
            //����������Ϣ�����ʱ�����ͱ�ʶ������Ϣ����ı�ʶ��ȷ����
            this.hasTypeId = false;
        }

        /// <summary>
        /// ��ʼ��ASDU��ʹ��һ��Ѳ���
        /// </summary>
        /// <param name="typeId">���ͱ�ʶ</param>
        /// <param name="cot">����ԭ��</param>
        /// <param name="isTest">�Ƿ����</param>
        /// <param name="isNegative">�϶�/�񶨻ش�</param>
        /// <param name="ca"> Ӧ�÷������ݵ�Ԫ������ַ </param>
        /// <param name="ra">��¼��ַ</param>
        /// <param name="isSequence">�Ƿ�������Ԫ�����ڹ���VSQ�������������������</param>
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
        /// ���һ����Ϣ��
        /// </summary>
        /// <param name="io">��Ϣ�����</param>
        public void AddInformationObject(InformationObject io)
        {
            if (informationObjects == null)
                informationObjects = new List<InformationObject>();

            if (hasTypeId)
            {
                //���ͱ�ʶ��ͬ�ģ�ֱ���쳣�����������
                if (io.Type != typeId)
                    throw new ArgumentException("Invalid information object type: expected " + typeId.ToString() + " was " + io.Type.ToString());
            }
            else
            {
                //��ʼ����ʱ��û���������ͱ�ʶ�����ڸ�ֵ��ʱ��ȷ�����ͱ�ʶ
                typeId = io.Type;
                hasTypeId = true;
            }

            //���Ӷ���
            informationObjects.Add(io);

            //�޸�����
            vsq = (byte)((vsq & 0x80) | informationObjects.Count);
        }

        /// <summary>
        /// ʹ��buff��ʼ��һ��ASDU���൱�ڽ���
        /// </summary>
        /// <param name="parameters">���Ӳ���</param>
        /// <param name="msg">���յ�����Ϣ����</param>
        /// <param name="msgLength">��Ϣ���ȣ��ڱ���ͷ�����У�</param>
        public ASDU(ConnectionParameters parameters, byte[] msg, int msgLength)
        {
            //�������Ӳ���
            this.parameters = parameters.clone();

            //��������ͷ��68H+���ȣ�2��+68H + ��·������1�� + ��·��ַ��2��
            int bufPos = 7;

            typeId = (TypeID)msg[bufPos++];     //���ͱ�ʶ

            vsq = msg[bufPos++];                //�ɱ�ṹ�޶���

            this.hasTypeId = true;

            byte cotByte = msg[bufPos++];       //����ԭ��

            //���Ա�־--0 δ���飨false����1  ���飨true��
            if ((cotByte & 0x80) != 0)
                isTest = true;
            else
                isTest = false;

            //ȷ�ϱ�־��0  �϶�ȷ�ϣ�false�� ��1 ��ȷ�ϣ�true��
            if ((cotByte & 0x40) != 0)
                isNegative = true;
            else
                isNegative = false;

            //����ԭ��
            cot = (CauseOfTransmission)(cotByte & 0x3f);

            //�����������ն��豸��ַ(2�ֽڣ�
            ca = msg[bufPos++];
            if (parameters.SizeOfCA > 1)
                ca += (msg[bufPos++] * 0x100);

            //��¼��ַ
            ra = (RecordAddress)msg[bufPos++];     //���ͱ�ʶ

            int payloadSize = msgLength - bufPos - 2;//�����ĺ�У��ͽ�����ȥ��

            payload = new byte[payloadSize];

            /* save payload */
            Buffer.BlockCopy(msg, bufPos, payload, 0, payloadSize);
        }

        /// <summary>
        /// ��ASDU���ݱ�����frame�С���
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="parameters"></param>
        public void Encode(Frame frame, ConnectionParameters parameters)
        {
            //�ȱ������ͱ�ʶ
            frame.SetNextByte((byte)typeId);
            //Ȼ����ǿɱ�ṹ�޶���
            frame.SetNextByte(vsq);

            #region ����ԭ��
            byte cotByte = (byte)cot;

            //���Ա�־
            if (isTest)
                cotByte = (byte)(cotByte | 0x80);
            //ȷ�ϱ�־
            if (isNegative)
                cotByte = (byte)(cotByte | 0x40);

            frame.SetNextByte(cotByte);
            #endregion

            #region �����������ն��豸��ַ(2�ֽڣ�
            frame.SetNextByte((byte)(ca % 256));

            if (parameters.SizeOfCA > 1)
                frame.SetNextByte((byte)(ca / 256));
            #endregion

            //��¼��ַ
            frame.SetNextByte((byte)ra);

            //��������������
            if (payload != null)
            {
                //����������£�ֱ�Ӱ����ݸ��ȥ�ͺ���
                frame.AppendBytes(payload);
            }
            else
            {
                //�ǽ���������£�������д�ѡ�����
                bool isFirst = true;

                foreach (InformationObject io in informationObjects)
                {

                    if (isFirst)
                    {
                        //��һ������������ַ��ȥ��sequence�����������false�ͺ���
                        io.Encode(frame, parameters, false);
                        isFirst = false;
                    }
                    else
                    {
                        //����ģ������Ƿ�Ϊ�����������Ƿ�����ַ
                        if (IsSquence)
                            io.Encode(frame, parameters, true);
                        else
                            io.Encode(frame, parameters, false);
                    }

                }
            }

        }

        /// <summary>
        /// ���ͱ�ʶ
        /// </summary>
        public TypeID TypeId
        {
            get
            {
                return this.typeId;
            }
        }

        /// <summary>
        /// ����ԭ��
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
        /// �����־
        /// </summary>
        public bool IsTest
        {
            get
            {
                return this.isTest;
            }
        }

        /// <summary>
        /// ȷ�ϱ�־
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
        /// Ӧ�÷������ݵ�Ԫ������ַ��2���ֽڣ�
        /// </summary>
        public int Ca
        {
            get
            {
                return this.ca;
            }
        }

        /// <summary>
        /// ��¼��ַ
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
        /// ��ɢ�����������ڿɱ�ṹ�޶�����ĵĵ�һλ�б�ʶSQ
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
        /// ��Ϣ������Ŀ���ɱ�ṹ�޶��ʵĺ���λ
        /// </summary>
        public int NumberOfElements
        {
            get
            {
                return (vsq & 0x7f);
            }
        }

        /// <summary>
        /// ��ȡ��n����Ϣ����
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public InformationObject GetElement(int index)
        {
            InformationObject retVal = null;

            int elementSize;

            switch (typeId)
            {
                #region ���ݲ�ͬ�����ͱ�ʶ������ͬ�Ľ���
                case TypeID.M_SP_TA_2: /* 1 ��λң�� */
                    //1���ֽڵ�ַ+ 1���ֽ�����+ 7���ֽڵ�ʱ��
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

