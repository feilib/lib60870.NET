using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lib102
{
    /// <summary>
    /// 读带时标的单点信息 C_SP_NA_2(101)
    /// <para>传送原因：下行（6,8），上行（7,9,10,13,14,15）</para>
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
    /// <para>2. 终端设备需要镜像回答报文确认，并用类型标识1（单点信息）回答</para>
    /// </summary>
    public class ReadSinglePoint : InformationObject
    {
        /// <summary>
        /// TypeID.C_SP_NA_2 101;
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.C_SP_NA_2;
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
        /// 初始化报文
        /// </summary>
        public ReadSinglePoint()
            : base(0)
        {
        }

        /// <summary>
        /// 通过信息解析报文
        /// </summary>
        /// <param name="msg">信息体内容</param>
        /// <param name="startIndex">信息体首地址</param>
        public ReadSinglePoint(byte[] msg, int startIndex)
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
    /// 读选定时间范围内的带时标的单点信息 C_SP_NB_2(102)
    /// <para>传送原因：下行（6,8），上行（7,9,10,13,14,15）</para>
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
    /// <para>1. 应避免两个时间点数值相等</para>
    /// <para>2. 只有一个信息体（2个时间）</para>
    /// <para>3. 控制站发送的读指令，终端站应给予镜像报文后再报告信息</para>
    /// <para>4. 信息对象就是跟两个5字节时标，没有信息地址</para>
    /// </summary>
    public class ReadSinglePointWithTimeRange : InformationObject
    {
        /// <summary>
        /// TypeID.C_SP_NB_2 102;
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.C_SP_NB_2;
            }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public CP40Time2b BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public CP40Time2b EndTime { get; set; }

        /// <summary>
        /// 不支持连续，仅1个信息体
        /// </summary>
        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 使用开始、结束时间初始化报文
        /// </summary>
        /// <param name="begin">开始时间</param>
        /// <param name="end">结束时间</param>
        public ReadSinglePointWithTimeRange(CP40Time2b begin, CP40Time2b end)
            : base(0)
        {
            BeginTime = begin;
            EndTime = end;
        }

        /// <summary>
        /// 通过信息解析报文
        /// </summary>
        /// <param name="msg">信息体内容</param>
        /// <param name="startIndex">信息体首地址</param>
        public ReadSinglePointWithTimeRange(byte[] msg, int startIndex)
            : base(msg, startIndex, true)
        {
            //最后一个isSequence强制标true，相当于不执行base，
            //因为base要解析信息对象地址，这里没有信息对象地址。。直接解析内容就好了

            BeginTime = new CP40Time2b(msg, 0);
            EndTime = new CP40Time2b(msg, 5);
        }

        /// <summary>
        /// 将日期、各种编码，编码进frame
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="isSequence"></param>
        internal override void Encode(Frame frame, bool isSequence = false)
        {
            //不编码地址，直接编码内容
            frame.AppendBytes(BeginTime.GetEncodedValue());
            frame.AppendBytes(EndTime.GetEncodedValue());
        }
    }

}
