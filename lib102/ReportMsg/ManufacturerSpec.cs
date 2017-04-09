using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lib102
{
    /// <summary>
    /// 电能累计量中断设备的制造厂和产品的规范 Manufacturer and product specification of integrated total 
    /// <para>P_MP_NA_2（71）</para>
    /// <para>1. 只有一个传送原因：初始化</para>
    /// <para>2. 只有一个信息体 包括标准日期，制造厂编码，产品编码</para>
    /// <para>3. 仅用于上行，报告产品信息</para>
    /// <para>4. 没有信息对象地址，注意使用，记录地址=0</para>
    /// </summary>
    public class ManufacturerSpec : InformationObject
    {
        /// <summary>
        /// TypeID.P_MP_NA_2 71;
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.P_MP_NA_2;
            }
        }

        /// <summary>
        /// 不支持连续，只有一个信息体
        /// </summary>
        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        #region 标准日期，第一字节，年月
        /// <summary>
        /// bit8-5(高4位）,取值范围0-9
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// bit1-4(第四位)，取值范围1-12
        /// </summary>
        public int Month { get; set; }
        #endregion
        /// <summary>
        /// 制造厂编码，第二字节
        /// </summary>
        public byte ManufacturerCode { get; set; }
        /// <summary>
        /// 产品编码，第3-6字节 共占用4个字节
        /// </summary>
        public int ProductCode { get; set; }

        /// <summary>
        /// 使用日期、编码初始化报文
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="manufacturerCode"></param>
        /// <param name="productCode"></param>
        public ManufacturerSpec(int year, int month, byte manufacturerCode, int productCode)
            : base(0)
        {
            Year = year;
            if (Year > 9) Year = 9;
            if (Year < 0) Year = 0;
            Month = month;
            if (Month > 12) Month = 12;
            if (Month < 1) Month = 1;
            ManufacturerCode = manufacturerCode;
            ProductCode = productCode;
        }

        /// <summary>
        /// 通过信息解析报文
        /// </summary>
        /// <param name="msg">信息体内容</param>
        /// <param name="startIndex">信息体首地址</param>
        public ManufacturerSpec(byte[] msg, int startIndex)
            : base(msg, startIndex, true)
        {
            //最后一个isSequence强制标true，相当于不执行base，
            //因为base要解析信息对象地址，这里没有信息对象地址。。直接解析内容就好了

            byte tmp = msg[startIndex++];
            Year = (tmp & 0xF0) >> 4;
            Month = tmp & 0x0F;
            ManufacturerCode = msg[startIndex++];
            ProductCode = msg[startIndex++];
            ProductCode += msg[startIndex++] * 0x100;
            ProductCode += msg[startIndex++] * 0x10000;
            ProductCode += msg[startIndex++] * 0x1000000;

        }

        /// <summary>
        /// 将日期、各种编码，编码进frame
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="isSequence"></param>
        internal override void Encode(Frame frame, bool isSequence = false)
        {
            //不编码地址，直接编码内容
            byte tmp = 0;
            tmp = (byte)(Month | ((Year << 4) & 0xF0));
            frame.SetNextByte(tmp);
            frame.SetNextByte(ManufacturerCode);
            frame.SetNextByte((byte)(ProductCode & 0x000000FF));
            frame.SetNextByte((byte)((ProductCode >> 8) & 0x000000FF));
            frame.SetNextByte((byte)((ProductCode >> 16) & 0x000000FF));
            frame.SetNextByte((byte)((ProductCode >> 24) & 0x000000FF));
        }
    }
}
