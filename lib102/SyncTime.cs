using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lib102
{
    /// <summary>
    /// 系统时间同步报文  C_SYN_TA_2  128
    /// <para>传送原因：48 同步</para>
    /// <para>1. 单个信息体（只有一个7字节时标）</para>
    /// <para>2. 只用于主站给中断对时</para>
    /// <para>3. 固定长度</para>
    /// <para>4. 按说是需要镜像确认。。。</para>
    /// <para></para>
    /// </summary>
    public class SyncTime : CurrentTime
    {
        /// <summary>
        /// TypeID.C_SYN_TA_2 128;
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.C_SYN_TA_2;
            }
        }

        /// <summary>
        /// 使用时间初始化报文
        /// </summary>
        /// <param name="currentTime">报文体内的时间</param>
        public SyncTime(CP56Time2b currentTime)
            : base(currentTime)
        { }

        /// <summary>
        /// 通过信息解析报文
        /// </summary>
        /// <param name="msg">信息体内容</param>
        /// <param name="startIndex">信息体首地址</param>
        public SyncTime(byte[] msg, int startIndex)
            : base(msg, startIndex)
        { }


    }
}
