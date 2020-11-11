using System;
using System.Collections.Generic;
using System.Text;
using WorkScheduleSystem.BaseModels.Models;
using WorkScheduleSystem.Common.AttributeExtend;

namespace WorkScheduleSystem.Model.Models
{
    [Mapping("ShiftSchedule")]
    public class ShiftScheduleModel : BaseModel
    {
      public  int sId                   {get;set;}
      public  int uId                   {get;set;}
      public  int stId                  {get;set;}
      public  DateTime? shiftDate       {get;set;}
      public double hours                {get;set;}
      public string memo                {get;set;}
      public  DateTime? createDatetime  {get;set;}
      public  string updateEmp          {get;set;}
    }
}
