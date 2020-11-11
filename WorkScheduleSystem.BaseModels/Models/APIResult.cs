using System;
using System.Collections.Generic;
using System.Text;

namespace WorkScheduleSystem.BaseModels.Models
{
    /// <summary>
    /// 呼叫 API 回傳的制式格式
    /// </summary>
    public class APIResult
    {
        /// <summary>
        /// 狀態碼
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 呼叫 API 失敗的錯誤訊息
        /// </summary>
        public string Message { get; set; } = "";        
        /// <summary>
        /// 取得回傳資料
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 呼叫此API所得到的其他內容
        /// </summary>
        public object Payload { get; set; }
        /// <summary>
        /// 呼叫此API所得到的其他內容
        /// </summary>
        public object DataList { get; set; }
   

    }
}
