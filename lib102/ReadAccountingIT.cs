using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lib102
{
    /// <summary>
    /// 读最早累计时段的记账(计费) 电能累计量  C_CI_NA_2 (104)
    /// <para>  控制方向： </para>
    /// <para>      （6）激活</para>
    /// <para>      （8）停止激活</para>
    /// <para>  监视方向： </para>
    /// <para>      （7）激活确认</para>
    /// <para>      （9）停止激活确认</para>
    /// <para>      （10）激活终止</para>
    /// <para>      （13）无所请求的数据记录</para>
    /// <para>      （14）无所请求的应用服务数据单元</para>
    /// <para>      （15）有主站（控制站）发送的应用服务数据单元中的记录地址不可知</para>
    /// <para>1. 不含信息体</para>
    /// <para>2. 如果有数据，先镜像报文回答，然后用累计量（类型2-4，记账电能累计量）回答报文</para>
    /// <para>3. 如果没有数据，用镜像报文回答，修改传送原因</para>
    /// </summary>
    public class ReadAccountingIT : InformationObject
    {
        /// <summary>
        /// TypeID.C_CI_NA_2 104;
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.C_CI_NA_2;
            }
        }

        /// <summary>
        /// 不支持连续，没有信息体
        /// </summary>
        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 初始化空报文
        /// </summary>
        public ReadAccountingIT()
            : base(0)
        {
        }

        /// <summary>
        /// 通过信息解析报文
        /// </summary>
        /// <param name="msg">信息体内容</param>
        /// <param name="startIndex">信息体首地址</param>
        public ReadAccountingIT(byte[] msg, int startIndex)
            : base(msg, startIndex, true)
        {
            //最后一个isSequence强制标true，相当于不执行base，
            //因为base要解析信息对象地址，这里没有信息对象地址。。直接解析内容就好了

        }

        /// <summary>
        /// 将日期、各种编码，编码进frame
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="isSequence"></param>
        internal override void Encode(Frame frame, bool isSequence = false)
        {
            //不编码地址，直接编码内容
        }
    }

    /// <summary>
    /// 读最早累计时段的和一个选定的地址范围的记账(计费)电能累计量 C_CI_NB_2 (105)
    /// <para>  控制方向： </para>
    /// <para>      （6）激活</para>
    /// <para>      （8）停止激活</para>
    /// <para>  监视方向： </para>
    /// <para>      （7）激活确认</para>
    /// <para>      （9）停止激活确认</para>
    /// <para>      （10）激活终止</para>
    /// <para>      （13）无所请求的数据记录</para>
    /// <para>      （14）无所请求的应用服务数据单元</para>
    /// <para>      （15）有主站（控制站）发送的应用服务数据单元中的记录地址不可知</para>
    /// <para>      （16）有主站（控制站）发送的应用服务数据单元中的地址规范不可知</para>
    /// <para>      （17）无所请求的信息体</para>
    /// <para>      （18）无所请求的累计时段</para>
    /// <para>1. 单个信息体（没有本身地址，由两个请求的信息地址组成）</para>
    /// <para>2. 如果有数据，先镜像报文回答，然后用累计量（类型2-4，记账电能累计量）回答报文</para>
    /// <para>3. 如果没有数据，用镜像报文回答，修改传送原因</para>
    /// </summary>
    public class ReadAccountingITWithAddressRange : InformationObject
    {
        /// <summary>
        /// TypeID.C_CI_NA_2 104;
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.C_CI_NB_2;
            }
        }

        /// <summary>
        /// 起始地址
        /// </summary>
        public byte BeginAddress { get; set; }
        /// <summary>
        /// 终止地址
        /// </summary>
        public byte EndAddress { get; set; }

        /// <summary>
        /// 不支持连续，没有信息体
        /// </summary>
        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 使用两个地址初始化报文
        /// </summary>
        /// <param name="begin">起始地址</param>
        /// <param name="end">终止地址</param>
        public ReadAccountingITWithAddressRange(byte begin,byte end)
            : base(0)
        {
            BeginAddress = begin;
            EndAddress = end;
        }

        /// <summary>
        /// 通过信息解析报文
        /// </summary>
        /// <param name="msg">信息体内容</param>
        /// <param name="startIndex">信息体首地址</param>
        public ReadAccountingITWithAddressRange(byte[] msg, int startIndex)
            : base(msg, startIndex, true)
        {
            //最后一个isSequence强制标true，相当于不执行base，
            //因为base要解析信息对象地址，这里没有信息对象地址。。直接解析内容就好了

            BeginAddress = msg[startIndex++];
            EndAddress = msg[startIndex++];
        }

        /// <summary>
        /// 将日期、各种编码，编码进frame
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="isSequence"></param>
        internal override void Encode(Frame frame, bool isSequence = false)
        {
            //不编码地址，直接编码内容
            frame.SetNextByte(BeginAddress);
            frame.SetNextByte(EndAddress);
        }
    }

}
