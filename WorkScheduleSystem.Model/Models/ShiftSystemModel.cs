using System;
using System.Collections.Generic;
using System.Text;
using WorkScheduleSystem.BaseModels.Models;
using WorkScheduleSystem.Common.AttributeExtend;

namespace WorkScheduleSystem.Model.Models
{
    [Mapping("ShiftSystem")]
    public class ShiftSystemModel : BaseModel
    {
      public  int departmentID          {get;set;}
      public  string name               {get;set;}
      public  DateTime startDate        {get;set;}
      public  DateTime endDate          {get;set;}
      public  int? status               {get;set;}
      public  bool hasNationalHoliday   {get;set;}
      public  DateTime? createDatetime  {get;set;}
      public  DateTime? updateDatetime  {get;set;}
      public  string updateEmp          {get;set;}
    }
}
