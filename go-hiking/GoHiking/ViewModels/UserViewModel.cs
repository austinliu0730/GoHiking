using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Compare = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace GoHiking.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "帳號一定要填")]
        [Display(Name = "帳號")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密碼一定要填")]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [Required(ErrorMessage = "確認密碼為必填")]
        [DataType(DataType.Password)]
        [Display(Name = "確認密碼")]
        [Compare("Password", ErrorMessage = "密碼與確認密碼不相符。")] //上網查的
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Email一定要填")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}