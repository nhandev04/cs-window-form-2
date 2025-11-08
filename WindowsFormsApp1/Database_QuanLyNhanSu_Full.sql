-- =============================================
-- DATABASE QUẢN LÝ NHÂN SỰ - SCRIPT TỔNG HỢP
-- Tạo database, bảng và thêm dữ liệu mẫu
-- Phiên bản: 1.1 (Fixed Attendance Structure)
-- Gộp tất cả các file SQL thành 1 để dễ chạy
-- =============================================

PRINT '🚀 BẮT ĐẦU CÀI ĐẶT DATABASE QUẢN LÝ NHÂN SỰ';
PRINT '==========================================';
PRINT '';

-- =============================================
-- BƯỚC 1: TẠO DATABASE
-- =============================================
PRINT '📋 BƯỚC 1: Tạo Database...';

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'QuanLyNhanSu')
BEGIN
    CREATE DATABASE QuanLyNhanSu;
    PRINT '✓ Database QuanLyNhanSu đã được tạo!';
END
ELSE
BEGIN
    PRINT '✓ Database QuanLyNhanSu đã tồn tại!';
END
GO

-- Chuyển sang database QuanLyNhanSu
USE QuanLyNhanSu;
GO

-- =============================================
-- BƯỚC 2: TẠO BẢNG ROLES (VAI TRÒ NGƯỜI DÙNG)
-- =============================================
PRINT '';
PRINT '📋 BƯỚC 2: Tạo bảng Roles...';

-- Xóa bảng nếu đã tồn tại (để reset)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Roles')
BEGIN
    DROP TABLE Roles;
    PRINT '⚠ Đã xóa bảng Roles cũ';
END

-- Tạo bảng Roles mới
CREATE TABLE Roles (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) UNIQUE NOT NULL,
    Description NVARCHAR(200) NULL,
    CreatedDate DATETIME DEFAULT GETDATE()
);

PRINT '✓ Bảng Roles đã được tạo!';
GO

-- Thêm vai trò mặc định
INSERT INTO Roles (RoleName, Description) VALUES
(N'Admin', N'Quản trị viên - Toàn quyền hệ thống'),
(N'Manager', N'Quản lý - Xem và quản lý nhân viên, duyệt lương'),
(N'HR', N'Nhân sự - Quản lý thông tin nhân viên, chấm công'),
(N'Employee', N'Nhân viên - Chỉ xem thông tin cá nhân');

PRINT '✓ Đã thêm 4 vai trò mặc định!';
GO

-- =============================================
-- BƯỚC 3: TẠO BẢNG USERS (NGƯỜI DÙNG HỆ THỐNG)
-- =============================================
PRINT '';
PRINT '📋 BƯỚC 3: Tạo bảng Users...';

-- Xóa bảng nếu đã tồn tại (để reset)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    DROP TABLE Users;
    PRINT '⚠ Đã xóa bảng Users cũ';
END

-- Tạo bảng Users mới
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

PRINT '✓ Bảng Users đã được tạo!';
GO

-- Tạo người dùng quản trị mặc định
-- Tài khoản: admin, Mật khẩu: admin123
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

PRINT '✓ Đã thêm người dùng admin mặc định!';
GO

-- =============================================
-- BƯỚC 4: TẠO BẢNG DEPARTMENTS (PHÒNG BAN)
-- =============================================
PRINT '';
PRINT '📋 BƯỚC 4: Tạo bảng Departments...';

-- Xóa bảng nếu đã tồn tại (để reset)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Departments')
BEGIN
    DROP TABLE Departments;
    PRINT '⚠ Đã xóa bảng Departments cũ';
END

-- Tạo bảng Departments mới
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

PRINT '✓ Bảng Departments đã được tạo!';
GO

-- Thêm phòng ban mẫu
INSERT INTO Departments (DepartmentCode, DepartmentName, Description, EstablishedDate, IsActive, CreatedBy, Location, PhoneNumber) VALUES
(N'IT', N'Công nghệ thông tin', N'Phòng phát triển phần mềm và bảo trì hệ thống', '2020-01-01', 1, N'admin', N'Tầng 5', '0901234567'),
(N'HR', N'Nhân sự', N'Phòng quản lý nguồn nhân lực và tuyển dụng', '2020-01-01', 1, N'admin', N'Tầng 2', '0901234568'),
(N'ACC', N'Kế toán', N'Phòng kế toán và tài chính', '2020-01-01', 1, N'admin', N'Tầng 3', '0901234569'),
(N'SALE', N'Kinh doanh', N'Phòng kinh doanh và marketing', '2020-01-01', 1, N'admin', N'Tầng 4', '0901234570'),
(N'ADMIN', N'Hành chính', N'Phòng hành chính tổng hợp', '2020-01-01', 1, N'admin', N'Tầng 1', '0901234571');

PRINT '✓ Đã thêm 5 phòng ban mẫu!';
GO

-- =============================================
-- BƯỚC 5: TẠO BẢNG EMPLOYEES (NHÂN VIÊN)
-- =============================================
PRINT '';
PRINT '📋 BƯỚC 5: Tạo bảng Employees...';

-- Xóa bảng nếu đã tồn tại (để reset)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')
BEGIN
    DROP TABLE Employees;
    PRINT '⚠ Đã xóa bảng Employees cũ';
END

-- Tạo bảng Employees mới
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

PRINT '✓ Bảng Employees đã được tạo!';
GO

CREATE INDEX IX_Employees_Status ON Employees(Status);
CREATE INDEX IX_Employees_Department ON Employees(DepartmentId);
GO

PRINT '✓ Đã tạo các chỉ mục trên bảng Employees!';
GO

-- Thêm nhân viên mẫu
INSERT INTO Employees (EmployeeCode, FullName, Gender, DateOfBirth, Email, PhoneNumber, Address, Position, DepartmentId, Salary, HireDate, Status, CreatedBy) VALUES
-- IT Department (ID: 1)
(N'NV001', N'Nguyễn Văn An', 'Male', '1988-05-15', 'an.nguyen@company.com', '0912345671', N'123 Hoàng Văn Thụ, Tân Bình, HCM', N'IT Manager', 1, 30000000, '2020-01-15', 'Active', 'admin'),
(N'NV002', N'Trần Thị Bình', 'Female', '1992-08-20', 'binh.tran@company.com', '0912345672', N'456 Nguyễn Văn Cừ, Quận 5, HCM', N'Senior Developer', 1, 25000000, '2020-02-01', 'Active', 'admin'),
(N'NV003', N'Lê Minh Cường', 'Male', '1990-03-10', 'cuong.le@company.com', '0912345673', N'789 Lê Lợi, Quận 1, HCM', N'Full-Stack Developer', 1, 22000000, '2020-03-15', 'Active', 'admin'),
(N'NV004', N'Phạm Thị Dung', 'Female', '1995-11-25', 'dung.pham@company.com', '0912345674', N'321 Võ Văn Tần, Quận 3, HCM', N'Frontend Developer', 1, 20000000, '2021-01-10', 'Active', 'admin'),
(N'NV005', N'Hoàng Văn Em', 'Male', '1993-07-08', 'em.hoang@company.com', '0912345675', N'654 Cách Mạng Tháng 8, Tân Bình, HCM', N'DevOps Engineer', 1, 23000000, '2021-05-20', 'Active', 'admin'),
(N'NV006', N'Võ Thị Phương', 'Female', '1994-12-12', 'phuong.vo@company.com', '0912345676', N'987 Nguyễn Thái Học, Quận 1, HCM', N'System Analyst', 1, 21000000, '2021-08-15', 'Active', 'admin'),

-- HR Department (ID: 2)
(N'NV007', N'Đỗ Văn Giang', 'Male', '1985-06-18', 'giang.do@company.com', '0912345677', N'147 Pasteur, Quận 1, HCM', N'HR Manager', 2, 28000000, '2020-01-20', 'Active', 'admin'),
(N'NV008', N'Bùi Thị Hạnh', 'Female', '1991-09-05', 'hanh.bui@company.com', '0912345678', N'258 Điện Biên Phủ, Quận 3, HCM', N'HR Specialist', 2, 18000000, '2020-04-10', 'Active', 'admin'),
(N'NV009', N'Ngô Văn Inh', 'Male', '1989-01-30', 'inh.ngo@company.com', '0912345679', N'369 Hai Bà Trưng, Quận 3, HCM', N'Recruitment Specialist', 2, 17000000, '2020-07-01', 'Active', 'admin'),
(N'NV010', N'Lý Thị Kim', 'Female', '1996-04-22', 'kim.ly@company.com', '0912345680', N'741 Trần Hưng Đạo, Quận 5, HCM', N'Training Coordinator', 2, 16000000, '2021-03-01', 'Active', 'admin'),

-- Accounting Department (ID: 3)
(N'NV011', N'Phan Văn Long', 'Male', '1987-02-14', 'long.phan@company.com', '0912345681', N'852 Lý Tự Trọng, Quận 1, HCM', N'Accounting Manager', 3, 26000000, '2020-02-01', 'Active', 'admin'),
(N'NV012', N'Tạ Thị Mai', 'Female', '1992-10-08', 'mai.ta@company.com', '0912345682', N'963 Nguyễn Đình Chiểu, Quận 3, HCM', N'Senior Accountant', 3, 19000000, '2020-05-15', 'Active', 'admin'),
(N'NV013', N'Vũ Văn Nam', 'Male', '1994-08-17', 'nam.vu@company.com', '0912345683', N'159 Bùi Viện, Quận 1, HCM', N'Financial Analyst', 3, 18000000, '2020-08-01', 'Active', 'admin'),
(N'NV014', N'Đinh Thị Oanh', 'Female', '1993-12-03', 'oanh.dinh@company.com', '0912345684', N'357 Nguyễn Trãi, Quận 5, HCM', N'Tax Specialist', 3, 17000000, '2021-02-15', 'Active', 'admin'),

-- Sales Department (ID: 4)
(N'NV015', N'Trương Văn Phúc', 'Male', '1986-11-11', 'phuc.truong@company.com', '0912345685', N'246 Tôn Đức Thắng, Quận 1, HCM', N'Sales Manager', 4, 29000000, '2020-02-15', 'Active', 'admin'),
(N'NV016', N'Lâm Thị Quỳnh', 'Female', '1990-07-25', 'quynh.lam@company.com', '0912345686', N'468 Lê Duẩn, Quận 3, HCM', N'Senior Sales Executive', 4, 22000000, '2020-06-01', 'Active', 'admin'),
(N'NV017', N'Cao Văn Rồng', 'Male', '1995-03-07', 'rong.cao@company.com', '0912345687', N'579 Cộng Hòa, Tân Bình, HCM', N'Sales Executive', 4, 18000000, '2020-09-01', 'Active', 'admin'),
(N'NV018', N'Đặng Thị Sương', 'Female', '1997-05-19', 'suong.dang@company.com', '0912345688', N'680 Nam Kỳ Khởi Nghĩa, Quận 3, HCM', N'Marketing Specialist', 4, 17000000, '2021-01-15', 'Active', 'admin'),
(N'NV019', N'Hồ Văn Tùng', 'Male', '1991-09-28', 'tung.ho@company.com', '0912345689', N'791 Phạm Ngũ Lão, Quận 1, HCM', N'Business Development', 4, 20000000, '2021-04-01', 'Active', 'admin'),

-- Admin Department (ID: 5)  
(N'NV020', N'Châu Thị Uyên', 'Female', '1989-06-13', 'uyen.chau@company.com', '0912345690', N'802 Hoàng Sa, Quận 3, HCM', N'Admin Manager', 5, 24000000, '2020-01-10', 'Active', 'admin'),
(N'NV021', N'Lưu Văn Việt', 'Male', '1993-02-26', 'viet.luu@company.com', '0912345691', N'913 Trường Sa, Phú Nhuận, HCM', N'Office Coordinator', 5, 15000000, '2020-08-15', 'Active', 'admin'),
(N'NV022', N'Khương Thị Xuân', 'Female', '1996-10-04', 'xuan.khuong@company.com', '0912345692', N'124 Xô Viết Nghệ Tĩnh, Bình Thạnh, HCM', N'Administrative Assistant', 5, 14000000, '2021-06-01', 'Active', 'admin');

PRINT '✓ Đã thêm 22 nhân viên mở rộng cho 5 phòng ban!';
GO

-- Add foreign key constraint after Employees table is created
ALTER TABLE Departments
ADD CONSTRAINT FK_Departments_Manager FOREIGN KEY (ManagerId)
    REFERENCES Employees(Id);
GO

-- Cập nhật quản lý phòng ban cho nhân viên
UPDATE Departments SET ManagerId = 1 WHERE DepartmentCode = 'IT';
UPDATE Departments SET ManagerId = 7 WHERE DepartmentCode = 'HR';
UPDATE Departments SET ManagerId = 11 WHERE DepartmentCode = 'ACC';
UPDATE Departments SET ManagerId = 15 WHERE DepartmentCode = 'SALE';
UPDATE Departments SET ManagerId = 20 WHERE DepartmentCode = 'ADMIN';
GO

PRINT '✓ Đã cập nhật quản lý phòng ban cho tất cả nhân viên!';
GO

-- =============================================
-- BƯỚC 5A: TẠO BẢNG ATTENDANCE (CHẤM CÔNG)
-- =============================================
PRINT '';
PRINT '📋 BƯỚC 5A: Tạo bảng Attendance...';

-- Xóa bảng nếu đã tồn tại (để reset)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Attendance')
BEGIN
    DROP TABLE Attendance;
    PRINT '⚠ Đã xóa bảng Attendance cũ';
END

-- Tạo bảng Attendance với cấu trúc đầy đủ
CREATE TABLE Attendance (
    Id INT PRIMARY KEY IDENTITY(1,1),
EmployeeId INT NOT NULL,
    AttendanceDate DATE NOT NULL,
    CheckInTime DATETIME NULL,
    CheckOutTime DATETIME NULL,
    WorkingHours DECIMAL(5,2) NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Present',
    IsLate BIT DEFAULT 0,
    LateMinutes INT DEFAULT 0,
    OvertimeHours DECIMAL(5,2) DEFAULT 0,
    Notes NVARCHAR(500) NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    CreatedBy NVARCHAR(50) NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy NVARCHAR(50) NULL,

    CONSTRAINT FK_Attendance_Employee FOREIGN KEY (EmployeeId)
        REFERENCES Employees(Id) ON DELETE CASCADE,
    
    CONSTRAINT UQ_Attendance_EmployeeDate UNIQUE (EmployeeId, AttendanceDate),
    
    CONSTRAINT CK_Attendance_Status CHECK (Status IN ('Present', 'Absent', 'Late', 'Leave', 'OnLeave')),
    
    -- Ràng buộc: Nếu Status = Present hoặc Late thì phải có CheckInTime
    CONSTRAINT CK_Attendance_CheckInTime CHECK (
        (Status IN ('Present', 'Late') AND CheckInTime IS NOT NULL) OR
     (Status IN ('Absent', 'Leave', 'OnLeave'))
    )
);

PRINT '✓ Bảng Attendance đã được tạo với đầy đủ ràng buộc!';
GO

-- Tạo chỉ mục để tăng hiệu suất truy vấn
CREATE INDEX IX_Attendance_EmployeeId ON Attendance(EmployeeId);
CREATE INDEX IX_Attendance_Date ON Attendance(AttendanceDate);
CREATE INDEX IX_Attendance_Status ON Attendance(Status);
CREATE INDEX IX_Attendance_EmployeeDate ON Attendance(EmployeeId, AttendanceDate);
GO

PRINT '✓ Đã tạo các chỉ mục trên bảng Attendance!';
GO

-- =============================================
-- BƯỚC 5B: THÊM DỮ LIỆU CHẤM CÔNG MẪU (CẢI TIẾN)
-- =============================================
PRINT '';
PRINT '📋 BƯỚC 5B: Thêm dữ liệu chấm công cho 15 ngày gần đây...';

-- Tạo dữ liệu chấm công cho 15 ngày gần đây
DECLARE @StartDate DATE = DATEADD(DAY, -15, CAST(GETDATE() AS DATE));
DECLARE @CurrentDate DATE = @StartDate;
DECLARE @EmployeeId INT = 1;
DECLARE @MaxEmployeeId INT = 22;
DECLARE @RecordCount INT = 0;

WHILE @CurrentDate <= CAST(GETDATE() AS DATE)
BEGIN
    -- Chỉ tạo chấm công cho ngày làm việc (thứ 2-6)
    IF DATEPART(WEEKDAY, @CurrentDate) BETWEEN 2 AND 6
    BEGIN
        SET @EmployeeId = 1;
    
        WHILE @EmployeeId <= @MaxEmployeeId
        BEGIN
DECLARE @CheckInTime DATETIME = NULL;
            DECLARE @CheckOutTime DATETIME = NULL;
    DECLARE @IsLate BIT = 0;
            DECLARE @LateMinutes INT = 0;
            DECLARE @WorkingHours DECIMAL(5,2) = 0;
         DECLARE @Status NVARCHAR(20) = 'Present';
        DECLARE @OvertimeHours DECIMAL(5,2) = 0;
   
            -- Random attendance pattern (85% present, 10% late, 5% absent)
     DECLARE @AttendanceType INT = ABS(CHECKSUM(NEWID())) % 100;

            IF @AttendanceType < 5 -- 5% absent
            BEGIN
         SET @Status = 'Absent';
         SET @CheckInTime = NULL;
                SET @CheckOutTime = NULL;
                SET @WorkingHours = 0;
            SET @IsLate = 0;
      SET @LateMinutes = 0;
            SET @OvertimeHours = 0;
            END
  ELSE IF @AttendanceType < 15 -- 10% late  
     BEGIN
SET @LateMinutes = ABS(CHECKSUM(NEWID())) % 60 + 10; -- 10-69 minutes late
      -- Convert DATE to DATETIME first, then add minutes
      SET @CheckInTime = DATEADD(MINUTE, 480 + @LateMinutes, CAST(@CurrentDate AS DATETIME)); -- 8:00 AM + late minutes
    SET @CheckOutTime = DATEADD(MINUTE, 480, @CheckInTime); -- 8 hours later
       SET @IsLate = 1;
          SET @WorkingHours = 8.0;
    SET @Status = 'Late';
   
-- Random overtime (30% chance for late people)
       IF (ABS(CHECKSUM(NEWID())) % 100) < 30
        BEGIN
         SET @OvertimeHours = (ABS(CHECKSUM(NEWID())) % 3) + 1; -- 1-3 hours OT
           SET @CheckOutTime = DATEADD(HOUR, @OvertimeHours, @CheckOutTime);
      END
            END
 ELSE -- 85% on time
          BEGIN
     -- Random check-in between 7:45-8:00 (early) or 8:00-8:15 (on time)
                DECLARE @CheckInVariation INT = (ABS(CHECKSUM(NEWID())) % 30) - 15; -- -15 to +15 minutes
     -- Convert DATE to DATETIME first, then add minutes
   SET @CheckInTime = DATEADD(MINUTE, 480 + @CheckInVariation, CAST(@CurrentDate AS DATETIME)); -- 8:00 AM +/- variation
           SET @CheckOutTime = DATEADD(MINUTE, 480, @CheckInTime); -- 8 hours later
SET @WorkingHours = 8.0;
          SET @Status = 'Present';
     SET @IsLate = 0;
 SET @LateMinutes = 0;
  
                -- Mark as late if check-in after 8:15
   IF @CheckInVariation > 15
       BEGIN
       SET @IsLate = 1;
         SET @LateMinutes = @CheckInVariation - 15;
                    SET @Status = 'Late';
                END
       
       -- Random overtime (20% chance for on-time people)
       IF (ABS(CHECKSUM(NEWID())) % 100) < 20
    BEGIN
        SET @OvertimeHours = (ABS(CHECKSUM(NEWID())) % 3) + 1; -- 1-3 hours OT
       SET @CheckOutTime = DATEADD(HOUR, @OvertimeHours, @CheckOutTime);
                END
            END
            
      -- Insert attendance record (chỉ insert nếu chưa tồn tại)
            IF NOT EXISTS (SELECT 1 FROM Attendance WHERE EmployeeId = @EmployeeId AND AttendanceDate = @CurrentDate)
          BEGIN
 INSERT INTO Attendance (
         EmployeeId, AttendanceDate, CheckInTime, CheckOutTime, 
    WorkingHours, Status, IsLate, LateMinutes, OvertimeHours, CreatedBy
   ) VALUES (
      @EmployeeId, @CurrentDate, @CheckInTime, @CheckOutTime,
         @WorkingHours, @Status, @IsLate, @LateMinutes, @OvertimeHours, 'admin'
        );
   
         SET @RecordCount = @RecordCount + 1;
            END      
            
      SET @EmployeeId = @EmployeeId + 1;
        END
    END
    
    SET @CurrentDate = DATEADD(DAY, 1, @CurrentDate);
END

PRINT '✓ Đã thêm ' + CAST(@RecordCount AS NVARCHAR(10)) + ' bản ghi chấm công cho 15 ngày gần đây!';
GO

-- =============================================
-- BƯỚC 5C: THÊM USER ACCOUNTS MỞ RỘNG
-- =============================================
PRINT '';
PRINT '📋 BƯỚC 5C: Thêm tài khoản user cho nhân viên...';

-- Thêm các tài khoản user cho nhân viên (Password: admin123 - SHA256)
INSERT INTO Users (Username, PasswordHash, FullName, Email, RoleId, EmployeeId, IsActive, CreatedBy) VALUES
-- Managers (Role 2 = Manager)
('an.nguyen', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Nguyễn Văn An', 'an.nguyen@company.com', 2, 1, 1, 'admin'),
('giang.do', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Đỗ Văn Giang', 'giang.do@company.com', 2, 7, 1, 'admin'),
('long.phan', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Phan Văn Long', 'long.phan@company.com', 2, 11, 1, 'admin'),
('phuc.truong', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Trương Văn Phúc', 'phuc.truong@company.com', 2, 15, 1, 'admin'),
('uyen.chau', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Châu Thị Uyên', 'uyen.chau@company.com', 2, 20, 1, 'admin'),

-- HR Staff (Role 3 = HR)
('hanh.bui', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Bùi Thị Hạnh', 'hanh.bui@company.com', 3, 8, 1, 'admin'),
('inh.ngo', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Ngô Văn Inh', 'inh.ngo@company.com', 3, 9, 1, 'admin'),
('kim.ly', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Lý Thị Kim', 'kim.ly@company.com', 3, 10, 1, 'admin'),

-- Regular Employees (Role 4 = Employee)
('binh.tran', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Trần Thị Bình', 'binh.tran@company.com', 4, 2, 1, 'admin'),
('cuong.le', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Lê Minh Cường', 'cuong.le@company.com', 4, 3, 1, 'admin'),
('dung.pham', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Phạm Thị Dung', 'dung.pham@company.com', 4, 4, 1, 'admin'),
('mai.ta', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Tạ Thị Mai', 'mai.ta@company.com', 4, 12, 1, 'admin'),
('quynh.lam', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Lâm Thị Quỳnh', 'quynh.lam@company.com', 4, 16, 1, 'admin'),
('viet.luu', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', N'Lưu Văn Việt', 'viet.luu@company.com', 4, 21, 1, 'admin');

PRINT '✓ Đã thêm 14 tài khoản user! (Password: admin123 cho tất cả)';
GO

-- =============================================
-- BƯỚC 10: TẠO STORED PROCEDURES
-- =============================================
PRINT '';
PRINT '📋 BƯỚC 10: Tạo stored procedures...';
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

PRINT '✓ Stored procedure sp_GetAttendanceSummary đã được tạo!';
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

PRINT '✓ Stored procedure sp_CalculatePayroll đã được tạo!';
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
        ISNULL(d.DepartmentName, N'Chưa phân bổ') as Department,
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

PRINT '✓ Stored procedure sp_GetDashboardStats đã được tạo!';
GO

-- =============================================
-- BƯỚC 11: TẠO VIEWS (Tùy chọn)
-- =============================================
PRINT '';
PRINT '📋 BƯỚC 11: Tạo views...';
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

PRINT '✓ View vw_EmployeeDetails đã được tạo!';
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

PRINT '✓ View vw_AttendanceDetails đã được tạo!';
GO

-- =============================================
-- TÓM TẮT CÀI ĐẶT
-- =============================================
PRINT '';
PRINT '🎉 HOÀN TẤT CÀI ĐẶT!';
PRINT '==========================================';
PRINT '';

-- Hiển thị danh sách nhân viên
PRINT '👥 DANH SÁCH NHÂN VIÊN:';
PRINT '----------------------------';

SELECT
    Id AS [ID],
    FullName AS [Họ Tên],
    Gender AS [GT],
    CONVERT(VARCHAR(10), DateOfBirth, 103) AS [Ngày Sinh],
    Position AS [Chức Vụ],
    ISNULL(Department, N'Chưa phân bổ') AS [Phòng Ban],
    FORMAT(Salary, 'N0') AS [Lương (VNĐ)]
FROM Employees
ORDER BY DepartmentId, Salary DESC;

PRINT '';
PRINT '📊 THỐNG KÊ THEO PHÒNG BAN:';
PRINT '----------------------------';

SELECT
    d.DepartmentName AS [Phòng Ban],
    COUNT(e.Id) AS [Số NV],
    d.DepartmentCode AS [Mã PB],
    mgr.FullName AS [Trưởng Phòng],
    FORMAT(AVG(e.Salary), 'N0') + ' VNĐ' AS [Lương TB],
    FORMAT(MIN(e.Salary), 'N0') + ' VNĐ' AS [Lương Min],
    FORMAT(MAX(e.Salary), 'N0') + ' VNĐ' AS [Lương Max]
FROM Departments d
LEFT JOIN Employees e ON d.Id = e.DepartmentId AND e.Status = 'Active'
LEFT JOIN Employees mgr ON d.ManagerId = mgr.Id
WHERE d.IsActive = 1
GROUP BY d.DepartmentName, d.DepartmentCode, mgr.FullName
ORDER BY COUNT(e.Id) DESC;

PRINT '';
PRINT '📊 THỐNG KÊ THEO GIỚI TÍNH:';
PRINT '----------------------------';

SELECT
    Gender AS [Giới Tính],
    COUNT(*) AS [Số Lượng],
    FORMAT(AVG(Salary), 'N0') + ' VNĐ' AS [Lương TB]
FROM Employees
GROUP BY Gender;

PRINT '';
PRINT '📈 THỐNG KÊ CHẤM CÔNG HÔM NAY:';
PRINT '-----------------------------';

DECLARE @TodayDate DATE = CAST(GETDATE() AS DATE);

IF EXISTS (SELECT 1 FROM Attendance WHERE AttendanceDate = @TodayDate)
BEGIN
    SELECT 
        Status as [Trạng Thái],
    COUNT(*) as [Số Lượng],
        CAST(COUNT(*) * 100.0 / (SELECT COUNT(*) FROM Attendance WHERE AttendanceDate = @TodayDate) AS DECIMAL(5,1)) as [Tỷ Lệ %]
    FROM Attendance 
    WHERE AttendanceDate = @TodayDate
    GROUP BY Status
    ORDER BY COUNT(*) DESC;
END
ELSE
BEGIN
    PRINT N'(Chưa có dữ liệu chấm công hôm nay)';
END

PRINT '';
PRINT '📊 TỔNG KẾT:';
PRINT '----------------------------';
SELECT
    COUNT(*) AS [Tổng số nhân viên],
  FORMAT(AVG(Salary), 'N0') + ' VNĐ' AS [Lương trung bình],
    FORMAT(SUM(Salary), 'N0') + ' VNĐ' AS [Tổng chi phí lương]
FROM Employees;

PRINT '';
PRINT '🔑 DANH SÁCH TÀI KHOẢN TEST:';
PRINT '----------------------------';
PRINT 'Username: admin, Password: admin123 (Admin)';
PRINT 'Username: an.nguyen, Password: admin123 (IT Manager)';
PRINT 'Username: giang.do, Password: admin123 (HR Manager)';
PRINT 'Username: long.phan, Password: admin123 (Accounting Manager)';
PRINT 'Username: phuc.truong, Password: admin123 (Sales Manager)';
PRINT 'Username: hanh.bui, Password: admin123 (HR Staff)';
PRINT 'Username: binh.tran, Password: admin123 (Employee)';
PRINT 'Username: cuong.le, Password: admin123 (Employee)';

PRINT '';
PRINT '📊 SỐ LIỆU TỔNG QUAN:';
PRINT '--------------------';
SELECT 
    'Phòng ban' as [Loại], 
    COUNT(*) as [Số lượng] 
FROM Departments WHERE IsActive = 1
UNION ALL
SELECT 
    'Nhân viên' as [Loại], 
    COUNT(*) as [Số lượng] 
FROM Employees WHERE Status = 'Active'
UNION ALL
SELECT 
    'Chấm công (15 ngày)' as [Loại], 
    COUNT(*) as [Số lượng] 
FROM Attendance
UNION ALL
SELECT 
    'Tài khoản user' as [Loại], 
    COUNT(*) as [Số lượng] 
FROM Users WHERE IsActive = 1;

PRINT '';
PRINT '✅ DATABASE SETUP COMPLETED SUCCESSFULLY!';
PRINT '==========================================';
PRINT '';
PRINT 'Bạn có thể bắt đầu sử dụng database QuanLyNhanSu ngay bây giờ!';
PRINT 'Tất cả dữ liệu đã được tạo đúng cấu trúc và ràng buộc.';