﻿using SakuraZeroCommon.Core;

namespace SakuraZeroCommon.Prorocal
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

        public ProtocolBase()
        {

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
        /// 一级协议名称，用于消息分发.
        /// </summary>
        /// <returns></returns>
        public virtual string GetRequestName()
        {
            return "";
        }

        public virtual ERequestCode GetRequestCode()
        {
            return ERequestCode.None;
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