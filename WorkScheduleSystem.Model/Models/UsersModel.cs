using System;
using WorkScheduleSystem.BaseModels.Models;
using WorkScheduleSystem.Common.AttributeExtend;
using WorkScheduleSystem.Common.AttributeExtend.Property;

namespace WorkScheduleSystem.Model.Models
{
    [Mapping("Users")]
    public class UsersModel: BaseModel
    {        
        [Column("工號")]
        public string no { get; set; }

        [PhoneValidation]
        [Column("帳號")]
        public string account { get; set; }

        [PasswordValidation]
        [Column("密碼")]
        public string password { get; set; }
        [Column("姓名")]
        public string name { get; set; }
        [Column("狀態")]
        public bool status { get; set; }
        [Column("年度總特休時數")]
        public double spcDayOff { get; set; }
        [Column("權限")]
        public int userTypeId { get; set; }
        [Column("創建時間")]
        public DateTime createDatetime { get; set; }
        [Column("異動時間")]
        public DateTime updateDatetime { get; set; }
        [Column("最後異動人員")]
        public string updateEmp { get; set; }
        [Column("所屬部門id")]
        public Nullable<int> depID { get; set; }
    }
}
