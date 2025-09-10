using GoHiking.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoHiking.ViewModels
{
    public class UsersDetailViewModel
    {

        // Users 表的欄位
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        // UserDetails 表的欄位
        public int UserId { get; set; }
        public string FullName { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        // 圖片相關屬性
        public HttpPostedFileBase ProfileImageFile { get; set; }
        public byte[] ProfileImage { get; set; }
        public string ProfileImageName { get; set; }


    }

}
