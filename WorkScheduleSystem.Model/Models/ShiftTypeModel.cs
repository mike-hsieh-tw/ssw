using System;
using System.Collections.Generic;
using System.Text;
using WorkScheduleSystem.BaseModels.Models;
using WorkScheduleSystem.Common.AttributeExtend;

namespace WorkScheduleSystem.Model.Models
{
    [Mapping("ShiftType")]
    public class ShiftTypeModel : BaseModel
    {
      public  string name             {get;set;}
      public  string displayName        { get;set;}
      public  string startTime            {get;set;}
      public  string endTime              {get;set;}
      public  double scheduleHours      { get;set;} // 表定工作時數
      public  double actualHours        { get;set;} // 實際工作時數(扣掉休息時間)
      public  bool status                {get;set;}
      public DateTime? createDatetime   {get;set;}
      public DateTime? updateDatetime   {get;set;}
      public  string updateEmp        {get;set;}
    }
}
