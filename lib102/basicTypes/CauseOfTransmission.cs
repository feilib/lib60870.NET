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
    /// 102规约中的传送原因
    /// </summary>
    public enum CauseOfTransmission
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
