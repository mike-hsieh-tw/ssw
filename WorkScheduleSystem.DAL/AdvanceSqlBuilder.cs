using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WorkScheduleSystem.BaseModels.Models;
using WorkScheduleSystem.Common.AttributeExtend.Helper;

namespace WorkScheduleSystem.DAL
{
    public class AdvanceSqlBuilder<T> where T : BaseModel
    {
        public static string FindSql = "";
        public static string FindSqlAll = "";
        public static string Add = "";
        public static string Update = "";
        public static string Delete = "";

        private static Type type = typeof(T);
        private static string TableName = AttributeHelper.GetTableMappingName<T>(); 

        static AdvanceSqlBuilder()
        {
            Type type = typeof(T);       
            string baseColumn = string.Join(",", type.GetProperties().Select(p => $"[{ p.GetColumnMappingName()}]"));                      
            string baseColumnNoId = string.Join(",", type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(p => $"[{p.GetColumnMappingName()}]"));


            #region Select one
            FindSql = $"SELECT {baseColumn} FROM [{ TableName }] WHERE Id = @Id";
            #endregion

            #region Select All
            FindSqlAll = $"SELECT {baseColumn} FROM [{TableName}]";
            #endregion


            #region Add return New item
            string addValueString = string.Join(",", type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Select(p => $"@{p.Name}"));         
            string addReturnValueString = string.Join(",", type.GetProperties()
                .Select(p => $"INSERTED.[{p.GetColumnMappingName()}]"));  
            Add = $@"INSERT [{TableName}] ({baseColumnNoId}) 
                     VALUES ({addValueString});
                     SELECT CAST(scope_identity() AS int);";
            #endregion

            #region Delete
            Delete = $@"DELETE FROM [{TableName}] WHERE Id = @Id";
            #endregion

            #region Update
            string updateColumnString = string.Join(",", type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(p => $"[{p.Name}] = @{p.Name}"));
            Update = $@"UPDATE [{TableName}] SET {updateColumnString} WHERE Id = @Id";
            #endregion

        }
    }
}
