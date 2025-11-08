# ?? Employee Management System - H? Th?ng Qu?n Lý Nhân S?

[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-blue.svg)](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net472)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2019+-red.svg)](https://www.microsoft.com/en-us/sql-server/)
[![Windows Forms](https://img.shields.io/badge/Windows%20Forms-Desktop%20App-green.svg)](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## ? **PHIÊN B?N M?I - ?ã Hoàn Thành Tính N?ng Export Excel** 

### **?? Excel Export cho t?t c? Module:**
- ? **Employee Management** - Xu?t danh sách nhân viên ?ang hi?n th?
- ? **Department Management** - Xu?t danh sách phòng ban
- ? **Attendance Management** - Xu?t b?ng ch?m công theo kho?ng th?i gian

### **?? Cách s? d?ng Export Excel:**
1. **M? module** b?t k? (Employee, Department, Attendance)
2. **Filter d? li?u** theo ý mu?n (n?u c?n)
3. **Click nút "Export Excel"** 
4. **Ch?n n?i l?u file** trong dialog
5. **File CSV t? ??ng m?** location sau khi xu?t

### **?? Technical Features:**
- **CSV Format** - T??ng thích v?i Excel, LibreOffice, Google Sheets
- **UTF-8 Encoding** - Hi?n th? chính xác ti?ng Vi?t  
- **Timestamp filenames** - Tránh trùng l?p tên file
- **Auto-open location** - T? ??ng m? folder ch?a file
- **Error handling** - X? lý l?i an toàn

---

## ?? Mô T? D? Án

**Employee Management System** là m?t ?ng d?ng Windows Forms ???c phát tri?n b?ng C# và .NET Framework 4.7.2, ???c thi?t k? ?? qu?n lý toàn di?n thông tin nhân s? trong doanh nghi?p. H? th?ng cung c?p các tính n?ng qu?n lý nhân viên, phòng ban, ch?m công và báo cáo v?i giao di?n thân thi?n và d? s? d?ng.

## ?? Tính N?ng Chính

### ?? **Qu?n Lý Nhân Viên**
- ? Thêm, s?a, xóa thông tin nhân viên
- ?? Tìm ki?m và l?c nhân viên theo nhi?u tiêu chí
- ?? Hi?n th? danh sách nhân viên v?i thông tin chi ti?t
- ?? Upload và qu?n lý ?nh ??i di?n nhân viên
- ?? Qu?n lý thông tin l??ng và ch?c v?
- ?? **Xu?t danh sách nhân viên ra Excel/CSV**

### ?? **Qu?n Lý Phòng Ban**
- ?? T?o và qu?n lý c?u trúc phòng ban
- ???? Gán tr??ng phòng cho t?ng phòng ban
- ?? Th?ng kê s? l??ng nhân viên theo phòng ban
- ? Qu?n lý tr?ng thái ho?t ??ng c?a phòng ban
- ?? **Xu?t danh sách phòng ban ra Excel/CSV**

### ? **H? Th?ng Ch?m Công**
- ?? Ghi nh?n gi? vào/ra c?a nhân viên
- ?? Xem l?ch s? ch?m công theo ngày/tháng
- ?? Theo dõi nhân viên ?i mu?n và t?ng ca
- ?? Báo cáo th?ng kê ch?m công chi ti?t
- ?? **Xu?t b?ng ch?m công theo kho?ng th?i gian ra Excel/CSV**

### ?? **H? Th?ng B?o M?t**
- ?? ??ng nh?p v?i xác th?c ng??i dùng
- ?? Phân quy?n theo vai trò (Admin, Manager, HR, Employee)
- ?? Mã hóa m?t kh?u b?ng SHA256
- ?? Ghi log ho?t ??ng (Audit Trail)

### ?? **Báo Cáo và Th?ng Kê**
- ?? Dashboard t?ng quan v?i các ch? s? quan tr?ng
- ?? Báo cáo nhân s? theo phòng ban, gi?i tính
- ? Báo cáo ch?m công và th?ng kê mu?n/s?m
- ?? Báo cáo l??ng và chi phí nhân s?

## ??? Ki?n Trúc H? Th?ng

### **3-Layer Architecture**
```
?? ?? Presentation Layer (UI)
?   ??? Form1.cs (Employee Management)
?   ??? FormLogin.cs (Authentication)
?   ??? FormMain.cs (Dashboard)
?   ??? FormDepartment.cs (Department Management)
?   ??? FormAttendance.cs (Attendance Management)
?
??? ?? Business Logic Layer (BLL)
?   ??? EmployeeBLL.cs
?   ??? DepartmentBLL.cs
?   ??? UserBLL.cs
?   ??? AttendanceBLL.cs
?
??? ??? Data Access Layer (DAL)
?   ??? EmployeeDAL.cs
?   ??? DepartmentDAL.cs
?   ??? UserDAL.cs
?   ??? AttendanceDAL.cs
?   ??? DatabaseConfig.cs
?
??? ?? Models
?   ??? Employee.cs
?   ??? Department.cs
?   ??? User.cs
?   ??? Attendance.cs
?   ??? Payroll.cs
?   ??? AuditLog.cs
?
??? ??? Utilities
    ??? SecurityHelper.cs
    ??? SessionManager.cs
    ??? AuditHelper.cs
```

## ?? H??ng D?n Cài ??t

### **Yêu C?u H? Th?ng**
- **OS**: Windows 10/11 ho?c Windows Server 2016+
- **.NET Framework**: 4.7.2 tr? lên
- **Database**: SQL Server 2016+ ho?c SQL Server Express
- **IDE**: Visual Studio 2019+ (khuy?n ngh?)
- **RAM**: T?i thi?u 4GB
- **Storage**: 500MB dung l??ng tr?ng

### **B??c 1: Clone Repository**
```bash
git clone https://github.com/nhandev04/cs-window-form-2.git
cd cs-window-form-2
```

### **B??c 2: C?u Hình Database** ? QUAN TR?NG

#### **?? Cách 1: T?o Database ??y ?? v?i D? Li?u M?u (KHUY?N NGH?)**

1. **M? SQL Server Management Studio (SSMS)**

2. **K?t n?i ??n SQL Server c?a b?n**

3. **Ch?y script t?o database ??y ??:**
   ```
   File: Database_QuanLyNhanSu_Full.sql
   ```
   
   **Các b??c th?c hi?n:**
 - M? file `Database_QuanLyNhanSu_Full.sql` trong SSMS
   - Nh?n **Execute** (F5) ho?c click nút ?? Execute
   - ??i script ch?y xong (kho?ng 10-30 giây)
   
   **Script này s? t? ??ng:**
   - ? T?o database `QuanLyNhanSu`
   - ? T?o t?t c? các b?ng v?i ??y ?? ràng bu?c
   - ? Thêm **4 vai trò** (Admin, Manager, HR, Employee)
   - ? Thêm **15 tài kho?n user** ?? test
 - ? Thêm **5 phòng ban** (IT, HR, ACC, SALE, ADMIN)
   - ? Thêm **22 nhân viên** v?i ??y ?? thông tin
   - ? Thêm **300+ b?n ghi ch?m công** (15 ngày g?n ?ây)
   - ? T?o **Stored Procedures** và **Views**

4. **Ch?y script s?a l?i UNIQUE constraint (QUAN TR?NG):**
   ```
   File: FixEmployeeCodeConstraint.sql
   ```
 
   **T?i sao c?n ch?y script này?**
   - S?a l?i khi c?p nh?t nhân viên không có mã nhân viên
   - Cho phép nhi?u nhân viên có EmployeeCode = NULL
   - V?n ??m b?o các mã nhân viên không b? trùng l?p
   
   **Các b??c:**
   - M? file `FixEmployeeCodeConstraint.sql` trong SSMS
   - Nh?n **Execute** (F5)
   - Ki?m tra k?t qu?: "? FIX COMPLETED SUCCESSFULLY!"

5. **(Tùy ch?n) Thêm c?t PhotoPath n?u ch?a có:**
   ```
   File: UpdateDatabase_PhotoPath.sql
   ```
   - Script này ?ã ???c tích h?p trong `Database_QuanLyNhanSu_Full.sql`
   - Ch? ch?y n?u b?n ?ã t?o database tr??c ?ó và mu?n thêm tính n?ng ?nh

#### **?? Ki?m Tra Database ?ã Có D? Li?u**

Sau khi ch?y script, ki?m tra b?ng các câu l?nh SQL:

```sql
-- Ki?m tra database ?ã t?o
USE QuanLyNhanSu;
GO

-- Ki?m tra s? l??ng b?n ghi trong các b?ng
SELECT 'Roles' AS TableName, COUNT(*) AS RecordCount FROM Roles
UNION ALL
SELECT 'Users', COUNT(*) FROM Users
UNION ALL
SELECT 'Departments', COUNT(*) FROM Departments
UNION ALL
SELECT 'Employees', COUNT(*) FROM Employees
UNION ALL
SELECT 'Attendance', COUNT(*) FROM Attendance;

-- Xem danh sách nhân viên
SELECT TOP 5 
    Id, EmployeeCode, FullName, Position, 
    DepartmentId, Salary, Status 
FROM Employees;

-- Xem danh sách phòng ban
SELECT * FROM Departments;

-- Xem danh sách tài kho?n
SELECT Username, FullName, RoleId, IsActive FROM Users;
```

**K?t qu? mong ??i:**
```
Roles : 4 records
Users       : 15 records  
Departments    : 5 records
Employees : 22 records
Attendance     : 300+ records
```

#### **?? X? Lý L?i Th??ng G?p**

**L?i: Database already exists**
```sql
-- Xóa database c? và t?o l?i
USE master;
GO
DROP DATABASE QuanLyNhanSu;
GO
-- Sau ?ó ch?y l?i Database_QuanLyNhanSu_Full.sql
```

**L?i: UNIQUE KEY constraint violation**
```
? Gi?i pháp: Ch?y script FixEmployeeCodeConstraint.sql
```

**L?i: Cannot connect to SQL Server**
```
1. Ki?m tra SQL Server ?ã kh?i ??ng ch?a
2. Ki?m tra tên server (localhost, .\SQLEXPRESS, v.v.)
3. Ki?m tra Windows Authentication ho?c SQL Authentication
```

### **B??c 3: C?p Nh?t Connection String**

1. **M? file `DatabaseConfig.cs`:**
   ```
 ???ng d?n: WindowsFormsApp1/DAL/DatabaseConfig.cs
```

2. **C?p nh?t Connection String:**
   
   **Windows Authentication (Khuy?n ngh?):**
   ```csharp
   public static string ConnectionString = 
     "Server=localhost;Database=QuanLyNhanSu;Integrated Security=true;";
   ```
   
   **Ho?c v?i SQL Server Express:**
   ```csharp
   public static string ConnectionString = 
       "Server=.\\SQLEXPRESS;Database=QuanLyNhanSu;Integrated Security=true;";
   ```
   
   **SQL Authentication:**
   ```csharp
   public static string ConnectionString = 
       "Server=localhost;Database=QuanLyNhanSu;User Id=sa;Password=YourPassword;";
   ```

3. **Ki?m tra k?t n?i:**
   - Build và ch?y ?ng d?ng
   - N?u k?t n?i thành công, form login s? hi?n th?
   - N?u l?i, ki?m tra l?i connection string và SQL Server

### **B??c 4: Build và Ch?y ?ng D?ng**

**S? d?ng Visual Studio:**
1. M? solution `WindowsFormsApp1.sln`
2. Build Solution: **Ctrl+Shift+B**
3. Run: **F5** ho?c click ?? Start

**S? d?ng Command Line:**
```bash
cd WindowsFormsApp1
msbuild WindowsFormsApp1.csproj /p:Configuration=Release
cd bin\Release
WindowsFormsApp1.exe
```

### **B??c 5: ??ng Nh?p L?n ??u**

S? d?ng tài kho?n admin m?c ??nh:
```
Username: admin
Password: admin123
```

?? **HOÀN T?T! B?n ?ã s?n sàng s? d?ng h? th?ng!**

---

## ?? Database Schema

### **Tables Overview**
| Table | Mô T? | Records M?u |
|-------|--------|-------------|
| **Roles** | Vai trò ng??i dùng | 4 roles |
| **Users** | Tài kho?n ??ng nh?p | 15 accounts |
| **Departments** | Phòng ban | 5 departments |
| **Employees** | Thông tin nhân viên | 22 employees |
| **Attendance** | Ch?m công | 300+ records |
| **Payroll** | B?ng l??ng | Dynamic |
| **AuditLogs** | L?ch s? ho?t ??ng | Auto-generated |
| **SystemSettings** | C?u hình h? th?ng | 15 settings |

### **Key Relationships**
```sql
Departments (1) ?? (N) Employees
Employees (1) ?? (N) Attendance
Employees (1) ?? (N) Payroll
Roles (1) ?? (N) Users
Users (1) ?? (0,1) Employees
```

### **D? Li?u M?u Chi Ti?t**

#### **5 Phòng Ban:**
- ?? **IT** - Công ngh? thông tin (6 nhân viên)
- ?? **HR** - Nhân s? (4 nhân viên)
- ?? **ACC** - K? toán (4 nhân viên)
- ?? **SALE** - Kinh doanh (5 nhân viên)
- ?? **ADMIN** - Hành chính (3 nhân viên)

#### **22 Nhân Viên:**
- ??y ?? thông tin: H? tên, gi?i tính, ngày sinh, email, S?T
- Ch?c v? ?a d?ng: Manager, Developer, HR Specialist, Accountant, Sales...
- L??ng t? 14,000,000 - 30,000,000 VN?
- Tr?ng thái: Active, OnLeave, Resigned

#### **300+ B?n Ghi Ch?m Công:**
- D? li?u 15 ngày g?n ?ây
- T? ??ng random: ?úng gi? (85%), Mu?n (10%), V?ng (5%)
- Có t?ng ca ng?u nhiên
- Tính toán t? ??ng: WorkingHours, LateMinutes, OvertimeHours

## ?? Tài Kho?n Test

### **?? Administrator**
- **Username**: `admin`
- **Password**: `admin123`
- **Quy?n**: Toàn quy?n h? th?ng

### **???? Managers**
- **IT Manager**: `an.nguyen` / `admin123`
- **HR Manager**: `giang.do` / `admin123`
- **Accounting Manager**: `long.phan` / `admin123`
- **Sales Manager**: `phuc.truong` / `admin123`
- **Admin Manager**: `uyen.chau` / `admin123`

### **?? HR Staff**
- **HR Specialist**: `hanh.bui` / `admin123`
- **Recruitment**: `inh.ngo` / `admin123`
- **Training**: `kim.ly` / `admin123`

### **????? Employees**
- **Developer**: `binh.tran` / `admin123`
- **Developer**: `cuong.le` / `admin123`
- **Developer**: `dung.pham` / `admin123`
- **Accountant**: `mai.ta` / `admin123`
- **Sales**: `quynh.lam` / `admin123`
- **Office**: `viet.luu` / `admin123`

**?? L?u ý:** T?t c? tài kho?n ??u dùng password: `admin123`

## ?? Screenshots

### **?? Login Screen**
- Giao di?n ??ng nh?p ??n gi?n và b?o m?t
- Xác th?c ng??i dùng v?i mã hóa m?t kh?u

### **?? Dashboard**
- T?ng quan h? th?ng v?i các ch? s? quan tr?ng
- Bi?u ?? th?ng kê nhân s? theo phòng ban
- Quick access t?i các ch?c n?ng chính

### **?? Employee Management**
- Danh sách nhân viên v?i filtering và search
- Form chi ti?t v?i upload ?nh ??i di?n
- Qu?n lý thông tin cá nhân và công vi?c

### **?? Department Management**
- Cây phòng ban v?i c?u trúc phân c?p
- Gán tr??ng phòng và qu?n lý nhân s?
- Th?ng kê nhân viên theo t?ng phòng ban

### **? Attendance Tracking**
- Ghi nh?n ch?m công hàng ngày
- L?ch s? và báo cáo ch?m công
- Theo dõi gi? làm và t?ng ca

## ??? Technologies Used

### **Frontend**
- **Windows Forms** - Desktop UI Framework
- **C#** - Programming Language
- **.NET Framework 4.7.2** - Runtime Platform

### **Backend**
- **ADO.NET** - Data Access Technology
- **SQL Server** - Database Management System
- **3-Layer Architecture** - Design Pattern

### **Security**
- **SHA256** - Password Hashing
- **Role-Based Access Control** - Authorization
- **Audit Trail** - Activity Logging
- **Session Management** - User State

### **Development Tools**
- **Visual Studio 2019+** - IDE
- **SQL Server Management Studio** - Database Tool
- **Git** - Version Control

## ?? Configuration

### **Database Configuration**
```csharp
// WindowsFormsApp1/DAL/DatabaseConfig.cs
public static class DatabaseConfig
{
    public static string ConnectionString = 
        "Server=localhost;Database=QuanLyNhanSu;Integrated Security=true;";
    
 // Ho?c s? d?ng SQL Authentication
    // "Server=localhost;Database=QuanLyNhanSu;User Id=sa;Password=yourpassword;";
}
```

### **Application Settings**
```csharp
// Có th? ?i?u ch?nh trong b?ng SystemSettings
- CheckInTime: 08:00
- CheckOutTime: 17:00  
- LateThreshold: 15 minutes
- StandardWorkingDays: 26 days/month
- OvertimeRate: 1.5x
```

## ?? Usage Guide

### **Qu?n Lý Nhân Viên**
1. **Thêm nhân viên m?i**: Click "New" ? ?i?n thông tin ? "Save"
2. **Ch?nh s?a**: Click ch?n nhân viên ? S?a thông tin ? "Save"
3. **Tìm ki?m**: Nh?p t? khóa vào ô "Search"
4. **L?c**: S? d?ng dropdown Gender, Department, Position
5. **Upload ?nh**: Click "Upload Photo" ? Ch?n file ?nh
6. **Xu?t Excel**: Click "Export Excel" ? Ch?n v? trí l?u

### **Qu?n Lý Phòng Ban**
1. **T?o phòng ban**: Vào Department ? "Add New"
2. **Gán tr??ng phòng**: Ch?n Manager t? dropdown
3. **C?p nh?t thông tin**: S?a tr?c ti?p và "Save"
4. **Xu?t Excel**: Click "Export Excel"

### **Ch?m Công**
1. **Ghi ch?m công**: Ch?n nhân viên ? Nh?p gi? vào/ra
2. **Xem báo cáo**: Filter theo ngày/tháng
3. **Xu?t báo cáo**: Click "Export Excel" ? Ch?n kho?ng th?i gian

## ?? Troubleshooting

### **L?i K?t N?i Database**
```
Error: Cannot connect to database
Solution: 
1. Ki?m tra SQL Server ?ã start: services.msc ? SQL Server
2. Ki?m tra connection string trong DatabaseConfig.cs
3. Ch?y l?i script Database_QuanLyNhanSu_Full.sql
4. Ki?m tra firewall và port 1433
```

### **L?i ??ng Nh?p**
```
Error: Invalid username or password
Solution:
1. Ki?m tra caps lock
2. S? d?ng tài kho?n: admin/admin123
3. Ki?m tra b?ng Users trong database:
   SELECT * FROM Users WHERE Username = 'admin'
4. N?u không có, ch?y l?i script t?o database
```

### **L?i Upload ?nh**
```
Error: Cannot save photo
Solution:
1. T?o th? m?c: WindowsFormsApp1\bin\Debug\EmployeePhotos\
2. Ki?m tra quy?n ghi file (Run as Administrator)
3. Ki?m tra ??nh d?ng ?nh (jpg, png, bmp, gif)
4. Ki?m tra dung l??ng ?nh < 5MB
```

### **L?i UNIQUE KEY Constraint**
```
Error: Violation of UNIQUE KEY constraint 'UQ_Employee...'
The duplicate key value is (<NULL>).

Solution:
? Ch?y script: FixEmployeeCodeConstraint.sql
   - Script này s?a l?i cho phép nhi?u nhân viên có EmployeeCode NULL
   - V?n ??m b?o các mã không b? trùng l?p
```

### **Không Có D? Li?u Trong Database**
```
Problem: Database tr?ng, không có nhân viên, phòng ban
Solution:
1. Ch?y l?i script Database_QuanLyNhanSu_Full.sql
2. Ki?m tra script ?ã ch?y xong: 
   - Xem OUTPUT trong SSMS
   - Ki?m tra: SELECT COUNT(*) FROM Employees
3. N?u v?n tr?ng, xóa database và t?o l?i:
 DROP DATABASE QuanLyNhanSu;
   GO
   -- Sau ?ó ch?y l?i Database_QuanLyNhanSu_Full.sql
```

## ?? Future Enhancements

### **Version 2.0 Planning**
- [ ] **Web API Integration** - RESTful services
- [ ] **Mobile App** - Xamarin/MAUI companion app
- [ ] **Advanced Reporting** - Crystal Reports integration
- [ ] **Email Notifications** - SMTP integration
- [ ] **Backup/Restore** - Automated database backup
- [ ] **Multi-language Support** - i18n implementation
- [ ] **Cloud Storage** - Azure/AWS integration
- [ ] **Real-time Updates** - SignalR implementation

### **Performance Improvements**
- [ ] **Caching Strategy** - Redis integration
- [ ] **Database Optimization** - Index tuning
- [ ] **Lazy Loading** - Improve UI responsiveness
- [ ] **Batch Operations** - Bulk data operations

## ?? Contributing

Chúng tôi hoan nghênh m?i ?óng góp! ?? contribute:

1. **Fork** repository
2. **Create feature branch** (`git checkout -b feature/AmazingFeature`)
3. **Commit changes** (`git commit -m 'Add some AmazingFeature'`)
4. **Push to branch** (`git push origin feature/AmazingFeature`)
5. **Open Pull Request**

### **Development Guidelines**
- Tuân th? coding standards c?a C#
- Thêm unit tests cho code m?i
- C?p nh?t documentation
- Ki?m tra performance impact

## ?? License

D? án này ???c phân ph?i d??i **MIT License**. Xem file [LICENSE](LICENSE) ?? bi?t thêm chi ti?t.

## ?????????? Author & Contact

**Developer**: [Nhan Nguyen](https://github.com/nhandev04)
- ?? **Email**: nhandev04@gmail.com
- ?? **GitHub**: [@nhandev04](https://github.com/nhandev04)
- ?? **LinkedIn**: [Connect with me](https://linkedin.com/in/nhandev04)

## ?? Acknowledgments

- Microsoft .NET Team for excellent documentation
- SQL Server team for robust database platform
- Windows Forms community for UI inspiration
- Stack Overflow community for problem-solving support

## ?? Project Statistics

```
?? Total Files: 50+
?? Lines of Code: 5,000+
??? Database Tables: 8
?? Test Users: 15
?? Sample Departments: 5
???? Sample Employees: 22
? Attendance Records: 300+
```

---

## ?? Quick Links

- [?? User Manual](docs/USER_MANUAL.md)
- [?? API Documentation](docs/API_DOCS.md)
- [??? Database Schema](docs/DATABASE_SCHEMA.md)
- [?? Report Issues](https://github.com/nhandev04/cs-window-form-2/issues)
- [?? Feature Requests](https://github.com/nhandev04/cs-window-form-2/discussions)

---

## ?? Checklist Cài ??t Nhanh

- [ ] Clone repository t? GitHub
- [ ] Cài ??t SQL Server (n?u ch?a có)
- [ ] Ch?y script `Database_QuanLyNhanSu_Full.sql`
- [ ] Ch?y script `FixEmployeeCodeConstraint.sql`
- [ ] Ki?m tra database có d? li?u (SELECT COUNT(*) FROM Employees)
- [ ] C?p nh?t Connection String trong `DatabaseConfig.cs`
- [ ] Build solution trong Visual Studio
- [ ] Run ?ng d?ng (F5)
- [ ] ??ng nh?p v?i admin/admin123
- [ ] Ki?m tra các ch?c n?ng chính
- [ ] Test export Excel

---

## ?? L?u Ý Quan Tr?ng Khi Cài ??t

1. **Ph?i cài ??t ??y ?? SQL Server và SSMS**
   - T?i SQL Server 2019+ [t?i ?ây](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
   - T?i SSMS [t?i ?ây](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)

2. **Ch?y SSMS v?i quy?n Administrator**
   - Nh?p chu?t ph?i vào bi?u t??ng SSMS ? Run as administrator

3. **Ki?m tra c?ng m?c ??nh c?a SQL Server**
   - M? SQL Server Configuration Manager
   - Ch?n SQL Server Network Configuration ? Protocols for SQLEXPRESS
   - ??m b?o TCP/IP ???c Enabled và ki?m tra c?ng (m?c ??nh là 1433)

4. **T?t t?m th?i Firewall ho?c thêm exception cho SQL Server**
   - Vào Control Panel ? System and Security ? Windows Defender Firewall
   - Ch?n Advanced settings
   - Thêm inbound rule cho c?ng 1433

5. **Kh?i ??ng l?i máy tính sau khi cài ??t và c?u hình**
   - ?? áp d?ng t?t c? thay ??i, hãy kh?i ??ng l?i máy tính c?a b?n.

6. **Ki?m tra l?i k?t n?i trong chu?i k?t n?i (Connection String)**
   - ??m b?o tên server, database, username và password chính xác.
   - S? d?ng `localhost` ho?c `.\SQLEXPRESS` n?u cài SQL Server Express.

---

**? N?u d? án này h?u ích, hãy star repo ?? ?ng h?!**

Made with ?? by [Nhan Nguyen](https://github.com/nhandev04)