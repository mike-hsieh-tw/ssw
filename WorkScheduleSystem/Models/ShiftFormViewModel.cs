using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorkScheduleSystem.Model.Models;

namespace WorkScheduleSystem.Models
{
    // 默認資料
    public class ShiftFormViewModel
    {
        public ShiftFormViewModel()
        {
            DepartmentModelList = new List<DepartmentModel>();
            ShiftTypeModelList = new List<ShiftTypeModel>();
        }

        public List<DepartmentModel> DepartmentModelList { get; set; }
        public List<ShiftTypeModel> ShiftTypeModelList { get; set; }
    }

    //  所有排班內容資料
    public class ShiftAllDataViewModel
    {
        public ShiftAllDataViewModel()
        {
            ShiftUnitDataViewModelList = new List<ShiftUnitDataViewModel>();
        }

        public string strDate { get; set; } // 該班表起始日期
        public string endDate { get; set; } // 該班表結束日期
        public bool hasNationalHoliday { get; set; } // 是否有國定假日
        public List<ShiftUnitDataViewModel> ShiftUnitDataViewModelList { get; set; }
    }

    //  個人排班內容/天
    public class ShiftUnitDataViewModel
    {
        public int uId { get; set; } // 員工ID
        public string uName { get; set; } // 使用者名稱
        public int sId { get; set; } // 班表ID
        public string sDate { get; set; } // 排班日期
        public string sMemo { get; set; } // 備註
        public int stId { get; set; } // 班別ID
        public string sName { get; set; } // 班別名稱
        public double sHours { get; set; } // 排班時數
        public string updateEmp { get; set; } // 更新者
    }

    //  個人排班總時數統計
    public class ShiftHoursUnitDataViewModel
    {
        public int uId { get; set; } // 員工ID
        public int sId { get; set; } // 班表ID
        public double totalShiftHours { get; set; } // 總需排班時數
        public double totalSetShiftHours { get; set; } // 總已排班時數
        public double totalSetSpcHours { get; set; } // 總特休已排時數
        public double totalNormalFixHours { get; set; } // 總計已排超額/不足時數
        public double totalNationalFixHours { get; set; } // 總計已排超額國定假日時數
        public string updateEmp { get; set; } // 更新者
    }

    // 班表
    public class ShiftUnitSystemViewModel
    {
        public int departmentID { get; set; }
        public string name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int? status { get; set; }
        public bool hasNationalHoliday { get; set; }
        public DateTime? createDatetime { get; set; }
        public DateTime? updateDatetime { get; set; }
        public string updateEmp { get; set; }
    }


}