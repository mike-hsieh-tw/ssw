using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace WorkScheduleSystem.Common
{
    public static class StaticConstraint
    {
        private static string ConnString = "";
        private static string DLLName = "";
        private static string TypeName = "";

        static StaticConstraint()
        {
            ConnString = ConfigurationManager.ConnectionStrings["WSS"].ConnectionString;
            DLLName = ConfigurationManager.AppSettings["DllSetting"].Split(',')[0];
            TypeName = ConfigurationManager.AppSettings["DllSetting"].Split(',')[1];
        }

        public static string GetConnString()
        {
            return ConnString;
        }

        public static string GetDLLName()
        {
            return DLLName;
        }

        public static string GetTypeName()
        {
            return TypeName;
        }
    }
}
