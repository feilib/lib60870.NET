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
    /// ���ͱ�ʶ
    /// </summary>
	public enum TypeID {
        /// <summary>
        /// ��λң��  ��Ʒ������������ʱ��
        /// </summary>
		M_SP_NA_1 = 1,
		M_SP_TA_1 = 2,
        /// <summary>
        /// ˫λң��  ��Ʒ������������ʱ��
        /// </summary>
		M_DP_NA_1 = 3,
		M_DP_TA_1 = 4,
		M_ST_NA_1 = 5,
		M_ST_TA_1 = 6,
		M_BO_NA_1 = 7,
		M_BO_TA_1 = 8,
        /// <summary>
        /// ��һ��ң��ֵ  ��Ʒ������������ʱ��
        /// </summary>
		M_ME_NA_1 = 9,
		M_ME_TA_1 = 10,
        /// <summary>
        /// ��Ȼ�ң��ֵ  ��Ʒ������������ʱ��
        /// </summary>
		M_ME_NB_1 = 11,
		M_ME_TB_1 = 12,
        /// <summary>
        /// �̸���ң��ֵ  ��Ʒ������������ʱ��
        /// </summary>
		M_ME_NC_1 = 13,
		M_ME_TC_1 = 14,
        /// <summary>
        /// �ۼ���  ��Ʒ������������ʱ��
        /// </summary>
		M_IT_NA_1 = 15,
		M_IT_TA_1 = 16,
		M_EP_TA_1 = 17,
		M_EP_TB_1 = 18,
		M_EP_TC_1 = 19,
        /// <summary>
        /// ���鵥λң��
        /// </summary>
		M_PS_NA_1 = 20,
        /// <summary>
        /// ��һ��ң��ֵ  ��Ʒ������������ʱ��
        /// </summary>
		M_ME_ND_1 = 21,
        /// <summary>
        /// ��λң�ţ�SOE��  ��Ʒ��������������ʱ��
        /// </summary>
		M_SP_TB_1 = 30,
        /// <summary>
        /// ˫λң�ţ�SOE��  ��Ʒ��������������ʱ��
        /// </summary>
		M_DP_TB_1 = 31,
		M_ST_TB_1 = 32,
		M_BO_TB_1 = 33,
        /// <summary>
        /// ��һ��ң��ֵ  ��Ʒ��������������ʱ��
        /// </summary>
		M_ME_TD_1 = 34,
        /// <summary>
        /// ��Ȼ�ң��ֵ  ��Ʒ��������������ʱ��
        /// </summary>
		M_ME_TE_1 = 35,
        /// <summary>
        /// �̸���ң��ֵ  ��Ʒ��������������ʱ��
        /// </summary>
		M_ME_TF_1 = 36,
        /// <summary>
        /// �ۼ���  ��Ʒ��������������ʱ��
        /// </summary>
		M_IT_TB_1 = 37,
		M_EP_TD_1 = 38,
		M_EP_TE_1 = 39,
		M_EP_TF_1 = 40,
        /// <summary>
        /// ��λң������
        /// </summary>
		C_SC_NA_1 = 45,
        /// <summary>
        /// ˫λң������
        /// </summary>
		C_DC_NA_1 = 46,
        /// <summary>
        /// ��λ��������
        /// </summary>
		C_RC_NA_1 = 47,
        /// <summary>
        /// ��һ���趨ֵ
        /// </summary>
		C_SE_NA_1 = 48,
        /// <summary>
        /// 
        /// </summary>
		C_SE_NB_1 = 49,
		C_SE_NC_1 = 50,
		C_BO_NA_1 = 51,
		C_SC_TA_1 = 58,
		C_DC_TA_1 = 59,
		C_RC_TA_1 = 60,
		C_SE_TA_1 = 61,
		C_SE_TB_1 = 62,
		C_SE_TC_1 = 63,
		C_BO_TA_1 = 64,
		M_EI_NA_1 = 70,
		C_IC_NA_1 = 100,
		C_CI_NA_1 = 101,
		C_RD_NA_1 = 102,
		C_CS_NA_1 = 103,
		C_TS_NA_1 = 104,
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
	
}
