CREATE TABLE UserProfile (
    ProfileId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,  -- 對應 Users 表 (FK)

    RealName NVARCHAR(100) NOT NULL,         -- 真實姓名
    Gender BIT NOT NULL,                     -- 性別 (男/女)   0 = 女, 1 = 男
    BirthDate DATE NOT NULL,                 -- 生日
    Nationality NVARCHAR(50) NOT NULL,       -- 國籍
    IdNumber NVARCHAR(50) NOT NULL,          -- 身分證字號 / 居留證 / 護照
    LineId NVARCHAR(50) NOT NULL,            -- LINE ID
    Phone NVARCHAR(20) NOT NULL,             -- 聯絡電話
    Address NVARCHAR(200) NOT NULL,          -- 地址（完整縣市/鄉鎮市區）

    EmergencyContactName NVARCHAR(100) NOT NULL,  -- 緊急聯絡人姓名
    EmergencyContactPhone NVARCHAR(20) NOT NULL,  -- 緊急聯絡人電話

    HasCompanion BIT NOT NULL,               -- 是否有同行夥伴
    CompanionName NVARCHAR(200) NULL,        -- 夥伴姓名 (可多人以逗號分隔，或另外做 table)

    HikingExperience NVARCHAR(MAX) NOT NULL, -- 近三年主要登山經歷
    ExerciseHabit NVARCHAR(MAX) NOT NULL,    -- 平時運動習慣
    CreatedAt DATETIME DEFAULT GETDATE(),    -- 建立時間
    UpdatedAt DATETIME DEFAULT GETDATE()     -- 最後更新時間
);
