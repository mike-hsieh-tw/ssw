using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WorkScheduleSystem.Common.AttributeExtend;
using WorkScheduleSystem.Common.AttributeExtend.Property;

namespace WorkScheduleSystem.Common.Cache
{
    public static class PropertyInfoCache
    {
        /// <summary>
        /// 欄位名稱 <=> 中文名稱
        /// </summary>
        public static Dictionary<PropertyInfo, string> ColumnChineseNameCache = new Dictionary<PropertyInfo, string>();

        /// <summary>
        /// 程式端[欄位]名稱 <=> 資料庫實際欄[位]名稱
        /// </summary>
        public static Dictionary<PropertyInfo, string> ColumnMappingNameCache = new Dictionary<PropertyInfo, string>();


        /// <summary>
        /// 取得欄位對應中文名稱
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static string GetColumnChineseName(PropertyInfo prop)
        {
            // 判斷緩存中是否已存在該屬性key值
            if (ColumnChineseNameCache.ContainsKey(prop))
            {
                return ColumnChineseNameCache[prop]; // 有的就回傳中文值
            }
            else
            {
                if (prop.IsDefined(typeof(ColumnAttribute), inherit: true)) // 使用取得的 Prop 來判斷是否有該特性
                {
                    var Attr = (ColumnAttribute)prop.GetCustomAttribute(typeof(ColumnAttribute));
                    ColumnChineseNameCache.Add(prop, Attr.Column); // 把中文加入到字典緩存中
                    return Attr.Column;
                }
                else
                {
                    return prop.Name;
                }
            }
        }

        /// <summary>
        /// 取得 程式端[欄位]名稱 <=> 資料庫實際欄[位]名稱
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static string GetColumnMappingName(PropertyInfo prop)
        {
            // 判斷緩存中是否已存在該屬性key值
            if (ColumnMappingNameCache.ContainsKey(prop))
            {
                return ColumnMappingNameCache[prop]; // 有的就回傳中文值
            }
            else
            {
                if (prop.IsDefined(typeof(MappingAttribute), inherit: true)) // 使用取得的 Prop 來判斷是否有該特性
                {
                    var Attr = (MappingAttribute)prop.GetCustomAttribute(typeof(MappingAttribute));
                    ColumnMappingNameCache.Add(prop, Attr.MappingName); // 把中文加入到字典緩存中
                    return Attr.MappingName;
                }
                else
                {
                    return prop.Name;
                }
            }
        }




    }
}
