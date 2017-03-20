/*
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

namespace lib60870
{
    /// <summary>
    /// 品质描述词QDS
    /// </summary>
	public class QualityDescriptor
    {
        private byte encodedValue;

        /// <summary>
        /// 构造函数，创建一个默认的描述词，全部位都置零
        /// </summary>
        public QualityDescriptor()
        {
            this.encodedValue = 0;
        }

        /// <summary>
        /// 构造函数，解析现有的内容
        /// </summary>
        /// <param name="encodedValue"></param>
        public QualityDescriptor(byte encodedValue)
        {
            this.encodedValue = encodedValue;
        }

        /// <summary>
        /// 溢出标志，表示遥测值是否发成溢出，bit1
        /// </summary>
		public bool Overflow
        {
            get
            {
                if ((encodedValue & 0x01) != 0)
                    return true;
                else
                    return false;
            }

            set
            {
                if (value)
                    encodedValue |= 0x01;
                else
                    encodedValue &= 0xfe;
            }
        }

        /// <summary>
        /// 封锁标志，，表示遥测是否被当地封锁，bit5
        /// </summary>
        public bool Blocked
        {
            get
            {
                if ((encodedValue & 0x10) != 0)
                    return true;
                else
                    return false;
            }

            set
            {
                if (value)
                    encodedValue |= 0x10;
                else
                    encodedValue &= 0xef;
            }
        }

        /// <summary>
        /// 取代标志，表示该遥测是否被人工设置活被其他装置取代，bit6
        /// </summary>
        public bool Substituted
        {
            get
            {
                if ((encodedValue & 0x20) != 0)
                    return true;
                else
                    return false;
            }

            set
            {
                if (value)
                    encodedValue |= 0x20;
                else
                    encodedValue &= 0xdf;
            }
        }

        /// <summary>
        /// 刷新标志，false代表为当前值，true代表本次采样刷新失败，没采样成功，bit7
        /// </summary>
        public bool NonTopical
        {
            get
            {
                if ((encodedValue & 0x40) != 0)
                    return true;
                else
                    return false;
            }

            set
            {
                if (value)
                    encodedValue |= 0x40;
                else
                    encodedValue &= 0xbf;
            }
        }

        /// <summary>
        /// 有效值，false（0）代表有效，true（1）代表无效，bit8
        /// </summary>
        public bool Invalid
        {
            get
            {
                if ((encodedValue & 0x80) != 0)
                    return true;
                else
                    return false;
            }

            set
            {
                if (value)
                    encodedValue |= 0x80;
                else
                    encodedValue &= 0x7f;
            }
        }

        /// <summary>
        /// 获取，或者设置当前品质描述词
        /// </summary>
        public byte EncodedValue
        {
            get
            {
                return this.encodedValue;
            }
            set
            {
                encodedValue = value;
            }
        }

        public override string ToString()
        {
            return string.Format("[QualityDescriptor: Overflow={0}, Blocked={1}, Substituted={2}, NonTopical={3}, Invalid={4}]", Overflow, Blocked, Substituted, NonTopical, Invalid);
        }
    }

}

