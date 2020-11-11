using System;
using System.Collections.Generic;
using System.Text;
using WorkScheduleSystem.BaseModels.Models;

namespace WorkScheduleSystem.Common.AttributeExtend.Property
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PhoneValidationAttribute : BaseRegularAttribute
    {
        private static string PhonePattern = @"[0][9]\d{8}|[0][9]\d{2}-\d{3}-\d{3}";
        public PhoneValidationAttribute() : base(PhonePattern)
        {
        }

        public override ValidateErrorModel Validate(object oValue) 
        {
            return base.Validate(oValue); 
        }
    }
}
