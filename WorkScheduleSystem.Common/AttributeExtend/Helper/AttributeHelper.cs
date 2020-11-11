using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WorkScheduleSystem.BaseModels.Models;
using WorkScheduleSystem.Common.Cache;

namespace WorkScheduleSystem.Common.AttributeExtend.Helper
{
    public static class AttributeHelper
    {
        public static string GetColumnName<T>(this PropertyInfo value) 
        {           
            return PropertyInfoCache.GetColumnChineseName(value);
        }
        
        public static string GetColumnMappingName(this PropertyInfo prop) 
        {            
            return PropertyInfoCache.GetColumnMappingName(prop);
        }

        
        public static string GetTableMappingName<T>() where T : BaseModel
        {           
            return GenericCache<T>.TypeName;
        }


        public static bool Validate<T>(this T t, out List<string> errorList) where T : BaseModel
        {            
            errorList = new List<string>();
            bool ValidateResult = true; 

            Type type = typeof(T);

            var properties = AttributeCache<T>.Property_Attribute_Cache; 

            foreach (var prop in properties.Keys) 
            {
                foreach (var attr in properties[prop]) 
                {
                    object oValue = prop.GetValue(t); 
                    ValidateErrorModel ValidateMsg = ((AbstractValidateAttribute)attr).Validate(oValue); 
                    if (ValidateMsg.Result == false) 
                    {
                        errorList.Add($"[{prop.GetColumnName<T>()}]{ValidateMsg.Message}");
                        ValidateResult = false;
                    }
                }
            }
            return ValidateResult;

        }
    }
}
