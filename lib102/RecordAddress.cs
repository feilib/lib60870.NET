using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib102
{
    /// <summary>
    /// 记录地址（RAD）
    /// </summary>
    public enum RecordAddress
    {
        /// <summary>
        /// 缺省
        /// </summary>
        Default = 0,
        /// <summary>
        /// 从记账（计费）时段开始的电能累计量的记录地址
        /// </summary>
        Total = 1,
        /// <summary>
        /// 电能累计量累计时段1的记录地址
        /// </summary>
        Period_Total_1 = 11,
        /// <summary>
        /// 电能累计量累计时段2的记录地址
        /// </summary>
        Period_Total_2 = 12,
        /// <summary>
        /// 电能累计量累计时段3的记录地址
        /// </summary>
        Period_Total_3 = 13,
        /// <summary>
        /// 电能累计量（日值）累计时段1的记录地址
        /// </summary>
        Period_Day_1 = 21,
        /// <summary>
        /// 电能累计量（日值）累计时段2的记录地址
        /// </summary>
        Period_Day_2 = 22,
        /// <summary>
        /// 电能累计量（日值）累计时段3的记录地址
        /// </summary>
        Period_Day_3 = 23,
        /// <summary>
        /// 电能累计量（周/旬值）累计时段1的记录地址
        /// </summary>
        Period_Week_1 = 31,
        /// <summary>
        /// 电能累计量（周/旬值）累计时段2的记录地址
        /// </summary>
        Period_Week_2 = 32,
        /// <summary>
        /// 电能累计量（周/旬值）累计时段3的记录地址
        /// </summary>
        Period_Week_3 = 33,
        /// <summary>
        /// 电能累计量（月值）累计时段1的记录地址
        /// </summary>
        Period_Monty_1 = 41,
        /// <summary>
        /// 电能累计量（月值）累计时段2的记录地址
        /// </summary>
        Period_Monty_2 = 42,
        /// <summary>
        /// 电能累计量（月值）累计时段3的记录地址
        /// </summary>
        Period_Monty_3 = 43,
        /// <summary>
        /// 最早的单点信息
        /// </summary>
        Point_Earliest = 50,
        /// <summary>
        /// 单点信息的全部记录
        /// </summary>
        Point_All = 51,
        /// <summary>
        /// 单点信息记录区段1
        /// </summary>
        Point_Section_1 = 52,
        /// <summary>
        /// 单点信息记录区段2
        /// </summary>
        Point_Section_2 = 53,
        /// <summary>
        /// 单点信息记录区段3
        /// </summary>
        Point_Section_3 = 54,
        /// <summary>
        /// 单点信息记录区段4
        /// </summary>
        Point_Section_4 = 55
    }
}
