using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using System.Web.Http;
using System.Web.Routing;
#region WebApi
//Visual Studio �w�N ASP.NET Web API 2 �� ��� �̩ۨʷs�W�ܱM�� 'WorkScheduleSystem'�C

//�M�פ��� Global.asax.cs �ɮ׻ݭn��L�ܧ�A�~��ҥ� ASP.NET Web API�C

//1. �s�W�U�C�R�W�Ŷ��Ѧ�:

//    using System.Web.Http;
//    using System.Web.Routing;

//2.�p�G�{���X�|���w�q Application_Start ��k�A�зs�W�U�C��k:

//    protected void Application_Start()
//{
//}

//3.�N�H�U�X��s�W�� Application_Start ��k���}�Y�B:

//    GlobalConfiguration.Configure(WebApiConfig.Register);
#endregion

namespace WorkScheduleSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // WebApi
            GlobalConfiguration.Configure(WebApiConfig.Register);


            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
