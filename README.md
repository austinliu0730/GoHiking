# \# GoHiking 專案架構分析文檔

# 

# \## 專案概述

# 

# \*\*GoHiking\*\* 是一個基於 ASP.NET MVC 的登山活動管理平台，提供登山路線瀏覽、群組組團、活動管理等功能。

# 

# \## 1. 專案目錄結構

# 

# \### 1.1 根目錄結構

# ```

# Hiking\_claude/

# ├── go-hiking/                  # 主專案資料夾

# │   ├── GoHiking/              # ASP.NET MVC 專案

# │   ├── GoHiking.sln          # Visual Studio 解決方案檔

# │   ├── data/                  # 資料庫相關檔案

# │   ├── packages/              # NuGet 套件

# │   └── untree.co-sterial/     # 前端模板資源

# ├── CLAUDE.md                  # Claude 指導文件

# └── .git/                      # Git 版控

# ```

# 

# \### 1.2 GoHiking 專案內部結構

# ```

# GoHiking/

# ├── Controllers/               # MVC 控制器

# │   ├── HomeController.cs

# │   ├── UserController.cs

# │   ├── ProductsController.cs

# │   ├── ProfileController.cs

# │   ├── TeamController.cs

# │   └── TripHelperController.cs

# ├── Views/                     # Razor 視圖 (28個檔案)

# │   ├── Home/

# │   ├── User/

# │   ├── Products/

# │   ├── Profile/

# │   ├── Team/

# │   ├── TripHelper/

# │   └── Shared/

# ├── ViewModels/               # 視圖模型

# │   ├── HomeViewModel.cs

# │   ├── UserViewModel.cs

# │   ├── ActivityViewModel.cs

# │   ├── TripGroupViewModel.cs

# │   └── 其他...

# ├── data/                     # Entity Framework 資料模型

# │   ├── HikingModel.edmx      # Entity Framework EDMX 模型

# │   ├── HikingModel.Context.cs

# │   ├── User.cs

# │   ├── TripActivity.cs

# │   ├── MountainTrail.cs

# │   └── 其他實體類別...

# ├── Models/                   # 商業邏輯模型

# ├── Content/                  # 靜態內容

# ├── css/                      # CSS 樣式

# │   ├── bootstrap/

# │   ├── mycss/               # 自定義樣式

# │   └── components/

# ├── js/                      # JavaScript 檔案

# ├── images/                  # 圖片資源

# ├── Scripts/                 # jQuery 腳本

# └── App\_Start/              # 應用程式啟動設定

# &nbsp;   └── RouteConfig.cs

# ```

# 

# \## 2. 技術框架和語言

# 

# \### 2.1 後端技術堆疊

# \- \*\*主框架\*\*: ASP.NET MVC 5.2.9

# \- \*\*開發平台\*\*: .NET Framework 4.8

# \- \*\*程式語言\*\*: C#

# \- \*\*ORM\*\*: Entity Framework 6.5.1 (Database First)

# \- \*\*資料庫\*\*: SQL Server Express (.\\sqlexpress)

# \- \*\*開發工具\*\*: Visual Studio

# \- \*\*包管理器\*\*: NuGet

# 

# \### 2.2 前端技術堆疊

# \- \*\*檢視引擎\*\*: Razor

# \- \*\*UI框架\*\*: Bootstrap 5

# \- \*\*JavaScript函式庫\*\*: 

# &nbsp; - jQuery 3.7.1

# &nbsp; - AOS (Animate On Scroll)

# &nbsp; - Flatpickr (日期選擇器)

# &nbsp; - GLightbox (圖片展示)

# &nbsp; - Tiny Slider (輪播)

# \- \*\*CSS預處理器\*\*: SCSS

# \- \*\*字型圖標\*\*: Flaticon, Icomoon

# 

# \### 2.3 關鍵NuGet套件

# ```xml

# \- EntityFramework (6.5.1)

# \- Microsoft.AspNet.Mvc (5.2.9)

# \- Microsoft.AspNet.Razor (3.2.9)

# \- Microsoft.AspNet.WebPages (3.2.9)

# \- jQuery (3.7.1)

# \- Microsoft.CodeDom.Providers.DotNetCompilerPlatform (2.0.1)

# ```

# 

# \## 3. MVC 架構組織方式

# 

# \### 3.1 Controller 層

# \*\*6個核心控制器\*\*:

# 

# | 控制器 | 主要功能 |

# |--------|----------|

# | `HomeController` | 首頁、活動列表、部落格 |

# | `UserController` | 使用者註冊、登入、管理 |

# | `ProductsController` | 產品/裝備管理 |

# | `ProfileController` | 使用者個人資料管理 |

# | `TeamController` | 團隊/群組管理 |

# | `TripHelperController` | 行程輔助功能 (評論、裝備、排程) |

# 

# \### 3.2 Model 層

# 採用 \*\*Database First\*\* 方式：

# 

# \*\*核心資料實體 (11個)\*\*:

# \- `User`, `UserDetail`, `UserProfile` - 使用者相關

# \- `TripActivity` - 登山活動

# \- `TripGroup` - 群組資料

# \- `MountainTrail` - 登山路線

# \- `GroupImage` - 圖片管理

# \- `Comment` - 評論系統

# 

# \*\*業務模型\*\*:

# \- `SportsEvent` - 運動賽事模型

# 

# \### 3.3 View 層

# \*\*檢視模型 (7個)\*\*:

# \- `HomeViewModel` - 首頁資料彙整

# \- `ActivityViewModel` - 活動顯示資料

# \- `UserViewModel`, `UserProfileViewModel` - 使用者相關檢視

# \- `TripGroupViewModel` - 群組管理檢視

# \- `CommentViewModel` - 評論檢視

# 

# \*\*檢視結構\*\*:

# \- 總計 28 個 `.cshtml` 檔案

# \- 使用 `\_Layout.cshtml` 作為主版型

# \- 各控制器有對應的檢視資料夾

# 

# \### 3.4 路由設定

# \- \*\*路由配置\*\*: `App\_Start/RouteConfig.cs`

# \- \*\*預設路由\*\*: `{controller}/{action}/{id}`

# \- \*\*開發伺服器\*\*: `http://localhost:55176/`

# 

# \## 4. 資料庫配置和連接方式

# 

# \### 4.1 資料庫設定

# \- \*\*資料庫類型\*\*: SQL Server Express

# \- \*\*資料來源\*\*: `.\\sqlexpress` (本地實例)

# \- \*\*資料庫名稱\*\*: `HikingDB`

# \- \*\*驗證方式\*\*: Windows 整合驗證 (Integrated Security)

# \- \*\*連線設定\*\*: 支援多個活動結果集 (MultipleActiveResultSets)

# 

# \### 4.2 Entity Framework 配置

# ```xml

# <connectionStrings>

# &nbsp; <add name="HikingDBEntities1" 

# &nbsp;      connectionString="metadata=res://\*/data.HikingModel.csdl|res://\*/data.HikingModel.ssdl|res://\*/data.HikingModel.msl;

# &nbsp;                       provider=System.Data.SqlClient;

# &nbsp;                       provider connection string=\&quot;data source=.\\sqlexpress;

# &nbsp;                       initial catalog=HikingDB;

# &nbsp;                       integrated security=True;

# &nbsp;                       multipleactiveresultsets=True;

# &nbsp;                       trustservercertificate=True;

# &nbsp;                       application name=EntityFramework\&quot;" 

# &nbsp;      providerName="System.Data.EntityClient" />

# </connectionStrings>

# ```

# 

# \### 4.3 DbContext 類別

# ```csharp

# public partial class HikingDBEntities1 : DbContext

# {

# &nbsp;   // DbSet 屬性對應資料庫表格

# &nbsp;   public virtual DbSet<User> Users { get; set; }

# &nbsp;   public virtual DbSet<TripActivity> TripActivities { get; set; }

# &nbsp;   public virtual DbSet<MountainTrail> MountainTrails { get; set; }

# &nbsp;   public virtual DbSet<TripGroup> TripGroups { get; set; }

# &nbsp;   public virtual DbSet<GroupImage> GroupImages { get; set; }

# &nbsp;   public virtual DbSet<Comment> Comments { get; set; }

# &nbsp;   // ...其他實體集合

# }

# ```

# 

# \### 4.4 資料庫架構特色

# \- \*\*EDMX模型\*\*: `data/HikingModel.edmx` 視覺化設計資料庫關聯

# \- \*\*自動生成\*\*: 實體類別透過 T4 模板自動產生

# \- \*\*圖片儲存\*\*: 使用 `varbinary(max)` 儲存圖片二進制資料

# \- \*\*關聯設計\*\*: 支援一對多、多對多關聯 (使用者-活動、群組-圖片等)

# 

# \## 5. 專案特色功能

# 

# \### 5.1 核心功能模組

# 1\. \*\*活動管理系統\*\* - 瀏覽、篩選、參加登山活動

# 2\. \*\*使用者系統\*\* - 註冊、登入、個人資料管理

# 3\. \*\*群組功能\*\* - 組團、群組圖片分享

# 4\. \*\*行程規劃\*\* - 裝備清單、行程排程、行事曆整合

# 5\. \*\*評論系統\*\* - 活動評論、經驗分享

# 6\. \*\*內容管理\*\* - 圖片上傳、部落格文章

# 

# \### 5.2 技術亮點

# \- \*\*響應式設計\*\*: Bootstrap 5 + 自定義CSS

# \- \*\*圖片處理\*\*: 二進制儲存 + 動態載入

# \- \*\*篩選功能\*\*: 依難度、地區、天數篩選活動

# \- \*\*動畫效果\*\*: AOS動畫庫增強使用者體驗

# \- \*\*日期功能\*\*: Flatpickr日期選擇器

# \- \*\*圖片展示\*\*: GLightbox燈箱效果

# 

# \## 6. 開發和部署

# 

# \### 6.1 開發環境

# \- \*\*建置工具\*\*: MSBuild / Visual Studio

# \- \*\*除錯模式\*\*: Debug Configuration

# \- \*\*開發伺服器\*\*: IIS Express

# \- \*\*連接埠\*\*: 55176

# 

# \### 6.2 建置指令

# ```bash

# \# 建置解決方案

# dotnet build

# 

# \# 還原NuGet套件

# dotnet restore

# 

# \# 使用Visual Studio

# \# Ctrl+Shift+B (建置)

# \# F5 (執行除錯)

# ```

# 

# \## 7. 專案維護建議

# 

# \### 7.1 架構優勢

# \- ✅ 使用成熟的ASP.NET MVC框架

# \- ✅ 清晰的MVC分層架構

# \- ✅ Entity Framework提供強型別資料存取

# \- ✅ 響應式前端設計

# 

# \### 7.2 改進建議

# \- 🔄 考慮升級至.NET Core/.NET 5+

# \- 🔄 實作Repository Pattern解耦資料存取

# \- 🔄 加入單元測試專案

# \- 🔄 實作API層支援行動應用

# \- 🔄 加入快取機制提升效能

# \- 🔄 實作記錄系統(Logging)

# 

# ---

# 

# \*此文檔基於專案現況分析，如有更新請同步修正。\*

