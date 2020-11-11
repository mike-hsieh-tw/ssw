using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkScheduleSystem.Controllers
{
    public class ShiftController : Controller
    {
        // GET: Shift
        // 排班
        public ActionResult Index()
        {
            return View();
        }
    }
}