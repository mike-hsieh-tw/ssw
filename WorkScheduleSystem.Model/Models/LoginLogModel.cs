using System;
using WorkScheduleSystem.BaseModels.Models;
using WorkScheduleSystem.Common.AttributeExtend;
using WorkScheduleSystem.Common.AttributeExtend.Property;

namespace WorkScheduleSystem.Model.Models
{
    [Mapping("LoginLog")]
    public class LoginLogModel: BaseModel
    {
        [Column("帳號")]
        public string account { get; set; }
        [Column("登入時間")]
        public DateTime loginDatetime { get; set; }
    }
}
