using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lib102
{
    /// <summary>
    /// 读周期地复位的最早累计时段的运行电能累计量  C_CI_NN_2 (116)
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
    /// <para>2. 如果有数据，先镜像报文回答，然后用周期复位的运行电能累计量计量（类型5-7）回答报文</para>
    /// <para>3. 如果没有数据，用镜像报文回答，修改传送原因</para>
    /// </summary>
    public class ReadPeriodOIT : ReadAccountingIT
    {
        /// <summary>
        /// TypeID.C_CI_NN_2 116;
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.C_CI_NN_2;
            }
        }

        /// <summary>
        /// 初始化空报文
        /// </summary>
        public ReadPeriodOIT()
            : base()
        {
        }

        /// <summary>
        /// 通过信息解析报文
        /// </summary>
        /// <param name="msg">信息体内容</param>
        /// <param name="startIndex">信息体首地址</param>
        public ReadPeriodOIT(byte[] msg, int startIndex)
            : base(msg, startIndex)
        {
        }
    }

    /// <summary>
    /// 读周期地复位的最早累计时段和一个选定的地址范围的运行电能累计量 C_CI_NO_2 (117)
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
    /// <para>2. 如果有数据，先镜像报文回答，然后用周期复位的运行电能累计量计量（类型5-7）回答报文</para>
    /// <para>3. 如果没有数据，用镜像报文回答，修改传送原因</para>
    /// </summary>
    public class ReadPeriodOITWithAddressRange : ReadAccountingITWithAddressRange
    {
        /// <summary>
        /// TypeID.C_CI_NO_2 117;
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.C_CI_NO_2;
            }
        }

        /// <summary>
        /// 使用两个地址初始化报文
        /// </summary>
        /// <param name="begin">起始地址</param>
        /// <param name="end">终止地址</param>
        public ReadPeriodOITWithAddressRange(byte begin, byte end)
            : base(begin, end)
        {
        }

        /// <summary>
        /// 通过信息解析报文
        /// </summary>
        /// <param name="msg">信息体内容</param>
        /// <param name="startIndex">信息体首地址</param>
        public ReadPeriodOITWithAddressRange(byte[] msg, int startIndex)
            : base(msg, startIndex)
        {
        }
    }

    /// <summary>
    /// 读一个指定过去的累计时段的周期地复位的运行电能累计量  C_CI_NP_2 (118)
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
    /// <para>      （18）无所请求的累计时段</para>
    /// <para>1. 单个信息体（没有本身地址，由一个5字节时标构成）</para>
    /// <para>2. 如果有数据，先镜像报文回答，然后用周期复位的运行电能累计量计量（类型5-7）回答报文</para>
    /// <para>3. 如果没有数据，用镜像报文回答，修改传送原因</para>
    /// </summary>
    public class ReadPeriodOITWithSpecificTime : ReadAccountingITWithSpecificTime
    {
        /// <summary>
        /// TypeID.C_CI_NP_2 118;
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.C_CI_NP_2;
            }
        }

        /// <summary>
        /// 使用具体时标初始化报文
        /// </summary>
        /// <param name="time">指定时段</param>
        public ReadPeriodOITWithSpecificTime(CP40Time2b time)
            : base(time)
        { }

        /// <summary>
        /// 通过信息解析报文
        /// </summary>
        /// <param name="msg">信息体内容</param>
        /// <param name="startIndex">信息体首地址</param>
        public ReadPeriodOITWithSpecificTime(byte[] msg, int startIndex)
            : base(msg, startIndex)
        { }

    }

    /// <summary>
    /// 读一个指定的过去累计时段和一个选定的地址范围的周期地复位的运行电能累计量  C_CI_NQ_2 (119)
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
    /// <para>      （18）无所请求的累计时段</para>
    /// <para>1. 单个信息体（没有本身地址，由一个5字节时标构成）</para>
    /// <para>2. 如果有数据，先镜像报文回答，然后用周期复位的运行电能累计量计量（类型5-7）回答报文</para>
    /// <para>3. 如果没有数据，用镜像报文回答，修改传送原因</para>
    /// </summary>
    public class ReadPeriodOITWithSpecificTimeAndAddressRange : ReadAccountingITWithSpecificTimeAndAddressRange
    {
        /// <summary>
        /// TypeID.C_CI_NQ_2 119;
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.C_CI_NQ_2;
            }
        }



        /// <summary>
        /// 使用具体时标初始化报文
        /// </summary>
        /// <param name="begin">起始地址</param>
        /// <param name="end">终止地址</param>
        /// <param name="time">指定时段</param>
        public ReadPeriodOITWithSpecificTimeAndAddressRange(byte begin, byte end, CP40Time2b time)
            : base(begin, end, time)
        { }

        /// <summary>
        /// 通过信息解析报文
        /// </summary>
        /// <param name="msg">信息体内容</param>
        /// <param name="startIndex">信息体首地址</param>
        public ReadPeriodOITWithSpecificTimeAndAddressRange(byte[] msg, int startIndex)
            : base(msg, startIndex)
        { }

    }

    /// <summary>
    /// 读周期地复位的一个选定的时间范围和一个选定的地址范围的运行电能累计量 C_CI_NU_2 (123)
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
    /// <para>1. 单个信息体（没有本身地址，2个地址，2个5字节时标）</para>
    /// <para>2. 如果有数据，先镜像报文回答，然后用周期复位的运行电能累计量计量（类型5-7）回答报文</para>
    /// <para>3. 如果没有数据，用镜像报文回答，修改传送原因</para>
    /// </summary>
    public class ReadPeriodOITWithTimeRangeAndAddressRange : ReadAccountingITWithTimeRangeAndAddressRange
    {
        /// <summary>
        /// TypeID.C_CI_NU_2 123;
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.C_CI_NU_2;
            }
        }

        /// <summary>
        /// 使用具体时标初始化报文
        /// </summary>
        /// <param name="beginAdd">起始地址</param>
        /// <param name="endAdd">终止地址</param>
        /// <param name="beginTime">指定时段</param>
        /// <param name="endTime">指定时段</param>
        public ReadPeriodOITWithTimeRangeAndAddressRange(byte beginAdd, byte endAdd, CP40Time2b beginTime, CP40Time2b endTime)
            : base(beginAdd, endAdd, beginTime, endTime)
        { }

        /// <summary>
        /// 通过信息解析报文
        /// </summary>
        /// <param name="msg">信息体内容</param>
        /// <param name="startIndex">信息体首地址</param>
        public ReadPeriodOITWithTimeRangeAndAddressRange(byte[] msg, int startIndex)
            : base(msg, startIndex)
        { }


    }


}
