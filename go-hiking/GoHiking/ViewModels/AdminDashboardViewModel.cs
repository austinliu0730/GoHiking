using System;
using System.Collections.Generic;

namespace GoHiking.ViewModels
{
    public class AdminDashboardViewModel
    {
        // 基本統計數據
        public int TotalActivities { get; set; }
        public int ActiveActivities { get; set; }
        public int TotalUsers { get; set; }
        public int TotalParticipants { get; set; }
        
        // 計算衍生數據
        public int InactiveActivities => TotalActivities - ActiveActivities;
        public double ActivityActiveRate => TotalActivities > 0 ? (double)ActiveActivities / TotalActivities * 100 : 0;
    }

    public class GroupStatusViewModel
    {
        public int ActivityId { get; set; }
        public string MountainName { get; set; }
        public string GroupName { get; set; }
        public DateTime ActivityDate { get; set; }
        public int MaxParticipants { get; set; }
        public int CurrentParticipants { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        
        // 計算成團率
        public double FillRate => MaxParticipants > 0 ? (double)CurrentParticipants / MaxParticipants * 100 : 0;
        
        // 剩餘名額
        public int RemainingSlots => MaxParticipants - CurrentParticipants;
    }

    // 圖表數據模型
    public class ChartDataViewModel
    {
        public string Label { get; set; }
        public int Value { get; set; }
        public string Color { get; set; }
    }

    public class MonthlyTrendViewModel
    {
        public string Month { get; set; }
        public int ActivityCount { get; set; }
        public int ParticipantCount { get; set; }
    }
}