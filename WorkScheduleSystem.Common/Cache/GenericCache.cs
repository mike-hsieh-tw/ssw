using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WorkScheduleSystem.Common.AttributeExtend;

namespace WorkScheduleSystem.Common.Cache
{
    public static class GenericCache<T>
    {
        public static string TypeName = null; 

        static GenericCache()
        {
            Type type = typeof(T);

            if (type.IsDefined(typeof(MappingAttribute), inherit: true))
            {
                MappingAttribute Attr = (MappingAttribute)type.GetCustomAttribute(typeof(MappingAttribute), true);
                TypeName = Attr.MappingName;
            }
            else
            {
                TypeName = type.Name;
            }
        }
    }
}
