using System;
using System.Collections.Generic;
using WorkScheduleSystem.BaseModels.Models;

namespace WorkScheduleSystem.IDAL
{
    public interface IBaseDAL
    {
        T Find<T>(int Id) where T : BaseModel;
        List<T> FindAll<T>() where T : BaseModel;
        int Add<T>(T t) where T : BaseModel;
        bool Update<T>(T t) where T : BaseModel;
        bool Delete<T>(T t) where T : BaseModel;
    }
}
