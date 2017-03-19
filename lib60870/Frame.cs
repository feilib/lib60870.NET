/*
 *  Frame.cs
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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace lib60870
{
    public abstract class Frame
    {
        /// <summary>
        /// ����ǰ������������ڸ��µ�ǰ���͵ļ������ͽ��ռ�����֮����仯���ݡ�����
        /// </summary>
        /// <param name="sendCounter"></param>
        /// <param name="receiveCounter"></param>
		public abstract void PrepareToSend(int sendCounter, int receiveCounter);

        /// <summary>
        /// ��λ֡
        /// </summary>
		public abstract void ResetFrame();

        /// <summary>
        /// ����һ����Ҫ���͵��ֽ�
        /// </summary>
        /// <param name="value"></param>
        public abstract void SetNextByte(byte value);

        /// <summary>
        /// ����һ����Ҫ���͵��ֽ�
        /// </summary>
        /// <param name="bytes"></param>
        public abstract void AppendBytes(byte[] bytes);

        /// <summary>
        /// ��ȡ�ܹ��ֽ���
        /// </summary>
        /// <returns></returns>
		public abstract int GetMsgSize();

        /// <summary>
        /// ��ȡ��Ҫ���͵Ļ�����
        /// </summary>
        /// <returns></returns>
		public abstract byte[] GetBuffer();

    }
}
