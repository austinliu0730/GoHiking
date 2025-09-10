using GoHiking.data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GoHiking.ViewModels
{
    public class ActivityViewModel
    {
        [Display(Name = "山Id")]
        public int mt_id { get; set; }

        [Display(Name = "團隊Id")]
        public int activity_id { get; set; }


        [Display(Name = "山名")]
        public string mt_name { get; set; }

        [Display(Name = "天數")]
        public int walk_days { get; set; }

        [Display(Name = "價格")]
        public int price { get; set; }

        [Display(Name = "難度")]
        public int diff { get; set; }
        [Display(Name = "地區")]
        public string region { get; set; }
        [Display(Name = "集合時間")]
        public string meeting_time { get; set; }


        //小圖

        [Display(Name = "入園證")]
        public byte park_permit { get; set; }

        [Display(Name = "入山證")]
        public byte mt_permit { get; set; }

        [Display(Name = "百岳")]
        public string badge { get; set; }


        // 圖片相關
        public byte[] ImageData { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        public List<HttpPostedFileBase> ImageFiles { get; set; }


        // 人數

        [Display(Name = "開團人數")]
        public int? max_participants { get; set; }

        [Display(Name = "報名人數")]
        public int participants { get; set; }

        [Display(Name = "日期")]
        public DateTime activity_date { get; set; }

        [Display(Name = "集合地點")]
        public string meeting_location { get; set; }

        //成團相關

        [Display(Name = "審核通過")]

        public bool? is_active { get; set; }


    }
}