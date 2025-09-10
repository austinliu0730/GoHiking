using GoHiking.data;
using GoHiking.ViewModels;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GoHiking.Controllers
{
    public class ProductsController : Controller
    {
        private HikingDBEntities1 _db = new HikingDBEntities1();

        // GET: Products (主畫面)
        public ActionResult Index()
        {
            var products = _db.TripActivities.ToList();
            return View(products);
        }

        // GET: Products/Details/5 (產品詳細頁面)
        public ActionResult Details(int id)
        {

            var mountainTrail = _db.MountainTrails.Find(id);

            if (mountainTrail == null)
            {
                return HttpNotFound();
            }

            return View(mountainTrail);
        }


        // GET: Products/Details/5 (我要報名)
        public ActionResult Join(int id)
        {

            var mountainTrail = _db.MountainTrails.Find(id);

            if (mountainTrail == null)
            {
                return HttpNotFound();
            }

            return View(mountainTrail);
        }


        // GET: Products/Create?mt_id=1(我要開團)
        // http://localhost:55176/Products/Create?mt_id=1
        public ActionResult Create(int mt_id)
        {
            var user = Session["UserLogin"] as GoHiking.data.User;
            if (user == null)
            {
                return RedirectToAction("Login", "User"); // 沒登入就踢回去
            }

            var userInfo = _db.Users.Find(user.Id);
            if (userInfo == null)
            {
                return HttpNotFound();
            }



            TripActivity activity = new TripActivity();
            var mountainTrail = _db.MountainTrails.Find(mt_id);

            // 建立完整的 ViewModel，包含所有需要的隱藏欄位資料
            var viewModel = new TripGroupViewModel
            {
                // 活動預設資料
                activity_date = DateTime.Now.AddDays(30),
                meeting_time = "早上 07:00",
                meeting_location = "台北車站",
                max_participants = 20,
                price = 3500,
                FeeIncludes = "接駁車,住宿費,嚮導費,300萬登山事故險",
                // 山資料 (隱藏欄位) - 從MountainTrails複製所有欄位

                mt_id = mountainTrail.ID,
                mt_name = mountainTrail.mt_name,
                badge = mountainTrail.badge,
                walk_days = mountainTrail.walk_days,
                location = mountainTrail.location,
                diff = mountainTrail.diff,
                dist_km = mountainTrail.dist_km,
                time_min = mountainTrail.time_min,
                ascend_m = mountainTrail.ascend_m,
                descend_m = mountainTrail.descend_m,
                trail_cond = mountainTrail.trail_cond,
                route_type = mountainTrail.route_type,
                min_alt_m = mountainTrail.min_alt_m,
                max_alt_m = mountainTrail.max_alt_m,
                mt_range = mountainTrail.mt_range,
                park_permit = mountainTrail.park_permit,
                mt_permit = mountainTrail.mt_permit,
                TripDetails = mountainTrail.TripDetails,
                region = mountainTrail.region,
                TripSchedule = mountainTrail.TripSchedule,

                // 用戶資料 (隱藏欄位) - 從Users複製所有欄位
                user_id = userInfo.Id,
                user_name = userInfo.UserName,
                user_email = userInfo.Email,
                user_password = userInfo.Password,

                // 自訂行程預設為山的行程
                custom_schedule = mountainTrail.TripSchedule,


            };


            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TripGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state.Errors.Count > 0)
                    {
                        errors.Add($"{key}: {string.Join(", ", state.Errors.Select(e => e.ErrorMessage))}");
                    }
                }

                // 直接拋出錯誤，會顯示在黃色錯誤頁面上
                throw new Exception("驗證錯誤：\n" + string.Join("\n", errors));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 1. 建立 TripActivity 物件並設定所有屬性
                    TripActivity activity = new TripActivity
                    {
                        // 活動基本資訊
                        group_name = model.mt_name,
                        activity_date = model.activity_date,
                        meeting_time = model.meeting_time,
                        meeting_location = model.meeting_location,
                        custom_schedule = model.custom_schedule ?? model.TripSchedule,
                        leader_message = model.leader_message ?? "",
                        team_name = model.team_name ?? "",
                        max_participants = model.max_participants,
                        price = model.price,

                        // 費用包含項目
                        FeeIncludes = model.FeeIncludes,


                        // 山的資訊 (注意型別轉換)
                        mt_id = model.mt_id,
                        mt_name = model.mt_name,
                        badge = model.badge,
                        walk_days = model.walk_days,
                        location = model.location,
                        diff = model.diff,
                        dist_km = model.dist_km,
                        time_min = model.time_min,
                        ascend_m = model.ascend_m,
                        descend_m = model.descend_m,
                        trail_cond = model.trail_cond,
                        route_type = model.route_type,
                        min_alt_m = model.min_alt_m,
                        max_alt_m = model.max_alt_m,
                        mt_range = model.mt_range,

                        // 型別轉換：int 轉 byte (TINYINT)
                        park_permit = model.park_permit,
                        mt_permit = model.mt_permit,

                        TripDetails = model.TripDetails,
                        region = model.region,
                        TripSchedule = model.TripSchedule,

                        // 用戶資訊
                        user_id = model.user_id,
                        user_name = model.user_name,
                        user_email = model.user_email,
                        user_password = model.user_password,

                        // 狀態資訊
                        status = "待成團",
                        participants = 0,
                        created_date = DateTime.Now,
                        updated_date = DateTime.Now,
                        is_active = false // 預設為未審核
                    };

                    // 先儲存 TripActivity 以取得 activity_id
                    _db.TripActivities.Add(activity);
                    _db.SaveChanges();



                    if (activity.activity_id > 0)  // 確認有取得 ID
                    {
                        // 3. 處理圖片上傳 (修改開始)
                        if (model.ImageFiles != null && model.ImageFiles.Any())
                        {
                            int uploadOrder = 1; // 用來記錄圖片順序
                            foreach (var imageFile in model.ImageFiles)
                            {
                                if (imageFile != null && imageFile.ContentLength > 0)
                                {
                                    // 驗證檔案類型
                                    var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
                                    if (!allowedTypes.Contains(imageFile.ContentType.ToLower()))
                                    {
                                        ModelState.AddModelError("ImageFiles", $"第 {uploadOrder} 張圖片格式不符，只允許 JPG, PNG, GIF。");
                                        continue; // 跳過這張不符格式的圖片
                                    }

                                    // 驗證檔案大小 (10MB)
                                    if (imageFile.ContentLength > 10 * 1024 * 1024)
                                    {
                                        ModelState.AddModelError("ImageFiles", $"第 {uploadOrder} 張圖片大小不能超過 10MB。");
                                        continue; // 跳過這張過大的圖片
                                    }

                                    var activityImage = new GroupImage
                                    {
                                        group_id = activity.activity_id,
                                        image_name = Path.GetFileName(imageFile.FileName),
                                        upload_date = DateTime.Now,
                                        upload_order = (byte)uploadOrder // 轉換成 byte
                                    };

                                    // 讀取圖片資料
                                    using (var binaryReader = new BinaryReader(imageFile.InputStream))
                                    {
                                        activityImage.image_data = binaryReader.ReadBytes(imageFile.ContentLength);
                                    }

                                    _db.GroupImages.Add(activityImage);
                                    uploadOrder++; // 順序 +1
                                }
                            }
                            _db.SaveChanges(); // 在迴圈外儲存所有圖片變更
                        }
                    }

                    TempData["SuccessMessage"] = $"成團成功！活動「{activity.group_name}」已建立";
                    return RedirectToAction("Create", "Profile", new
                    {
                        ActivityId = activity.activity_id,
                        is_creator = true
                    });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"建立活動時發生錯誤：{ex.Message}");
                    return View(model);
                }
            }

            return View(model);
        }

        // GET: Products/Detail/5 (預覽畫面) 
        public ActionResult Detail(int id)
        {

            var activity = _db.TripActivities.FirstOrDefault(a => a.activity_id == id);

            if (activity == null)
            {
                return HttpNotFound();
            }


            return View(activity);
        }


        // GET: Products/Activity/5 (開團畫面) 
        public ActionResult Activity(int id)
        {

            var activity = _db.TripActivities.FirstOrDefault(a => a.activity_id == id);

            if (activity == null)
            {
                return HttpNotFound();
            }


            return View(activity);
        }

        // GET: Products/Delete/5 (刪除頁面) 
        public ActionResult Delete(int id)
        {
            TripActivity activity = _db.TripActivities.FirstOrDefault(a => a.activity_id == id);

            if (activity != null)
            {
                // 先刪除相關的 UserProfile 記錄
                var userProfiles = _db.UserProfiles.Where(up => up.activity_id == id).ToList();
                _db.UserProfiles.RemoveRange(userProfiles);

                // 刪除對應的 GroupImage
                var images = _db.GroupImages.Where(g => g.group_id == id).ToList();
                _db.GroupImages.RemoveRange(images);

                // 最後刪除 activity
                _db.TripActivities.Remove(activity);

                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Products/Edit/5 (編輯)
        public ActionResult Edit(int id)
        {
            TripActivity activity = _db.TripActivities.FirstOrDefault(a => a.activity_id == id);

            if (activity == null)
            {
                return HttpNotFound();
            }

            return View(activity);
        }

        // GET: Products/Edit/5 (編輯頁面)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TripActivity model)
        {
            // 找到資料庫原始資料
            var activity = _db.TripActivities.FirstOrDefault(a => a.activity_id == id);
            if (activity == null)
            {
                return HttpNotFound();
            }

            try
            {
                // 只更新前端有送過來的欄位
                activity.group_name = model.group_name;
                activity.activity_date = model.activity_date;
                activity.meeting_time = model.meeting_time;
                activity.meeting_location = model.meeting_location;
                activity.leader_message = model.leader_message;
                activity.team_name = model.team_name;
                activity.max_participants = model.max_participants;
                activity.price = model.price;


                activity.mt_id = model.mt_id;
                activity.mt_name = model.mt_name;
                activity.badge = model.badge;
                activity.walk_days = model.walk_days;
                activity.location = model.location;
                activity.diff = model.diff;
                activity.dist_km = model.dist_km;
                activity.time_min = model.time_min;
                activity.ascend_m = model.ascend_m;
                activity.descend_m = model.descend_m;

                activity.mt_range = model.mt_range;
                activity.park_permit = model.park_permit;
                activity.mt_permit = model.mt_permit;

                activity.FeeIncludes = model.FeeIncludes;

                // 其他可選欄位
                activity.TripDetails = model.TripDetails;
                activity.region = model.region;
                activity.TripSchedule = model.TripSchedule;

                // 狀態更新
                activity.status = model.status;
                activity.is_active = model.is_active;

                activity.updated_date = DateTime.Now; // 更新時間

                _db.SaveChanges();

                TempData["SuccessMessage"] = "活動已更新！";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"更新活動時發生錯誤：{ex.Message}");
                return View(model);
            }
        }

        public ActionResult Audit(int id, bool approve)
        {
            var activity = _db.TripActivities.FirstOrDefault(a => a.activity_id == id);

            if (activity == null)
            {
                return HttpNotFound();
            }

            try
            {
                // 根據審核結果更新狀態
                activity.is_active = approve ? true : false;
                activity.updated_date = DateTime.Now;

                _db.SaveChanges();

                TempData["Message"] = approve ? "活動審核通過！" : "活動審核不通過！";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "審核失敗：" + ex.Message;
            }

            return RedirectToAction("Index");
        }

        public ActionResult TestImages()
        {
            return View();
        }


        // 取得圖片
        // Products/GetGroupImage/5
        public ActionResult GetGroupImage(int imageId)
        {
            var image = _db.GroupImages.FirstOrDefault(img => img.image_id == imageId);

            if (image != null && image.image_data != null)
            {
                string contentType = "image/jpeg";

                if (!string.IsNullOrEmpty(image.image_name))
                {
                    string extension = Path.GetExtension(image.image_name).ToLower();
                    switch (extension)
                    {
                        case ".png":
                            contentType = "image/png";
                            break;
                        case ".gif":
                            contentType = "image/gif";
                            break;
                        case ".jpg":
                        case ".jpeg":
                        default:
                            contentType = "image/jpeg";
                            break;
                    }
                }

                return File(image.image_data, contentType);
            }

            return HttpNotFound();
        }


        public ActionResult ImgDetails(int id)
        {
            // 取得活動資料，包含圖片
            var activity = _db.TripActivities
                             .Include("GroupImages")
                             .FirstOrDefault(a => a.activity_id == id);

            if (activity == null)
            {
                return HttpNotFound();
            }

            // 準備預設圖片清單
            var defaultImages = new List<dynamic>
                {
                    new {
                        Url = "https://images.unsplash.com/photo-1506905925346-21bda4d32df4?w=900&h=400&fit=crop&crop=center",
                        Alt = "玉山主峰風景 1"
                    },
                    new {
                        Url = "https://images.unsplash.com/photo-1502085671122-2d218cd434e6?q=80&w=1526&auto=format&fit=crop&crop=center&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                        Alt = "玉山主峰風景 2"
                    },
                    new {
                        Url = "https://images.unsplash.com/photo-1501854140801-50d01698950b?q=80&w=1840&auto=format&fit=crop&crop=center",
                        Alt = "玉山主峰風景 3"
                    }
                   };

            // 建立混合圖片清單
            var allImages = new List<dynamic>();

            // 先加入上傳的圖片
            if (activity.GroupImages != null && activity.GroupImages.Any())
            {
                foreach (var img in activity.GroupImages.OrderBy(g => g.upload_order))
                {
                    allImages.Add(new
                    {
                        Url = Url.Action("GetGroupImage", new { imageId = img.image_id }),
                        Alt = $"活動圖片 {allImages.Count + 1}",
                        IsDefault = false
                    });
                }
            }

            // 補足預設圖片到3張
            while (allImages.Count < 3)
            {
                var defaultImg = defaultImages[allImages.Count];
                allImages.Add(new
                {
                    Url = defaultImg.Url,
                    Alt = defaultImg.Alt,
                    IsDefault = true
                });
            }

            ViewBag.AllImages = allImages;
            return View(activity);
        }



    }
}