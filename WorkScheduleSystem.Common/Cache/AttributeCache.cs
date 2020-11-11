using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WorkScheduleSystem.BaseModels.Models;
using WorkScheduleSystem.Common.AttributeExtend;

namespace WorkScheduleSystem.Common.Cache
{
    public static class AttributeCache<T> where T : BaseModel
    {
        
        public static Dictionary<PropertyInfo, object[]> Property_Attribute_Cache = new Dictionary<PropertyInfo, object[]>();
        static AttributeCache()
        {
            Type type = typeof(T);
            foreach (PropertyInfo prop in type.GetProperties()) 
            {
                Property_Attribute_Cache.Add(prop, prop.GetCustomAttributes(typeof(AbstractValidateAttribute), true));
            }
        }
    }
}
