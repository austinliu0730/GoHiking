using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GoHiking.data;

namespace GoHiking.ViewModels
{
    public class CommentViewModel
    {
        // Comment 表的欄位
        [Display(Name = "留言Id")]
        public int CommentId { get; set; }

        [Display(Name = "活動Id")]
        public int Activity_Id { get; set; }

        [Display(Name = "真實姓名")]
        public string RealName { get; set; }

        [Display(Name = "留言時間")]
        public DateTime CommentTime { get; set; }

        [Display(Name = "內容")]
        public string Content { get; set; }

        // 新增的欄位支援刪除功能和頭像顯示
        [Display(Name = "留言者用戶名")]
        public string UserName { get; set; }

        [Display(Name = "留言者用戶Id")]
        public int? UserId { get; set; }

        [Display(Name = "頭像資料")]
        public byte[] ProfileImage { get; set; }

        [Display(Name = "頭像檔名")]
        public string ProfileImageName { get; set; }

        // 判斷是否為當前用戶的留言（用於顯示刪除按鈕）
        public bool IsCurrentUserComment { get; set; }
    }
}