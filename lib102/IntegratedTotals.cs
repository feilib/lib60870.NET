using System;

namespace lib102
{
    /// <summary>
    /// 记账（计费）电能累计量 4字节量
    /// <para>非连续</para>
    /// <para>信息对象地址+电能累计量+校核</para>
    /// <para>所有信息对象最后，增加一个5字节时标</para>
    /// </summary>
    public class IntegratedTotals : InformationObject
    {
        /// <summary>
        /// 类型标识M_IT_TA_2（2），记账电能累计量，每个量占4字节，表示范围：-99 999 999~+99 999 999
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.M_IT_TA_2;
            }
        }

        //不支持连续
        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        #region 专门用于子类通知基类，目前使用多少个字节的变量
        /// <summary>
        /// Value总共有4个字节
        /// </summary>
        protected int BytesOfValue;

        /// <summary>
        /// 在使用BytesOfValue之前调用一下这个，如果有变化的话，在子类中就已经更改了。。。
        /// </summary>
        protected virtual void UpdateBytesOfValue()
        {
            BytesOfValue = 4;
        }
        #endregion

        #region 用于子类通知基类，目前是否需要校核(类型标识8-13 不需要校核)
        protected bool HasCheckSum;
        /// <summary>
        /// 更新校核,类型标识8-13请覆写此函数，并将HasCheckSum置false即可
        /// </summary>
        protected virtual void UpdateHasCheckSum()
        {
            HasCheckSum = true;
        }
        #endregion

        #region 电能累计量IT
        //电能累计量结构：
        //---------------------------------
        //         八位位组1
        //---------------------------------
        //         八位位组2
        //---------------------------------
        //         八位位组3
        //---------------------------------
        //         八位位组4
        //---------------------------------
        //  IV | CA | CY | 顺序号（bit0-5）
        //---------------------------------


        /// <summary>
        /// 电能累计量数据，量程（-99 999 999~  +99 999 999），单位kWh
        /// </summary>
        public int Value;
        /// <summary>
        /// （CY）进位位 
        /// <para>true（1）：累计时间段内计数器溢出</para>
        /// <para>false（0）：累计时间段内计数器未溢出</para>
        /// </summary>
        public bool Carry;
        /// <summary>
        /// （CA）计数器调整位
        /// <para>true（1）：累计时间段内计数器被调整</para>
        /// <para>false（0）：累计时间段内计数器未被调整</para>
        /// </summary>
        public bool CounterAdjusted;
        /// <summary>
        /// （IV）无效位
        /// <para>true（1）：计数器读数无效</para>
        /// <para>false（0）：计数器读数有效</para>
        /// </summary>
        public bool Invalid;
        protected int serialNo;
        /// <summary>
        /// 顺序号,取值范围0-31
        /// <para>当电能累计量数据终端设备复位时，顺序号复位为0，一个累计时段改变时，顺序号加1</para>
        /// </summary>
        public int SerialNo
        {
            get
            {
                return serialNo;
            }
            set
            {
                serialNo = value;
                if (value < 0) serialNo = 0;
                if (value > 31) serialNo = 31;
            }
        }
        #endregion

        /// <summary>
        /// 电能累计量的校核(仅用于类型标识2-7)
        /// </summary>
        protected byte checksum;

        /// <summary>
        /// 使用基本信息创建
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
        /// 使用接收到的数据解析出来一个对象
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        /// <param name="isSquence"></param>
        internal IntegratedTotals(byte[] msg, int startIndex, bool isSquence) :
            base(msg, startIndex, isSquence)
        {
            UpdateBytesOfValue();
            UpdateHasCheckSum();
            if (!isSquence)
                startIndex++; /* skip IOA */
            //4字节的数据
            Value = msg[startIndex++];
            Value += msg[startIndex++] * 0x100;
            if (BytesOfValue > 2)
                Value += msg[startIndex++] * 0x10000;
            if (BytesOfValue > 3)
                Value += msg[startIndex++] * 0x1000000;

            //下一字节的信息数据
            byte tmp = msg[startIndex++];
            Invalid = ((tmp & 0x80) != 0);
            CounterAdjusted = ((tmp & 0x40) != 0);
            Carry = ((tmp & 0x20) != 0);
            SerialNo = tmp * 0x1F;
            //校核
            if (HasCheckSum)
            {
                checksum = msg[startIndex++];

                #region 计算校核
                byte checksum1 = 0;
                checksum1 += ((byte)(Value & 0x00FF));
                checksum1 += ((byte)((Value & 0x00FF00) >> 8));
                if (BytesOfValue > 2)
                    checksum1 += ((byte)((Value & 0x00FF0000) >> 16));
                if (BytesOfValue > 3)
                    checksum1 += ((byte)((Value & 0x00FF000000) >> 24));
                checksum1 += tmp;

                if (checksum != checksum1)
                {
                    Console.WriteLine("校核检验错误");
                }
                #endregion
            }
        }

        /// <summary>
        /// 将信息编码进入frame
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="isSequence"></param>
        internal override void Encode(Frame frame, bool isSequence)
        {
            base.Encode(frame, isSequence);

            UpdateBytesOfValue();
            UpdateHasCheckSum();
            //4字节的数据
            frame.SetNextByte((byte)(Value & 0x00FF));
            frame.SetNextByte((byte)((Value & 0x00FF00) >> 8));
            if (BytesOfValue > 2)
                frame.SetNextByte((byte)((Value & 0x00FF0000) >> 16));
            if (BytesOfValue > 3)
                frame.SetNextByte((byte)((Value & 0x00FF000000) >> 24));

            //下一字节
            byte tmp = 0;
            if (Invalid) tmp |= 0x80;
            if (CounterAdjusted) tmp |= 0x40;
            if (Carry) tmp |= 0x20;
            tmp |= (byte)(serialNo & 0x1F);
            frame.SetNextByte(tmp);

            //校核
            if (HasCheckSum)
            {
                checksum = 0;
                #region 计算校核
                checksum += ((byte)(Value & 0x00FF));
                checksum += ((byte)((Value & 0x00FF00) >> 8));
                if (BytesOfValue > 2)
                    checksum += ((byte)((Value & 0x00FF0000) >> 16));
                if (BytesOfValue > 3)
                    checksum += ((byte)((Value & 0x00FF000000) >> 24));
                checksum += tmp;
                #endregion
                frame.SetNextByte(checksum);
            }
        }
    }

    /// <summary>
    /// 记账（计费）电能累计量  3字节量
    /// <para>非连续</para>
    /// <para>信息对象地址+电能累计量+校核</para>
    /// <para>所有信息对象最后，增加一个5字节时标</para>
    /// </summary>
    public class IntegratedTotalsWith3Byte : IntegratedTotals
    {
        /// <summary>
        /// 类型标识M_IT_TB_2（3），记账电能累计量，每个量占3字节，表示范围：-999 999~+999 999
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.M_IT_TB_2;
            }
        }

        //不支持连续
        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 覆写这个函数，变更变量字节数为3
        /// </summary>
        protected override void UpdateBytesOfValue()
        {
            BytesOfValue = 3;
        }

        /// <summary>
        /// 使用基本信息创建
        /// </summary>
        /// <param name="ioa"></param>
        /// <param name="val"></param>
        /// <param name="carry"></param>
        /// <param name="counterAdj"></param>
        /// <param name="invalid"></param>
        /// <param name="sn"></param>
        public IntegratedTotalsWith3Byte(int ioa, int val, bool carry, bool counterAdj, bool invalid, int sn)
            : base(ioa, val, carry, counterAdj, invalid, sn)
        {
        }

        /// <summary>
        /// 使用接收到的数据解析出来一个对象
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        /// <param name="isSquence"></param>
        internal IntegratedTotalsWith3Byte(byte[] msg, int startIndex, bool isSquence) :
            base(msg, startIndex, isSquence)
        {
        }

    }


    /// <summary>
    /// 记账（计费）电能累计量  2字节量
    /// <para>非连续</para>
    /// <para>信息对象地址+电能累计量+校核</para>
    /// <para>所有信息对象最后，增加一个5字节时标</para>
    /// </summary>
    public class IntegratedTotalsWith2Byte : IntegratedTotals
    {
        /// <summary>
        /// 类型标识M_IT_TC_2（4），记账电能累计量，每个量占2字节，表示范围：-999~+999
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.M_IT_TC_2;
            }
        }

        //不支持连续
        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 覆写这个函数，变更变量字节数为2
        /// </summary>
        protected override void UpdateBytesOfValue()
        {
            BytesOfValue = 2;
        }

        /// <summary>
        /// 使用基本信息创建
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
        /// 使用接收到的数据解析出来一个对象
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        /// <param name="isSquence"></param>
        internal IntegratedTotalsWith2Byte(byte[] msg, int startIndex, bool isSquence) :
            base(msg, startIndex, isSquence)
        {
        }

    }
}

