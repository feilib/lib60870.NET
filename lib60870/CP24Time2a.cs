/*
 *  CP24Time2a.cs
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

namespace lib60870
{
    /// <summary>
    /// 3�ֽ�ʱ�꣬����ʹ�ã�����һ���ֽڵķ��ӣ�2���ֽڵĺ��룬������Ҫmod1000��ȡʵ�ʺ���
    /// </summary>
    public class CP24Time2a
    {
        private byte[] encodedValue = new byte[3];

        internal CP24Time2a(byte[] msg, int startIndex)
        {
            if (msg.Length < startIndex + 3)
                throw new ASDUParsingException("Message too small for parsing CP24Time2a");

            for (int i = 0; i < 3; i++)
                encodedValue[i] = msg[startIndex + i];
        }

        /// <summary>
        /// Gets the totoal milliseconds of the elapsed time
        /// </summary>
        /// <returns>The milliseconds.</returns>
        public int GetMilliseconds()
        {

            int millies = Minute * (60000) + Second * 1000 + Millisecond;

            return millies;
        }

        /// <summary>
        /// Gets the millisecond part of the time value
        /// </summary>
        /// <value>The millisecond.</value>
        public int Millisecond
        {
            get
            {
                return (encodedValue[0] + (encodedValue[1] * 0x100)) % 1000;
            }
        }

        public int Second
        {
            get
            {
                return (encodedValue[0] + (encodedValue[1] * 0x100)) / 1000;
            }
        }

        public int Minute
        {
            get
            {
                return (encodedValue[2] & 0x3f);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="lib60870.CP24Time2a"/> is invalid.
        /// </summary>
        /// <value><c>true</c> if invalid; otherwise, <c>false</c>.</value>
        public bool Invalid
        {
            get
            {
                return ((encodedValue[2] & 0x80) == 0x80);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="lib60870.CP24Time2a"/> was substitued by an intermediate station
        /// </summary>
        /// <value><c>true</c> if substitued; otherwise, <c>false</c>.</value>
        public bool Substitued
        {
            get
            {
                return ((encodedValue[2] & 0x40) == 0x40);
            }
        }

        public byte[] GetEncodedValue()
        {
            return encodedValue;
        }


        public override string ToString()
        {
            return string.Format("[CP24Time2a: Millisecond={0}, Second={1}, Minute={2}, Invalid={3}, Substitued={4}]", Millisecond, Second, Minute, Invalid, Substitued);
        }

    }
}

