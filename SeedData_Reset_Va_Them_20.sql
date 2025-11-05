-- =============================================
-- RESET DATABASE VÀ THÊM 20 NHÂN VIÊN MỚI
-- Script này sẽ XÓA TOÀN BỘ dữ liệu cũ và thêm 20 nhân viên mới
-- =============================================

USE QuanLyNhanSu;
GO

PRINT '⚠⚠⚠ CẢNH BÁO: Script này sẽ XÓA TOÀN BỘ dữ liệu cũ! ⚠⚠⚠';
PRINT '';
PRINT 'Đang xóa dữ liệu cũ...';

-- Xóa toàn bộ dữ liệu
DELETE FROM Employees;

-- Reset ID về 1
DBCC CHECKIDENT ('Employees', RESEED, 0);

PRINT '✓ Đã xóa dữ liệu cũ';
PRINT '';
PRINT 'Đang thêm 20 nhân viên mới...';
GO

-- Thêm 20 nhân viên
INSERT INTO Employees (FullName, Gender, DateOfBirth, Position, Department, Salary, PhotoPath)
VALUES
    -- IT Department (8 người)
    (N'Nguyễn Văn An', N'Male', '1985-05-15', N'IT Director', N'IT', 45000000, NULL),
    (N'Trần Thị Bình', N'Female', '1990-08-20', N'Senior Full-Stack Developer', N'IT', 28000000, NULL),
    (N'Lê Văn Cường', N'Male', '1988-03-10', N'Technical Lead', N'IT', 35000000, NULL),
    (N'Phạm Thị Dung', N'Female', '1993-11-25', N'Backend Developer', N'IT', 22000000, NULL),
    (N'Hoàng Minh Em', N'Male', '1991-07-08', N'DevOps Engineer', N'IT', 26000000, NULL),
    (N'Vũ Thị Phương', N'Female', '1994-02-14', N'Frontend Developer', N'IT', 20000000, NULL),
    (N'Đinh Văn Giang', N'Male', '1992-09-30', N'Mobile Developer', N'IT', 24000000, NULL),
    (N'Bùi Thị Hoa', N'Female', '1996-12-05', N'Junior Developer', N'IT', 14000000, NULL),

    -- HR Department (3 người)
    (N'Mai Văn Hùng', N'Male', '1986-04-18', N'HR Director', N'HR', 38000000, NULL),
    (N'Đỗ Thị Lan', N'Female', '1992-06-22', N'HR Manager', N'HR', 25000000, NULL),
    (N'Trương Văn Minh', N'Male', '1995-01-10', N'Talent Acquisition Specialist', N'HR', 18000000, NULL),

    -- Finance Department (3 người)
    (N'Phan Thị Nga', N'Female', '1987-11-30', N'Chief Financial Officer', N'Finance', 50000000, NULL),
    (N'Lý Văn Ông', N'Male', '1990-03-15', N'Finance Manager', N'Finance', 30000000, NULL),
    (N'Võ Thị Phúc', N'Female', '1994-08-25', N'Senior Accountant', N'Finance', 22000000, NULL),

    -- Sales Department (4 người)
    (N'Ngô Văn Quang', N'Male', '1988-07-12', N'Sales Director', N'Sales', 42000000, NULL),
    (N'Dương Thị Ru', N'Female', '1991-05-20', N'Key Account Manager', N'Sales', 32000000, NULL),
    (N'Cao Văn Sơn', N'Male', '1993-10-08', N'Senior Sales Executive', N'Sales', 24000000, NULL),
    (N'Hồ Thị Tâm', N'Female', '1996-02-28', N'Sales Executive', N'Sales', 16000000, NULL),

    -- Marketing Department (2 người)
    (N'Tô Văn Uyên', N'Male', '1989-09-05', N'Marketing Director', N'Marketing', 36000000, NULL),
    (N'Chu Thị Vân', N'Female', '1993-04-15', N'Senior Marketing Specialist', N'Marketing', 23000000, NULL);

GO

PRINT '';
PRINT '✓✓✓ HOÀN TẤT! ĐÃ THÊM 20 NHÂN VIÊN MỚI ✓✓✓';
PRINT '';

-- Hiển thị danh sách
SELECT
    Id,
    FullName AS [Họ Tên],
    Gender AS [GT],
    CONVERT(VARCHAR(10), DateOfBirth, 103) AS [Ngày Sinh],
    Position AS [Chức Vụ],
    Department AS [Phòng Ban],
    FORMAT(Salary, 'N0') AS [Lương (VNĐ)]
FROM Employees
ORDER BY Id;

PRINT '';
PRINT '📊 THỐNG KÊ NHANH:';
PRINT '-------------------';

SELECT
    Department AS [Phòng Ban],
    COUNT(*) AS [Số NV],
    FORMAT(AVG(Salary), 'N0') + ' VNĐ' AS [Lương TB]
FROM Employees
GROUP BY Department
ORDER BY Department;

PRINT '';
SELECT
    Gender AS [Giới Tính],
    COUNT(*) AS [Số Lượng]
FROM Employees
GROUP BY Gender;

GO
