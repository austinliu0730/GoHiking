using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoHiking.data;
using GoHiking.ViewModels;

namespace GoHiking.Controllers
{
    public class HomeController : Controller
    {
        private HikingDBEntities1 _db = new HikingDBEntities1();

        // GET: Home
        public ActionResult Index()
        {

            var viewModel = new HomeViewModel();



            viewModel.ActiveActivities = _db.TripActivities
                                    .Where(t => t.is_active == true && 
                                           t.activity_date >= DateTime.Today && 
                                           (int)(t.max_participants - t.participants) != 0)
                                    .Select(t => new ActivityViewModel
                                    {
                                        mt_name = t.mt_name,
                                        activity_date = t.activity_date,
                                        meeting_location = t.meeting_location,
                                        participants = (int)(t.max_participants - t.participants),
                                        activity_id = t.activity_id,
                                        meeting_time = t.meeting_time,
                                    })
                                    .OrderBy(t => t.participants)
                                    .Take(6)
                                    .ToList();


            viewModel.BeginnerActivities = _db.TripActivities
                                    .Where(t => t.diff <= 2 && 
                                           t.is_active == true && 
                                           t.activity_date >= DateTime.Today && 
                                           (int)(t.max_participants - t.participants) != 0)
                                    .Select(t => new BeginnerActivityViewModel
                                    {
                                        activity_id = t.activity_id,
                                        mt_name = t.mt_name,
                                        price = t.price,
                                        difficulty_level = t.diff,
                                        ImageData = t.GroupImages.FirstOrDefault().image_data,
                                        ImageName = t.GroupImages.FirstOrDefault().image_name,
                                        walk_days = t.walk_days,
                                        mt_id = t.mt_id,
                                    })
                                    .Take(4) // 
                                    .ToList();

            viewModel.IntermediateActivities = _db.TripActivities
                                    .Where(t => t.diff > 2 && 
                                           t.is_active == true && 
                                           t.activity_date >= DateTime.Today && 
                                           (int)(t.max_participants - t.participants) != 0)
                                    .Select(t => new IntermediateActivityViewModel
                                    {
                                        activity_id = t.activity_id,
                                        mt_name = t.mt_name,
                                        price = t.price,
                                        difficulty_level = t.diff,
                                        ImageData = t.GroupImages.FirstOrDefault().image_data,
                                        ImageName = t.GroupImages.FirstOrDefault().image_name,
                                        walk_days = t.walk_days,
                                        mt_id = t.mt_id,
                                    })
                                    .Take(4)
                                    .ToList();
            return View(viewModel);
        }



        // GET: Home/Upcoming
        public ActionResult Upcoming(int page = 1)
        {
            int pageSize = 9;
            var viewModel = new HomeViewModel();

            var activeActivitiesQuery = _db.TripActivities
                .Where(t => t.is_active == true && 
                       t.activity_date >= DateTime.Today && 
                       (int)(t.max_participants - t.participants) > 0)
                .OrderBy(t => t.max_participants - t.participants);

            int totalCount = activeActivitiesQuery.Count();
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            viewModel.ActiveActivities = activeActivitiesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new ActivityViewModel
                {
                    mt_name = t.mt_name,
                    activity_date = t.activity_date,
                    meeting_location = t.meeting_location,
                    participants = (int)(t.max_participants - t.participants),
                    activity_id = t.activity_id,
                })
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;

            return View(viewModel);
        }



        // GET: Home/Blog
        public ActionResult Blog()
        {
            return View();
        }

        //GET: Home/Activity
        public ActionResult Activity(int page = 1, int? days = null, string region = null)
        {
            int pageSize = 9;

            // 有開團的（只包含未過期且有名額的團）
            var activeActivities = _db.TripActivities
                .Where(t => t.activity_date >= DateTime.Today && (int)(t.max_participants - t.participants) > 0)
                .GroupBy(t => t.mt_id)
                .OrderByDescending(g => g.Count())
                .SelectMany(g => g)
                .Select(t => new ActivityViewModel
                {
                    activity_id = t.activity_id,
                    mt_id = t.mt_id,
                    mt_name = t.mt_name,
                    walk_days = t.walk_days,
                    price = t.price,
                    diff = t.diff,
                    park_permit = t.park_permit,
                    mt_permit = t.mt_permit,
                    badge = t.badge,
                    region = t.region,
                    ImageData = t.GroupImages.FirstOrDefault().image_data,
                    ImageName = t.GroupImages.FirstOrDefault().image_name,
                }).ToList();

            // 沒有有效開團的（包含過期團和完全沒開團的）
            var validActiveActivityMtIds = activeActivities.Select(a => a.mt_id).Distinct();
            var inactiveActivities = _db.MountainTrails
                    .Where(mt => !validActiveActivityMtIds.Contains(mt.ID))
                    .Select(m => new ActivityViewModel
                    {
                        activity_id = 0,
                        mt_id = m.ID,
                        mt_name = m.mt_name,
                        walk_days = m.walk_days,
                        price = 3500,
                        diff = m.diff,
                        park_permit = m.park_permit,
                        mt_permit = m.mt_permit,
                        badge = m.badge,
                        region = m.region,
                        ImageData = null,
                        ImageName = m.mt_name,

                    }).ToList();

            // 合併：有開團的在前，沒開團的在後
            var allActivities = activeActivities.Union(inactiveActivities);

            // 篩選條件
            if (days.HasValue)
            {
                allActivities = allActivities.Where(a => a.walk_days == days.Value);
            }

            if (!string.IsNullOrEmpty(region))
            {
                allActivities = allActivities.Where(a => a.region == region);
            }



            // 排序：有開團先，然後按篩選條件排序
            var sortedActivities = allActivities
                .OrderByDescending(a => a.activity_id != 0);

            // 根據篩選條件調整排序
            if (!string.IsNullOrEmpty(region))
            {
                sortedActivities = sortedActivities
                    .ThenByDescending(a => a.region == region) // 符合地區的排前面
                    .ThenBy(a => a.mt_id);
            }
            else if (days.HasValue)
            {
                sortedActivities = sortedActivities
                    .ThenByDescending(a => a.walk_days == days.Value) // 符合天數的排前面
                    .ThenBy(a => a.mt_id);
            }
            else
            {
                sortedActivities = sortedActivities.ThenBy(a => a.mt_id);
            }

            // 分頁
            int totalCount = allActivities.Count();
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var pagedActivities = allActivities
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentDays = days;
            ViewBag.CurrentRegion = region;

            return View(pagedActivities);

        }


    }
}