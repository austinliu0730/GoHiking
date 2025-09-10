# GoHiking 管理後台功能說明

## 新增功能概述

本次更新為 GoHiking 登山活動管理系統增加了強化的後台管理功能，包括：

### 1. 活動統計儀表板 (`/Admin/Dashboard`)

#### 功能特色：
- **總覽統計卡片**：顯示總活動數、進行中活動、註冊用戶數、參與人次
- **難度等級分布餅圖**：視覺化不同難度等級的活動分布情況
- **成團狀態統計圖**：顯示各種成團狀態的活動數量
- **每月活動趨勢線圖**：追蹤最近12個月的活動建立趨勢
- **地區活動熱度分布**：橫條圖顯示各地區的活動熱門度

#### 技術實現：
- 使用 Chart.js 製作互動式圖表
- AJAX API 呼叫取得即時數據
- 響應式設計，支援各種螢幕尺寸

### 2. 成團狀態管理 (`/Admin/GroupManagement`)

#### 功能特色：
- **智能狀態分類**：
  - 招募中：參與率 < 50%
  - 即將成團：參與率 50-80%
  - 已成團：參與率 >= 80%
  - 額滿：參與人數 = 最大人數
  - 已結束：活動日期已過

- **多維度篩選**：
  - 按成團狀態篩選
  - 按啟用狀態篩選  
  - 按時間範圍篩選

- **即時統計面板**：顯示各狀態活動數量
- **詳細資訊表格**：包含進度條、剩餘名額等視覺化資訊
- **快速操作按鈕**：查看、編輯、啟用/停用功能

### 3. 權限控制

- 只有 Permission = 1 的管理員才能訪問新的統計功能
- 整合到現有的管理選單中
- 保持與現有審核開團、人員管理功能的一致性

## 文件結構

### 新增的文件：

#### Controllers
- `Controllers/AdminController.cs` - 管理後台控制器
  - Dashboard() - 儀表板首頁
  - GetDifficultyDistribution() - API: 難度分布數據
  - GetMonthlyActivityTrend() - API: 月度趨勢數據
  - GetRegionDistribution() - API: 地區分布數據
  - GetGroupStatus() - API: 成團狀態數據
  - GroupManagement() - 成團狀態管理頁面

#### ViewModels  
- `ViewModels/AdminDashboardViewModel.cs`
  - AdminDashboardViewModel - 儀表板數據模型
  - GroupStatusViewModel - 成團狀態數據模型
  - ChartDataViewModel - 圖表數據模型

#### Views
- `Views/Admin/Dashboard.cshtml` - 統計儀表板頁面
- `Views/Admin/GroupManagement.cshtml` - 成團狀態管理頁面

#### 修改的文件：
- `Views/Shared/_Layout.cshtml` - 增加管理選單項目和 Font Awesome 支援

## 數據分析邏輯

### 難度等級對應：
- 1: 簡單
- 2: 輕鬆  
- 3: 普通
- 4: 困難
- 5: 挑戰

### 成團狀態判斷：
```csharp
private string GetGroupStatus(TripActivity activity)
{
    if (activity.activity_date < DateTime.Now)
        return "已結束";
    
    if (activity.participants >= activity.max_participants)
        return "額滿";
    
    double fillRate = (double)activity.participants / (activity.max_participants ?? 1);
    
    if (fillRate >= 0.8)
        return "即將成團";
    else if (fillRate >= 0.5)
        return "已成團";
    else
        return "招募中";
}
```

## 使用方式

### 訪問管理後台：
1. 使用管理員帳號 (Permission = 1) 登入系統
2. 點擊導航欄中的「管理」選單
3. 選擇「統計儀表板」或「成團狀態」

### 查看統計數據：
1. 進入統計儀表板查看總體數據
2. 各種圖表會自動載入並顯示互動式數據
3. 可以透過滑鼠懸停查看詳細數值

### 管理成團狀態：
1. 進入成團狀態管理頁面
2. 使用篩選功能找到特定狀態的活動
3. 查看成團進度和剩餘名額
4. 使用操作按鈕進行管理

## 技術依賴

- **Chart.js**: 圖表庫，透過 CDN 載入
- **Font Awesome 6.0**: 圖標庫，透過 CDN 載入  
- **jQuery**: 用於 AJAX 請求和 DOM 操作
- **Bootstrap**: 響應式佈局和樣式
- **Entity Framework 6**: 數據庫操作

## 未來擴展建議

1. **匯出功能**：增加圖表和數據的 PDF/Excel 匯出
2. **即時通知**：成團狀態變化時的即時提醒
3. **更多統計維度**：如用戶活躍度、收入統計等
4. **權限細分**：不同管理員權限等級
5. **歷史數據**：長期趨勢分析和預測