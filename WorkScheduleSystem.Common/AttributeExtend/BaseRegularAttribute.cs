using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WorkScheduleSystem.BaseModels.Models;

namespace WorkScheduleSystem.Common.AttributeExtend
{
    public class BaseRegularAttribute : AbstractValidateAttribute
    {
        private string _Regualr = null;
        public BaseRegularAttribute(string regualr)
        {
            this._Regualr = regualr;
        }


        public override ValidateErrorModel Validate(object oValue)
        {
            ValidateErrorModel validateErrorModel = new ValidateErrorModel() { };
            if (oValue != null &&
                !string.IsNullOrWhiteSpace(oValue.ToString()) &&
                Regex.IsMatch(oValue.ToString(), _Regualr))
            {
                validateErrorModel.Result = true;
                validateErrorModel.Message = "驗證通過!";
            }
            else
            {
                validateErrorModel.Result = false;
                validateErrorModel.Message = "資料格式錯誤!";
            }
            return validateErrorModel;
        }
    }
}
