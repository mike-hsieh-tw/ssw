using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WorkScheduleSystem.IDAL;

namespace WorkScheduleSystem.Common
{
    public class SimpleFactory
    {
        private static string DLLName = StaticConstraint.GetDLLName();
        private static string TypeName = StaticConstraint.GetTypeName();

        public static IBaseDAL CreateInstance()
        {
            Assembly assembly = Assembly.Load(DLLName);
            Type type = assembly.GetType(TypeName);
            return Activator.CreateInstance(type) as IBaseDAL;   
        }
    }
}
