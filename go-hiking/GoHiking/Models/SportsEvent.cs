using System;
using System.ComponentModel.DataAnnotations;

namespace GoHiking.Models
{
    public class SportsEvent
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "活動標題為必填")]
        [StringLength(100, ErrorMessage = "標題長度不能超過100字")]
        public string Title { get; set; }

        [Required(ErrorMessage = "活動日期為必填")]
        public DateTime EventDate { get; set; }

        [StringLength(500, ErrorMessage = "描述長度不能超過500字")]
        public string Description { get; set; }

        [Required(ErrorMessage = "運動類型為必填")]
        public string SportsType { get; set; }

        public int Duration { get; set; } // 持續時間（分鐘）

        public string Location { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public SportsEvent()
        {
            CreatedDate = DateTime.Now;
            Duration = 60; // 預設1小時
        }
    }

    public enum SportsTypes
    {
        跑步,
        游泳,
        騎腳踏車,
        健身,
        瑜伽,
        登山,
        球類運動,
        其他
    }
}