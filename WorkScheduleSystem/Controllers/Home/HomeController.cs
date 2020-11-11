using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkScheduleSystem.Utilities.Attributes.Filters;

namespace WorkScheduleSystem.Controllers
{
    
    public class HomeController : Controller
    {
        public ActionResult Index()
        {            
            return RedirectToAction("Index","Login");
        }
    }
}