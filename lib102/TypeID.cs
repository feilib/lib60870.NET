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

namespace lib102
{

    /// <summary>
    /// 102 的类型标识
    /// <para>在控制方向上标有(CON)的应用服务数据单元被应用服务所确认后.在监视方向发送的报文和控制方向</para>
    /// <para>的一样，只是二者的传送原因不同。这些镜像的应用服务数据单元可以用于肯定/ 否定确认（证实） </para>
    /// </summary>
    public enum TypeID
    {
        /// <summary>
        /// 单位遥信  带时标，带品质描述
        /// </summary>
		M_SP_TA_2 = 1,
        /// <summary>
        /// 记账(计费)电能累计量，每个量为四个八位位组
        /// </summary>
        M_IT_TA_2 = 2,
        /// <summary>
        /// 记账( 计费)电能累计量，每个量为三个八位位组
        /// </summary>
        M_IT_TB_2 = 3,
        /// <summary>
        /// 记账( 计费)电能累计量，每个量二个八位位组
        /// </summary>
        M_IT_TC_2 = 4,
        /// <summary>
        /// 周期复位记账(计费)电能累计量，每个量为四个八位位组
        /// </summary>
        M_IT_TD_2 = 5,
        /// <summary>
        /// 周期复位记账( 计费)电能累计量。每个量为三个八位位组
        /// </summary>
        M_IT_TE_2 = 6,
        /// <summary>
        /// 周期复位记账( 计费)电能累计量。每个量为二个八位位组
        /// </summary>
        M_IT_TF_2 = 7,
        /// <summary>
        /// 运行电能累计量，每个量为四个八位位组
        /// </summary>
        M_IT_TG_2 = 8,
        /// <summary>
        /// 运行电能累计量，每个量为三个八位位组
        /// </summary>
        M_IT_TH_2 = 9,
        /// <summary>
        /// 运行电能累计量，每个量为二个八位位组
        /// </summary>
        M_IT_TI_2 = 10,
        /// <summary>
        /// 周期复位运行电能累计最.每个量为四个八位位组
        /// </summary>
        M_IT_TK_2 = 11,
        /// <summary>
        /// 周期复位运行电能累计最.每个量为三个八位位组
        /// </summary>
        M_IT_TL_2 = 12,
        /// <summary>
        /// 周期复位运行电能累计最.每个量为二个八位位组
        /// </summary>
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
        /// (CON)读制造厂和产品规范
        /// </summary>
        C_RD_NA_2 = 100,
        /// <summary>
        /// (CON)读带时标的单点信息的记录
        /// </summary>
        C_SP_NA_2 = 101,
        /// <summary>
        /// (CON)读一个所选定时间范围的带时标的单点信息的记录
        /// </summary>
        C_SP_NB_2 = 102,
        /// <summary>
        /// (CON)读电能累计量数据终端设备的当前系统时间
        /// </summary>
        C_TI_NA_2 = 103,
        /// <summary>
        /// (CON)读最早累计时段的记账(计费) 电能累计量 
        /// </summary>
        C_CI_NA_2 = 104,
        /// <summary>
        /// (CON)读最早累计时段的和一个选定的地址范围的记账(计费)电能累计量
        /// </summary>
        C_CI_NB_2 = 105,
        /// <summary>
        /// (CON)读一个指定的过去累计时段的记账(计费)电能累计量
        /// </summary>
        C_CI_NC_2 = 106,
        /// <summary>
        /// (CON)读一个指定的过去累计时段和 1个选定的地址范围的记账(计费)电能累计量
        /// </summary>
        C_CI_ND_2 = 107,
        /// <summary>
        /// (CON)读周期地复位的最早累计时段的记账(计费)电能累计量
        /// </summary>
        C_CI_NE_2 = 108,
        /// <summary>
        /// (CON)读周期地复位的最早累计时段和一个选定的地址范围的记账(计费)电能累计量
        /// </summary>
        C_CI_NF_2 = 109,
        /// <summary>
        /// (CON)读一个指定的过去累计时段的周期地复位的记账(计费)电能累计量
        /// </summary>
        C_CI_NG_2 = 110,
        /// <summary>
        /// (CON)读一个指定的过去累计时段和一个选定的地址范围的周期地复位的记帐(计费) 电能累计量
        /// </summary>
        C_CI_NH_2 = 111,
        /// <summary>
        /// (CON)读最早累计时段的运行电能累计量
        /// </summary>
        C_CI_NI_2 = 112,
        /// <summary>
        /// (CON)读最早累计时段的和一个选定的地址范围运行电能累计量
        /// </summary>
        C_CI_NK_2 = 113,
        /// <summary>
        /// (CON)读一个指定的过去累计时段的运行电能累计量
        /// </summary>
        C_CI_NL_2 = 114,
        /// <summary>
        /// (CON)读一个指定的过去累计时段和一个选定的地址范围的运行电能累计董
        /// </summary>
        C_CI_NM_2 = 115,
        /// <summary>
        /// (CON)读周期地复位的最早累计时段的运行电能累计量
        /// </summary>
        C_CI_NN_2 = 116,
        /// <summary>
        /// (CON)读周期地复位的最早累计时段和一 个选定的地址范围的运行电能累计量
        /// </summary>
        C_CI_NO_2 = 117,
        /// <summary>
        /// (CON)读一个指定过去的累计时段的周期地复位的运行电能累计量
        /// </summary>
        C_CI_NP_2 = 118,
        /// <summary>
        /// (CON)读一个指定的过去累计时段和一个选定的地址范围的周期地复位的运行电能累计量
        /// </summary>
        C_CI_NQ_2 = 119,
        /// <summary>
        /// (CON)读一个选定的时间范围和一个选定的地址范围的记账(计费)电能累计量
        /// </summary>
        C_CI_NR_2 = 120,
        /// <summary>
        /// (CON)读周期地复位的一个选定的时间范围和一个选定的地址范围的记账(计费)电能累计量
        /// </summary>
        C_CI_NS_2 = 121,
        /// <summary>
        /// (CON)读一个选定的时间范围和一个选定的地址范围的运行电能累计量
        /// </summary>
        C_CI_NT_2 = 122,
        /// <summary>
        /// (CON)读周期地复位的一个选定的时间范围和一个选定的地址范围的运行电能累计量
        /// </summary>
        C_CI_NU_2 = 123,
        /// <summary>
        /// (CON)时钟同步报文
        /// </summary>
        C_SYN_TA_2 = 128,

    }

}
