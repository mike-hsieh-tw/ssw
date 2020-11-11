using System;
using System.Collections.Generic;
using System.Text;

namespace WorkScheduleSystem.Common
{
    public class Singleton
    {
        private static Singleton _DBHelper = null;

        private static string _ConnString = "";

        private Singleton()
        {
            _ConnString = StaticConstraint.GetConnString(); 
        }

        static Singleton()
        {
            _DBHelper = new Singleton();
        }

        public static string GetConnectionString()
        {
            return _ConnString;
        }
    }
}
