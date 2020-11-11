using System;
using WorkScheduleSystem.BaseModels.Models;

namespace WorkScheduleSystem.Common.AttributeExtend.Property
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PasswordValidationAttribute : BaseRegularAttribute
    {
        // https://www.itread01.com/content/1527929888.html
        // 至少8個字符，至少1個大寫字母，1個小寫字母和1個數字:  ^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$

        // 目前不限位數，至少1個大寫字母，1個小寫字母和1個數字
        private static string PasswordPattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{0,}$";
        public PasswordValidationAttribute() : base(PasswordPattern)
        {
        }

        public override ValidateErrorModel Validate(object oValue) 
        {
            return base.Validate(oValue); 
        }
    }
}
