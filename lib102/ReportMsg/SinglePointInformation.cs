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

namespace lib102
{
    /// <summary>
    /// ������Ϣ-TypeID.M_SP_TA_2;--102��ֻ����һ����ֱ�Ӵ�Ʒ��������ʱ��56λ 
    /// <para>1. ������Ϣ��ָӰ������ۼ�������Ч���¼�����Ϊ��ʷ��־��������</para>
    /// <para>2. �������ӵ����ۼ�����վ�����ն��豸���쳧����ĵ�����Ϣ�Ƕ��ݵĻ������õ�</para>
    /// <para>3. ����ַ(SPA)���޶���(SPQ)������Ϣ״̬(SPI)��ռ2���ֽ�</para>
    /// <para>�ر�˵����</para>
    /// <para>1. ������Ϣ�ڱ����豸�ڿɱ��������Ͽɣ���ʱSPI���ÿɱ�����Ϊ0</para>
    /// <para>2. ������Ϣ������˲����Ϣ���䣬����Ϣ��������״̬SPI=0��1</para>
    /// <para>3. ������Ϣ����ͻ�����ͣ�Ҳ���Ա�������</para>
    /// <para>4. �ɵ����Ͽ�ʱ�����ĵ�����Ϣ���Բ�����</para>
    /// <para></para>
    /// </summary>
	public class SinglePointInformation : InformationObject
    {
        /// <summary>
        /// TypeID.M_SP_TA_2;
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.M_SP_TA_2;
            }
        }

        /// <summary>
        /// ��֧��������ÿ����Ϣ��ʱ��͵�ַ���е�������
        /// </summary>
        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// ʵ�ʵ���ֵ��bit1
        /// </summary>
        private bool value;

        /// <summary>
        /// ʵ�ʵ�ֵ
        /// </summary>
        public bool Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// �����ַ��Ϣ����һ���ֽ�(SPA)-
        /// <para>��ͬ�ĵ�ַ����ͬ�ĺ��壬���������õ��У�</para>
        /// <para>  1  ϵͳ��������</para>
        /// <para>  3  ��Դ����</para>
        /// <para>  7  ʱ��ƫ��</para>
        /// <para>  13 �����ۼ����Ĳ�������</para>
        /// <para>  15 �����ı�</para>
        /// <para>  17 �˹�����</para>
        /// <para>  18 ���汨��</para>
        /// <para>  19 ����ź�</para>
        /// </summary>
        private int objectAddress;

        /// <summary>
        /// �����ַ��Ϣ����һ���ֽ�(SPA)--
        /// <para>��ͬ�ĵ�ַ����ͬ�ĺ��壬���������õ��У�</para>
        /// <para>  1  ϵͳ��������</para>
        /// <para>  3  ��Դ����</para>
        /// <para>  7  ʱ��ƫ��</para>
        /// <para>  13 �����ۼ����Ĳ�������</para>
        /// <para>  15 �����ı�</para>
        /// <para>  17 �˹�����</para>
        /// <para>  18 ���汨��</para>
        /// <para>  19 ����ź�</para>
        /// </summary>
        /// ��ʵ�������У��������ר�Ÿ���һ��˵���ļ�
        public new int ObjectAddress
        {
            get
            {
                return this.objectAddress;
            }
        }

        /// <summary>
        /// Ʒ��������(SPQ)
        /// <para>���SPAʹ�ã����Դ���ͬ�ĺ��壬������</para>
        /// </summary>
        private int quality;

        /// <summary>
        /// Ʒ��������(SPQ)
        /// <para>���SPAʹ�ã����Դ���ͬ�ĺ��壬������</para>
        /// </summary>
        public int Quality
        {
            get
            {
                return this.quality;
            }
        }

        /// <summary>
        /// ʱ��
        /// </summary>
        private CP56Time2b timestamp;

        /// <summary>
        /// ʱ��
        /// </summary>
        public CP56Time2b Timestamp
        {
            get
            {
                return this.timestamp;
            }
        }

        /// <summary>
        /// ͨ����Ϣ���ݽ�������
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="msg"></param>
        /// <param name="startIndex"></param>
        /// <param name="isSequence"></param>
        internal SinglePointInformation(byte[] msg, int startIndex, bool isSequence) :
            base(msg, startIndex, isSequence)
        {
            if (!isSequence)
                startIndex += 1; /* skip IOA ������Ϣ���ַ(�ڻ����н�������) */

            /* parse SPI (single point information with qualitiy)  ����ʵ��ֵ */
            byte spi = msg[startIndex++];

            //���ֵ bit1
            value = ((spi & 0x01) == 0x01);

            //SPQ ��Ϣ�޶��ʣ�Ʒ�������� bit2-8
            quality = (byte)(spi & 0xFE);

            /* parse CP56Time2a (time stamp) */
            timestamp = new CP56Time2b(msg, startIndex);
        }

        /// <summary>
        /// ����һ��������Ϣ��ʹ����Ϣ��ַ��ֵ��Ʒ��������
        /// </summary>
        /// <param name="objectAddress">��Ϣ���ַ</param>
        /// <param name="value">����ֵ</param>
        /// <param name="quality">Ʒ��������</param>
        /// <param name="timestamp">ʱ��</param>
        public SinglePointInformation(int objectAddress, bool value, int quality, CP56Time2b timestamp) :
            base(objectAddress)
        {
            this.value = value;
            this.quality = quality;
            this.timestamp = timestamp;
        }

        /// <summary>
        /// �������frame
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="isSequence">��֧��������ǿ��дfalse��</param>
        internal override void Encode(Frame frame, bool isSequence = false)
        {
            //ֱ��д���ѣ���������֧������
            base.Encode(frame, false);

            //quality ��Ҫ����һλ
            byte val = (byte)((quality * 2) & 0xFF);

            //����ֵ
            if (value)
            {
                val = (byte)(val | 0x01);
            }
            else
            {
                val = (byte)(val & 0xFE);
            }

            frame.SetNextByte(val);

            //֮��дʱ��
            frame.AppendBytes(timestamp.GetEncodedValue());
        }

    }

}

