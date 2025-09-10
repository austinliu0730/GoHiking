using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoHiking.ViewModels
{
    public class UserProfileViewModel
    {
        [Display(Name = "使用者Id")]
        public int UserId { get; set; }

        [Display(Name = "活動Id")]
        public int ActivityId { get; set; }

        [Required(ErrorMessage = "請輸入真實姓名")]
        [Display(Name = "真實姓名")]
        public string RealName { get; set; }

        [Required(ErrorMessage = "請選擇性別")]
        [Display(Name = "性別")]
        public string Gender { get; set; } // "M" 或 "F"

        [Required(ErrorMessage = "請輸入生日")]
        [Display(Name = "生日")]
        public string BirthDate { get; set; } // 接收字串格式日期

        [Required(ErrorMessage = "請選擇國籍")]
        [Display(Name = "國籍")]
        public string Nationality { get; set; }

        public string OtherNationality { get; set; } // 其他國籍的自填欄位

        [Required(ErrorMessage = "請輸入身份證字號")]
        [Display(Name = "身份證字號")]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "請輸入LINE ID")]
        [Display(Name = "LINE ID")]
        public string LineId { get; set; }

        [Required(ErrorMessage = "請輸入電話號碼")]
        [Display(Name = "聯絡電話")]
        public string Phone { get; set; }

        // 地址相關欄位
        public string County { get; set; }
        public string District { get; set; }
        public string Zipcode { get; set; }

        [Required(ErrorMessage = "請輸入詳細地址")]
        [Display(Name = "詳細地址")]
        public string Address { get; set; }

        [Required(ErrorMessage = "請輸入緊急聯絡人")]
        [Display(Name = "緊急聯絡人")]
        public string EmergencyContactName { get; set; }

        [Required(ErrorMessage = "請輸入緊急聯絡人電話")]
        [Display(Name = "緊急聯絡人電話")]
        public string EmergencyContactPhone { get; set; }

        [Required(ErrorMessage = "請選擇是否有同行夥伴")]
        [Display(Name = "是否有同行夥伴")]
        public string HasCompanion { get; set; } // "是" 或 "否"

        [Display(Name = "同行夥伴姓名")]
        public string CompanionName { get; set; }

        [Required(ErrorMessage = "請輸入登山經歷")]
        [Display(Name = "登山經歷")]
        public string HikingExperience { get; set; }

        [Required(ErrorMessage = "請輸入運動習慣")]
        [Display(Name = "運動習慣")]
        public string ExerciseHabit { get; set; }

        [Display(Name = "疾病史及過敏史")]
        public string MedicalHistory { get; set; }

        [Display(Name = "其他注意事項")]
        public string OtherNotes { get; set; }

        [Display(Name = "飲食習慣")]
        public string DietPreference { get; set; }

        [Required(ErrorMessage = "請同意條款")]
        public bool AgreeTerms { get; set; }

        [Display(Name = "開團者1參加者0")]
        public bool? is_creator { get; set; }
    }
}