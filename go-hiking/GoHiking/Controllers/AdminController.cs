using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GoHiking.data;
using GoHiking.ViewModels;

namespace GoHiking.Controllers
{
    public class AdminController : Controller
    {
        private HikingDBEntities1 _db = new HikingDBEntities1();

        // GET: Admin/Dashboard
        public ActionResult Dashboard()
        {
            var viewModel = new AdminDashboardViewModel();

            // 基本統計數據
            viewModel.TotalActivities = _db.TripActivities.Count();
            viewModel.ActiveActivities = _db.TripActivities.Count(t => t.is_active == true);
            viewModel.TotalUsers = _db.Users.Count();
            viewModel.TotalParticipants = _db.UserProfiles.Count();

            return View(viewModel);
        }

        // GET: Admin/GetDifficultyDistribution
        // API: 取得難度等級分布數據
        public JsonResult GetDifficultyDistribution()
        {
            var difficultyData = _db.TripActivities
                .GroupBy(t => t.diff)
                .Select(g => new
                {
                    difficulty = g.Key,
                    count = g.Count()
                })
                .OrderBy(x => x.difficulty)
                .ToList()
                .Select(x => new
                {
                    difficulty = x.difficulty,
                    count = x.count,
                    label = GetDifficultyLabel(x.difficulty)
                })
                .ToList();

            return Json(difficultyData, JsonRequestBehavior.AllowGet);
        }



        // GET: Admin/GetMonthlyActivityTrend
        [HttpGet]
        public JsonResult GetMonthlyActivityTrend()
        {
            // 計算12個月前的日期
            var twelveMonthsAgo = DateTime.Now.AddMonths(-12);

            var monthlyData = _db.TripActivities
                .Where(t => t.activity_date >= twelveMonthsAgo)
                .GroupBy(t => new
                {
                    Year = t.activity_date.Year,
                    Month = t.activity_date.Month
                })
                .Select(g => new
                {
                    year = g.Key.Year,
                    month = g.Key.Month,
                    count = g.Count()
                })
                .OrderBy(x => x.year).ThenBy(x => x.month)
                .ToList()
                .Select(x => new
                {
                    year = x.year,
                    month = x.month,
                    count = x.count,
                    date = x.year + "-" + x.month.ToString().PadLeft(2, '0')
                })
                .ToList();

            return Json(monthlyData, JsonRequestBehavior.AllowGet);
        }

        // API: 取得地區活動分布
        [HttpGet]
        public JsonResult GetRegionDistribution()
        {
            var regionData = _db.TripActivities
                .Where(t => !string.IsNullOrEmpty(t.region))
                .GroupBy(t => t.region)
                .Select(g => new
                {
                    region = g.Key,
                    count = g.Count()
                })
                .OrderByDescending(x => x.count)
                .ToList();

            return Json(regionData, JsonRequestBehavior.AllowGet);
        }

        // API: 取得成團狀態統計
        [HttpGet]
        public JsonResult GetGroupStatus()
        {
            // 先取得所有活動資料
            var activities = _db.TripActivities.ToList();
            var statusData = new List<object>();

            // 在記憶體中計算各種狀態的數量
            int recruiting = activities.Count(t => GetGroupStatus(t) == "招募中");
            int almostFull = activities.Count(t => GetGroupStatus(t) == "即將成團");
            int stable = activities.Count(t => GetGroupStatus(t) == "招募穩定");
            int full = activities.Count(t => GetGroupStatus(t) == "額滿");
            int finished = activities.Count(t => GetGroupStatus(t) == "已結束");

            statusData.Add(new { status = "招募中", count = recruiting });
            statusData.Add(new { status = "即將成團", count = almostFull });
            statusData.Add(new { status = "招募穩定", count = stable });
            statusData.Add(new { status = "額滿", count = full });
            statusData.Add(new { status = "已結束", count = finished });

            return Json(statusData, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/GroupManagement
        public ActionResult GroupManagement(int page = 1)
        {
            int pageSize = 10;

            // 先取得原始資料
            var activitiesData = _db.TripActivities
                .OrderByDescending(t => t.created_date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList(); // 先執行查詢

            // 再在記憶體中計算狀態
            var activities = activitiesData.Select(t => new GroupStatusViewModel
            {
                ActivityId = t.activity_id,
                MountainName = t.mt_name,
                GroupName = t.group_name,
                ActivityDate = t.activity_date,
                MaxParticipants = t.max_participants ?? 0,
                CurrentParticipants = t.participants,
                Status = GetGroupStatus(t), // 現在可以在記憶體中呼叫
                IsActive = t.is_active ?? false,
                CreatedDate = t.created_date
            }).ToList();

            int totalCount = _db.TripActivities.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.CurrentPage = page;

            // 計算狀態統計
            var allActivities = _db.TripActivities.ToList();
            ViewBag.RecruitingCount = allActivities.Count(t => GetGroupStatus(t) == "招募中");
            ViewBag.AlmostFullCount = allActivities.Count(t => GetGroupStatus(t) == "即將成團");
            ViewBag.StableCount = allActivities.Count(t => GetGroupStatus(t) == "招募穩定");
            ViewBag.FullCount = allActivities.Count(t => GetGroupStatus(t) == "額滿");
            ViewBag.FinishedCount = allActivities.Count(t => GetGroupStatus(t) == "已結束");

            return View(activities);
        }

        // 輔助方法：取得難度等級標籤
        private string GetDifficultyLabel(int difficulty)
        {
            switch (difficulty)
            {
                case 1: return "簡單";
                case 2: return "輕鬆";
                case 3: return "普通";
                default: return "未分類";
            }
        }

        // 輔助方法：判斷成團狀態
        private string GetGroupStatus(TripActivity activity)
        {
            if (activity.activity_date < DateTime.Now)
                return "已結束";

            if (activity.participants >= activity.max_participants)
                return "額滿";

            double fillRate = (double)activity.participants / (activity.max_participants ?? 1);

            if (fillRate >= 0.6)
                return "即將成團";
            else if (fillRate >= 0.5)
                return "招募穩定";
            else
                return "招募中";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}