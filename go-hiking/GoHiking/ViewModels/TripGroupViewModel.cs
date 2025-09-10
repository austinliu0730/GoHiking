using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GoHiking.ViewModels
{

    public class TripGroupViewModel
    {
        // 活動資料 (用戶可編輯)
        public string group_name { get; set; }

        [Required]
        [Display(Name = "活動日期")]
        public DateTime activity_date { get; set; }

        [Display(Name = "集合時間")]
        public string meeting_time { get; set; }

        [Display(Name = "集合地點")]
        public string meeting_location { get; set; }

        [Display(Name = "自訂行程")]
        public string custom_schedule { get; set; }

        [Display(Name = "團長留言")]
        public string leader_message { get; set; }

        [Display(Name = "最大人數")]
        public int max_participants { get; set; }

        [Display(Name = "價格")]
        public int price { get; set; }

        [Display(Name = "包含接駁車")]
        public bool include_transport { get; set; }

        [Display(Name = "包含住宿")]
        public bool include_accommodation { get; set; }

        [Display(Name = "包含嚮導")]
        public bool include_guide { get; set; }

        [Display(Name = "包含保險")]
        public bool include_insurance { get; set; }

        [Display(Name = "包含課程")]
        public bool include_course { get; set; }

        [Display(Name = "團隊名稱")]
        public string team_name { get; set; }

        // 隱藏欄位 - 山資料 (從MountainTrails複製)
        public int mt_id { get; set; }
        public string mt_name { get; set; }
        public string badge { get; set; }
        public int walk_days { get; set; }
        public string location { get; set; }
        public int diff { get; set; }
        public decimal dist_km { get; set; }
        public int time_min { get; set; }
        public int ascend_m { get; set; }
        public int descend_m { get; set; }
        public string trail_cond { get; set; }
        public string route_type { get; set; }
        public int min_alt_m { get; set; }
        public int max_alt_m { get; set; }
        public string mt_range { get; set; }
        public byte park_permit { get; set; }
        public byte mt_permit { get; set; }
        public string TripDetails { get; set; }
        public string region { get; set; }
        public string TripSchedule { get; set; }

        public string FeeIncludes { get; set; }  // 存放 "接駁車,住宿費,嚮導費"

        // 隱藏欄位 - 用戶資料 (從Users複製)
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_email { get; set; }
        public string user_password { get; set; }


        // 圖片相關
        public byte[] CurrentImageData { get; set; }
        public string CurrentImageName { get; set; }

        [NotMapped]
        public List<HttpPostedFileBase> ImageFiles { get; set; }

        public TripGroupViewModel()
        {
            // 設定預設值
            include_transport = true;
            include_accommodation = true;
            include_guide = true;
            include_insurance = true;
            include_course = false;
            max_participants = 20;
            activity_date = DateTime.Now.AddDays(30);
            meeting_time = "早上 07:00";
            meeting_location = "台北車站";
            price = 3500;
        }
    }
}