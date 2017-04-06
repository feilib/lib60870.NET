using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lib102
{
    /// <summary>
    /// 初始化结束 （M_EI_NA_2，70，0x46)
    /// <para>报文长度固定，1</para>
    /// <para>只有一个传送原因：初始化</para>
    /// <para>信息地址，记录地址都是0</para>
    /// </summary>
    public class EndOfInit : InformationObject
    {
        /// <summary>
        /// 类型标识M_EI_NA_2（70），初始化结束
        /// </summary>
        public override TypeID Type
        {
            get
            {
                return TypeID.M_EI_NA_2;
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

        //初始化原因：
        // |bit8 | bit7-bit1 |
        // | bs1 | couse

        /// <summary>
        /// 初始化原因（bit1-7）
        /// </summary>
        public CauseOfInit Cause { get; set; }


        /// <summary>
        /// 参数变化标志BS1(初始化原因的最高位bit8)
        /// <para>BS1=0(false)： 初始化过程中 当地参数未被改变</para>
        /// <para>BS1=1(true)： 初始化过程中 当地参数被改变</para>
        /// </summary>
        public bool Bs1 { get; set; }

        /// <summary>
        /// 构造函数，通过信息内容（主要解析信息体地址）
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        /// <param name="isSequence"></param>
        public EndOfInit(byte[] msg, int startIndex, bool isSequence)
            : base(msg, startIndex, isSequence)
        {
            if (!isSequence)
                startIndex++; /* skip IOA */
            //传送原因
            byte tmp = msg[startIndex++];
            switch (tmp & 0xEF)
            {
                default:
                case 0:
                    Cause = CauseOfInit.LocalPowerOn;
                    break;
                case 1:
                    Cause = CauseOfInit.LocalManualReset;
                    break;
                case 2:
                    Cause = CauseOfInit.RemoteReset;
                    break;
            }

            //bs1
            Bs1 = ((tmp & 0x80) > 0);

        }

        /// <summary>
        /// 使用信息体地址初始化信息体对象
        /// </summary>
        /// <param name="objectAddress"> 信息对象地址默认为0</param>
        public EndOfInit(CauseOfInit cause, bool bs, int objectAddress = 0)
            : base(0)
        {
            this.Cause = cause;
            this.Bs1 = bs;
        }

        /// <summary>
        /// 将信息编码进入frame
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="isSequence"></param>
        internal override void Encode(Frame frame, bool isSequence)
        {
            //先编码地址
            base.Encode(frame, isSequence);

            //初始化原因
            byte tmp = (byte)Cause;
            if (Bs1) tmp |= 0x80;
            frame.SetNextByte(tmp);

        }
    }

    /// <summary>
    /// 初始化原因
    /// </summary>
    public enum CauseOfInit
    {
        /// <summary>
        /// 当地电源合上
        /// </summary>
        LocalPowerOn = 0,
        /// <summary>
        /// 当地手动复位
        /// </summary>
        LocalManualReset = 1,
        /// <summary>
        /// 远方复位
        /// </summary>
        RemoteReset = 2
    }
}
