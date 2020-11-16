using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WorkScheduleSystem.BaseModels.Models;
using WorkScheduleSystem.Common;
using WorkScheduleSystem.Model.Models;
using WorkScheduleSystem.Models;

namespace WorkScheduleSystem.Controllers.Shift.Service
{
    public class ShiftService
    {
        // 取得用戶資料
        public UsersModel GetUserInfoById(int id)
        {
            return SimpleFactory.CreateInstance().Find<UsersModel>(id);
        }

        // 取得所有部門資料
        public List<DepartmentModel> GetDepartmentInfo()
        {
            return SimpleFactory.CreateInstance().FindAll<DepartmentModel>();
        }

        // 取得班別資料
        public List<ShiftTypeModel> GetShiftTypeInfo()
        {
            return SimpleFactory.CreateInstance().FindAll<ShiftTypeModel>();
        }

        // 取得班表列表 By 對應部門ID
        public List<ShiftSystemModel> GetShiftSystemByDepartmentId(int id)
        {
            return SimpleFactory.CreateInstance().FindAll<ShiftSystemModel>().AsQueryable().Where(x => x.departmentID == id).OrderBy(x => x.startDate).ToList();
        }

        // 取得班表的
        public List<ShiftScheduleHoursModel> GetShiftScheduleHoursModelByStId(int sid, int uid)
        {
            return SimpleFactory.CreateInstance().FindAll<ShiftScheduleHoursModel>().AsQueryable().Where(x => x.sId == sid && x.uId == uid).ToList();
        }

        // 取得人員排班資料 By 班表總表ID
        public List<ShiftUnitDataViewModel> GetShiftUnitDataViewModelList(int sId)
        {
            using (SqlConnection conn = new SqlConnection(Singleton.GetConnectionString()))
            {
                List<ShiftUnitDataViewModel> result = new List<ShiftUnitDataViewModel>();
                string sql = $@"
                    select 
	                    u.id as 'uId', -- 使用者ID
	                    u.name as 'uName', -- 使用者名稱
	                    sSche.sId as 'sId', -- 班表ID
	                    sSche.shiftDate as 'sDate', -- 排班日期
	                    sSche.memo as 'sMemo', -- 備註
	                    sSche.stId as 'stId', -- 班別ID
	                    sType.name as 'sName', -- 班別名稱
                        sSche.hours as 'sHours', -- 實際排班時數  
                	    sSche.createDatetime -- 創建日期 排序用
                    from ShiftSchedule sSche
                    inner join ShiftSystem  sSyst on sSche.sid = sSyst.id
                    inner join Users u on  u.id =  sSche.uId
                    inner join ShiftType sType on sType.id = sSche.stId
                    where sSche.sId = @sId -- 篩選條件
                    order by sDate, createDatetime desc -- 創建日期 排序用
                ";
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
                            cmd.Parameters.AddWithValue("@sId", sId);
                            SqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                ShiftUnitDataViewModel item = new ShiftUnitDataViewModel {
                                    sId = (int)dr["sId"],
                                    uId = (int)dr["uId"],
                                    stId = (int)dr["stId"],
                                    sDate = Convert.ToDateTime(dr["sDate"]).ToString("u"),
                                    sHours = (double)dr["sHours"],
                                    sMemo = dr["sMemo"].ToString(),
                                    sName = dr["sName"].ToString(),
                                    uName = dr["uName"].ToString()
                                };
                                result.Add(item);
                            }                            
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

        // 取得上一個班表的總時數資料
        public ShiftUnitSystemViewModel GetPreviousShiftSystem(int departmentID, string startDate)
        {
            using (SqlConnection conn = new SqlConnection(Singleton.GetConnectionString()))
            {
                ShiftUnitSystemViewModel result = new ShiftUnitSystemViewModel();
                string sql = $@"
                     SELECT 
                        [departmentID],
	                    [name],
	                    [startDate],
	                    [endDate],
	                    [status],
	                    [hasNationalHoliday]
                    FROM [ShiftSystem]
                    WHERE id = (
	                    SELECT  TOP 1 id 
	                    FROM ShiftSystem
	                    WHERE departmentID = @departmentID AND startDate < @startDate
	                    ORDER BY startDate DESC
                    )
                ";
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
                            cmd.Parameters.AddWithValue("@departmentID", departmentID);
                            cmd.Parameters.AddWithValue("@startDate", startDate.Substring(0, 10));
                            SqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                result.departmentID = (int)dr["departmentID"];
                                result.name = dr["name"].ToString();
                                result.startDate = Convert.ToDateTime(dr["startDate"]);
                                result.endDate = Convert.ToDateTime(dr["endDate"]);
                                result.status = (int)dr["status"];
                                result.hasNationalHoliday = (bool)dr["hasNationalHoliday"];
                            }
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

        // 取得上一個班表的總時數資料
        public ShiftHoursUnitDataViewModel GetPreviousShiftScheduleHours(int departmentID, string startDate)
        {
            using (SqlConnection conn = new SqlConnection(Singleton.GetConnectionString()))
            {
                ShiftHoursUnitDataViewModel result = new ShiftHoursUnitDataViewModel();
                string sql = $@"
                    SELECT 
                        [sId],
	                    [uId],
	                    [totalShiftHours],
	                    [totalSetShiftHours],
	                    [totalSetSpcHours],
	                    [totalNormalFixHours],
	                    [totalNationalFixHours]
                    FROM [ShiftScheduleHours]
                    WHERE sid = (
	                    SELECT  TOP 1 id 
	                    FROM ShiftSystem
	                    WHERE departmentID = @departmentID AND startDate < @startDate
	                    ORDER BY startDate DESC
                    )
                ";
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
                            cmd.Parameters.AddWithValue("@departmentID", departmentID);
                            cmd.Parameters.AddWithValue("@startDate", startDate.Substring(0, 10));
                            SqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                result.sId = (int)dr["sId"];
                                result.uId = (int)dr["uId"];
                                result.totalShiftHours = (double)dr["totalShiftHours"];
                                result.totalSetSpcHours = (double)dr["totalSetSpcHours"];
                                result.totalSetShiftHours = (double)dr["totalSetShiftHours"];
                                result.totalNormalFixHours = (double)dr["totalNormalFixHours"];
                                result.totalNationalFixHours = (double)dr["totalNationalFixHours"];
                            }
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


        // 取得班表的起始日和結束日
        public ShiftSystemModel GetShiftSystemDates(int sId)
        {
            var result = SimpleFactory.CreateInstance().Find<ShiftSystemModel>(sId);
            return result;
        }
       

        // 新增個人每日班表資訊
        public APIResult InsertDataToShiftScheduleModel(List<ShiftUnitDataViewModel> shiftFormData)
        {
            APIResult apiResult = new APIResult();
            List<string> errDate = new List<string>();
            foreach (var item in shiftFormData)
            {
                // 找出是否有這筆資料
                var currItem = SimpleFactory.CreateInstance().FindAll<ShiftScheduleModel>().AsQueryable().Where(x =>
                x.shiftDate == DateTime.ParseExact(item.sDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces) &&
                x.stId == item.stId &&
                x.hours == item.sHours &&
                x.memo == item.sMemo
                ).FirstOrDefault();

                // 若是沒有才新增進去
                if (currItem == null)
                {
                    ShiftScheduleModel shiftScheduleModelData = new ShiftScheduleModel()
                    {
                        sId = item.sId,
                        uId = item.uId,
                        stId = item.stId,
                        shiftDate = DateTime.ParseExact(item.sDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces), // string "yyyyMMdd" => Datetime 
                        hours = item.sHours,
                        memo = item.sMemo,
                        createDatetime = DateTime.Now,
                        updateEmp = item.updateEmp
                    };
                    int NewId = SimpleFactory.CreateInstance().Add<ShiftScheduleModel>(shiftScheduleModelData);

                    if (NewId == 0)
                    {
                        errDate.Add(item.sDate);
                    }
                }


            }

            if (errDate.Count != 0)
            {
                apiResult.Status = 401;
                apiResult.Message = "fail";
                apiResult.DataList = errDate;
            }
            else
            {
                apiResult.Status = 200;
                apiResult.Message = "success";
            }
            return apiResult;
        }

        // 新增/修改入排班時數統計表
        public APIResult InsertDataToShiftScheduleHoursModel(List<ShiftHoursUnitDataViewModel> shiftFormData)
        {
            APIResult apiResult = new APIResult();
            List<string> errUid = new List<string>();
            foreach (var item in shiftFormData)
            {
                // 找出是否有這筆資料
                var currItem = SimpleFactory.CreateInstance().FindAll<ShiftScheduleHoursModel>().AsQueryable().Where(x =>
                    x.uId == item.uId &&
                    x.sId == item.sId
                ).FirstOrDefault();

                // 若是沒有才新增進去
                if (currItem == null)
                {
                    ShiftScheduleHoursModel shiftScheduleHoursModelData = new ShiftScheduleHoursModel()
                    {
                        sId = item.sId,
                        uId = item.uId,
                        totalShiftHours = item.totalShiftHours,
                        totalSetShiftHours = item.totalSetShiftHours,
                        totalSetSpcHours = item.totalSetSpcHours,
                        totalNormalFixHours = item.totalNormalFixHours,
                        totalNationalFixHours = item.totalNationalFixHours,
                        updateDatetime = DateTime.Now,
                        createDatetime = DateTime.Now,
                        updateEmp = item.updateEmp
                    };
                    int NewId = SimpleFactory.CreateInstance().Add<ShiftScheduleHoursModel>(shiftScheduleHoursModelData);

                    if (NewId == 0)
                    {
                        errUid.Add(item.uId.ToString());

                        apiResult.Status = 401;
                        apiResult.Message = "fail";
                    }
                }
                else // 若是有資料，使用更新
                {
                    ShiftScheduleHoursModel shiftScheduleHoursModelData = new ShiftScheduleHoursModel()
                    {
                        Id = currItem.Id,
                        sId = item.sId,
                        uId = item.uId,
                        totalShiftHours = item.totalShiftHours,
                        totalSetShiftHours = item.totalSetShiftHours,
                        totalSetSpcHours = item.totalSetSpcHours,
                        totalNormalFixHours = item.totalNormalFixHours,
                        totalNationalFixHours = item.totalNationalFixHours,
                        updateDatetime = DateTime.Now,
                        createDatetime = currItem.createDatetime,
                        updateEmp = item.updateEmp
                    };
                    bool result = SimpleFactory.CreateInstance().Update<ShiftScheduleHoursModel>(shiftScheduleHoursModelData);

                    if (result == false)
                    {
                        errUid.Add(item.updateEmp.ToString());

                        apiResult.Status = 401;
                        apiResult.Message = "fail";
                    }
                }

            }
            if (errUid.Count != 0)
            {
                apiResult.Status = 401;
                apiResult.Message = "fail";
                apiResult.DataList = errUid;
            }
            else
            {
                apiResult.Status = 200;
                apiResult.Message = "success";
            }

            return apiResult;
        }


    }
}