using System;
using System.Collections.Generic;
using System.Text;
using WorkScheduleSystem.BaseModels.Models;
using WorkScheduleSystem.Common.AttributeExtend;

namespace WorkScheduleSystem.Model.Models
{
    [Mapping("Department")]
    public class DepartmentModel : BaseModel
    {
       public string name { get; set; }
       public DateTime? createDatetime { get; set; }
       public DateTime? updateDatetime { get; set; }
       public string updateEmp { get; set; }

    }
}
