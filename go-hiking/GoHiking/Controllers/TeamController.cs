using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoHiking.data;
using GoHiking.ViewModels;

namespace GoHiking.Controllers
{
    public class TeamController : Controller
    {
        private HikingDBEntities1 _db = new HikingDBEntities1();

        // GET: Team/Index
        public ActionResult Index()
        {


            var user = Session["UserLogin"] as GoHiking.data.User;
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            var userInfo = _db.Users.FirstOrDefault(t => t.Id == user.Id);
            var userDetail = _db.UserDetails.FirstOrDefault(m => m.UserId == user.Id);

            var userViewModels = new UsersDetailViewModel
            {
                Id = userInfo.Id,
                UserName = userInfo.UserName,
                Password = userInfo.Password,
                Email = userInfo.Email,
                UserId = userInfo.Id,
                FullName = userDetail?.FullName,
                Gender = userDetail?.Gender,
                Birthday = userDetail?.Birthday,
                Phone = userDetail?.Phone,
                Address = userDetail?.Address,
                ProfileImage = userDetail?.ProfileImage,
                ProfileImageName = userDetail?.ProfileImageName
            };


            return View(userViewModels);
        }


        // GET: Team/EditPartial
        public ActionResult _EditPartial()
        {
            var user = Session["UserLogin"] as GoHiking.data.User;
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            var userInfo = _db.Users.FirstOrDefault(t => t.Id == user.Id);
            var userDetail = _db.UserDetails.FirstOrDefault(m => m.UserId == user.Id);

            var userViewModels = new UsersDetailViewModel
            {
                Id = userInfo.Id,
                UserName = userInfo.UserName,
                Password = userInfo.Password,
                Email = userInfo.Email,
                UserId = userInfo.Id,
                FullName = userDetail?.FullName,
                Gender = userDetail?.Gender,
                Birthday = userDetail?.Birthday,
                Phone = userDetail?.Phone,
                Address = userDetail?.Address,
                ProfileImage = userDetail?.ProfileImage,
                ProfileImageName = userDetail?.ProfileImageName
            };


            return PartialView(userViewModels);
        }

        // POST: Team/EditPartial
        [HttpPost]
        public ActionResult _EditPartial(UsersDetailViewModel model)
        {
            User oldData = _db.Users.FirstOrDefault(x => x.Id == model.Id);
            UserDetail DetailData = _db.UserDetails.FirstOrDefault(y => y.UserId == model.UserId);

            // 更新 User 資料
            if (oldData != null)
            {
                oldData.UserName = model.UserName ?? oldData.UserName;
                oldData.Email = model.Email ?? oldData.Email;
                oldData.Password = model.Password ?? oldData.Password;
            }

            // 處理圖片上傳
            if (model.ProfileImageFile != null && model.ProfileImageFile.ContentLength > 0)
            {
                // 驗證檔案類型
                string[] allowedTypes = { "image/jpeg", "image/jpg", "image/png", "image/gif" };
                if (!allowedTypes.Contains(model.ProfileImageFile.ContentType.ToLower()))
                {
                    ModelState.AddModelError("ProfileImageFile", "只支援 JPG、PNG、GIF 格式的圖片");
                    return PartialView(model);
                }

                // 驗證檔案大小 (2MB)
                if (model.ProfileImageFile.ContentLength > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("ProfileImageFile", "圖片檔案不能超過 2MB");
                    return PartialView(model);
                }
            }

            // 處理 UserDetail 資料
            if (DetailData == null)
            {
                // 如果不存在，建立新的 UserDetail
                DetailData = new UserDetail
                {
                    UserId = model.UserId,
                    FullName = model.FullName,
                    Gender = model.Gender,
                    Birthday = model.Birthday,
                    Phone = model.Phone,
                    Address = model.Address
                };
                _db.UserDetails.Add(DetailData);
            }
            else
            {
                // 如果存在，更新現有資料
                DetailData.FullName = model.FullName;
                DetailData.Gender = model.Gender;
                DetailData.Birthday = model.Birthday;
                DetailData.Phone = model.Phone;
                DetailData.Address = model.Address;
            }

            // 處理圖片上傳到資料庫
            if (model.ProfileImageFile != null && model.ProfileImageFile.ContentLength > 0)
            {
                using (var binaryReader = new BinaryReader(model.ProfileImageFile.InputStream))
                {
                    DetailData.ProfileImage = binaryReader.ReadBytes(model.ProfileImageFile.ContentLength);
                    DetailData.ProfileImageName = model.ProfileImageFile.FileName;
                }
            }

            _db.SaveChanges();
            return RedirectToAction("Index", "Team");
        }

        //GET: /Team/Trip
        public ActionResult Trip()
        {


            var user = Session["UserLogin"] as GoHiking.data.User;
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            var currentUserId = user.Id;

            // 用戶開團的活動
            var userActivities = _db.TripActivities
                .Where(t => t.user_id == currentUserId)
                .Select(t => new ActivityViewModel
                {
                    activity_id = t.activity_id,
                    mt_id = t.mt_id,
                    mt_name = t.mt_name,
                    walk_days = t.walk_days,
                    price = t.price,
                    diff = t.diff,
                    max_participants = t.max_participants,
                    participants = t.participants,
                    is_active = t.is_active,
                    ImageData = t.GroupImages.FirstOrDefault().image_data,
                    ImageName = t.GroupImages.FirstOrDefault().image_name,
                })
                .OrderByDescending(a => a.activity_id) 
                .ToList();



            // 用戶參團的活動
            var participantActivities = _db.UserProfiles
                .Where(up => up.UserId == currentUserId && up.is_creator != true)
                .Select(up => up.TripActivity) 
                .Where(t => t != null) 
                .Select(t => new ActivityViewModel
                {
                    activity_id = t.activity_id,
                    mt_id = t.mt_id,
                    mt_name = t.mt_name,
                    walk_days = t.walk_days,
                    price = t.price,
                    diff = t.diff,
                    max_participants = t.max_participants,
                    participants = t.participants,
                    is_active = t.is_active,
                    ImageData = t.GroupImages.FirstOrDefault().image_data,
                    ImageName = t.GroupImages.FirstOrDefault().image_name,
                })
                .OrderByDescending(a => a.activity_id)
                .ToList();


            ViewBag.ParticipantActivities = participantActivities;


            return View(userActivities);
        }





    }
}