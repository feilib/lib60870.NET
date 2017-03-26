/*
 *  CP56Time2a.cs
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
    /// 7�ֽ�ʱ�ֱ꣬��ת��Ϊdatetime
    /// <para>102�е�ʱ��Ƚ����⣬���߰�����ֶΣ�ע������</para>
    /// </summary>
	public class CP56Time2b
    {
        private byte[] encodedValue = new byte[7];

        internal CP56Time2b(byte[] msg, int startIndex)
        {
            if (msg.Length < startIndex + 7)
                throw new ASDUParsingException("Message too small for parsing TimeInfoB");

            for (int i = 0; i < 7; i++)
                encodedValue[i] = msg[startIndex + i];
        }

        public CP56Time2b(DateTime time)
        {
            Millisecond = time.Millisecond;
            Second = time.Second;
            Year = time.Year % 100;
            Console.WriteLine("Year: " + time.Year + " " + Year);

            Month = time.Month;
            DayOfMonth = time.Day;
            Hour = time.Hour;
            Minute = time.Minute;
        }

        public CP56Time2b()
        {
            for (int i = 0; i < 7; i++)
                encodedValue[i] = 0;
        }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <returns>The date time.</returns>
        /// <param name="startYear">Start year.</param>
        public DateTime GetDateTime(int startYear)
        {
            int baseYear = (startYear / 100) * 100;

            if (this.Year < (startYear % 100))
                baseYear += 100;

            DateTime value = new DateTime(baseYear + this.Year, this.Month, this.DayOfMonth, this.Hour, this.Minute, this.Second, this.Millisecond);

            return value;
        }

        public DateTime GetDateTime()
        {
            return GetDateTime(1970);
        }


        /// <summary>
        /// Gets or sets the millisecond part of the time value
        /// </summary>
        /// <value>The millisecond.</value>
        public int Millisecond
        {
            get
            {
                return (encodedValue[0] + ((encodedValue[1] & 0x03) * 0x100)) % 1000;
            }

            set
            {
                int millies = (Second * 1000) + value;

                encodedValue[0] = (byte)(millies & 0xff);
                encodedValue[1] = (byte)((encodedValue[1] & 0xFC) | ((millies / 0x100) & 0x03));
            }
        }

        /// <summary>
        /// Gets or sets the second (range 0 to 59)
        /// </summary>
        /// <value>The second.</value>
        public int Second
        {
            get
            {
                return ((encodedValue[1] >> 2) & 0x3F);
            }

            set
            {

                encodedValue[1] = (byte)((encodedValue[1] & 0x03) | (((value) << 2) & 0xFC));
            }
        }

        /// <summary>
        /// Gets or sets the minute (range 0 to 59)
        /// </summary>
        /// <value>The minute.</value>
        public int Minute
        {
            get
            {
                return (encodedValue[2] & 0x3f);
            }

            set
            {
                encodedValue[2] = (byte)((encodedValue[2] & 0xc0) | (value & 0x3f));
            }
        }

        /// <summary>
        /// Gets or sets the hour (range 0 to 23)
        /// </summary>
        /// <value>The hour.</value>
        public int Hour
        {
            get
            {
                return (encodedValue[3] & 0x1f);
            }

            set
            {
                encodedValue[3] = (byte)((encodedValue[3] & 0xe0) | (value & 0x1f));
            }
        }

        /// <summary>
        /// Gets or sets the day of week in range from 1 (Monday) until 7 (Sunday)
        /// </summary>
        /// <value>The day of week.</value>
        public int DayOfWeek
        {
            get
            {
                return ((encodedValue[4] & 0xe0) >> 5);
            }

            set
            {
                encodedValue[4] = (byte)((encodedValue[4] & 0x1f) | ((value & 0x07) << 5));
            }
        }

        /// <summary>
        /// Gets or sets the day of month in range 1 to 31.
        /// </summary>
        /// <value>The day of month.</value>
        public int DayOfMonth
        {
            get
            {
                return (encodedValue[4] & 0x1f);
            }

            set
            {
                encodedValue[4] = (byte)((encodedValue[4] & 0xe0) + (value & 0x1f));
            }
        }

        /// <summary>
        /// Gets the month in range from 1 (January) to 12 (December)
        /// </summary>
        /// <value>The month.</value>
        public int Month
        {
            get
            {
                return (encodedValue[5] & 0x0f);
            }

            set
            {
                encodedValue[5] = (byte)((encodedValue[5] & 0xf0) + (value & 0x0f));
            }
        }

        /// <summary>
        /// Gets the year in the range 0 to 99
        /// </summary>
        /// <value>The year.</value>
        public int Year
        {
            get
            {
                return (encodedValue[6] & 0x7f);
            }

            set
            {
                encodedValue[6] = (byte)((encodedValue[6] & 0x80) + (value & 0x7f));
            }
        }

        /// <summary>
        /// �Ƿ�����ʱ��SU��
        /// </summary>
        public bool SummerTime
        {
            get
            {
                return ((encodedValue[3] & 0x80) != 0);
            }

            set
            {
                if (value)
                    encodedValue[3] |= 0x80;
                else
                    encodedValue[3] &= 0x7f;
            }
        }

        /// <summary>
        /// (IV)Gets a value indicating whether this <see cref="lib60870.CP56Time2a"/> is invalid.
        /// <para>ʱ���Ƿ���Ч��true(1) ��Ч��false(0) ��Ч</para>
        /// </summary>
        /// <value><c>true</c> if invalid; otherwise, <c>false</c>.</value>
        public bool Invalid
        {
            get
            {
                return ((encodedValue[2] & 0x80) != 0);
            }

            set
            {
                if (value)
                    encodedValue[2] |= 0x80;
                else
                    encodedValue[2] &= 0x7f;
            }
        }

        /// <summary>
        /// (TIS)������Ϣ����
        /// <para>false(0) ���ʳ����ض�(OFF), true(1) ���ʳ�������(ON)</para>
        /// </summary>
        /// <value><c>true</c> if substitued; otherwise, <c>false</c>.</value>
        public bool TarifInformation
        {
            get
            {
                return ((encodedValue[2] & 0x40) == 0x40);
            }

            set
            {
                if (value)
                    encodedValue[2] |= 0x40;
                else
                    encodedValue[2] &= 0xbf;
            }
        }

        /// <summary>
        /// (PTF)���ʷ�����Ϣ-Power����
        /// <para>ȡֵ��Χ0-3,�ֱ���������1-4</para>
        /// </summary>
        public int PowerTarifInformation
        {
            get
            {
                return ((encodedValue[5] >> 6) & 0x03);
            }

            set
            {

                encodedValue[5] = (byte)((encodedValue[5] & 0x3f) | (((value & 0x03) << 6) & 0xC0));
            }
        }

        /// <summary>
        /// (ETI)����������Ϣ-Energy ��Ϣ
        /// <para>ȡֵ��Χ1-4</para>
        /// </summary>
        /// <value><c>true</c> if substitued; otherwise, <c>false</c>.</value>
        public int EnergyTarifInformation
        {
            get
            {
                return ((encodedValue[5] >> 4) & 0x03);
            }

            set
            {
                encodedValue[5] = (byte)((encodedValue[5] & 0xCF) | (((value & 0x03) << 4) & 0x30));
            }
        }



        public byte[] GetEncodedValue()
        {
            return encodedValue;
        }

        public override string ToString()
        {
            return string.Format("[CP56Time2a: Millisecond={0}, Second={1}, Minute={2}, Hour={3}, DayOfWeek={4}, DayOfMonth={5}, Month={6}, Year={7}, SummerTime={8}, Invalid={9} TarifInfo={10}  EnergyTarif={11}  PowerTarif={12}]", Millisecond, Second, Minute, Hour, DayOfWeek, DayOfMonth, Month, Year, SummerTime, Invalid, TarifInformation, EnergyTarifInformation, PowerTarifInformation);
        }

    }

}

