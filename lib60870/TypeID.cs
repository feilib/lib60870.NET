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

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace lib60870
{

    /// <summary>
    /// 类型标识
    /// </summary>
	public enum TypeID
    {
        /// <summary>
        /// 单位遥信  带品质描述，不带时标
        /// </summary>
		M_SP_NA_1 = 1,
        M_SP_TA_1 = 2,
        /// <summary>
        /// 双位遥信  带品质描述，不带时标
        /// </summary>
		M_DP_NA_1 = 3,
        M_DP_TA_1 = 4,
        M_ST_NA_1 = 5,
        M_ST_TA_1 = 6,
        M_BO_NA_1 = 7,
        M_BO_TA_1 = 8,
        /// <summary>
        /// 归一化遥测值  带品质描述，不带时标
        /// </summary>
		M_ME_NA_1 = 9,
        M_ME_TA_1 = 10,
        /// <summary>
        /// 标度化遥测值  带品质描述，不带时标
        /// </summary>
		M_ME_NB_1 = 11,
        M_ME_TB_1 = 12,
        /// <summary>
        /// 短浮点遥测值  带品质描述，不带时标
        /// </summary>
		M_ME_NC_1 = 13,
        M_ME_TC_1 = 14,
        /// <summary>
        /// 累计量  带品质描述，不带时标
        /// </summary>
		M_IT_NA_1 = 15,
        M_IT_TA_1 = 16,
        M_EP_TA_1 = 17,
        M_EP_TB_1 = 18,
        M_EP_TC_1 = 19,
        /// <summary>
        /// 成组单位遥信
        /// </summary>
		M_PS_NA_1 = 20,
        /// <summary>
        /// 归一化遥测值  带品质描述，不带时标
        /// </summary>
		M_ME_ND_1 = 21,
        /// <summary>
        /// 单位遥信（SOE）  带品质描述，带绝对时标
        /// </summary>
		M_SP_TB_1 = 30,
        /// <summary>
        /// 双位遥信（SOE）  带品质描述，带绝对时标
        /// </summary>
		M_DP_TB_1 = 31,
        M_ST_TB_1 = 32,
        M_BO_TB_1 = 33,
        /// <summary>
        /// 归一化遥测值  带品质描述，带绝对时标
        /// </summary>
		M_ME_TD_1 = 34,
        /// <summary>
        /// 标度化遥测值  带品质描述，带绝对时标
        /// </summary>
		M_ME_TE_1 = 35,
        /// <summary>
        /// 短浮点遥测值  带品质描述，带绝对时标
        /// </summary>
		M_ME_TF_1 = 36,
        /// <summary>
        /// 累计量  带品质描述，带绝对时标
        /// </summary>
		M_IT_TB_1 = 37,
        M_EP_TD_1 = 38,
        M_EP_TE_1 = 39,
        M_EP_TF_1 = 40,
        /// <summary>
        /// 单位遥控命令  每个报文只能包含一个遥控信息体
        /// </summary>
		C_SC_NA_1 = 45,
        /// <summary>
        /// 双位遥控命令  每个报文只能包含一个遥控信息体
        /// </summary>
		C_DC_NA_1 = 46,
        /// <summary>
        /// 档位调节命令  每个报文只能包含一个档位信息体
        /// </summary>
		C_RC_NA_1 = 47,
        /// <summary>
        /// 归一化设定值  每个报文只能包含一个设定值
        /// </summary>
		C_SE_NA_1 = 48,
        /// <summary>
        /// 标度化设定值  每个报文只能包含一个设定值
        /// </summary>
		C_SE_NB_1 = 49,
        /// <summary>
        /// 短浮点设定值  每个报文只能包含一个设定值
        /// </summary>
		C_SE_NC_1 = 50,
        /// <summary>
        /// 归一化设定值  每个报文可以包含多个设定值
        /// </summary>
        C_SE_ND_1 = 136,
        C_BO_NA_1 = 51,
        C_SC_TA_1 = 58,
        C_DC_TA_1 = 59,
        C_RC_TA_1 = 60,
        C_SE_TA_1 = 61,
        C_SE_TB_1 = 62,
        C_SE_TC_1 = 63,
        C_BO_TA_1 = 64,
        /// <summary>
        /// 初始化结束  报告站端初始化完成
        /// </summary>
		M_EI_NA_1 = 70,
        /// <summary>
        /// 站召唤命令  带不同的限定词可以用于组召唤
        /// </summary>
		C_IC_NA_1 = 100,
        /// <summary>
        /// 累计量召唤命令  带不同的限定词可以用于组召唤
        /// </summary>
		C_CI_NA_1 = 101,
        /// <summary>
        /// 读命令  读单个信息对象值
        /// </summary>
		C_RD_NA_1 = 102,
        /// <summary>
        /// 时钟同步命令  需要通过测量通道延时加以校正
        /// </summary>
		C_CS_NA_1 = 103,
        C_TS_NA_1 = 104,
        /// <summary>
        /// 复位进程命令  使用前需要与双方确认
        /// </summary>
		C_RP_NA_1 = 105,
        C_CD_NA_1 = 106,
        C_TS_TA_1 = 107,
        P_ME_NA_1 = 110,
        P_ME_NB_1 = 111,
        P_ME_NC_1 = 112,
        P_AC_NA_1 = 113,
        F_FR_NA_1 = 120,
        F_SR_NA_1 = 121,
        F_SC_NA_1 = 122,
        F_LS_NA_1 = 123,
        F_AF_NA_1 = 124,
        F_SG_NA_1 = 125,
        F_DR_TA_1 = 126,
        F_SC_NB_1 = 127
    }

    public enum TypeID102
    {
        /// <summary>
        /// 单位遥信  带时标，带品质描述
        /// </summary>
		M_SP_TA_2 = 1,
        /// <summary>
        /// 记账(计费)电能累计量，每个量为四个八位位组
        /// </summary>
        M_IT_TA_2 = 2,
        M_IT_TB_2 = 3,
        M_IT_TC_2 = 4,
        /// <summary>
        /// 周期复位记账(计费)电能累计量，每个量为四个八位位组
        /// </summary>
        M_IT_TD_2 = 5,
        M_IT_TE_2 = 6,
        M_IT_TF_2 = 7,
        /// <summary>
        /// 运行电能累计量，每个量为四个八位位组
        /// </summary>
        M_IT_TG_2 = 8,
        M_IT_TH_2 = 9,
        M_IT_TI_2 = 10,
        /// <summary>
        /// 周期复位运行电能累计最.每个量为四个八位位组
        /// </summary>
        M_IT_TK_2 = 11,
        M_IT_TL_2 = 12,
        M_IT_TM_2 = 13,
        /// <summary>
        /// 初始化结束  
        /// </summary>
		M_EI_NA_2 = 70,
        /// <summary>
        /// 电能累计量数据终端设备的制造厂和产品规范
        /// </summary>
        P_MP_NA_2 = 71,
        /// <summary>
        /// 电能累计量数据终端设备的当前系统时间
        /// </summary>
        M_TI_TA_2 = 72,
        /// <summary>
        /// 读制造厂和产品规范
        /// </summary>
        C_RD_NA_2 = 100,
        /// <summary>
        /// 读带时标的单点信息的记录
        /// </summary>
        C_SP_NA_2 = 101,
        /// <summary>
        /// 读一个所选定时间范围的带时标的单点信息的记录
        /// </summary>
        C_SP_NB_2 = 102,
        /// <summary>
        /// 读电能累计量数据终端设备的当前系统时间
        /// </summary>
        C_TI_NA_2 = 103,
        /// <summary>
        /// 读最早累计时段的记账(计费) 电能累计量 
        /// </summary>
        C_CI_NA_2 = 104,
        /// <summary>
        /// 读最早累计时段的和一个选定的地址范围的记账(计费)电能累计量
        /// </summary>
        C_CI_NB_2 = 105,
        /// <summary>
        /// 读一个指定的过去累计时段的记账(计费)电能累计量
        /// </summary>
        C_CI_NC_2 = 106,
        /// <summary>
        /// 读一个指定的过去累计时段和 1个选定的地址范围的记账(计费)电能累计量
        /// </summary>
        C_CI_ND_2 = 107,
        /// <summary>
        /// 读周期地复位的最早累计时段的记账(计费)电能累计量
        /// </summary>
        C_CI_NE_2 = 108,
        /// <summary>
        /// 读周期地复位的最早累计时段和一个选定的地址范围的记账(计费费)电能累计量
        /// </summary>
        C_CI_NF_2 = 109,
        /// <summary>
        /// 读一个指定的过去累计时段的周期地复位的记账(计费)电能累计量
        /// </summary>
        C_CI_NG_2 = 110,
        /// <summary>
        /// 读一个指定的过去累计时段和一个选定的地址范围的周期地复位的记帐(计费) 电能累计量
        /// </summary>
        C_CI_NH_2 = 111,
        /// <summary>
        /// 读最早累计时段的运行电能累计量
        /// </summary>
        C_CI_NI_2 = 112,
        /// <summary>
        /// 读最早累计时段的和一个选定的地址范围运行电能累计量
        /// </summary>
        C_CI_NK_2 = 113,
        /// <summary>
        /// 读一个指定的过去累计时段的运行电能累计量
        /// </summary>
        C_CI_NL_2 = 114,
        /// <summary>
        /// 读一个指定的过去累计时段和一个选定的地址范围的运行电能累计董
        /// </summary>
        C_CI_NM_2 = 115,
        /// <summary>
        /// 读周期地复位的最早累计时段的运行电能累计量
        /// </summary>
        C_CI_NN_2 = 116,
        /// <summary>
        /// 读周期地复位的最早累计时段和一 个选定的地址范围的运行电能累计量
        /// </summary>
        C_CI_NO_2 = 117,
        /// <summary>
        /// 读一个指定过去的累计时段的周期地复位的运行电能累计量
        /// </summary>
        C_CI_NP_2 = 118,
        /// <summary>
        /// 读一个指定的过去累计时段和一个选定的地址范围的周期地复位的运行电能累计量
        /// </summary>
        C_CI_NQ_2 = 119,
        /// <summary>
        /// 读一个选定的时间范围和一个选定的地址范围的记账(计费)电能累计量
        /// </summary>
        C_CI_NR_2 = 120,
        /// <summary>
        /// 读周期地复位的一个选定的时间范围和一个选定的地址范围的记账(计费)电能累计量
        /// </summary>
        C_CI_NS_2 = 121,
        /// <summary>
        /// 读一个选定的时间范围和一个选定的地址范围的运行电能累计量
        /// </summary>
        C_CI_NT_2 = 122,
        /// <summary>
        /// 读周期地复位的一个选定的时间范围和一个选定的地址范围的运行电能累计量
        /// </summary>
        C_CI_NU_2 = 123,
        /// <summary>
        /// 时钟同步报文
        /// </summary>
        C_SYN_TA_2 = 128
    }

}
