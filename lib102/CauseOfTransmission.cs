/*
 *  CauseOfTransmission.cs
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

namespace lib102
{

    /// <summary>
    /// 102��Լ�еĴ���ԭ��
    /// </summary>
    public enum CauseOfTransmission
    {
        /// <summary>
        /// ���ڡ�ѭ�� - ����
        /// </summary>
		PERIODIC = 1,
        /// <summary>
        /// <para>����ɨ�� - ����</para>
        /// <para></para>
        /// <para>�����������ͷ�ʽ�����ڼ��ӷ��򽫱���վ�Ĺ�����Ϣȥͬ������վ�����ݿ⣬���ȼ��ϵ͡�</para>
        /// </summary>
        BACKGROUND_SCAN = 2,
        /// <summary>
        /// ͻ��(�Է�) - ����
        /// </summary>
        SPONTANEOUS = 3,
        /// <summary>
        /// ��ʼ�� - ����
        /// </summary>
        INITIALIZED = 4,
        /// <summary>
        /// ������߱����� - ���С�����
        /// </summary>
        REQUEST = 5,
        /// <summary>
        /// ���� - ����
        /// </summary>
        ACTIVATION = 6,
        /// <summary>
        /// ����ȷ�� - ����
        /// </summary>
        ACTIVATION_CON = 7,
        /// <summary>
        /// ֹͣ���� - ����
        /// </summary>
        DEACTIVATION = 8,
        /// <summary>
        /// ֹͣ����ȷ�� - ����
        /// </summary>
		DEACTIVATION_CON = 9,
        /// <summary>
        /// ������ֹ - ����
        /// </summary>
        ACTIVATION_TERMINATION = 10,
        /// <summary>
        /// ������������ݼ�¼ - ����
        /// </summary>
        NO_RECORD = 13,
        /// <summary>
        /// ���������Ӧ�÷������ݵ�Ԫ--���� - ����
        /// </summary>
        NO_ASDU_TYPE = 14,
        /// <summary>
        /// ����վ(����վ)���͵�Ӧ�÷������ݵ�Ԫ�еļ�¼��Ų���֪ - ����
        /// </summary>
        NO_ASDU_RNO = 15,
        /// <summary>
        /// ����վ(����վ)���͵�Ӧ�÷������ݵ�Ԫ�еĵ�ַ˵������֪ - ����
        /// </summary>
        NO_ASDU_ADD = 16,
        /// <summary>
        /// �����������Ϣ��
        /// </summary>
        NO_MSG_BODY = 17,
        /// <summary>
        /// ����������ۼ�ʱ��
        /// </summary>
        NO_ACCUMULATE_TIME = 18,
        /// <summary>
        /// ʱ��ͬ�� - ����
        /// </summary>
		SYNC_TIME = 48
    }

}
