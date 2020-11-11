using System;
using System.Collections.Generic;
using System.Text;
using WorkScheduleSystem.BaseModels.Models;

namespace WorkScheduleSystem.Common.AttributeExtend
{
    public abstract class AbstractValidateAttribute : Attribute
    {
        public abstract ValidateErrorModel Validate(object oValue);
    }
}
