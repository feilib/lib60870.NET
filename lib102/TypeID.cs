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
    /// 102 �����ͱ�ʶ
    /// <para>�ڿ��Ʒ����ϱ���(CON)��Ӧ�÷������ݵ�Ԫ��Ӧ�÷�����ȷ�Ϻ�.�ڼ��ӷ����͵ı��ĺͿ��Ʒ���</para>
    /// <para>��һ����ֻ�Ƕ��ߵĴ���ԭ��ͬ����Щ�����Ӧ�÷������ݵ�Ԫ�������ڿ϶�/ ��ȷ�ϣ�֤ʵ�� </para>
    /// </summary>
    public enum TypeID
    {
        /// <summary>
        /// ��λң��  ��ʱ�꣬��Ʒ������
        /// </summary>
		M_SP_TA_2 = 1,
        /// <summary>
        /// ����(�Ʒ�)�����ۼ�����ÿ����Ϊ�ĸ���λλ��
        /// </summary>
        M_IT_TA_2 = 2,
        /// <summary>
        /// ����( �Ʒ�)�����ۼ�����ÿ����Ϊ������λλ��
        /// </summary>
        M_IT_TB_2 = 3,
        /// <summary>
        /// ����( �Ʒ�)�����ۼ�����ÿ����������λλ��
        /// </summary>
        M_IT_TC_2 = 4,
        /// <summary>
        /// ���ڸ�λ����(�Ʒ�)�����ۼ�����ÿ����Ϊ�ĸ���λλ��
        /// </summary>
        M_IT_TD_2 = 5,
        /// <summary>
        /// ���ڸ�λ����( �Ʒ�)�����ۼ�����ÿ����Ϊ������λλ��
        /// </summary>
        M_IT_TE_2 = 6,
        /// <summary>
        /// ���ڸ�λ����( �Ʒ�)�����ۼ�����ÿ����Ϊ������λλ��
        /// </summary>
        M_IT_TF_2 = 7,
        /// <summary>
        /// ���е����ۼ�����ÿ����Ϊ�ĸ���λλ��
        /// </summary>
        M_IT_TG_2 = 8,
        /// <summary>
        /// ���е����ۼ�����ÿ����Ϊ������λλ��
        /// </summary>
        M_IT_TH_2 = 9,
        /// <summary>
        /// ���е����ۼ�����ÿ����Ϊ������λλ��
        /// </summary>
        M_IT_TI_2 = 10,
        /// <summary>
        /// ���ڸ�λ���е����ۼ���.ÿ����Ϊ�ĸ���λλ��
        /// </summary>
        M_IT_TK_2 = 11,
        /// <summary>
        /// ���ڸ�λ���е����ۼ���.ÿ����Ϊ������λλ��
        /// </summary>
        M_IT_TL_2 = 12,
        /// <summary>
        /// ���ڸ�λ���е����ۼ���.ÿ����Ϊ������λλ��
        /// </summary>
        M_IT_TM_2 = 13,
        /// <summary>
        /// ��ʼ������  
        /// </summary>
		M_EI_NA_2 = 70,
        /// <summary>
        /// �����ۼ��������ն��豸�����쳧�Ͳ�Ʒ�淶
        /// </summary>
        P_MP_NA_2 = 71,
        /// <summary>
        /// �����ۼ��������ն��豸�ĵ�ǰϵͳʱ��
        /// </summary>
        M_TI_TA_2 = 72,
        /// <summary>
        /// (CON)�����쳧�Ͳ�Ʒ�淶
        /// </summary>
        C_RD_NA_2 = 100,
        /// <summary>
        /// (CON)����ʱ��ĵ�����Ϣ�ļ�¼
        /// </summary>
        C_SP_NA_2 = 101,
        /// <summary>
        /// (CON)��һ����ѡ��ʱ�䷶Χ�Ĵ�ʱ��ĵ�����Ϣ�ļ�¼
        /// </summary>
        C_SP_NB_2 = 102,
        /// <summary>
        /// (CON)�������ۼ��������ն��豸�ĵ�ǰϵͳʱ��
        /// </summary>
        C_TI_NA_2 = 103,
        /// <summary>
        /// (CON)�������ۼ�ʱ�εļ���(�Ʒ�) �����ۼ��� 
        /// </summary>
        C_CI_NA_2 = 104,
        /// <summary>
        /// (CON)�������ۼ�ʱ�εĺ�һ��ѡ���ĵ�ַ��Χ�ļ���(�Ʒ�)�����ۼ���
        /// </summary>
        C_CI_NB_2 = 105,
        /// <summary>
        /// (CON)��һ��ָ���Ĺ�ȥ�ۼ�ʱ�εļ���(�Ʒ�)�����ۼ���
        /// </summary>
        C_CI_NC_2 = 106,
        /// <summary>
        /// (CON)��һ��ָ���Ĺ�ȥ�ۼ�ʱ�κ� 1��ѡ���ĵ�ַ��Χ�ļ���(�Ʒ�)�����ۼ���
        /// </summary>
        C_CI_ND_2 = 107,
        /// <summary>
        /// (CON)�����ڵظ�λ�������ۼ�ʱ�εļ���(�Ʒ�)�����ۼ���
        /// </summary>
        C_CI_NE_2 = 108,
        /// <summary>
        /// (CON)�����ڵظ�λ�������ۼ�ʱ�κ�һ��ѡ���ĵ�ַ��Χ�ļ���(�Ʒ�)�����ۼ���
        /// </summary>
        C_CI_NF_2 = 109,
        /// <summary>
        /// (CON)��һ��ָ���Ĺ�ȥ�ۼ�ʱ�ε����ڵظ�λ�ļ���(�Ʒ�)�����ۼ���
        /// </summary>
        C_CI_NG_2 = 110,
        /// <summary>
        /// (CON)��һ��ָ���Ĺ�ȥ�ۼ�ʱ�κ�һ��ѡ���ĵ�ַ��Χ�����ڵظ�λ�ļ���(�Ʒ�) �����ۼ���
        /// </summary>
        C_CI_NH_2 = 111,
        /// <summary>
        /// (CON)�������ۼ�ʱ�ε����е����ۼ���
        /// </summary>
        C_CI_NI_2 = 112,
        /// <summary>
        /// (CON)�������ۼ�ʱ�εĺ�һ��ѡ���ĵ�ַ��Χ���е����ۼ���
        /// </summary>
        C_CI_NK_2 = 113,
        /// <summary>
        /// (CON)��һ��ָ���Ĺ�ȥ�ۼ�ʱ�ε����е����ۼ���
        /// </summary>
        C_CI_NL_2 = 114,
        /// <summary>
        /// (CON)��һ��ָ���Ĺ�ȥ�ۼ�ʱ�κ�һ��ѡ���ĵ�ַ��Χ�����е����ۼƶ�
        /// </summary>
        C_CI_NM_2 = 115,
        /// <summary>
        /// (CON)�����ڵظ�λ�������ۼ�ʱ�ε����е����ۼ���
        /// </summary>
        C_CI_NN_2 = 116,
        /// <summary>
        /// (CON)�����ڵظ�λ�������ۼ�ʱ�κ�һ ��ѡ���ĵ�ַ��Χ�����е����ۼ���
        /// </summary>
        C_CI_NO_2 = 117,
        /// <summary>
        /// (CON)��һ��ָ����ȥ���ۼ�ʱ�ε����ڵظ�λ�����е����ۼ���
        /// </summary>
        C_CI_NP_2 = 118,
        /// <summary>
        /// (CON)��һ��ָ���Ĺ�ȥ�ۼ�ʱ�κ�һ��ѡ���ĵ�ַ��Χ�����ڵظ�λ�����е����ۼ���
        /// </summary>
        C_CI_NQ_2 = 119,
        /// <summary>
        /// (CON)��һ��ѡ����ʱ�䷶Χ��һ��ѡ���ĵ�ַ��Χ�ļ���(�Ʒ�)�����ۼ���
        /// </summary>
        C_CI_NR_2 = 120,
        /// <summary>
        /// (CON)�����ڵظ�λ��һ��ѡ����ʱ�䷶Χ��һ��ѡ���ĵ�ַ��Χ�ļ���(�Ʒ�)�����ۼ���
        /// </summary>
        C_CI_NS_2 = 121,
        /// <summary>
        /// (CON)��һ��ѡ����ʱ�䷶Χ��һ��ѡ���ĵ�ַ��Χ�����е����ۼ���
        /// </summary>
        C_CI_NT_2 = 122,
        /// <summary>
        /// (CON)�����ڵظ�λ��һ��ѡ����ʱ�䷶Χ��һ��ѡ���ĵ�ַ��Χ�����е����ۼ���
        /// </summary>
        C_CI_NU_2 = 123,
        /// <summary>
        /// (CON)ʱ��ͬ������
        /// </summary>
        C_SYN_TA_2 = 128,

    }

}
