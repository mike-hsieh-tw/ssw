using System;
using System.Collections.Generic;
using System.Text;
using WorkScheduleSystem.BaseModels.Models;
using WorkScheduleSystem.Common.AttributeExtend;

namespace WorkScheduleSystem.Model.Models
{
    [Mapping("ShiftScheduleHours")]
    public class ShiftScheduleHoursModel: BaseModel
    {
        /// <summary>
        /// 使用者ID
        /// </summary>
        public int uId                     {get;set;}   

        /// <summary>
        /// 對應班表ID
        /// </summary>
        public int sId                     {get;set; } 

        /// <summary>
        /// 當前總計需上班時數
        /// </summary>
        public double totalShiftHours       {get;set; } 

        /// <summary>
        /// 當前總計已排上班時數
        /// </summary>
        public double totalSetShiftHours    {get;set; } 
        /// <summary>
        /// 當前班表已排特休
        /// </summary>
        public double totalSetSpcHours      {get;set; } 
        /// <summary>
        /// 當前普通缺少/超額時數  
        /// </summary>
        public double totalNormalFixHours   {get;set; } 

        /// <summary>
        /// 當前國定假日缺少/超額時數
        /// </summary>
        public double totalNationalFixHours {get;set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime createDatetime { get; set; }

        /// <summary>
        /// 異動時間
        /// </summary>
        public DateTime updateDatetime { get; set; }

        /// <summary>
        /// 最後異動人員
        /// </summary>
        public string updateEmp { get; set; }
    }
}
