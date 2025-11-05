-- =============================================
-- SEED DATA - 20 NHÂN VIÊN MẪU
-- =============================================

USE QuanLyNhanSu;
GO

-- Xóa dữ liệu cũ (nếu muốn reset)
-- DELETE FROM Employees;
-- DBCC CHECKIDENT ('Employees', RESEED, 0);
-- GO

PRINT 'Bắt đầu thêm 20 nhân viên mẫu...';
PRINT '';

-- Insert 20 nhân viên
INSERT INTO Employees (FullName, Gender, DateOfBirth, Position, Department, Salary, PhotoPath)
VALUES
    -- IT Department (8 người)
    (N'Nguyễn Văn An', N'Male', '1990-05-15', N'IT Director', N'IT', 40000000, NULL),
    (N'Trần Thị Bình', N'Female', '1992-08-20', N'Senior Developer', N'IT', 25000000, NULL),
    (N'Lê Văn Cường', N'Male', '1988-03-10', N'Tech Lead', N'IT', 30000000, NULL),
    (N'Phạm Thị Dung', N'Female', '1995-11-25', N'Full-Stack Developer', N'IT', 18000000, NULL),
    (N'Hoàng Minh Em', N'Male', '1993-07-08', N'DevOps Engineer', N'IT', 22000000, NULL),
    (N'Vũ Thị Phương', N'Female', '1996-02-14', N'Frontend Developer', N'IT', 16000000, NULL),
    (N'Đinh Văn Giang', N'Male', '1991-09-30', N'Backend Developer', N'IT', 20000000, NULL),
    (N'Bùi Thị Hoa', N'Female', '1997-12-05', N'Junior Developer', N'IT', 12000000, NULL),

    -- HR Department (3 người)
    (N'Mai Văn Hùng', N'Male', '1987-04-18', N'HR Director', N'HR', 35000000, NULL),
    (N'Đỗ Thị Lan', N'Female', '1994-06-22', N'HR Manager', N'HR', 20000000, NULL),
    (N'Trương Văn Minh', N'Male', '1995-01-10', N'Recruiter', N'HR', 15000000, NULL),

    -- Finance Department (3 người)
    (N'Phan Thị Nga', N'Female', '1989-11-30', N'Finance Manager', N'Finance', 32000000, NULL),
    (N'Lý Văn Ông', N'Male', '1992-03-15', N'Senior Accountant', N'Finance', 22000000, NULL),
    (N'Võ Thị Phúc', N'Female', '1996-08-25', N'Accountant', N'Finance', 16000000, NULL),

    -- Sales Department (4 người)
    (N'Ngô Văn Quang', N'Male', '1990-07-12', N'Sales Director', N'Sales', 38000000, NULL),
    (N'Dương Thị Ru', N'Female', '1993-05-20', N'Senior Sales Executive', N'Sales', 24000000, NULL),
    (N'Cao Văn Sơn', N'Male', '1994-10-08', N'Sales Executive', N'Sales', 18000000, NULL),
    (N'Hồ Thị Tâm', N'Female', '1997-02-28', N'Sales Representative', N'Sales', 13000000, NULL),

    -- Marketing Department (2 người)
    (N'Tô Văn Uyên', N'Male', '1991-09-05', N'Marketing Manager', N'Marketing', 28000000, NULL),
    (N'Chu Thị Vân', N'Female', '1995-04-15', N'Digital Marketing Specialist', N'Marketing', 17000000, NULL);

GO

PRINT '';
PRINT '✓✓✓ ĐÃ THÊM 20 NHÂN VIÊN THÀNH CÔNG! ✓✓✓';
PRINT '';

-- Hiển thị kết quả
PRINT '====================================';
PRINT 'DANH SÁCH NHÂN VIÊN';
PRINT '====================================';

SELECT
    Id AS [ID],
    FullName AS [Họ Tên],
    Gender AS [Giới Tính],
    DATEDIFF(YEAR, DateOfBirth, GETDATE()) AS [Tuổi],
    Position AS [Chức Vụ],
    Department AS [Phòng Ban],
    FORMAT(Salary, 'N0') + N' VNĐ' AS [Lương]
FROM Employees
ORDER BY Department, Salary DESC;

PRINT '';
PRINT '====================================';
PRINT 'THỐNG KÊ';
PRINT '====================================';

-- Thống kê theo phòng ban
SELECT
    Department AS [Phòng Ban],
    COUNT(*) AS [Số Lượng],
    FORMAT(AVG(Salary), 'N0') + N' VNĐ' AS [Lương TB],
    FORMAT(MIN(Salary), 'N0') + N' VNĐ' AS [Lương Min],
    FORMAT(MAX(Salary), 'N0') + N' VNĐ' AS [Lương Max]
FROM Employees
GROUP BY Department
ORDER BY COUNT(*) DESC;

PRINT '';

-- Thống kê theo giới tính
SELECT
    Gender AS [Giới Tính],
    COUNT(*) AS [Số Lượng],
    FORMAT(AVG(Salary), 'N0') + N' VNĐ' AS [Lương TB]
FROM Employees
GROUP BY Gender;

PRINT '';
PRINT 'Tổng số nhân viên: 20';
PRINT 'Hoàn tất!';
GO
