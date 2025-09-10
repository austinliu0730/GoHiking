# GoHiking 登山活動管理平台

一個基於 ASP.NET MVC 的登山活動管理平台，提供登山路線瀏覽、群組組團、活動管理等功能。
## 📋 目錄

- [專案概述](#專案概述)
- [功能特色](#功能特色)
- [技術架構](#技術架構)
- [專案結構](#專案結構)
- [快速開始](#快速開始)
- [資料庫配置](#資料庫配置)
- [開發指南](#開發指南)
- [貢獻指南](#貢獻指南)

## 🏔️ 專案概述

GoHiking 是一個全方位的登山活動管理平台，專為登山愛好者設計。提供完整的活動組織、路線管理、群組互動等功能，讓登山變得更安全、更有趣。

### 主要功能

- 🔍 **活動瀏覽** - 搜尋和篩選各種登山活動
- 👥 **群組管理** - 建立和管理登山群組
- 📅 **行程規劃** - 完整的行程安排和裝備清單
- 💬 **互動評論** - 分享登山經驗和心得
- 📱 **響應式設計** - 支援各種裝置使用

## ✨ 功能特色

### 核心功能模組

| 模組 | 描述 | 狀態 |
|------|------|------|
| 🏠 活動管理 | 瀏覽、篩選、參加登山活動 | ✅ 完成 |
| 👤 使用者系統 | 註冊、登入、個人資料管理 | ✅ 完成 |
| 👥 群組功能 | 組團、群組圖片分享 | ✅ 完成 |
| 📋 行程規劃 | 裝備清單、行程排程、行事曆整合 | ✅ 完成 |
| 💬 評論系統 | 活動評論、經驗分享 | ✅ 完成 |
| 📸 內容管理 | 圖片上傳、部落格文章 | ✅ 完成 |

### 技術亮點

- **響應式設計** - 使用 Bootstrap 5 + 自定義 CSS
- **圖片處理** - 二進制儲存 + 動態載入
- **智能篩選** - 依難度、地區、天數篩選活動
- **動畫效果** - AOS 動畫庫增強使用者體驗
- **現代化UI** - 包含日期選擇器、圖片燈箱等功能

## 🛠️ 技術架構

### 後端技術

- **框架**: ASP.NET MVC 5.2.9
- **平台**: .NET Framework 4.8
- **語言**: C#
- **ORM**: Entity Framework 6.5.1 (Database First)
- **資料庫**: SQL Server Express
- **開發工具**: Visual Studio

### 前端技術

- **檢視引擎**: Razor
- **UI 框架**: Bootstrap 5
- **JavaScript 函式庫**:
  - jQuery 3.7.1
  - AOS (Animate On Scroll)
  - Flatpickr (日期選擇器)
  - GLightbox (圖片展示)
  - Tiny Slider (輪播)

## 📁 專案結構

```
Hiking_claude/
├── go-hiking/                      # 主專案資料夾
│   ├── GoHiking/                  # ASP.NET MVC 專案
│   │   ├── Controllers/           # MVC 控制器
│   │   │   ├── HomeController.cs
│   │   │   ├── UserController.cs
│   │   │   ├── ProductsController.cs
│   │   │   ├── ProfileController.cs
│   │   │   ├── TeamController.cs
│   │   │   └── TripHelperController.cs
│   │   ├── Views/                 # Razor 視圖 (28個檔案)
│   │   ├── ViewModels/           # 視圖模型
│   │   ├── data/                 # Entity Framework 資料模型
│   │   ├── Models/               # 商業邏輯模型
│   │   ├── Content/              # 靜態內容
│   │   ├── css/                  # CSS 樣式
│   │   ├── js/                   # JavaScript 檔案
│   │   ├── images/               # 圖片資源
│   │   └── Scripts/              # jQuery 腳本
│   ├── GoHiking.sln              # Visual Studio 解決方案檔
│   ├── data/                     # 資料庫相關檔案
│   ├── packages/                 # NuGet 套件
│   └── untree.co-sterial/        # 前端模板資源
├── CLAUDE.md                     # Claude 指導文件
└── README.md                     # 本文件
```

### MVC 架構組織

#### 控制器層 (6個核心控制器)

| 控制器 | 主要功能 |
|--------|----------|
| `HomeController` | 首頁、活動列表、部落格 |
| `UserController` | 使用者註冊、登入、管理 |
| `ProductsController` | 產品/裝備管理 |
| `ProfileController` | 使用者個人資料管理 |
| `TeamController` | 團隊/群組管理 |
| `TripHelperController` | 行程輔助功能 |

#### 資料模型層

採用 **Database First** 方式，核心實體包括:

- `User`, `UserDetail`, `UserProfile` - 使用者相關
- `TripActivity` - 登山活動
- `TripGroup` - 群組資料
- `MountainTrail` - 登山路線
- `GroupImage` - 圖片管理
- `Comment` - 評論系統

## 🚀 快速開始

### 環境需求

- Windows 10/11
- Visual Studio 2019 或更新版本
- SQL Server Express
- .NET Framework 4.8

### 安裝步驟

1. **複製專案**
   ```bash
   git clone [your-repository-url]
   cd Hiking_claude/go-hiking
   ```

2. **還原 NuGet 套件**
   ```bash
   dotnet restore
   ```

3. **建置專案**
   ```bash
   dotnet build
   # 或在 Visual Studio 中按 Ctrl+Shift+B
   ```

4. **設定資料庫** (請參考下方資料庫配置章節)

5. **執行專案**
   ```bash
   # 在 Visual Studio 中按 F5
   # 或直接執行
   dotnet run
   ```

6. **瀏覽應用程式**
   - 開啟瀏覽器訪問: `http://localhost:55176/`

## 🗄️ 資料庫配置

### 連接字串設定

在 `web.config` 中配置資料庫連接:

```xml
<connectionStrings>
  <add name="HikingDBEntities1" 
       connectionString="metadata=res://*/data.HikingModel.csdl|res://*/data.HikingModel.ssdl|res://*/data.HikingModel.msl;
                        provider=System.Data.SqlClient;
                        provider connection string=&quot;data source=.\sqlexpress;
                        initial catalog=HikingDB;
                        integrated security=True;
                        multipleactiveresultsets=True;
                        trustservercertificate=True;
                        application name=EntityFramework&quot;" 
       providerName="System.Data.EntityClient" />
</connectionStrings>
```

### 資料庫特色

- **EDMX 模型**: 使用 `data/HikingModel.edmx` 進行視覺化資料庫設計
- **自動生成**: 實體類別透過 T4 模板自動產生
- **圖片儲存**: 使用 `varbinary(max)` 儲存圖片二進制資料
- **關聯設計**: 支援一對多、多對多關聯

## 💻 開發指南

### 建置指令

```bash
# 建置解決方案
dotnet build

# 還原 NuGet 套件
dotnet restore

# 清理建置檔案
dotnet clean
```

### 開發環境

- **建置工具**: MSBuild / Visual Studio
- **除錯模式**: Debug Configuration
- **開發伺服器**: IIS Express
- **連接埠**: 55176

### 程式碼風格

- 遵循 Microsoft C# 編碼慣例
- 使用 PascalCase 命名類別和方法
- 使用 camelCase 命名變數和參數
- 適當的註解和文件

## 📈 專案狀態與規劃

### 目前狀況

- ✅ 基礎 MVC 架構完成
- ✅ 使用者系統實作完成
- ✅ 活動管理功能完成
- ✅ 響應式前端設計完成

### 改進建議

- 🔄 考慮升級至 .NET Core/.NET 5+
- 🔄 實作 Repository Pattern 解耦資料存取
- 🔄 加入單元測試專案
- 🔄 實作 API 層支援行動應用
- 🔄 加入快取機制提升效能
- 🔄 實作記錄系統 (Logging)

## 🤝 貢獻指南

我們歡迎任何形式的貢獻！請遵循以下步驟：

1. Fork 本專案
2. 建立 feature 分支 (`git checkout -b feature/AmazingFeature`)
3. 提交變更 (`git commit -m 'Add some AmazingFeature'`)
4. Push 到分支 (`git push origin feature/AmazingFeature`)
5. 建立 Pull Request


**感謝使用 GoHiking！讓我們一起探索美麗的山林世界 🏔️**
