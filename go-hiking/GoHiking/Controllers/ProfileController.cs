using GoHiking.data;
using GoHiking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoHiking.Views
{
    public class ProfileController : Controller
    {

        private HikingDBEntities1 _db = new HikingDBEntities1();
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Create(int ActivityId, bool? is_creator)
        {

            var user = Session["UserLogin"] as GoHiking.data.User;
            if (user == null)
            {
                return RedirectToAction("Login", "User"); // 沒登入就踢回去
            }

            var viewModel = new UserProfileViewModel
            {
                UserId = user.Id,
                ActivityId = ActivityId,
                is_creator = is_creator ?? false
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserProfileViewModel viewModel)
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

                throw new Exception("驗證錯誤：\n" + string.Join("\n", errors));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 驗證同行夥伴邏輯
                    if (viewModel.HasCompanion == "是" && string.IsNullOrEmpty(viewModel.CompanionName))
                    {
                        ModelState.AddModelError("CompanionName", "選擇有同行夥伴時，請填寫夥伴姓名");
                        return View(viewModel);
                    }


                    // 轉換 ViewModel 到 Model
                    var userProfile = new UserProfile
                    {
                        UserId = viewModel.UserId,
                        RealName = viewModel.RealName,
                        Gender = viewModel.Gender == "M", // true=男, false=女
                        BirthDate = DateTime.Parse(viewModel.BirthDate),
                        Nationality = viewModel.Nationality == "其他" ? viewModel.OtherNationality : "臺灣",
                        IdNumber = viewModel.IdNumber,
                        LineId = viewModel.LineId,
                        Phone = viewModel.Phone,
                        Address = $"{viewModel.County}{viewModel.District}{viewModel.Zipcode} {viewModel.Address}",
                        EmergencyContactName = viewModel.EmergencyContactName,
                        EmergencyContactPhone = viewModel.EmergencyContactPhone,
                        HasCompanion = viewModel.HasCompanion == "是",
                        CompanionName = viewModel.CompanionName,
                        HikingExperience = viewModel.HikingExperience,
                        ExerciseHabit = viewModel.ExerciseHabit,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        is_creator = viewModel.is_creator,
                        activity_id = viewModel.ActivityId
                    };

                    _db.UserProfiles.Add(userProfile);
                    _db.SaveChanges();

                    // 設定成功訊息
                    TempData["Success"] = "報名資料已成功送出！";
                    TempData["AlertType"] = "success";

                    // 重導向到首頁
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // 錯誤處理
                    ModelState.AddModelError("", "送出資料時發生錯誤：" + ex.Message);
                }
            }


            return View(viewModel);
        }
    }
}