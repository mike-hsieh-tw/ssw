using System;
using System.Collections.Generic;
using System.Reflection;
using WorkScheduleSystem.BaseModels.Models;
using WorkScheduleSystem.IDAL;
using System.Data.SqlClient;
using System.Linq;
using WorkScheduleSystem.Common;
using WorkScheduleSystem.Common.AttributeExtend.Helper;

namespace WorkScheduleSystem.DAL
{
    public class BaseDAL : IBaseDAL
    {
        public int Add<T>(T t) where T : BaseModel
        {           
            Type type = t.GetType();
            return ExecuteSql<int>(AdvanceSqlBuilder<T>.Add, (cmd) =>
            {
                var parameterList = type
                               .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                               .Select(p => new SqlParameter($"@{p.Name}", p.GetValue(t) ?? DBNull.Value));
                cmd.Parameters.AddRange(parameterList.ToArray());
                return (int)cmd.ExecuteScalar();
            });
        }

        public bool Delete<T>(T t) where T : BaseModel
        {                      
            return ExecuteSql<bool>(AdvanceSqlBuilder<T>.Delete, (cmd) => {
                cmd.Parameters.AddWithValue("@Id", t.Id);
                return cmd.ExecuteNonQuery() == 1;
            });
        }

        public T ExecuteSql<T>(string sql, Func<SqlCommand, T> func)
        {
            using (SqlConnection conn = new SqlConnection(Singleton.GetConnectionString())) 
            {
                T result = default(T); 
                string ErrorMsg = "";
                SqlTransaction tran = null; 
                try
                {
                    conn.Open(); 
                    using (tran = conn.BeginTransaction())
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, conn, tran)) 
                        {
                            //LogMethod.WriteLog($"[T-SQL] {sql}");
                            result = func.Invoke(cmd); 
                            tran.Commit(); 
                        }
                    }
                }
                catch (Exception ex1)
                {
                    try
                    {
                        ErrorMsg = $"{ex1}";
                        tran.Rollback(); 
                    }
                    catch (Exception ex2)
                    {
                       
                        ErrorMsg += $"\r\n{ex2}";
                    }
                }
                finally
                {
                    // 若是有錯誤資料就寫log
                    //if (!string.IsNullOrEmpty(ErrorMsg)) LogMethod.WriteLog(ErrorMsg, true);
                }
                return result;
            }
        }

        private T Trans<T>(Type type, SqlDataReader dr) where T : BaseModel
        {
            object oObject = Activator.CreateInstance(type);

            foreach (var prop in type.GetProperties())
            {                         
                prop.SetValue(oObject, dr[prop.GetColumnMappingName()] is DBNull ? null : dr[prop.GetColumnMappingName()]);
            }

            return (T)oObject;
        }

        public T Find<T>(int Id) where T : BaseModel
        {           
            Type type = typeof(T);
            string sql = AdvanceSqlBuilder<T>.FindSql;
            return ExecuteSql<T>(sql, (cmd) => {
                cmd.Parameters.AddWithValue("@Id", Id);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return Trans<T>(type, dr);
                    }
                    else
                    {
                        return null;
                    }
                }
            });
        }


        public List<T> FindAll<T>() where T : BaseModel
        {           
            Type type = typeof(T);

            string sql = AdvanceSqlBuilder<T>.FindSqlAll;

            return ExecuteSql<List<T>>(sql, (cmd) =>
            {
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    List<T> tList = new List<T>();
                    while (dr.Read())
                    {
                        tList.Add(Trans<T>(type, dr));
                    }
                    return tList;
                }
            });
        }
        public bool Update<T>(T t) where T : BaseModel
        {
            
            Type type = typeof(T);
            string sql = AdvanceSqlBuilder<T>.Update;
            var parameterList = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Select(p => new SqlParameter(p.Name, p.GetValue(t) ?? DBNull.Value));
            return ExecuteSql<bool>(sql, (cmd) =>
            {
                cmd.Parameters.AddRange(parameterList.ToArray());
                cmd.Parameters.AddWithValue("@Id", t.Id);
                return cmd.ExecuteNonQuery() == 1;
            });

        }
    }
}
