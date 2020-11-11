using System;
using System.Collections.Generic;
using System.Text;

namespace WorkScheduleSystem.Common.AttributeExtend
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class MappingAttribute:Attribute
    {
        public string MappingName { get; set; }
        public MappingAttribute(string mappingName)
        {
            this.MappingName = mappingName;
        }
    }
}
