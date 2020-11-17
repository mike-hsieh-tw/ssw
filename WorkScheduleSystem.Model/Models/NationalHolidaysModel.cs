using System;
using System.Collections.Generic;
using System.Text;
using WorkScheduleSystem.BaseModels.Models;
using WorkScheduleSystem.Common.AttributeExtend;

namespace WorkScheduleSystem.Model.Models
{
    [Mapping("NationalHolidays")]
    public class NationalHolidaysModel : BaseModel
    {
       public string name { get; set; }
       public int? year                  {get;set;}
       public DateTime? strDate         {get;set;}
       public DateTime? endDate         {get;set;}
       public DateTime? createDatetime  {get;set;}
       public string updateEmp          {get;set;}

    }
}
