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
    /// 101协议和104协议中的传送原因
    /// </summary>
	public enum CauseOfTransmission {
        /// <summary>
        /// 周期、循环 - 上行
        /// </summary>
		PERIODIC = 1,
        /// <summary>
        /// <para>背景扫描 - 上行</para>
        /// <para></para>
        /// <para>基于连续传送方式，用于监视方向将被控站的过程信息去同步控制站的数据库，优先级较低。</para>
        /// </summary>
        BACKGROUND_SCAN = 2,
        /// <summary>
        /// 突发(自发) - 上行
        /// </summary>
        SPONTANEOUS = 3,
        /// <summary>
        /// 初始化 - 上行
        /// </summary>
        INITIALIZED = 4,
        /// <summary>
        /// 请求或者被请求 - 上行、下行
        /// </summary>
        REQUEST = 5,
        /// <summary>
        /// 激活 - 下行
        /// </summary>
        ACTIVATION = 6,
        /// <summary>
        /// 激活确认 - 上行
        /// </summary>
        ACTIVATION_CON = 7,
        /// <summary>
        /// 停止激活 - 下行
        /// </summary>
        DEACTIVATION = 8,
        /// <summary>
        /// 停止激活确认 - 上行
        /// </summary>
		DEACTIVATION_CON = 9,
        /// <summary>
        /// 激活终止 - 下行
        /// </summary>
        ACTIVATION_TERMINATION = 10,
        /// <summary>
        /// 远方命令引起的返送信息 - 上行
        /// </summary>
        RETURN_INFO_REMOTE = 11,
        /// <summary>
        /// 当地命令引起的返送信息 - 上行
        /// </summary>
        RETURN_INFO_LOCAL = 12,
        /// <summary>
        /// 文件传输
        /// </summary>
        FILE_TRANSFER =	13,
		AUTHENTICATION = 14,
		MAINTENANCE_OF_AUTH_SESSION_KEY = 15,
		MAINTENANCE_OF_USER_ROLE_AND_UPDATE_KEY = 16,
        /// <summary>
        /// 响应站召唤 - 上行
        /// </summary>
		INTERROGATED_BY_STATION = 20,
        /// <summary>
        /// 响应第 1 组召唤 - 上行
        /// </summary>
        INTERROGATED_BY_GROUP_1 = 21,
        /// <summary>
        /// 响应第 2 组召唤 - 上行
        /// </summary>
        INTERROGATED_BY_GROUP_2 = 22,
        /// <summary>
        /// 响应第 3 组召唤 - 上行
        /// </summary>
        INTERROGATED_BY_GROUP_3 = 23,
        /// <summary>
        /// 响应第 4 组召唤 - 上行
        /// </summary>
		INTERROGATED_BY_GROUP_4 = 24,
        /// <summary>
        /// 响应第 5 组召唤 - 上行
        /// </summary>
        INTERROGATED_BY_GROUP_5 = 25,
        /// <summary>
        /// 响应第 6 组召唤 - 上行
        /// </summary>
        INTERROGATED_BY_GROUP_6 = 26,
        /// <summary>
        /// 响应第 7 组召唤 - 上行
        /// </summary>
        INTERROGATED_BY_GROUP_7 = 27,
        /// <summary>
        /// 响应第 8 组召唤 - 上行
        /// </summary>
		INTERROGATED_BY_GROUP_8 = 28,
        /// <summary>
        /// 响应第 9 组召唤 - 上行
        /// </summary>
        INTERROGATED_BY_GROUP_9 = 29,
        /// <summary>
        /// 响应第 10 组召唤 - 上行
        /// </summary>
        INTERROGATED_BY_GROUP_10 = 30,
        /// <summary>
        /// 响应第 11 组召唤 - 上行
        /// </summary>
        INTERROGATED_BY_GROUP_11 = 31,
        /// <summary>
        /// 响应第 12 组召唤 - 上行
        /// </summary>
        INTERROGATED_BY_GROUP_12 = 32,
        /// <summary>
        /// 响应第 13 组召唤 - 上行
        /// </summary>
        INTERROGATED_BY_GROUP_13 = 33,
        /// <summary>
        /// 响应第 14 组召唤 - 上行
        /// </summary>
        INTERROGATED_BY_GROUP_14 = 34,
        /// <summary>
        /// 响应第 15 组召唤 - 上行
        /// </summary>
        INTERROGATED_BY_GROUP_15 = 35,
        /// <summary>
        /// 响应第 16 组召唤 - 上行
        /// </summary>
        INTERROGATED_BY_GROUP_16 = 36,
        /// <summary>
        /// 响应计数量总召唤 - 上行
        /// </summary>
        REQUESTED_BY_GENERAL_COUNTER = 37,
        /// <summary>
        /// 响应第 1 组计数量召唤 - 上行
        /// </summary>
        REQUESTED_BY_GROUP_1_COUNTER = 38,
        /// <summary>
        /// 响应第 2 组计数量召唤 - 上行
        /// </summary>
        REQUESTED_BY_GROUP_2_COUNTER = 39,
        /// <summary>
        /// 响应第 3 组计数量召唤 - 上行
        /// </summary>
		REQUESTED_BY_GROUP_3_COUNTER = 40,
        /// <summary>
        /// 响应第 4 组计数量召唤 - 上行
        /// </summary>
        REQUESTED_BY_GROUP_4_COUNTER = 41,
        /// <summary>
        /// 未知的类型标识 - 上行
        /// </summary>
        UNKNOWN_TYPE_ID = 44,
        /// <summary>
        /// 未知的传送原因 - 上行
        /// </summary>
        UNKNOWN_CAUSE_OF_TRANSMISSION =	45,
        /// <summary>
        /// 未知的应用服务数据单元公共地址 - 上行
        /// </summary>
		UNKNOWN_COMMON_ADDRESS_OF_ASDU = 46,
        /// <summary>
        /// 未知的信息对象地址 - 上行
        /// </summary>
        UNKNOWN_INFORMATION_OBJECT_ADDRESS = 47
	}

    /// <summary>
    /// 102规约中的传送原因
    /// </summary>
    public enum CauseOfTransmission102
    {
        /// <summary>
        /// 周期、循环 - 上行
        /// </summary>
		PERIODIC = 1,
        /// <summary>
        /// <para>背景扫描 - 上行</para>
        /// <para></para>
        /// <para>基于连续传送方式，用于监视方向将被控站的过程信息去同步控制站的数据库，优先级较低。</para>
        /// </summary>
        BACKGROUND_SCAN = 2,
        /// <summary>
        /// 突发(自发) - 上行
        /// </summary>
        SPONTANEOUS = 3,
        /// <summary>
        /// 初始化 - 上行
        /// </summary>
        INITIALIZED = 4,
        /// <summary>
        /// 请求或者被请求 - 上行、下行
        /// </summary>
        REQUEST = 5,
        /// <summary>
        /// 激活 - 下行
        /// </summary>
        ACTIVATION = 6,
        /// <summary>
        /// 激活确认 - 上行
        /// </summary>
        ACTIVATION_CON = 7,
        /// <summary>
        /// 停止激活 - 下行
        /// </summary>
        DEACTIVATION = 8,
        /// <summary>
        /// 停止激活确认 - 上行
        /// </summary>
		DEACTIVATION_CON = 9,
        /// <summary>
        /// 激活终止 - 下行
        /// </summary>
        ACTIVATION_TERMINATION = 10,
        /// <summary>
        /// 无所请求的数据记录 - 上行
        /// </summary>
        NO_RECORD = 13,
        /// <summary>
        /// 无所请求的应用服务数据单元--类型 - 上行
        /// </summary>
        NO_ASDU_TYPE = 14,
        /// <summary>
        /// 由主站(控制站)发送的应用服务数据单元中的记录序号不可知 - 上行
        /// </summary>
        NO_ASDU_RNO = 15,
        /// <summary>
        /// 由主站(控制站)发送的应用服务数据单元中的地址说明不可知 - 上行
        /// </summary>
        NO_ASDU_ADD = 16,
        /// <summary>
        /// 无所请求的信息体
        /// </summary>
        NO_MSG_BODY = 17,
        /// <summary>
        /// 无所请求的累计时段
        /// </summary>
        NO_ACCUMULATE_TIME = 18,
        /// <summary>
        /// 时钟同步 - 上行
        /// </summary>
		SYNC_TIME = 48
    }

}
