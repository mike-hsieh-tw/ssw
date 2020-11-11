using System;
using System.Collections.Generic;
using System.Text;
using WorkScheduleSystem.BaseModels.Models;
using WorkScheduleSystem.Common.AttributeExtend;

namespace WorkScheduleSystem.Model.Models
{
    [Mapping("Note")]
    public class NoteModel : BaseModel
    {
      public string title            {get;set;}
      public string description      {get;set;}
      public DateTime startDatetime  {get;set;}
      public DateTime endDatetime    {get;set;}
      public bool? Enabled            {get;set;}
      public DateTime? createDatetime {get;set;}
      public DateTime? updateDatetime {get;set;}
      public string updateEmp        {get;set;}
    }
}
