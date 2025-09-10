using GoHiking.data;
using GoHiking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoHiking.Controllers
{
    public class UserController : Controller
    {
        private HikingDBEntities1 _db = new HikingDBEntities1();
        // GET: User
        public ActionResult Index()
        {
            List<User> model = _db.Users.ToList();

            return View(model);
        }

        // GET: User/Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: User/Login
        [HttpPost]
        public ActionResult Login(User model, string returnUrl, bool rememberMe = false)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);
            if (user != null)
            {
                Session["UserLogin"] = user;

                if (rememberMe)
                {
                    // 建立 cookie，設定 7 天過期
                    var cookie = new HttpCookie("RememberUser", user.UserName)
                    {
                        Expires = DateTime.Now.AddDays(7),
                        HttpOnly = true  // 增加安全性
                    };
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    // 如果沒勾選記住我，清除舊的 cookie
                    var cookie = new HttpCookie("RememberUser", "")
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    };
                    Response.Cookies.Add(cookie);
                }

                // 如果有 returnUrl 且是本地 URL，就導回原頁面
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }



            else
            {
                ModelState.AddModelError("", "帳號密碼檢查失敗");
                // 登入失敗時也要保留 returnUrl，讓使用者重新輸入後還能回到原頁面
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
        }

        // 登出
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(UserViewModel model)
        {

            if (ModelState.IsValid)
            {

                bool userExists = _db.Users.Any(u => u.UserName == model.UserName);
                if (userExists)
                {
                    ModelState.AddModelError("UserName", "此帳號已被註冊過");
                    return View(model);
                }

                if (_db.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("UserName", "此帳號已被註冊過");
                    return View(model);
                }

                User user = new User()
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    Email = model.Email
                };

                _db.Users.Add(user);
                _db.SaveChanges();

                TempData["AccountCreated"] = true;
                return RedirectToAction("Login");

            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            User model = _db.Users.FirstOrDefault(x => x.Id == id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(User model)
        {

            //先把DB舊資料找回來
            User oldData = _db.Users.FirstOrDefault(x => x.Id == model.Id);

            //改成網頁送回來的資料
            oldData.UserName = model.UserName;
            oldData.Password = model.Password;
            oldData.Email = model.Email;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        //GET:User/Delete/?
        public ActionResult Delete(int id)
        {
            User users = _db.Users.FirstOrDefault(x => x.Id == id);

            if (users != null)
            {
                // 刪除相關聯的 UserProfile 記錄
                var userProfiles = _db.UserProfiles.Where(up => up.UserId == id).ToList();
                if (userProfiles.Any())
                {
                    _db.UserProfiles.RemoveRange(userProfiles);
                }

                // 刪除相關聯的 UserDetails 記錄
                var userDetails = _db.UserDetails.Where(ud => ud.UserId == id).ToList();
                if (userDetails.Any())
                {
                    _db.UserDetails.RemoveRange(userDetails);
                }

                // 刪除相關聯的 TripActivities 記錄
                var tripActivities = _db.TripActivities.Where(ta => ta.user_id == id).ToList();
                if (tripActivities.Any())
                {
                    _db.TripActivities.RemoveRange(tripActivities);
                }

                // 刪除相關聯的 TripGroup 記錄
                var tripGroups = _db.TripGroups.Where(tg => tg.user_id == id).ToList();
                if (tripGroups.Any())
                {
                    _db.TripGroups.RemoveRange(tripGroups);
                }

                // 最後刪除 User
                _db.Users.Remove(users);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}