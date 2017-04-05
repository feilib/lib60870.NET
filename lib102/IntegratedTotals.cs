using System;

namespace lib102
{
    /// <summary>
    /// ���ˣ��Ʒѣ������ۼ��� 4�ֽ���
    /// <para>������</para>
    /// <para>��Ϣ�����ַ+�����ۼ���+У��</para>
    /// <para>������Ϣ�����������һ��5�ֽ�ʱ��</para>
    /// </summary>
    public class IntegratedTotals : InformationObject
    {
        /// <summary>
        /// ���ͱ�ʶM_IT_TA_2��2�������˵����ۼ�����ÿ����ռ4�ֽڣ���ʾ��Χ��-99 999 999~+99 999 999
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.M_IT_TA_2;
            }
        }

        //��֧������
        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        #region �����ۼ���IT
        //�����ۼ����ṹ��
        //---------------------------------
        //         ��λλ��1
        //---------------------------------
        //         ��λλ��2
        //---------------------------------
        //         ��λλ��3
        //---------------------------------
        //         ��λλ��4
        //---------------------------------
        //  IV | CA | CY | ˳��ţ�bit0-5��
        //---------------------------------


        /// <summary>
        /// �����ۼ������ݣ����̣�-99 999 999~  +99 999 999������λkWh
        /// </summary>
        public int Value;
        /// <summary>
        /// ��CY����λλ 
        /// <para>true��1�����ۼ�ʱ����ڼ��������</para>
        /// <para>false��0�����ۼ�ʱ����ڼ�����δ���</para>
        /// </summary>
        public bool Carry;
        /// <summary>
        /// ��CA������������λ
        /// <para>true��1�����ۼ�ʱ����ڼ�����������</para>
        /// <para>false��0�����ۼ�ʱ����ڼ�����δ������</para>
        /// </summary>
        public bool CounterAdjusted;
        /// <summary>
        /// ��IV����Чλ
        /// <para>true��1����������������Ч</para>
        /// <para>false��0����������������Ч</para>
        /// </summary>
        public bool Invalid;
        protected int serialNo;
        /// <summary>
        /// ˳���,ȡֵ��Χ0-31
        /// <para>�������ۼ��������ն��豸��λʱ��˳��Ÿ�λΪ0��һ���ۼ�ʱ�θı�ʱ��˳��ż�1</para>
        /// </summary>
        public int SerialNo
        {
            get
            {
                return serialNo;
            }
            set
            {
                if (value < 0) serialNo = 0;
                if (value > 31) serialNo = 31;
            }
        }
        #endregion

        /// <summary>
        /// �����ۼ�����У��(���������ͱ�ʶ2-7)
        /// </summary>
        protected byte checksum;

        /// <summary>
        /// ʹ�û�����Ϣ����
        /// </summary>
        /// <param name="ioa"></param>
        /// <param name="val"></param>
        /// <param name="carry"></param>
        /// <param name="counterAdj"></param>
        /// <param name="invalid"></param>
        /// <param name="sn"></param>
        public IntegratedTotals(int ioa, int val, bool carry, bool counterAdj, bool invalid, int sn)
            : base(ioa)
        {
            this.Value = val;
            this.Carry = carry;
            this.CounterAdjusted = counterAdj;
            this.Invalid = invalid;
            this.SerialNo = sn;
        }

        /// <summary>
        /// ʹ�ý��յ������ݽ�������һ������
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        /// <param name="isSquence"></param>
        internal IntegratedTotals(byte[] msg, int startIndex, bool isSquence) :
            base(msg, startIndex, isSquence)
        {
            if (!isSquence)
                startIndex++; /* skip IOA */
            //4�ֽڵ�����
            Value = msg[startIndex++];
            Value += msg[startIndex++] * 0x100;
            Value += msg[startIndex++] * 0x10000;
            Value += msg[startIndex++] * 0x1000000;

            //�����ֽڵ���Ϣ����
            byte tmp = msg[startIndex++];
            Invalid = ((tmp & 0x80) != 0);
            CounterAdjusted = ((tmp & 0x40) != 0);
            Carry = ((tmp & 0x20) != 0);
            SerialNo = tmp * 0x1F;
            //У��
            checksum = msg[startIndex++];

            #region ����У��
            byte checksum1 = 0;
            checksum1 += ((byte)(Value & 0x00FF));
            checksum1 += ((byte)((Value & 0x00FF00) >> 2));
            checksum1 += ((byte)((Value & 0x00FF0000) >> 4));
            checksum1 += ((byte)((Value & 0x00FF000000) >> 6));
            checksum1 += tmp;

            if(checksum != checksum1)
            {
                Console.WriteLine("У�˼������");
            }
            #endregion
        }

        /// <summary>
        /// ����Ϣ�������frame
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="isSequence"></param>
        internal override void Encode(Frame frame, bool isSequence)
        {
            base.Encode(frame, isSequence);
            //4�ֽڵ�����
            frame.SetNextByte((byte)(Value & 0x00FF));
            frame.SetNextByte((byte)((Value & 0x00FF00) >> 2));
            frame.SetNextByte((byte)((Value & 0x00FF0000) >> 4));
            frame.SetNextByte((byte)((Value & 0x00FF000000) >> 6));

            //�����ֽ�
            byte tmp = 0;
            if (Invalid) tmp |= 0x80;
            if (CounterAdjusted) tmp |= 0x40;
            if (Carry) tmp |= 0x20;
            tmp |= (byte)(serialNo & 0x1F);
            frame.SetNextByte(tmp);

            checksum = 0;
            #region ����У��
            checksum+=((byte)(Value & 0x00FF));
            checksum += ((byte)((Value & 0x00FF00) >> 2));
            checksum += ((byte)((Value & 0x00FF0000) >> 4));
            checksum += ((byte)((Value & 0x00FF000000) >> 6));
            checksum += tmp;
            #endregion
            frame.SetNextByte(tmp);
        }
    }

    /// <summary>
    /// ���ˣ��Ʒѣ������ۼ���  3�ֽ���
    /// <para>������</para>
    /// <para>��Ϣ�����ַ+�����ۼ���+У��</para>
    /// <para>������Ϣ�����������һ��5�ֽ�ʱ��</para>
    /// </summary>
    public class IntegratedTotalsWith3Byte:IntegratedTotals
    {
        /// <summary>
        /// ���ͱ�ʶM_IT_TB_2��3�������˵����ۼ�����ÿ����ռ3�ֽڣ���ʾ��Χ��-999 999~+999 999
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.M_IT_TB_2;
            }
        }

        //��֧������
        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// ʹ�û�����Ϣ����
        /// </summary>
        /// <param name="ioa"></param>
        /// <param name="val"></param>
        /// <param name="carry"></param>
        /// <param name="counterAdj"></param>
        /// <param name="invalid"></param>
        /// <param name="sn"></param>
        public IntegratedTotalsWith3Byte(int ioa, int val, bool carry, bool counterAdj, bool invalid, int sn)
            : base(ioa,val,carry,counterAdj,invalid,sn)
        {
        }

        /// <summary>
        /// ʹ�ý��յ������ݽ�������һ������
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        /// <param name="isSquence"></param>
        internal IntegratedTotalsWith3Byte(byte[] msg, int startIndex, bool isSquence) :
            base(msg, startIndex, isSquence)
        {
            if (!isSquence)
                startIndex++; /* skip IOA */
            //3�ֽڵ�����
            Value = msg[startIndex++];
            Value += msg[startIndex++] * 0x100;
            Value += msg[startIndex++] * 0x10000;


            //��4�ֽڵ���Ϣ����
            byte tmp = msg[startIndex++];
            Invalid = ((tmp & 0x80) != 0);
            CounterAdjusted = ((tmp & 0x40) != 0);
            Carry = ((tmp & 0x20) != 0);
            SerialNo = tmp * 0x1F;
            //У��
            checksum = msg[startIndex++];

            #region ����У��
            byte checksum1 = 0;
            checksum1 += ((byte)(Value & 0x00FF));
            checksum1 += ((byte)((Value & 0x00FF00) >> 2));
            checksum1 += ((byte)((Value & 0x00FF0000) >> 4));
            checksum1 += tmp;

            if (checksum != checksum1)
            {
                Console.WriteLine("У�˼������");
            }
            #endregion
        }

        /// <summary>
        /// ����Ϣ�������frame
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="isSequence"></param>
        internal override void Encode(Frame frame, bool isSequence)
        {
            base.Encode(frame, isSequence);
            //3�ֽڵ�����
            frame.SetNextByte((byte)(Value & 0x00FF));
            frame.SetNextByte((byte)((Value & 0x00FF00) >> 2));
            frame.SetNextByte((byte)((Value & 0x00FF0000) >> 4));

            //��4�ֽ�
            byte tmp = 0;
            if (Invalid) tmp |= 0x80;
            if (CounterAdjusted) tmp |= 0x40;
            if (Carry) tmp |= 0x20;
            tmp |= (byte)(serialNo & 0x1F);
            frame.SetNextByte(tmp);

            checksum = 0;
            #region ����У��
            checksum += ((byte)(Value & 0x00FF));
            checksum += ((byte)((Value & 0x00FF00) >> 2));
            checksum += ((byte)((Value & 0x00FF0000) >> 4));
            checksum += tmp;
            #endregion
            frame.SetNextByte(tmp);
        }

    }


    /// <summary>
    /// ���ˣ��Ʒѣ������ۼ���  2�ֽ���
    /// <para>������</para>
    /// <para>��Ϣ�����ַ+�����ۼ���+У��</para>
    /// <para>������Ϣ�����������һ��5�ֽ�ʱ��</para>
    /// </summary>
    public class IntegratedTotalsWith2Byte : IntegratedTotals
    {
        /// <summary>
        /// ���ͱ�ʶM_IT_TC_2��4�������˵����ۼ�����ÿ����ռ2�ֽڣ���ʾ��Χ��-999~+999
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.M_IT_TC_2;
            }
        }

        //��֧������
        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// ʹ�û�����Ϣ����
        /// </summary>
        /// <param name="ioa"></param>
        /// <param name="val"></param>
        /// <param name="carry"></param>
        /// <param name="counterAdj"></param>
        /// <param name="invalid"></param>
        /// <param name="sn"></param>
        public IntegratedTotalsWith2Byte(int ioa, int val, bool carry, bool counterAdj, bool invalid, int sn)
            : base(ioa, val, carry, counterAdj, invalid, sn)
        {
        }

        /// <summary>
        /// ʹ�ý��յ������ݽ�������һ������
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        /// <param name="isSquence"></param>
        internal IntegratedTotalsWith2Byte(byte[] msg, int startIndex, bool isSquence) :
            base(msg, startIndex, isSquence)
        {
            if (!isSquence)
                startIndex++; /* skip IOA */
            //2�ֽڵ�����
            Value = msg[startIndex++];
            Value += msg[startIndex++] * 0x100;


            //��3�ֽڵ���Ϣ����
            byte tmp = msg[startIndex++];
            Invalid = ((tmp & 0x80) != 0);
            CounterAdjusted = ((tmp & 0x40) != 0);
            Carry = ((tmp & 0x20) != 0);
            SerialNo = tmp * 0x1F;
            //У��
            checksum = msg[startIndex++];

            #region ����У��
            byte checksum1 = 0;
            checksum1 += ((byte)(Value & 0x00FF));
            checksum1 += ((byte)((Value & 0x00FF00) >> 2));
            checksum1 += tmp;

            if (checksum != checksum1)
            {
                Console.WriteLine("У�˼������");
            }
            #endregion
        }

        /// <summary>
        /// ����Ϣ�������frame
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="isSequence"></param>
        internal override void Encode(Frame frame, bool isSequence)
        {
            base.Encode(frame, isSequence);
            //2�ֽڵ�����
            frame.SetNextByte((byte)(Value & 0x00FF));
            frame.SetNextByte((byte)((Value & 0x00FF00) >> 2));

            //��3�ֽ�
            byte tmp = 0;
            if (Invalid) tmp |= 0x80;
            if (CounterAdjusted) tmp |= 0x40;
            if (Carry) tmp |= 0x20;
            tmp |= (byte)(serialNo & 0x1F);
            frame.SetNextByte(tmp);

            checksum = 0;
            #region ����У��
            checksum += ((byte)(Value & 0x00FF));
            checksum += ((byte)((Value & 0x00FF00) >> 2));
            checksum += tmp;
            #endregion
            frame.SetNextByte(tmp);
        }

    }
}

