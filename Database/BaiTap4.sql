IF DB_ID('BaiTap4') IS NULL CREATE DATABASE BaiTap4;
GO
USE BaiTap4;
GO
IF OBJECT_ID('dbo.Users') IS NULL
CREATE TABLE dbo.Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(200) NOT NULL   -- dạng "salt:hash" (PBKDF2)
);
IF OBJECT_ID('dbo.Courses') IS NULL
CREATE TABLE dbo.Courses (
    CourseId INT IDENTITY(1,1) PRIMARY KEY,
    CourseName NVARCHAR(100) NOT NULL
);
IF OBJECT_ID('dbo.Registrations') IS NULL
CREATE TABLE dbo.Registrations (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    BirthDate DATE NOT NULL,
    CourseId INT NOT NULL REFERENCES dbo.Courses(CourseId),
    Interests NVARCHAR(200) NULL,
    RegisteredAt DATETIME NOT NULL DEFAULT GETDATE()
);
-- Tài khoản mẫu admin/123456 và danh sách khóa học sẽ được ứng dụng tự seed khi chạy lần đầu.
