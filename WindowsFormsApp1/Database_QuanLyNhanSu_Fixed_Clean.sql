-- =============================================
-- DATABASE QU?N LÝ NHÂN S? - SCRIPT T?NG H?P (FIXED)
-- T?o database, b?ng và thêm d? li?u m?u
-- Phiên b?n: 2.0 (Clean & Fixed)
-- G?p t?t c? các file SQL thành 1 ?? d? ch?y
-- =============================================

PRINT '?? B?T ??U CÀI ??T DATABASE QU?N LÝ NHÂN S?';
PRINT '==========================================';
PRINT '';

-- =============================================
-- B??C 1: T?O DATABASE
-- =============================================
PRINT '?? B??C 1: T?o Database...';

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'QuanLyNhanSu')
BEGIN
    CREATE DATABASE QuanLyNhanSu;
    PRINT '? Database QuanLyNhanSu ?ã ???c t?o!';
END
ELSE
BEGIN
    PRINT '? Database QuanLyNhanSu ?ã t?n t?i!';
END
GO

-- Chuy?n sang database QuanLyNhanSu
USE QuanLyNhanSu;
GO

-- =============================================
-- B??C 2: T?O B?NG ROLES (VAI TRÒ NG??I DÙNG)
-- =============================================
PRINT '';
PRINT '?? B??C 2: T?o b?ng Roles...';

-- Xóa b?ng n?u ?ã t?n t?i (?? reset)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Roles')
BEGIN
    DROP TABLE Roles;
    PRINT '? ?ã xóa b?ng Roles c?';
END

-- T?o b?ng Roles m?i
CREATE TABLE Roles (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) UNIQUE NOT NULL,
    Description NVARCHAR(200) NULL,
    CreatedDate DATETIME DEFAULT GETDATE()
);

PRINT '? B?ng Roles ?ã ???c t?o!';
GO

-- Thêm vai trò m?c ??nh
INSERT INTO Roles (RoleName, Description) VALUES
(N'Admin', N'Qu?n tr? viên - Toàn quy?n h? th?ng'),
(N'Manager', N'Qu?n lý - Xem và qu?n lý nhân viên, duy?t l??ng'),
(N'HR', N'Nhân s? - Qu?n lý thông tin nhân viên, ch?m công'),
(N'Employee', N'Nhân viên - Ch? xem thông tin cá nhân');

PRINT '? ?ã thêm 4 vai trò m?c ??nh!';
GO

-- =============================================
-- B??C 3: T?O B?NG USERS (NG??I DÙNG H? TH?NG)
-- =============================================
PRINT '';
PRINT '?? B??C 3: T?o b?ng Users...';

-- Xóa b?ng n?u ?ã t?n t?i (?? reset)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    DROP TABLE Users;
    PRINT '? ?ã xóa b?ng Users c?';
END

-- T?o b?ng Users m?i
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(256) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NULL,
    RoleId INT NOT NULL,
    EmployeeId INT NULL,
    IsActive BIT DEFAULT 1,
 LastLogin DATETIME NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    CreatedBy NVARCHAR(50) NULL,

CONSTRAINT FK_Users_Role FOREIGN KEY (RoleId)
  REFERENCES Roles(Id)
);

PRINT '? B?ng Users ?ã ???c t?o!';
GO

-- T?o ng??i dùng qu?n tr? m?c ??nh
-- Tài kho?n: admin, M?t kh?u: admin123
-- PasswordHash = SHA256("admin123")
INSERT INTO Users (Username, PasswordHash, FullName, Email, RoleId, IsActive, CreatedBy)
VALUES
(N'admin',
 '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9',
 N'Administrator',
 'admin@company.com',
 1,
 1,
 N'SYSTEM');

PRINT '? ?ã thêm ng??i dùng admin m?c ??nh!';
GO

-- =============================================
-- B??C 4: T?O B?NG DEPARTMENTS (PHÒNG BAN)
-- =============================================
PRINT '';
PRINT '?? B??C 4: T?o b?ng Departments...';

-- Xóa b?ng n?u ?ã t?n t?i (?? reset)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Departments')
BEGIN
    DROP TABLE Departments;
    PRINT '? ?ã xóa b?ng Departments c?';
END

-- T?o b?ng Departments m?i
CREATE TABLE Departments (
    Id INT PRIMARY KEY IDENTITY(1,1),
 DepartmentCode NVARCHAR(20) UNIQUE NOT NULL,
  DepartmentName NVARCHAR(100) NOT NULL,
    ManagerId INT NULL,
    Description NVARCHAR(500) NULL,
    EstablishedDate DATE NULL,
    PhoneNumber NVARCHAR(20) NULL,
    Email NVARCHAR(100) NULL,
    Location NVARCHAR(200) NULL,
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    CreatedBy NVARCHAR(50) NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy NVARCHAR(50) NULL
);

PRINT '? B?ng Departments ?ã ???c t?o!';
GO

-- Thêm phòng ban m?u
INSERT INTO Departments (DepartmentCode, DepartmentName, Description, EstablishedDate, IsActive, CreatedBy, Location, PhoneNumber) VALUES
(N'DEPT001', N'Công ngh? thông tin', N'Phòng phát tri?n ph?n m?m và b?o trì h? th?ng', '2020-01-01', 1, N'admin', N'T?ng 5', '0901234567'),
(N'DEPT002', N'Nhân s?', N'Phòng qu?n lý ngu?n nhân l?c và tuy?n d?ng', '2020-01-01', 1, N'admin', N'T?ng 2', '0901234568'),
(N'DEPT003', N'K? toán', N'Phòng k? toán và tài chính', '2020-01-01', 1, N'admin', N'T?ng 3', '0901234569'),
(N'DEPT004', N'Kinh doanh', N'Phòng kinh doanh và marketing', '2020-01-01', 1, N'admin', N'T?ng 4', '0901234570'),
(N'DEPT005', N'Hành chính', N'Phòng hành chính t?ng h?p', '2020-01-01', 1, N'admin', N'T?ng 1', '0901234571');

PRINT '? ?ã thêm 5 phòng ban m?u!';
GO

-- =============================================
-- B??C 5: T?O B?NG EMPLOYEES (NHÂN VIÊN)
-- =============================================
PRINT '';
PRINT '?? B??C 5: T?o b?ng Employees...';

-- Xóa b?ng n?u ?ã t?n t?i (?? reset)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')
BEGIN
    DROP TABLE Employees;
    PRINT '? ?ã xóa b?ng Employees c?';
END

-- T?o b?ng Employees m?i
CREATE TABLE Employees (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EmployeeCode NVARCHAR(20) UNIQUE NULL,
  FullName NVARCHAR(100) NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    DateOfBirth DATE NOT NULL,
 Email NVARCHAR(100) NULL,
    PhoneNumber NVARCHAR(20) NULL,
    Address NVARCHAR(200) NULL,
    Position NVARCHAR(50) NOT NULL,
    DepartmentId INT NULL,
    Department NVARCHAR(50) NULL,
  Salary DECIMAL(18,2) NOT NULL,
    HireDate DATE NULL,
    Status NVARCHAR(20) DEFAULT N'Active',
    PhotoPath NVARCHAR(500) NULL,
    Notes NVARCHAR(500) NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
 CreatedBy NVARCHAR(50) NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy NVARCHAR(50) NULL,

    CONSTRAINT FK_Employees_Department FOREIGN KEY (DepartmentId)
        REFERENCES Departments(Id)
);

PRINT '? B?ng Employees ?ã ???c t?o!';
GO

CREATE INDEX IX_Employees_Status ON Employees(Status);
CREATE INDEX IX_Employees_Department ON Employees(DepartmentId);
GO

PRINT '? ?ã t?o các ch? m?c trên b?ng Employees!';
GO

-- Thêm nhân viên m?u v?i ID chính xác
INSERT INTO Employees (EmployeeCode, FullName, Gender, DateOfBirth, Email, PhoneNumber, Address, Position, DepartmentId, Salary, HireDate, Status, CreatedBy) VALUES
-- IT Department (DepartmentId: 1)
(N'NV001', N'Nguy?n V?n An', 'Male', '1988-05-15', 'an.nguyen@company.com', '0912345671', N'123 Hoàng V?n Th?, Tân Bình, HCM', N'IT Manager', 1, 30000000, '2020-01-15', 'Active', 'admin'),
(N'NV002', N'Tr?n Th? Bình', 'Female', '1992-08-20', 'binh.tran@company.com', '0912345672', N'456 Nguy?n V?n C?, Qu?n 5, HCM', N'Senior Developer', 1, 25000000, '2020-02-01', 'Active', 'admin'),
(N'NV003', N'Lê Minh C??ng', 'Male', '1990-03-10', 'cuong.le@company.com', '0912345673', N'789 Lê L?i, Qu?n 1, HCM', N'Full-Stack Developer', 1, 22000000, '2020-03-15', 'Active', 'admin'),
(N'NV004', N'Ph?m Th? Dung', 'Female', '1995-11-25', 'dung.pham@company.com', '0912345674', N'321 Võ V?n T?n, Qu?n 3, HCM', N'Frontend Developer', 1, 20000000, '2021-01-10', 'Active', 'admin'),
(N'NV005', N'Hoàng V?n Em', 'Male', '1993-07-08', 'em.hoang@company.com', '0912345675', N'654 Cách M?ng Tháng 8, Tân Bình, HCM', N'DevOps Engineer', 1, 23000000, '2021-05-20', 'Active', 'admin'),
(N'NV006', N'Võ Th? Ph??ng', 'Female', '1994-12-12', 'phuong.vo@company.com', '0912345676', N'987 Nguy?n Thái H?c, Qu?n 1, HCM', N'System Analyst', 1, 21000000, '2021-08-15', 'Active', 'admin'),

-- HR Department (DepartmentId: 2)
(N'NV007', N'?? V?n Giang', 'Male', '1985-06-18', 'giang.do@company.com', '0912345677', N'147 Pasteur, Qu?n 1, HCM', N'HR Manager', 2, 28000000, '2020-01-20', 'Active', 'admin'),
(N'NV008', N'Bùi Th? H?nh', 'Female', '1991-09-05', 'hanh.bui@company.com', '0912345678', N'258 ?i?n Biên Ph?, Qu?n 3, HCM', N'HR Specialist', 2, 18000000, '2020-04-10', 'Active', 'admin'),
(N'NV009', N'Ngô V?n Inh', 'Male', '1989-01-30', 'inh.ngo@company.com', '0912345679', N'369 Hai Bà Tr?ng, Qu?n 3, HCM', N'Recruitment Specialist', 2, 17000000, '2020-07-01', 'Active', 'admin'),
(N'NV010', N'Lý Th? Kim', 'Female', '1996-04-22', 'kim.ly@company.com', '0912345680', N'741 Tr?n H?ng ??o, Qu?n 5, HCM', N'Training Coordinator', 2, 16000000, '2021-03-01', 'Active', 'admin'),

-- Accounting Department (DepartmentId: 3)
(N'NV011', N'Phan V?n Long', 'Male', '1987-02-14', 'long.phan@company.com', '0912345681', N'852 Lý T? Tr?ng, Qu?n 1, HCM', N'Accounting Manager', 3, 26000000, '2020-02-01', 'Active', 'admin'),
(N'NV012', N'T? Th? Mai', 'Female', '1992-10-08', 'mai.ta@company.com', '0912345682', N'963 Nguy?n ?ình Chi?u, Qu?n 3, HCM', N'Senior Accountant', 3, 19000000, '2020-05-15', 'Active', 'admin'),
(N'NV013', N'V? V?n Nam', 'Male', '1994-08-17', 'nam.vu@company.com', '0912345683', N'159 Bùi Vi?n, Qu?n 1, HCM', N'Financial Analyst', 3, 18000000, '2020-08-01', 'Active', 'admin'),
(N'NV014', N'?inh Th? Oanh', 'Female', '1993-12-03', 'oanh.dinh@company.com', '0912345684', N'357 Nguy?n Trãi, Qu?n 5, HCM', N'Tax Specialist', 3, 17000000, '2021-02-15', 'Active', 'admin'),

-- Sales Department (DepartmentId: 4)
(N'NV015', N'Tr??ng V?n Phúc', 'Male', '1986-11-11', 'phuc.truong@company.com', '0912345685', N'246 Tôn ??c Th?ng, Qu?n 1, HCM', N'Sales Manager', 4, 29000000, '2020-02-15', 'Active', 'admin'),
(N'NV016', N'Lâm Th? Qu?nh', 'Female', '1990-07-25', 'quynh.lam@company.com', '0912345686', N'468 Lê Du?n, Qu?n 3, HCM', N'Senior Sales Executive', 4, 22000000, '2020-06-01', 'Active', 'admin'),
(N'NV017', N'Cao V?n R?ng', 'Male', '1995-03-07', 'rong.cao@company.com', '0912345687', N'579 C?ng Hòa, Tân Bình, HCM', N'Sales Executive', 4, 18000000, '2020-09-01', 'Active', 'admin'),
(N'NV018', N'??ng Th? S??ng', 'Female', '1997-05-19', 'suong.dang@company.com', '0912345688', N'680 Nam K? Kh?i Ngh?a, Qu?n 3, HCM', N'Marketing Specialist', 4, 17000000, '2021-01-15', 'Active', 'admin'),
(N'NV019', N'H? V?n Tùng', 'Male', '1991-09-28', 'tung.ho@company.com', '0912345689', N'791 Ph?m Ng? Lão, Qu?n 1, HCM', N'Business Development', 4, 20000000, '2021-04-01', 'Active', 'admin'),

-- Admin Department (DepartmentId: 5)  
(N'NV020', N'Châu Th? Uyên', 'Female', '1989-06-13', 'uyen.chau@company.com', '0912345690', N'802 Hoàng Sa, Qu?n 3, HCM', N'Admin Manager', 5, 24000000, '2020-01-10', 'Active', 'admin'),
(N'NV021', N'L?u V?n Vi?t', 'Male', '1993-02-26', 'viet.luu@company.com', '0912345691', N'913 Tr??ng Sa, Phú Nhu?n, HCM', N'Office Coordinator', 5, 15000000, '2020-08-15', 'Active', 'admin'),
(N'NV022', N'Kh??ng Th? Xuân', 'Female', '1996-10-04', 'xuan.khuong@company.com', '0912345692', N'124 Xô Vi?t Ngh? T?nh, Bình Th?nh, HCM', N'Administrative Assistant', 5, 14000000, '2021-06-01', 'Active', 'admin');

PRINT '? ?ã thêm 22 nhân viên cho 5 phòng ban!';
GO

-- Add foreign key constraint after Employees table is created
ALTER TABLE Departments
ADD CONSTRAINT FK_Departments_Manager FOREIGN KEY (ManagerId)
    REFERENCES Employees(Id);
GO

-- C?p nh?t manager cho các phòng ban v?i ID chính xác
UPDATE Departments SET ManagerId = 1 WHERE DepartmentCode = 'DEPT001';  -- IT: Nguy?n V?n An
UPDATE Departments SET ManagerId = 7 WHERE DepartmentCode = 'DEPT002';  -- HR: ?? V?n Giang  
UPDATE Departments SET ManagerId = 11 WHERE DepartmentCode = 'DEPT003'; -- Accounting: Phan V?n Long
UPDATE Departments SET ManagerId = 15 WHERE DepartmentCode = 'DEPT004'; -- Sales: Tr??ng V?n Phúc
UPDATE Departments SET ManagerId = 20 WHERE DepartmentCode = 'DEPT005'; -- Admin: Châu Th? Uyên
GO

PRINT '? ?ã c?p nh?t tr??ng phòng cho t?t c? phòng ban!';
GO

-- =============================================
-- B??C 6: T?O B?NG ATTENDANCE (CH?M CÔNG)
-- =============================================
PRINT '';
PRINT '?? B??C 6: T?o b?ng Attendance...';

-- Xóa b?ng n?u ?ã t?n t?i (?? reset)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Attendance')
BEGIN
    DROP TABLE Attendance;
    PRINT '? ?ã xóa b?ng Attendance c?';
END

-- T?o b?ng Attendance m?i
CREATE TABLE Attendance (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EmployeeId INT NOT NULL,
    AttendanceDate DATE NOT NULL,
    CheckInTime DATETIME NULL,
    CheckOutTime DATETIME NULL,
    WorkingHours DECIMAL(5,2) NULL,
    Status NVARCHAR(20) DEFAULT N'Present',
    IsLate BIT DEFAULT 0,
    LateMinutes INT DEFAULT 0,
    IsEarlyLeave BIT DEFAULT 0,
    EarlyLeaveMinutes INT DEFAULT 0,
    OvertimeHours DECIMAL(5,2) DEFAULT 0,
    Notes NVARCHAR(500) NULL,
 CreatedDate DATETIME DEFAULT GETDATE(),
    CreatedBy NVARCHAR(50) NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy NVARCHAR(50) NULL,

    CONSTRAINT FK_Attendance_Employee FOREIGN KEY (EmployeeId)
  REFERENCES Employees(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Attendance_EmployeeDate UNIQUE(EmployeeId, AttendanceDate)
);

PRINT '? B?ng Attendance ?ã ???c t?o!';
GO

CREATE INDEX IX_Attendance_Date ON Attendance(AttendanceDate DESC);
CREATE INDEX IX_Attendance_Employee ON Attendance(EmployeeId);
CREATE INDEX IX_Attendance_Status ON Attendance(Status);
GO

PRINT '? ?ã t?o các ch? m?c trên b?ng Attendance!';
GO

-- =============================================
-- B??C 6B: THÊM D? LI?U CH?M CÔNG M?U
-- =============================================
PRINT '';
PRINT '?? B??C 6B: Thêm d? li?u ch?m công cho 15 ngày g?n ?ây...';

-- T?o d? li?u ch?m công cho 15 ngày g?n ?ây
DECLARE @StartDate DATE = DATEADD(DAY, -15, GETDATE());
DECLARE @CurrentDate DATE = @StartDate;
DECLARE @EmployeeId INT = 1;
DECLARE @MaxEmployeeId INT = 22;

WHILE @CurrentDate <= CAST(GETDATE() AS DATE)
BEGIN
    -- Ch? t?o ch?m công cho ngày làm vi?c (th? 2-6)
    IF DATEPART(WEEKDAY, @CurrentDate) BETWEEN 2 AND 6
    BEGIN
     SET @EmployeeId = 1;
    
        WHILE @EmployeeId <= @MaxEmployeeId
        BEGIN
   DECLARE @CheckInTime DATETIME;
      DECLARE @CheckOutTime DATETIME;
 DECLARE @IsLate BIT = 0;
       DECLARE @LateMinutes INT = 0;
 DECLARE @WorkingHours DECIMAL(5,2);
            DECLARE @Status NVARCHAR(20) = 'Present';
   
            -- Random attendance pattern (90% present, 5% late, 5% absent)
   DECLARE @AttendanceType INT = CAST(RAND() * 100 AS INT);
    
   IF @AttendanceType < 5 -- 5% absent
     BEGIN
    SET @Status = 'Absent';
         SET @CheckInTime = NULL;
           SET @CheckOutTime = NULL;
       SET @WorkingHours = 0;
    END
            ELSE IF @AttendanceType < 10 -- 5% late  
        BEGIN
      SET @LateMinutes = CAST(RAND() * 60 + 10 AS INT); -- 10-70 minutes late
 -- Convert DATE to DATETIME and add minutes
       SET @CheckInTime = DATEADD(MINUTE, 480 + @LateMinutes, CAST(@CurrentDate AS DATETIME)); -- 8:00 AM + late minutes
         SET @CheckOutTime = DATEADD(HOUR, 8, @CheckInTime); -- 8 hours later
     SET @IsLate = 1;
                SET @WorkingHours = 8.0;
       SET @Status = 'Late';
  END
        ELSE -- 90% on time
         BEGIN
                -- Random check-in between 7:45-8:15
  DECLARE @CheckInVariation INT = CAST(RAND() * 30 - 15 AS INT); -- -15 to +15 minutes
  -- Convert DATE to DATETIME and add minutes
       SET @CheckInTime = DATEADD(MINUTE, 480 + @CheckInVariation, CAST(@CurrentDate AS DATETIME)); -- 8:00 AM +/- variation
     SET @CheckOutTime = DATEADD(HOUR, 8, @CheckInTime); -- 8 hours later
         SET @WorkingHours = 8.0;
    
        IF @CheckInVariation > 15
          BEGIN
       SET @IsLate = 1;
         SET @LateMinutes = @CheckInVariation - 15;
        SET @Status = 'Late';
                END
            END
    
    -- Insert attendance record
   IF NOT EXISTS (SELECT 1 FROM Attendance WHERE EmployeeId = @EmployeeId AND AttendanceDate = @CurrentDate)
            BEGIN
    INSERT INTO Attendance (
      EmployeeId, AttendanceDate, CheckInTime, CheckOutTime, 
           WorkingHours, Status, IsLate, LateMinutes, CreatedBy
  ) VALUES (
          @EmployeeId, @CurrentDate, @CheckInTime, @CheckOutTime,
  @WorkingHours, @Status, @IsLate, @LateMinutes, 'admin'
    );
   END
      
            SET @EmployeeId = @EmployeeId + 1;
        END
    END
    
    SET @CurrentDate = DATEADD(DAY, 1, @CurrentDate);
END

PRINT '? ?ã thêm d? li?u ch?m công cho 15 ngày g?n ?ây!';
GO

-- =============================================
-- B??C 7: THÊM USER ACCOUNTS M? R?NG
-- =============================================
PRINT '';
PRINT '?? B??C 7: Thêm tài kho?n user cho nhân viên...';

-- Thêm các tài kho?n user cho nhân viên (Password: admin123 - SHA256)
INSERT INTO Users (Username, PasswordHash, FullName, Email, RoleId, EmployeeId, IsActive, CreatedBy) VALUES
-- Managers (Role 2 = Manager)
('an.nguyen', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Nguy?n V?n An', 'an.nguyen@company.com', 2, 1, 1, 'admin'),
('giang.do', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'?? V?n Giang', 'giang.do@company.com', 2, 7, 1, 'admin'),
('long.phan', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Phan V?n Long', 'long.phan@company.com', 2, 11, 1, 'admin'),
('phuc.truong', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Tr??ng V?n Phúc', 'phuc.truong@company.com', 2, 15, 1, 'admin'),
('uyen.chau', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Châu Th? Uyên', 'uyen.chau@company.com', 2, 20, 1, 'admin'),

-- HR Staff (Role 3 = HR)
('hanh.bui', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Bùi Th? H?nh', 'hanh.bui@company.com', 3, 8, 1, 'admin'),
('inh.ngo', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Ngô V?n Inh', 'inh.ngo@company.com', 3, 9, 1, 'admin'),
('kim.ly', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Lý Th? Kim', 'kim.ly@company.com', 3, 10, 1, 'admin'),

-- Regular Employees (Role 4 = Employee)
('binh.tran', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Tr?n Th? Bình', 'binh.tran@company.com', 4, 2, 1, 'admin'),
('cuong.le', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Lê Minh C??ng', 'cuong.le@company.com', 4, 3, 1, 'admin'),
('dung.pham', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Ph?m Th? Dung', 'dung.pham@company.com', 4, 4, 1, 'admin'),
('mai.ta', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'T? Thì Mai', 'mai.ta@company.com', 4, 12, 1, 'admin'),
('quynh.lam', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Lâm Th? Qu?nh', 'quynh.lam@company.com', 4, 16, 1, 'admin'),
('viet.luu', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'L?u V?n Vi?t', 'viet.luu@company.com', 4, 21, 1, 'admin');

PRINT '? ?ã thêm 14 tài kho?n user! (Password: admin123 cho t?t c?)';
GO

-- =============================================
-- B??C 8: T?O B?NG PAYROLL (B?NG L??NG)
-- =============================================
PRINT '';
PRINT '?? B??C 8: T?o b?ng Payroll...';

-- Xóa b?ng n?u ?ã t?n t?i (?? reset)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Payroll')
BEGIN
    DROP TABLE Payroll;
    PRINT '? ?ã xóa b?ng Payroll c?';
END

-- T?o b?ng Payroll m?i
CREATE TABLE Payroll (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EmployeeId INT NOT NULL,
PayrollMonth INT NOT NULL,
    PayrollYear INT NOT NULL,
    BaseSalary DECIMAL(18,2) NOT NULL,
    WorkingDays INT DEFAULT 0,
    StandardDays INT DEFAULT 26,

    -- Thu nh?p
 Allowance DECIMAL(18,2) DEFAULT 0,
    Bonus DECIMAL(18,2) DEFAULT 0,
    OvertimeHours DECIMAL(5,2) DEFAULT 0,
    OvertimePay DECIMAL(18,2) DEFAULT 0,

    -- Kh?u tr?
    Penalty DECIMAL(18,2) DEFAULT 0,
    SocialInsurance DECIMAL(18,2) DEFAULT 0,
    HealthInsurance DECIMAL(18,2) DEFAULT 0,
    UnemploymentIns DECIMAL(18,2) DEFAULT 0,
    TaxableIncome DECIMAL(18,2) DEFAULT 0,
    PersonalDeduction DECIMAL(18,2) DEFAULT 11000000,
    DependentDeduction DECIMAL(18,2) DEFAULT 0,
    IncomeTax DECIMAL(18,2) DEFAULT 0,

    -- T?ng k?t
    TotalIncome DECIMAL(18,2) NOT NULL,
    TotalDeduction DECIMAL(18,2) NOT NULL,
    NetSalary DECIMAL(18,2) NOT NULL,

    Notes NVARCHAR(500) NULL,
    IsPaid BIT DEFAULT 0,
    PaidDate DATETIME NULL,
    PaymentMethod NVARCHAR(50) NULL,

    CreatedDate DATETIME DEFAULT GETDATE(),
    CreatedBy NVARCHAR(50) NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy NVARCHAR(50) NULL,

    CONSTRAINT FK_Payroll_Employee FOREIGN KEY (EmployeeId)
        REFERENCES Employees(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Payroll_EmployeeMonthYear
   UNIQUE(EmployeeId, PayrollMonth, PayrollYear)
);

PRINT '? B?ng Payroll ?ã ???c t?o!';
GO

CREATE INDEX IX_Payroll_MonthYear ON Payroll(PayrollYear DESC, PayrollMonth DESC);
CREATE INDEX IX_Payroll_IsPaid ON Payroll(IsPaid);
GO

PRINT '? ?ã t?o các ch? m?c trên b?ng Payroll!';
GO

-- =============================================
-- B??C 9: T?O B?NG AUDIT LOGS (L?CH S? HO?T ??NG)
-- =============================================
PRINT '';
PRINT '?? B??C 9: T?o b?ng AuditLogs...';

-- Xóa b?ng n?u ?ã t?n t?i (?? reset)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'AuditLogs')
BEGIN
    DROP TABLE AuditLogs;
    PRINT '? ?ã xóa b?ng AuditLogs c?';
END

-- T?o b?ng AuditLogs m?i
CREATE TABLE AuditLogs (
  Id INT PRIMARY KEY IDENTITY(1,1),
    TableName NVARCHAR(50) NOT NULL,
    RecordId INT NOT NULL,
    Action NVARCHAR(20) NOT NULL,
    FieldName NVARCHAR(50) NULL,
 OldValue NVARCHAR(MAX) NULL,
    NewValue NVARCHAR(MAX) NULL,
    Description NVARCHAR(500) NULL,
    PerformedBy NVARCHAR(100) NOT NULL,
    PerformedDate DATETIME DEFAULT GETDATE(),
    IPAddress NVARCHAR(50) NULL,
    MachineName NVARCHAR(100) NULL
);

PRINT '? B?ng AuditLogs ?ã ???c t?o!';
GO

CREATE INDEX IX_AuditLogs_TableRecord ON AuditLogs(TableName, RecordId);
CREATE INDEX IX_AuditLogs_Date ON AuditLogs(PerformedDate DESC);
CREATE INDEX IX_AuditLogs_User ON AuditLogs(PerformedBy);
CREATE INDEX IX_AuditLogs_Action ON AuditLogs(Action);
GO

PRINT '? ?ã t?o các ch? m?c trên b?ng AuditLogs!';
GO

-- =============================================
-- B??C 10: T?O B?NG SYSTEM SETTINGS (CÀI ??T H? TH?NG)
-- =============================================
PRINT '';
PRINT '?? B??C 10: T?o b?ng SystemSettings...';

-- Xóa b?ng n?u ?ã t?n t?i (?? reset)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'SystemSettings')
BEGIN
    DROP TABLE SystemSettings;
    PRINT '? ?ã xóa b?ng SystemSettings c?';
END

-- T?o b?ng SystemSettings m?i
CREATE TABLE SystemSettings (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SettingKey NVARCHAR(50) UNIQUE NOT NULL,
    SettingValue NVARCHAR(200) NOT NULL,
    DataType NVARCHAR(20) DEFAULT 'String',
    Description NVARCHAR(200) NULL,
    Category NVARCHAR(50) NULL,
    UpdatedDate DATETIME DEFAULT GETDATE(),
    UpdatedBy NVARCHAR(50) NULL
);

PRINT '? B?ng SystemSettings ?ã ???c t?o!';
GO

-- Thêm c?u hình h? th?ng m?c ??nh
INSERT INTO SystemSettings (SettingKey, SettingValue, DataType, Description, Category) VALUES
-- Attendance settings
('CheckInTime', '08:00', 'Time', N'Gi? vào làm chu?n', 'Attendance'),
('CheckOutTime', '17:00', 'Time', N'Gi? tan làm chu?n', 'Attendance'),
('LateThreshold', '15', 'Int', N'S? phút ???c phép mu?n', 'Attendance'),
('LunchBreakMinutes', '60', 'Int', N'S? phút ngh? tr?a', 'Attendance'),
('LatePenaltyPerMinute', '5000', 'Decimal', N'Ph?t m?i phút mu?n (VND)', 'Attendance'),

-- Payroll settings
('StandardWorkingDays', '26', 'Int', N'S? ngày công chu?n/tháng', 'Payroll'),
('OvertimeRate', '1.5', 'Decimal', N'H? s? l??ng t?ng ca', 'Payroll'),
('SocialInsuranceRate', '8', 'Decimal', N'% BHXH', 'Payroll'),
('HealthInsuranceRate', '1.5', 'Decimal', N'% BHYT', 'Payroll'),
('UnemploymentInsRate', '1', 'Decimal', N'% BHTN', 'Payroll'),
('PersonalDeduction', '11000000', 'Decimal', N'Gi?m tr? b?n thân', 'Payroll'),
('DependentDeduction', '4400000', 'Decimal', N'Gi?m tr?/ng??i ph? thu?c', 'Payroll'),

-- General settings
('CompanyName', N'Công ty TNHH ABC', 'String', N'Tên công ty', 'General'),
('CompanyAddress', N'123 ???ng ABC, Qu?n 1, TP.HCM', 'String', N'??a ch? công ty', 'General'),
('CompanyPhone', '0901234567', 'String', N'S? ?i?n tho?i', 'General'),
('CompanyEmail', 'info@company.com', 'String', N'Email công ty', 'General');

PRINT '? ?ã thêm c?u hình h? th?ng m?c ??nh!';
GO

-- =============================================
-- B??C 11: T?O STORED PROCEDURES
-- =============================================
PRINT '';
PRINT '?? B??C 11: T?o stored procedures...';
GO

-- SP: Get employee attendance summary by month
CREATE PROCEDURE sp_GetAttendanceSummary
    @EmployeeId INT,
    @Month INT,
    @Year INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
      COUNT(*) as TotalDays,
        SUM(CASE WHEN Status = 'Present' OR Status = 'Late' THEN 1 ELSE 0 END) as PresentDays,
        SUM(CASE WHEN Status = 'Absent' THEN 1 ELSE 0 END) as AbsentDays,
        SUM(CASE WHEN Status = 'Leave' THEN 1 ELSE 0 END) as LeaveDays,
   SUM(CASE WHEN IsLate = 1 THEN 1 ELSE 0 END) as LateDays,
   SUM(LateMinutes) as TotalLateMinutes,
        SUM(OvertimeHours) as TotalOvertimeHours,
        SUM(WorkingHours) as TotalWorkingHours
    FROM Attendance
    WHERE EmployeeId = @EmployeeId
      AND MONTH(AttendanceDate) = @Month
      AND YEAR(AttendanceDate) = @Year;
END
GO

PRINT '? Stored procedure sp_GetAttendanceSummary ?ã ???c t?o!';
GO

-- SP: Calculate payroll for an employee
CREATE PROCEDURE sp_CalculatePayroll
    @EmployeeId INT,
@Month INT,
    @Year INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @BaseSalary DECIMAL(18,2);
    DECLARE @WorkingDays INT;
    DECLARE @StandardDays INT = 26;
    DECLARE @OvertimeHours DECIMAL(5,2);
    DECLARE @LateMinutes INT;
    DECLARE @LatePenaltyRate DECIMAL(18,2) = 5000;

    -- Get employee salary
    SELECT @BaseSalary = Salary FROM Employees WHERE Id = @EmployeeId;

    -- Get attendance summary
    SELECT
      @WorkingDays = SUM(CASE WHEN Status IN ('Present', 'Late') THEN 1 ELSE 0 END),
        @OvertimeHours = ISNULL(SUM(OvertimeHours), 0),
   @LateMinutes = ISNULL(SUM(LateMinutes), 0)
    FROM Attendance
    WHERE EmployeeId = @EmployeeId
      AND MONTH(AttendanceDate) = @Month
      AND YEAR(AttendanceDate) = @Year;

    -- Calculate components
 DECLARE @ActualSalary DECIMAL(18,2) = (@BaseSalary / @StandardDays) * ISNULL(@WorkingDays, 0);
    DECLARE @OvertimePay DECIMAL(18,2) = @OvertimeHours * (@BaseSalary / @StandardDays / 8) * 1.5;
    DECLARE @LatePenalty DECIMAL(18,2) = @LateMinutes * @LatePenaltyRate;
    DECLARE @SocialIns DECIMAL(18,2) = @BaseSalary * 0.08;
    DECLARE @HealthIns DECIMAL(18,2) = @BaseSalary * 0.015;
    DECLARE @UnemplIns DECIMAL(18,2) = @BaseSalary * 0.01;

    SELECT
        @BaseSalary as BaseSalary,
    ISNULL(@WorkingDays, 0) as WorkingDays,
  @StandardDays as StandardDays,
        @ActualSalary as ActualSalary,
        @OvertimePay as OvertimePay,
        @LatePenalty as LatePenalty,
        @SocialIns as SocialInsurance,
   @HealthIns as HealthInsurance,
  @UnemplIns as UnemploymentInsurance;
END
GO

PRINT '? Stored procedure sp_CalculatePayroll ?ã ???c t?o!';
GO

-- SP: Get dashboard statistics
CREATE PROCEDURE sp_GetDashboardStats
AS
BEGIN
    SET NOCOUNT ON;

    -- Basic employee stats
    SELECT
        COUNT(*) as TotalEmployees,
        SUM(CASE WHEN Status = 'Active' THEN 1 ELSE 0 END) as ActiveEmployees,
SUM(CASE WHEN Status = 'OnLeave' THEN 1 ELSE 0 END) as OnLeaveEmployees,
        SUM(CASE WHEN Status = 'Resigned' THEN 1 ELSE 0 END) as ResignedEmployees,
        AVG(Salary) as AverageSalary
    FROM Employees;

    -- Employees by department
    SELECT
  ISNULL(d.DepartmentName, N'Ch?a phân b?') as Department,
        COUNT(e.Id) as EmployeeCount
    FROM Employees e
    LEFT JOIN Departments d ON e.DepartmentId = d.Id
    WHERE e.Status = 'Active'
    GROUP BY d.DepartmentName;

    -- Employees by gender
    SELECT
 Gender,
        COUNT(*) as Count
    FROM Employees
    WHERE Status = 'Active'
    GROUP BY Gender;
END
GO

PRINT '? Stored procedure sp_GetDashboardStats ?ã ???c t?o!';
GO

-- =============================================
-- B??C 12: T?O VIEWS (Tùy ch?n)
-- =============================================
PRINT '';
PRINT '?? B??C 12: T?o views...';
GO

-- View: Employee with Department info
CREATE VIEW vw_EmployeeDetails AS
SELECT
    e.Id,
    e.EmployeeCode,
    e.FullName,
    e.Gender,
    e.DateOfBirth,
    DATEDIFF(YEAR, e.DateOfBirth, GETDATE()) as Age,
    e.Email,
    e.PhoneNumber,
    e.Address,
    e.Position,
    e.DepartmentId,
    d.DepartmentName,
    d.DepartmentCode,
    e.Salary,
    e.HireDate,
    e.Status,
    e.PhotoPath,
    u.Username as UserAccount
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentId = d.Id
LEFT JOIN Users u ON u.EmployeeId = e.Id;
GO

PRINT '? View vw_EmployeeDetails ?ã ???c t?o!';
GO

-- View: Attendance with Employee info
CREATE VIEW vw_AttendanceDetails AS
SELECT
    a.Id,
    a.EmployeeId,
    e.EmployeeCode,
    e.FullName as EmployeeName,
    d.DepartmentName,
    a.AttendanceDate,
    a.CheckInTime,
    a.CheckOutTime,
 a.WorkingHours,
    a.Status,
    a.IsLate,
    a.LateMinutes,
    a.OvertimeHours,
    a.Notes
FROM Attendance a
INNER JOIN Employees e ON a.EmployeeId = e.Id
LEFT JOIN Departments d ON e.DepartmentId = d.Id;
GO

PRINT '? View vw_AttendanceDetails ?ã ???c t?o!';
GO

-- =============================================
-- TÓM T?T CÀI ??T
-- =============================================
PRINT '';
PRINT '?? HOÀN T?T CÀI ??T!';
PRINT '==========================================';
PRINT '';

-- Hi?n th? mapping ID chính xác
PRINT '?? MAPPING ID CHÍNH XÁC:';
PRINT '------------------------';
SELECT 
    d.Id as [Dept ID],
 d.DepartmentCode as [Mã PB],
    d.DepartmentName as [Tên Phòng Ban],
    d.ManagerId as [Manager ID],
    mgr.FullName as [Tên Tr??ng Phòng],
 COUNT(e.Id) as [S? NV]
FROM Departments d
LEFT JOIN Employees mgr ON d.ManagerId = mgr.Id  
LEFT JOIN Employees e ON d.Id = e.DepartmentId AND e.Status = 'Active'
WHERE d.IsActive = 1
GROUP BY d.Id, d.DepartmentCode, d.DepartmentName, d.ManagerId, mgr.FullName
ORDER BY d.Id;

PRINT '';
PRINT '?? TH?NG KÊ THEO PHÒNG BAN:';
PRINT '----------------------------';
SELECT
    d.DepartmentName AS [Phòng Ban],
    COUNT(e.Id) AS [S? NV],
    d.DepartmentCode AS [Mã PB],
    mgr.FullName AS [Tr??ng Phòng],
    FORMAT(AVG(e.Salary), 'N0') + ' VN?' AS [L??ng TB],
  FORMAT(MIN(e.Salary), 'N0') + ' VN?' AS [L??ng Min],
    FORMAT(MAX(e.Salary), 'N0') + ' VN?' AS [L??ng Max]
FROM Departments d
LEFT JOIN Employees e ON d.Id = e.DepartmentId AND e.Status = 'Active'
LEFT JOIN Employees mgr ON d.ManagerId = mgr.Id
WHERE d.IsActive = 1
GROUP BY d.DepartmentName, d.DepartmentCode, mgr.FullName
ORDER BY COUNT(e.Id) DESC;

PRINT '';
PRINT '?? TH?NG KÊ CH?M CÔNG HÔM NAY:';
PRINT '-----------------------------';
IF EXISTS (SELECT 1 FROM Attendance WHERE AttendanceDate = CAST(GETDATE() AS DATE))
BEGIN
    SELECT 
        Status as [Tr?ng Thái],
      COUNT(*) as [S? L??ng],
   CAST(COUNT(*) * 100.0 / (SELECT COUNT(*) FROM Attendance WHERE AttendanceDate = CAST(GETDATE() AS DATE)) AS DECIMAL(5,1)) as [T? L? %]
    FROM Attendance 
    WHERE AttendanceDate = CAST(GETDATE() AS DATE)
    GROUP BY Status
    ORDER BY COUNT(*) DESC;
END
ELSE
BEGIN
    PRINT N'(Ch?a có d? li?u ch?m công hôm nay)';
END

PRINT '';
PRINT '?? DANH SÁCH TÀI KHO?N TEST:';
PRINT '----------------------------';
PRINT 'Username: admin, Password: admin123 (Admin)';
PRINT 'Username: an.nguyen, Password: admin123 (IT Manager)';
PRINT 'Username: giang.do, Password: admin123 (HR Manager)';
PRINT 'Username: long.phan, Password: admin123 (Accounting Manager)';
PRINT 'Username: phuc.truong, Password: admin123 (Sales Manager)';
PRINT 'Username: uyen.chau, Password: admin123 (Admin Manager)';
PRINT 'Username: hanh.bui, Password: admin123 (HR Staff)';
PRINT 'Username: binh.tran, Password: admin123 (Employee)';
PRINT 'Username: cuong.le, Password: admin123 (Employee)';

PRINT '';
PRINT '?? S? LI?U T?NG QUAN:';
PRINT '--------------------';
SELECT 
    'Phòng ban' as [Lo?i], 
    COUNT(*) as [S? l??ng] 
FROM Departments WHERE IsActive = 1
UNION ALL
SELECT 
  'Nhân viên' as [Lo?i], 
    COUNT(*) as [S? l??ng] 
FROM Employees WHERE Status = 'Active'
UNION ALL
SELECT 
    'Ch?m công (15 ngày)' as [Lo?i], 
    COUNT(*) as [S? l??ng] 
FROM Attendance
UNION ALL
SELECT 
    'Tài kho?n user' as [Lo?i], 
    COUNT(*) as [S? l??ng] 
FROM Users WHERE IsActive = 1;

PRINT '';
PRINT '? Database ?ã s?n sàng s? d?ng!';
PRINT '? T?t c? ID ?ã ???c match chính xác!';
PRINT '? B?n có th? ch?y ?ng d?ng Windows Forms ngay bây gi?.';
PRINT '';
PRINT '==========================================';
GO

-- H?T SCRIPT