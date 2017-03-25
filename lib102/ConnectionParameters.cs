/*
 *  ConnectionParameters.cs
 *
 *  Copyright 2016 MZ Automation GmbH
 *
 *  This file is part of lib60870.NET
 *
 *  lib60870.NET is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  lib60870.NET is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with lib60870.NET.  If not, see <http://www.gnu.org/licenses/>.
 *
 *  See COPYING file for the complete license text.
 */

using System;

namespace lib102
{
    /// <summary>
    /// 这里主要是102规约中用到的应用参数
    /// </summary>
	public class ConnectionParameters
    {
        /// <summary>
        /// 链路地址：标明本站链路地址是多少
        /// </summary>
        private int linkAddress = 0;

        /// <summary>
        /// 电能量数据终端设备地址的字节数（ASDU 的 Address），默认2
        /// </summary>
        private int sizeOfCA = 2; /* (parameter a) CA = common address of ASDUs (1/2) */

        public ConnectionParameters()
        {
        }

        /// <summary>
        /// 复制一份，深度复制
        /// </summary>
        /// <returns></returns>
        public ConnectionParameters clone()
        {
            ConnectionParameters copy = new ConnectionParameters();

            copy.linkAddress = linkAddress;
            copy.sizeOfCA = sizeOfCA;

            return copy;
        }

        /// <summary>
        /// 链路地址：标明本站链路地址是多少
        /// </summary>
        public int LinkAddress
        {
            get
            {
                return this.linkAddress;
            }
            set
            {
                linkAddress = value;
            }
        }

        /// <summary>
        /// 电能量数据终端设备地址的字节数（ASDU 的 Address），默认2
        /// </summary>
        public int SizeOfCA
        {
            get
            {
                return this.sizeOfCA;
            }
            set
            {
                sizeOfCA = value;
            }
        }

    }
}

