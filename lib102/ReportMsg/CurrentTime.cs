using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lib102
{
    /// <summary>
    /// 电能累计量数据终端设备目前时间M_TI_TA_2
    /// <para>1. 传送原因：被请求</para>
    /// <para>2. 用于报告系统当前时间</para>
    /// <para>3. 没有“信息对象地址”</para>
    /// <para>4. 只有一个信息体(时间)</para>
    /// </summary>
    public class CurrentTime:InformationObject
    {
        /// <summary>
        /// TypeID.M_TI_TA_2 72;
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.M_TI_TA_2;
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

        /// <summary>
        /// 信息体，时间信息
        /// </summary>
        CP56Time2b CurrentValue { get; set; }

        /// <summary>
        /// 使用时间初始化报文
        /// </summary>
        /// <param name="currentTime">报文体内的时间</param>
        public CurrentTime(CP56Time2b currentTime)
            : base(0)
        {
            CurrentValue = currentTime;
        }

        /// <summary>
        /// 通过信息解析报文
        /// </summary>
        /// <param name="msg">信息体内容</param>
        /// <param name="startIndex">信息体首地址</param>
        public CurrentTime(byte[] msg, int startIndex)
            : base(msg, startIndex, true)
        {
            //最后一个isSequence强制标true，相当于不执行base，
            //因为base要解析信息对象地址，这里没有信息对象地址。。直接解析内容就好了

            CurrentValue = new CP56Time2b(msg, startIndex);
        }

        /// <summary>
        /// 将日期、各种编码，编码进frame
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="isSequence"></param>
        internal override void Encode(Frame frame, bool isSequence = false)
        {
            //不编码地址，直接编码内容
            frame.AppendBytes(CurrentValue.GetEncodedValue());
        }
    }
}
