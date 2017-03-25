using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib102
{
    /// <summary>
    /// 链路功能码的基类，主要分上行和下行
    /// </summary>
    public class LinkControl
    {
        //值
        protected byte val;

        /// <summary>
        /// 启动方向-bit6(0~7)
        /// <para>true  启动站发送报文时置1（下行）</para>
        /// <para>false  从动站发送报文时置0（上行）</para>
        /// </summary>
        protected bool prm
        {
            get
            {
                return (val & 0x40) == 1;
            }
            set
            {
                if (value)
                    val = (byte)(val | 0x40);
                else
                    val = (byte)(val & 0xBF);
            }
        }

        public LinkControl()
        {
            val = 0;
        }

        /// <summary>
        /// 获取实际byte值
        /// </summary>
        /// <returns></returns>
        public byte getValue()
        {
            return val;
        }
    }

    /// <summary>
    /// 下行报文链路控制域（主控站→被控站）
    /// </summary>
    public class LinkControlDown : LinkControl
    {
        /// <summary>
        /// 帧计数位 bit5（0-7）
        /// <para>每次收到确认报文后，要翻转此标志</para>
        /// <para>被控站根据判断此标志是否翻转，来决定是否重发上一报文</para>
        /// <para>复位后，FCB置零(false)</para>
        /// </summary>
        public bool FCB
        {
            get
            {
                return (val & 0x20) == 1;
            }
            set
            {
                if (value)
                    val = (byte)(val | 0x20);
                else
                    val = (byte)(val & 0xDF);
            }
        }

        /// <summary>
        /// 帧计数位有效位 bit4（0-7）
        /// <para>true(1) 表示帧计数位FCB的变化有效</para>
        /// <para>false(0) 表示帧计数位FCB的变化无效</para>
        /// <para>复位后，FCV置零（false）</para>
        /// </summary>
        public bool FCV
        {
            get
            {
                return (val & 0x10) == 1;
            }
            set
            {
                if (value)
                    val = (byte)(val | 0x10);
                else
                    val = (byte)(val & 0xEF);
            }
        }

        /// <summary>
        /// 链路功能码 bit0-3
        /// </summary>
        public LinkFunctionCodeDown FuncCode
        {
            get
            {
                int fc = val & 0x0F;
                return (LinkFunctionCodeDown)fc;
            }
            set
            {
                val = (byte)((val & 0xF0) + (byte)value);
            }
        }

        public LinkControlDown()
            : base()
        {
            //设置方向
            prm = true;
            FCB = false;
            FCV = false;
            FuncCode = LinkFunctionCodeDown.LinkReset;
        }
    }

    /// <summary>
    /// 上行报文链路控制域（主控站←被控站）
    /// </summary>
    public class LinkControlUp : LinkControl
    {
        /// <summary>
        /// 要求访问位 bit5（0-7）
        /// <para>true  表示采集终端希望向控制站传输1级用户数据</para>
        /// </summary>
        public bool ACD
        {
            get
            {
                return (val & 0x20) == 1;
            }
            set
            {
                if (value)
                    val = (byte)(val | 0x20);
                else
                    val = (byte)(val & 0xDF);
            }
        }

        /// <summary>
        /// 数据流控制位 bit4（0-7）
        /// <para>true(1) 表示采集终端的缓冲区已满，无法接收新数据</para>
        /// <para>false(0) 表示采集终端可以接收数据</para>
        /// </summary>
        public bool DFC
        {
            get
            {
                return (val & 0x10) == 1;
            }
            set
            {
                if (value)
                    val = (byte)(val | 0x10);
                else
                    val = (byte)(val & 0xEF);
            }
        }

        /// <summary>
        /// 链路功能码 bit0-3
        /// </summary>
        public LinkFunctionCodeUp FuncCode
        {
            get
            {
                int fc = val & 0x0F;
                return (LinkFunctionCodeUp)fc;
            }
            set
            {
                val = (byte)((val & 0xF0) + (byte)value);
            }
        }

        public LinkControlUp()
            : base()
        {
            //设置方向
            prm = false;
            ACD = false;
            DFC = false;
            FuncCode = LinkFunctionCodeUp.LinkStatOrRequest;
        }
    }

    /// <summary>
    /// 链路功能码 下行(主站给从站发报文用）
    /// </summary>
    public enum LinkFunctionCodeDown
    {
        /// <summary>
        /// 远方链路复位   FCV=0
        /// <para>帧类型：发送/确认</para>
        /// </summary>
        LinkReset = 0,
        /// <summary>
        /// 传送数据   FCV=1
        /// <para>帧类型：发送/确认</para>
        /// </summary>
        UserData = 3,
        /// <summary>
        /// 召唤链路状态   FCV=0
        /// <para>帧类型：请求/响应</para>
        /// </summary>
        RequestLinkStats = 9,
        /// <summary>
        /// 召唤1级用户数据   FCV=1
        /// <para>帧类型：请求/响应</para>
        /// </summary>
        RequestUserData_1 = 10,
        /// <summary>
        /// 召唤2级用户数据   FCV=1
        /// <para>帧类型：请求/响应</para>
        /// </summary>
        RequestUserData_2 = 11,
    }

    /// <summary>
    /// 链路功能码 上行(从站给主站发报文用）
    /// </summary>
    public enum LinkFunctionCodeUp
    {
        /// <summary>
        /// 肯定认可
        /// <para>帧类型：确认</para>
        /// </summary>
        Positive = 0,
        /// <summary>
        /// 否定认可（没有收到报文）
        /// <para>帧类型：确认</para>
        /// </summary>
        Negative = 1,
        /// <summary>
        /// 用户数据（以数据回答请求帧）
        /// <para>帧类型：响应</para>
        /// </summary>
        UserData = 8,
        /// <summary>
        /// 没有所召唤的数据
        /// <para>帧类型：响应</para>
        /// </summary>
        NoData = 9,
        /// <summary>
        /// 以链路状态或访问请求回答请求帧
        /// <para>帧类型：响应</para>
        /// </summary>
        LinkStatOrRequest = 11,
    }
}
