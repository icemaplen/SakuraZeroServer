using SakuraZeroCommon.Core;

namespace SakuraZeroCommon.Protocol
{
    public class ProtocolBase
    {

        public virtual ERequestCode RequestCode
        {
            get;
            protected set;
        }

        public virtual EActionCode ActionCode
        {
            get;
            protected set;
        }

        public virtual EReturnCode ReturnCode
        {
            get;
            protected set;
        }

        // 解码器，解码readbuff中从start开始的length字节
        public virtual ProtocolBase Decode(byte[] readBuff, int start, int length)
        {
            return new ProtocolBase();
        }

        // 编码器
        public virtual byte[] Encode()
        {
            return new byte[] { };
        }

        /// <summary>
        /// 描述
        /// </summary>
        /// <returns></returns>
        public virtual string GetDesc()
        {
            return "";
        }
    }
}
