using System;
using System.Collections.Generic;
using System.Text;

namespace WorkScheduleSystem.Common.AttributeExtend.Property
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ColumnAttribute:Attribute
    {
        public string Column { get;private set; }

        public ColumnAttribute(string column)
        {
            this.Column = column;
        }
    }
}
