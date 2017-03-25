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
    /// ������Ҫ��102��Լ���õ���Ӧ�ò���
    /// </summary>
	public class ConnectionParameters
    {
        /// <summary>
        /// ��·��ַ��������վ��·��ַ�Ƕ���
        /// </summary>
        private int linkAddress = 0;

        /// <summary>
        /// �����������ն��豸��ַ���ֽ�����ASDU �� Address����Ĭ��2
        /// </summary>
        private int sizeOfCA = 2; /* (parameter a) CA = common address of ASDUs (1/2) */

        public ConnectionParameters()
        {
        }

        /// <summary>
        /// ����һ�ݣ���ȸ���
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
        /// ��·��ַ��������վ��·��ַ�Ƕ���
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
        /// �����������ն��豸��ַ���ֽ�����ASDU �� Address����Ĭ��2
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

