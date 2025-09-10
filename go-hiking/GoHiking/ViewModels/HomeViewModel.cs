using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoHiking.ViewModels
{
    public class HomeViewModel
    {
        public List<ActivityViewModel> ActiveActivities { get; set; }
        public List<BeginnerActivityViewModel> BeginnerActivities { get; set; }
        public List<IntermediateActivityViewModel> IntermediateActivities { get; set; }
    }


    public class BeginnerActivityViewModel
    {
        public int activity_id { get; set; }
        public string mt_name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int difficulty_level { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageName { get; set; }

        [Display(Name = "天數")]
        public int walk_days { get; set; }
        public int nights { get; set; }
        [Display(Name = "山Id")]
        public int mt_id { get; set; }
    }

    public class IntermediateActivityViewModel
    {
        public int activity_id { get; set; }
        public string mt_name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int difficulty_level { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageName { get; set; }

        [Display(Name = "天數")]
        public int walk_days { get; set; }
        public int nights { get; set; }
        [Display(Name = "山Id")]
        public int mt_id { get; set; }
    }
}