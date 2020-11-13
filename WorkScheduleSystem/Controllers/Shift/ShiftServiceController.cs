using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkScheduleSystem.BaseModels.Models;
using WorkScheduleSystem.Common;
using WorkScheduleSystem.Controllers.Shift.Service;
using WorkScheduleSystem.IDAL;
using WorkScheduleSystem.Model.Models;
using WorkScheduleSystem.Models;
using WorkScheduleSystem.Utilities.CookiesHelper;

namespace WorkScheduleSystem.Controllers.Shift
{
    public class ShiftServiceController : Controller
    {
        private ShiftService _shiftService;
        private ShiftService shiftService
        {
            get
            {
                if (_shiftService == null)
                { _shiftService = new ShiftService(); }
                return _shiftService;
            }
        }

        // GET: ShiftService
        [HttpGet]
        public JsonResult GetUserInfo()
        {
            APIResult result = new APIResult();
            UsersModel userInfo = new UsersModel();
            int Uid = AuthCookies.GetAuthUserUid();
            if (Uid > 0) // cookie錯誤
            {
                #region 取得UserInfo
                userInfo = shiftService.GetUserInfoById(Uid);
                userInfo.password = null;
                userInfo.account = null;
                #endregion

                result.Status = 200;
                result.Message = "success";
            }
            else
            {
                result.Status = 401;
                result.Message = "fail";
            }            

            return Json(
                new {
                    Status = result.Status,
                    Message = result.Message,
                    Data = userInfo
                }, JsonRequestBehavior.AllowGet);
        }

        // 初始化頁面取得資料
        [HttpGet]
        public JsonResult GetForm()
        {
            ShiftFormViewModel viewModel;
            viewModel = new ShiftFormViewModel
            {
                DepartmentModelList = shiftService.GetDepartmentInfo(),
                ShiftTypeModelList = shiftService.GetShiftTypeInfo()
            };
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        // 取得班表列表 By 對應部門ID
        [HttpGet]
        public JsonResult GetShiftSystemByDepartmentId(int id)
        {
            var result = shiftService.GetShiftSystemByDepartmentId(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // 取得班表內容 by ShiftSystemID(班表ID)
        [HttpGet]
        public JsonResult GetShiftAllDataViewModel(int sId)
        {
            ShiftAllDataViewModel viewModel;

            var ShiftSystemDates = shiftService.GetShiftSystemDates(sId);

            viewModel = new ShiftAllDataViewModel {
                strDate = ShiftSystemDates.startDate.ToString("u"), // 起始日
                endDate = ShiftSystemDates.endDate.ToString("u"), // 結束日
                hasNationalHoliday = ShiftSystemDates.hasNationalHoliday, // 是否有國慶假日
                ShiftUnitDataViewModelList = shiftService.GetShiftUnitDataViewModelList(sId)
            };
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        // 儲存排班表資訊
        [HttpPost]
        public JsonResult SaveShiftFormData(List<ShiftUnitDataViewModel> shiftFormData)
        {
            APIResult apiResult = new APIResult();
            APIResult ShiftScheduleResult = shiftService.InsertDataToShiftScheduleModel(shiftFormData);     // 每日班表資訊               
            if (ShiftScheduleResult.Message == "fail" )
            {
                apiResult.Status = 401;
                apiResult.Message = "fail";
                apiResult.DataList = new {
                    ShiftScheduleResult = ShiftScheduleResult.DataList,
                };
            }
            else
            {
                apiResult.Status = 200;
                apiResult.Message = "success";
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        // 儲存排班總時數表資訊
        [HttpPost]
        public JsonResult SaveShiftHoursFormData(List<ShiftHoursUnitDataViewModel> shiftFormHoursData)
        {
            APIResult apiResult = new APIResult();           
            APIResult ShiftScheduleHoursResult = shiftService.InsertDataToShiftScheduleHoursModel(shiftFormHoursData); // 總班表時數統計

            if (ShiftScheduleHoursResult.Message == "fail")
            {
                apiResult.Status = 401;
                apiResult.Message = "fail";
                apiResult.DataList = new
                {
                    ShiftScheduleHoursResult = ShiftScheduleHoursResult.DataList
                };
            }
            else
            {
                apiResult.Status = 200;
                apiResult.Message = "success";
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sid">班表sid</param>
        /// <param name="uid">用戶uid</param>
        /// <param name="strDate">當前班表起始日期</param>
        /// <param name="depId">當前部門</param>
        /// <returns></returns>
        [HttpGet]        
        public JsonResult GetShiftScheduleHours(int sid, int uid, string strDate, int depId)
        {
            APIResult apiResult = new APIResult();

            try
            {
                var ShiftScheduleHours = shiftService.GetShiftScheduleHoursModelByStId(sid, uid);
                var previousShiftScheduleHours = shiftService.GetPreviousShiftScheduleHours(depId, strDate);
                var previousShiftSystem = shiftService.GetPreviousShiftSystem(depId, strDate);

                apiResult.Status = 200;
                apiResult.Message = "success";
                apiResult.DataList = new {
                    ShiftScheduleHours, // 當前總時數表
                    previousShiftScheduleHours,  // 前一個總時數表
                    previousShiftSystem // 前一個班表
                };
            }
            catch (Exception ex)
            {
                apiResult.Status = 401;
                apiResult.Message = "fail";
                apiResult.Data = ex.ToString();
            }                        

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }



    }
}