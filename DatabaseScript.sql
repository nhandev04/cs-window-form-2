-- =============================================
-- Employee Management System - Database Script
-- =============================================

-- Create Database
USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'QuanLyNhanSu')
BEGIN
    CREATE DATABASE QuanLyNhanSu;
END
GO

USE QuanLyNhanSu;
GO

-- Create Employees Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')
BEGIN
    CREATE TABLE Employees (
        Id INT PRIMARY KEY IDENTITY(1,1),
        FullName NVARCHAR(100) NOT NULL,
        Gender NVARCHAR(10) NOT NULL,
        DateOfBirth DATE NOT NULL,
        Position NVARCHAR(50) NOT NULL,
        Department NVARCHAR(50) NOT NULL,
        Salary DECIMAL(18,2) NOT NULL,
        PhotoPath NVARCHAR(500) NULL
    );
END
GO

-- Insert Sample Data
IF NOT EXISTS (SELECT * FROM Employees)
BEGIN
    INSERT INTO Employees (FullName, Gender, DateOfBirth, Position, Department, Salary, PhotoPath)
    VALUES
        (N'Nguyễn Văn An', N'Male', '1990-05-15', N'Manager', N'IT', 25000000, NULL),
        (N'Trần Thị Bình', N'Female', '1992-08-20', N'Developer', N'IT', 18000000, NULL),
        (N'Lê Văn Cường', N'Male', '1988-03-10', N'Team Lead', N'IT', 22000000, NULL),
        (N'Phạm Thị Dung', N'Female', '1995-11-25', N'Developer', N'IT', 16000000, NULL),
        (N'Hoàng Văn Em', N'Male', '1991-07-08', N'HR Manager', N'HR', 20000000, NULL),
        (N'Đỗ Thị Phương', N'Female', '1993-02-14', N'Accountant', N'Finance', 17000000, NULL),
        (N'Vũ Văn Giang', N'Male', '1989-09-30', N'Sales Manager', N'Sales', 23000000, NULL),
        (N'Bùi Thị Hoa', N'Female', '1994-12-05', N'Sales Executive', N'Sales', 15000000, NULL),
        (N'Đinh Văn Hùng', N'Male', '1987-04-18', N'Senior Developer', N'IT', 21000000, NULL),
        (N'Mai Thị Lan', N'Female', '1996-06-22', N'Junior Developer', N'IT', 12000000, NULL);
END
GO

-- Create indexes for better performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Employees_Department')
BEGIN
    CREATE INDEX IX_Employees_Department ON Employees(Department);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Employees_Position')
BEGIN
    CREATE INDEX IX_Employees_Position ON Employees(Position);
END
GO

PRINT 'Database and table created successfully!';
PRINT 'Sample data inserted successfully!';
GO

-- Query to verify data
SELECT * FROM Employees;
GO
