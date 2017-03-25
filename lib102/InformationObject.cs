/*
 *  InformationObject.cs
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
    /// ��Ϣ�����-���Ǹ������࣬���Ҫ�̳���������
    /// </summary>
	public abstract class InformationObject
    {
        /// <summary>
        /// <para>��Ϣ������ַ</para>
        /// <para>  ����Ϊ1-3���ֽڣ���ConnectionParameters������</para>
        /// <para>  �������䷽ʽ�£��ӵڶ�����Ϣ�忪ʼ����ַ���ٳ��֣�Ĭ��Ϊ��һ����ַ+1</para>
        /// <para>  ��ɢ���䣬ÿ����Ϣ�嶼�����е�ַ</para>
        /// <para>  û����ȷ�Ķ����ַʱ����0����</para>
        /// </summary>
		private int objectAddress;

        /// <summary>
        /// ������Ϣ������ַ������ConnectionParameters�����õĵ�ַ���Ƚ��н���
        /// </summary>
        /// <param name="msg">��Ϣ��</param>
        /// <param name="startIndex">��ʼ�ֽ�</param>
        /// <returns>����������Ϣ�����ַ</returns>
		internal static int ParseInformationObjectAddress(byte[] msg, int startIndex)
        {
            //��һ���ֽ��ǵ�ַ
            int ioa = msg[startIndex];
            return ioa;
        }

        /// <summary>
        /// ���캯����ͨ����Ϣ���ݣ���Ҫ������Ϣ���ַ��
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        /// <param name="isSequence"></param>
        internal InformationObject( byte[] msg, int startIndex, bool isSequence)
        {
            //������״̬�£��Ű��Ž�����ַ������
            //����״̬ʱ���ڶ�����Ϣ�忪ʼ�Ͳ����ֵ�ַ�ˣ�Ĭ��Ϊ��һ����ַ+1
            if (!isSequence)
                objectAddress = ParseInformationObjectAddress( msg, startIndex);
        }

        /// <summary>
        /// ʹ����Ϣ���ַ��ʼ����Ϣ�����
        /// </summary>
        /// <param name="objectAddress"></param>
        public InformationObject(int objectAddress)
        {
            this.objectAddress = objectAddress;
        }

        /// <summary>
        /// <para>��Ϣ������ַ</para>
        /// <para>  ����Ϊ1-3���ֽڣ���ConnectionParameters������</para>
        /// <para>  �������䷽ʽ�£��ӵڶ�����Ϣ�忪ʼ����ַ���ٳ��֣�Ĭ��Ϊ��һ����ַ+1</para>
        /// <para>  ��ɢ���䣬ÿ����Ϣ�嶼�����е�ַ</para>
        /// <para>  û����ȷ�Ķ����ַʱ����0����</para>
        /// </summary>
        public int ObjectAddress
        {
            get
            {
                return this.objectAddress;
            }
            internal set
            {
                objectAddress = value;
            }
        }

        /// <summary>
        /// �Ƿ�֧��������������ʵ��
        /// </summary>
        public abstract bool SupportsSequence
        {
            get;
        }

        /// <summary>
        /// ��ȡ��Ϣ���������ͣ���������ʵ��
        /// </summary>
        public abstract TypeID Type
        {
            get;
        }

        /// <summary>
        /// ������Ϣ�������frame��עΪȷ����ַ��˳��������룬���������ȵ��û���
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="parameters"></param>
        /// <param name="isSequence"></param>
        internal virtual void Encode(Frame frame, ConnectionParameters parameters, bool isSequence)
        {
            //������������룬��ÿ�α���ǰ�ȱ����ַ
            if (!isSequence)
            {
                frame.SetNextByte((byte)(objectAddress & 0xff));
            }
        }


    }
}

