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

using System;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace lib60870
{
    /// <summary>
    /// 101Э���104Э���еĴ���ԭ��
    /// </summary>
	public enum CauseOfTransmission {
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
        /// Զ����������ķ�����Ϣ - ����
        /// </summary>
        RETURN_INFO_REMOTE = 11,
        /// <summary>
        /// ������������ķ�����Ϣ - ����
        /// </summary>
        RETURN_INFO_LOCAL = 12,
        /// <summary>
        /// �ļ�����
        /// </summary>
        FILE_TRANSFER =	13,
		AUTHENTICATION = 14,
		MAINTENANCE_OF_AUTH_SESSION_KEY = 15,
		MAINTENANCE_OF_USER_ROLE_AND_UPDATE_KEY = 16,
        /// <summary>
        /// ��Ӧվ�ٻ� - ����
        /// </summary>
		INTERROGATED_BY_STATION = 20,
        /// <summary>
        /// ��Ӧ�� 1 ���ٻ� - ����
        /// </summary>
        INTERROGATED_BY_GROUP_1 = 21,
        /// <summary>
        /// ��Ӧ�� 2 ���ٻ� - ����
        /// </summary>
        INTERROGATED_BY_GROUP_2 = 22,
        /// <summary>
        /// ��Ӧ�� 3 ���ٻ� - ����
        /// </summary>
        INTERROGATED_BY_GROUP_3 = 23,
        /// <summary>
        /// ��Ӧ�� 4 ���ٻ� - ����
        /// </summary>
		INTERROGATED_BY_GROUP_4 = 24,
        /// <summary>
        /// ��Ӧ�� 5 ���ٻ� - ����
        /// </summary>
        INTERROGATED_BY_GROUP_5 = 25,
        /// <summary>
        /// ��Ӧ�� 6 ���ٻ� - ����
        /// </summary>
        INTERROGATED_BY_GROUP_6 = 26,
        /// <summary>
        /// ��Ӧ�� 7 ���ٻ� - ����
        /// </summary>
        INTERROGATED_BY_GROUP_7 = 27,
        /// <summary>
        /// ��Ӧ�� 8 ���ٻ� - ����
        /// </summary>
		INTERROGATED_BY_GROUP_8 = 28,
        /// <summary>
        /// ��Ӧ�� 9 ���ٻ� - ����
        /// </summary>
        INTERROGATED_BY_GROUP_9 = 29,
        /// <summary>
        /// ��Ӧ�� 10 ���ٻ� - ����
        /// </summary>
        INTERROGATED_BY_GROUP_10 = 30,
        /// <summary>
        /// ��Ӧ�� 11 ���ٻ� - ����
        /// </summary>
        INTERROGATED_BY_GROUP_11 = 31,
        /// <summary>
        /// ��Ӧ�� 12 ���ٻ� - ����
        /// </summary>
        INTERROGATED_BY_GROUP_12 = 32,
        /// <summary>
        /// ��Ӧ�� 13 ���ٻ� - ����
        /// </summary>
        INTERROGATED_BY_GROUP_13 = 33,
        /// <summary>
        /// ��Ӧ�� 14 ���ٻ� - ����
        /// </summary>
        INTERROGATED_BY_GROUP_14 = 34,
        /// <summary>
        /// ��Ӧ�� 15 ���ٻ� - ����
        /// </summary>
        INTERROGATED_BY_GROUP_15 = 35,
        /// <summary>
        /// ��Ӧ�� 16 ���ٻ� - ����
        /// </summary>
        INTERROGATED_BY_GROUP_16 = 36,
        /// <summary>
        /// ��Ӧ���������ٻ� - ����
        /// </summary>
        REQUESTED_BY_GENERAL_COUNTER = 37,
        /// <summary>
        /// ��Ӧ�� 1 ��������ٻ� - ����
        /// </summary>
        REQUESTED_BY_GROUP_1_COUNTER = 38,
        /// <summary>
        /// ��Ӧ�� 2 ��������ٻ� - ����
        /// </summary>
        REQUESTED_BY_GROUP_2_COUNTER = 39,
        /// <summary>
        /// ��Ӧ�� 3 ��������ٻ� - ����
        /// </summary>
		REQUESTED_BY_GROUP_3_COUNTER = 40,
        /// <summary>
        /// ��Ӧ�� 4 ��������ٻ� - ����
        /// </summary>
        REQUESTED_BY_GROUP_4_COUNTER = 41,
        /// <summary>
        /// δ֪�����ͱ�ʶ - ����
        /// </summary>
        UNKNOWN_TYPE_ID = 44,
        /// <summary>
        /// δ֪�Ĵ���ԭ�� - ����
        /// </summary>
        UNKNOWN_CAUSE_OF_TRANSMISSION =	45,
        /// <summary>
        /// δ֪��Ӧ�÷������ݵ�Ԫ������ַ - ����
        /// </summary>
		UNKNOWN_COMMON_ADDRESS_OF_ASDU = 46,
        /// <summary>
        /// δ֪����Ϣ�����ַ - ����
        /// </summary>
        UNKNOWN_INFORMATION_OBJECT_ADDRESS = 47
	}

    /// <summary>
    /// 102��Լ�еĴ���ԭ��
    /// </summary>
    public enum CauseOfTransmission102
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
