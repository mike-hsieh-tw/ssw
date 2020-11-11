using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkScheduleSystem.BaseModels.Models;
using WorkScheduleSystem.Common;
using WorkScheduleSystem.Model.Models;
using WorkScheduleSystem.Common.AttributeExtend.Helper;
using WorkScheduleSystem.Utilities.CookiesHelper;

namespace WorkScheduleSystem.Controllers.Login
{
    public class LoginServiceController : Controller
    {
        [HttpGet]
        public JsonResult AuthCookieCheck()
        {
            APIResult result = new APIResult();
            if (AuthCookies.AuthCookiesReader() == 1) {
                // 查無此帳戶
                result.Status = 200;
                result.Message = "success";
                result.Data = "帳戶驗證通過!";
            }
            else
            {
                result.Status = 401;
                result.Message = "fial";
                result.Data = "帳戶驗證失敗，請重新登入!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        // GET: LoginService
        // 鎖定帳號
        [HttpPost]
        public JsonResult DisableUser(UsersModel user)
        {
            var IsUpdate = false;
            APIResult result = new APIResult();

            UsersModel User = SimpleFactory.CreateInstance().FindAll<UsersModel>().Where(u => u.account == user.account).FirstOrDefault();

            // 是否有該帳戶
            if (User == null)
            {
                // 查無此帳戶
                result.Status = 400;
                result.Message = "fail";
                result.Data = "查無此帳戶!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                User.updateDatetime = DateTime.Now;
                User.updateEmp = "Admin";
                User.status = false;
            }
            

            if (!User.Validate<UsersModel>(out List<string> errorList))
            {
                // 如果欄位驗證失敗
                result.Status = 400;
                result.Message = "fail";
                //result.Data = string.Join(",", errorList.ToArray());
                result.Data = "帳戶鎖定失敗";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            IsUpdate = SimpleFactory.CreateInstance().Update<UsersModel>(User);                        
            if (IsUpdate) 
            {
                // 更新成功，帳戶鎖定成功
                result.Status = 200;
                result.Message = "Success";
                result.Data = "該帳戶已鎖定，請通知主管!";
            }
            else
            {   // 更新失敗，帳戶鎖定失敗
                result.Status = 500;
                result.Message = "fail";
                result.Data = "帳戶鎖定失敗";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}