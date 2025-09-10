using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoHiking.data;
using GoHiking.Models;
using GoHiking.ViewModels;

namespace GoHiking.Controllers
{
    public class TripHelperController : Controller
    {
        private HikingDBEntities1 _db = new HikingDBEntities1();

        // 成員列表
        // GET: TripHelper
        public ActionResult Index(int id)
        {

            // 測試用
            //var user = _db.Users.FirstOrDefault(x => x.UserName == "Tony");
            //Session["UserLogin"] = user;

            var user = Session["UserLogin"] as GoHiking.data.User;
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            var GroupMember = _db.UserProfiles
                     .Include("User")
                     .Include("User.UserDetails")
                     .Where(t => t.activity_id == id)
                     .ToList();

            // 計算主辦人的活動舉辦數量
            var creator = GroupMember.FirstOrDefault(m => m.is_creator == true);
            if (creator != null)
            {
                var activityCount = _db.TripActivities.Count(t => t.user_id == creator.UserId);
                ViewBag.ActivityCount = activityCount;
            }

            return View(GroupMember);
        }

        // 訊息討論區

        public ActionResult Comment(int id)
        {
            // 測試用
            //var user = _db.Users.FirstOrDefault(x => x.UserName == "Tony");
            //Session["UserLogin"] = user;


            var user = Session["UserLogin"] as GoHiking.data.User;
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            var GroupMember = _db.UserProfiles
                              .Include("User")
                              .Include("User.UserDetails")
                              .Where(t => t.activity_id == id)
                              .ToList();

            // 取得所有留言，並包含用戶詳細資料
            var comments = _db.Comments
                .Include("UserDetail") // 包含 UserDetail 導航屬性
                .Where(t => t.Activity_Id == id)
                .OrderBy(t => t.CommentTime) // 按時間順序排列
                .ToList();

            // 建立 ViewModel 列表
            var commentViewModels = new List<CommentViewModel>();

            if (comments.Any())
            {
                foreach (var c in comments)
                {
                    var commentViewModel = new CommentViewModel
                    {
                        CommentId = c.CommentId,
                        Activity_Id = id,
                        // 優先顯示 UserDetail.FullName，如果沒有才用 UserName
                        RealName = c.UserDetail?.FullName ?? c.UserName,
                        UserName = c.UserName,
                        UserId = c.UserId,
                        CommentTime = (DateTime)c.CommentTime,
                        Content = c.Content,
                        // 直接透過導航屬性取得頭像資料
                        ProfileImage = c.UserDetail?.ProfileImage,
                        ProfileImageName = c.UserDetail?.ProfileImageName,
                        // 檢查是否為當前用戶的留言（比較 UserId）
                        IsCurrentUserComment = c.UserId == user.Id
                    };
                    commentViewModels.Add(commentViewModel);
                }
            }
            else
            {
                // 如果沒有留言，建立一個空的 ViewModel 給表單使用
                commentViewModels.Add(new CommentViewModel
                {
                    Activity_Id = id,
                    RealName = GroupMember.FirstOrDefault()?.RealName ?? user.UserName,
                    CommentTime = DateTime.Now,
                    Content = ""
                });
            }

            ViewBag.GroupMembers = GroupMember;

            return View(commentViewModels);

        }

        [HttpPost]
        public ActionResult Comment(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Session["UserLogin"] as GoHiking.data.User;
                if (user == null)
                {
                    return RedirectToAction("Login", "User");
                }

                // 取得用戶的 DetailId
                var userDetail = _db.UserDetails.FirstOrDefault(ud => ud.UserId == user.Id);

                // 建立新留言
                var newComment = new Comment
                {
                    Activity_Id = model.Activity_Id,
                    Content = model.Content,
                    UserName = user.UserName, // 使用登入用戶的 UserName
                    UserId = user.Id,     // 設定 UserId
                    DetailId = userDetail?.DetailId, // 設定 DetailId，如果存在的話
                    CommentTime = DateTime.Now
                };

                _db.Comments.Add(newComment);
                _db.SaveChanges();

                // 重新導向到 GET 方法顯示更新後的留言
                return RedirectToAction("Comment", new { id = model.Activity_Id });
            }

            return View(model);
        }

        // 刪除留言功能
        [HttpPost]
        public ActionResult DeleteComment(int commentId, int activityId)
        {
            var user = Session["UserLogin"] as GoHiking.data.User;
            if (user == null)
            {
                return Json(new { success = false, message = "請先登入" });
            }

            var comment = _db.Comments.FirstOrDefault(c => c.CommentId == commentId);
            if (comment == null)
            {
                return Json(new { success = false, message = "留言不存在" });
            }

            // 檢查是否為留言作者本人（使用 UserId 比較更準確）
            if (comment.UserId != user.Id)
            {
                return Json(new { success = false, message = "您沒有權限刪除此留言" });
            }

            _db.Comments.Remove(comment);
            _db.SaveChanges();

            return Json(new { success = true, message = "留言已刪除" });
        }

        // 返回用戶頭像圖片
        public ActionResult GetProfileImage(int? userId)
        {
            if (!userId.HasValue)
            {
                // 返回預設頭像
                var defaultImagePath = Server.MapPath("~/images/person_2.jpg");
                if (System.IO.File.Exists(defaultImagePath))
                {
                    return File(defaultImagePath, "image/jpeg");
                }
                return HttpNotFound();
            }

            var userDetail = _db.UserDetails.FirstOrDefault(ud => ud.UserId == userId.Value);
            
            if (userDetail?.ProfileImage != null && userDetail.ProfileImage.Length > 0)
            {
                string contentType = "image/jpeg"; // 預設格式，可以根據 ProfileImageName 調整
                if (!string.IsNullOrEmpty(userDetail.ProfileImageName))
                {
                    var extension = System.IO.Path.GetExtension(userDetail.ProfileImageName).ToLower();
                    switch (extension)
                    {
                        case ".png":
                            contentType = "image/png";
                            break;
                        case ".gif":
                            contentType = "image/gif";
                            break;
                    }
                }
                return File(userDetail.ProfileImage, contentType);
            }

            // 返回預設頭像
            var defaultImagePath2 = Server.MapPath("~/images/person_2.jpg");
            if (System.IO.File.Exists(defaultImagePath2))
            {
                return File(defaultImagePath2, "image/jpeg");
            }

            // 如果預設頭像也不存在，返回 404
            return HttpNotFound();
        }

        // 裝備準備
        public ActionResult Gear(int id)
        {

            // 測試用
            //var user = _db.Users.FirstOrDefault(x => x.UserName == "Tony");
            //Session["UserLogin"] = user;

            var user = Session["UserLogin"] as GoHiking.data.User;
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            var GroupMember = _db.UserProfiles
                              .Include("User")
                              .Include("User.UserDetails")
                              .Where(t => t.activity_id == id)
                              .ToList();



            return View(GroupMember);
        }


        //// 裝備選擇
        //public ActionResult Equipment(int id)
        //{

        //    // 測試用
        //    //var user = _db.Users.FirstOrDefault(x => x.UserName == "Tony");
        //    //Session["UserLogin"] = user;

        //    var user = Session["UserLogin"] as GoHiking.data.User;
        //    if (user == null)
        //    {
        //        return RedirectToAction("Login", "User");
        //    }

        //    var GroupMember = _db.UserProfiles
        //                      .Where(t => t.activity_id == id)
        //                      .ToList();



        //    return View(GroupMember);
        //}


        // 日曆

        //private static List<SportsEvent> events = new List<SportsEvent>();

        private static List<SportsEvent> events = new List<SportsEvent>
          {
              new SportsEvent {
                  Id = 1, Title = "晨跑訓練", SportsType = "跑步",
                  EventDate = DateTime.Today.AddHours(6), Duration = 60,
                  Description = "5公里晨跑，提升心肺功能", Location = "大安森林公園"
              },
              new SportsEvent {
                  Id = 2, Title = "重量訓練", SportsType = "健身",
                  EventDate = DateTime.Today.AddDays(1).AddHours(19), Duration = 90,
                  Description = "上半身肌力訓練", Location = "健身房"
              },
              new SportsEvent {
                  Id = 3, Title = "瑜伽課程", SportsType = "瑜伽",
                  EventDate = DateTime.Today.AddDays(2).AddHours(18), Duration = 75,
                  Description = "放鬆身心的哈達瑜伽", Location = "瑜伽教室"
              },
              new SportsEvent {
                  Id = 4, Title = "登山健行", SportsType = "登山",
                  EventDate = DateTime.Today.AddDays(6).AddHours(7), Duration = 480,
                  Description = "象山步道健行，欣賞台北市景", Location = "象山親山步道"
              },
              new SportsEvent {
                  Id = 5, Title = "游泳訓練", SportsType = "游泳",
                  EventDate = DateTime.Today.AddDays(3).AddHours(20), Duration = 60,
                  Description = "自由式技巧練習", Location = "運動中心游泳池"
              }
          };

        public ActionResult Schedule()
        {
            return View(events);
        }

        public ActionResult Calendar()
        {
            return View(events);
        }

        // 整合的 Calendar 和 Schedule 頁面
        public ActionResult CalendarIntegrated(int id, string view = "calendar")
        {
            // 測試用
            //var user = _db.Users.FirstOrDefault(x => x.UserName == "Tony");


            var user = Session["UserLogin"] as GoHiking.data.User;
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            Session["UserLogin"] = user;

            var GroupMember = _db.UserProfiles
                              .Include("User")
                              .Include("User.UserDetails")
                              .Where(t => t.activity_id == id)
                              .ToList();

            // 權限檢查：判斷當前用戶是否為創建者
            var creator = GroupMember.FirstOrDefault(m => m.is_creator == true);
            var isCreator = creator != null && creator.UserId == user.Id;

            ViewBag.CurrentView = view;
            ViewBag.ActivityId = id;
            ViewBag.Events = events;
            ViewBag.IsCreator = isCreator;

            return View(GroupMember);
        }

        [HttpPost]
        public JsonResult AddEvent(SportsEvent newEvent, int activityId = 0)
        {
            var user = Session["UserLogin"] as GoHiking.data.User;
            if (user == null)
            {
                return Json(new { success = false, message = "請先登入" });
            }

            // 權限檢查：只有創建者可以新增事件
            if (activityId > 0)
            {
                var creator = _db.UserProfiles
                    .FirstOrDefault(up => up.activity_id == activityId && up.is_creator == true);
                
                if (creator == null || creator.UserId != user.Id)
                {
                    return Json(new { success = false, message = "您沒有權限執行此操作" });
                }
            }

            if (ModelState.IsValid)
            {
                newEvent.Id = events.Count + 1;
                events.Add(newEvent);
                return Json(new { success = true, eventId = newEvent.Id });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult UpdateEvent(int id, string newDate, int newHour, int newMinute = 0, int activityId = 0)
        {
            var user = Session["UserLogin"] as GoHiking.data.User;
            if (user == null)
            {
                return Json(new { success = false, message = "請先登入" });
            }

            // 權限檢查：只有創建者可以更新事件
            if (activityId > 0)
            {
                var creator = _db.UserProfiles
                    .FirstOrDefault(up => up.activity_id == activityId && up.is_creator == true);
                
                if (creator == null || creator.UserId != user.Id)
                {
                    return Json(new { success = false, message = "您沒有權限執行此操作" });
                }
            }

            try
            {
                var eventItem = events.FirstOrDefault(e => e.Id == id);
                if (eventItem != null && DateTime.TryParse(newDate, out DateTime parsedDate))
                {
                    eventItem.EventDate = parsedDate.AddHours(newHour).AddMinutes(newMinute);
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "事件不存在或日期格式錯誤" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteEvent(int id, int activityId = 0)
        {
            var user = Session["UserLogin"] as GoHiking.data.User;
            if (user == null)
            {
                return Json(new { success = false, message = "請先登入" });
            }

            // 權限檢查：只有創建者可以刪除事件
            if (activityId > 0)
            {
                var creator = _db.UserProfiles
                    .FirstOrDefault(up => up.activity_id == activityId && up.is_creator == true);
                
                if (creator == null || creator.UserId != user.Id)
                {
                    return Json(new { success = false, message = "您沒有權限執行此操作" });
                }
            }

            var eventItem = events.FirstOrDefault(e => e.Id == id);
            if (eventItem != null)
            {
                events.Remove(eventItem);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "事件不存在" });
        }

        [HttpGet]
        public JsonResult GetEvents()
        {
            var calendarEvents = events.Select(e => new
            {
                id = e.Id,
                title = e.Title,
                start = e.EventDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                description = e.Description,
                sportsType = e.SportsType
            }).ToList();

            return Json(calendarEvents, JsonRequestBehavior.AllowGet);
        }


    }
}