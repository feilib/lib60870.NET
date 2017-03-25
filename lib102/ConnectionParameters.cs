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
        /// ���ͱ�ʶ���ȣ�Ĭ��1
        /// </summary>
        private int sizeOfTypeId = 1;

        /// <summary>
        /// �ɱ�ṹ�޶��ʵĳ��ȣ�Ĭ��1
        /// </summary>
        private int sizeOfVSQ = 1; /* VSQ = variable sturcture qualifier */

        /// <summary>
        /// ����ԭ����ֽ�����102��Ĭ��1
        /// </summary>
        private int sizeOfCOT = 1; /* (parameter b) COT = cause of transmission (1/2) */

        /// <summary>
        /// ��·��ַ��������վ��·��ַ�Ƕ���
        /// </summary>
        private int linkAddress = 0;

        /// <summary>
        /// Ӧ�÷������ݵ�Ԫ������ַ���ֽ�����ASDU �� Address����Ĭ��2
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

            copy.sizeOfTypeId = sizeOfTypeId;
            copy.sizeOfVSQ = sizeOfVSQ;
            copy.sizeOfCOT = sizeOfCOT;
            copy.linkAddress = linkAddress;
            copy.sizeOfCA = sizeOfCA;

            return copy;
        }



        /// <summary>
        /// ����ԭ����ֽ�����Ĭ��2
        /// </summary>
        public int SizeOfCOT
        {
            get
            {
                return this.sizeOfCOT;
            }
            set
            {
                sizeOfCOT = value;
            }
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
        /// Ӧ�÷������ݵ�Ԫ������ַ���ֽ�����ASDU �� Address����Ĭ��2
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


        /// <summary>
        /// ���ͱ�ʶ���ȣ�Ĭ��1
        /// </summary> 
        public int SizeOfTypeId
        {
            get
            {
                return this.sizeOfTypeId;
            }
        }

        /// <summary>
        /// �ɱ�ṹ�޶��ʵĳ��ȣ�Ĭ��1
        /// </summary>
        public int SizeOfVSQ
        {
            get
            {
                return this.sizeOfVSQ;
            }
        }
    }
}

