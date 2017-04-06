//------------------------------------
//这个文档里面保存的都是不需要信息体的报文
//当然了，你要非添加一个空的进去也不是不行。。
//主要还是当手册看把。这里。。
//
//
//------------------------------------


namespace lib102
{
    /// <summary>
    /// 读制造厂和产品规范 C_RD_NA_2(100)
    /// <para>传送原因：下行（请求5），上行（13：吴）</para>
    /// <para>  控制方向：请求（5）</para>
    /// <para>  监视方向：无所请求的数据（13），无所请求的应用服务数据单元（14）</para>
    /// <para>1. 不含信息体</para>
    /// <para>2. 如果有数据，用类型标识71（制造厂和产品规范）回答</para>
    /// <para>3. 如果没有相关信息，用镜像报文未出回答，修改传送原因</para>
    /// <para>4. 没有信息体</para>
    /// </summary>
    public class ReadManufacturerSpec:InformationObject
    {
        /// <summary>
        /// TypeID.C_RD_NA_2 100;
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.C_RD_NA_2;
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
        /// 使用时间初始化报文
        /// </summary>
        /// <param name="currentTime">报文体内的时间</param>
        public ReadManufacturerSpec()
            : base(0)
        {
        }

        /// <summary>
        /// 通过信息解析报文
        /// </summary>
        /// <param name="msg">信息体内容</param>
        /// <param name="startIndex">信息体首地址</param>
        public ReadManufacturerSpec(byte[] msg, int startIndex)
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


}
