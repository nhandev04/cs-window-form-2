# ?? Employee Management System - H? Th?ng Quân Lý Nhân S?

[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-blue.svg)](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net472)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2019+-red.svg)](https://www.microsoft.com/en-us/sql-server/)
[![Windows Forms](https://img.shields.io/badge/Windows%20Forms-Desktop%20App-green.svg)](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## ?? **PHIÊN B?N M?I - ?ã Hoàn Thành Tính N?ng Export Excel** 

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

## ? Tính N?ng Chính

### ?? **Qu?n Lý Nhân Viên**
- ? Thêm, s?a, xóa thông tin nhân viên
- ?? Tìm ki?m và l?c nhân viên theo nhi?u tiêu chí
- ?? Hi?n th? danh sách nhân viên v?i thông tin chi ti?t
- ?? Upload và qu?n lý ?nh ??i di?n nhân viên
- ?? Qu?n lý thông tin l??ng và ch?c v?
- ?? **Xu?t danh sách nhân viên ra Excel/CSV**

### ?? **Qu?n Lý Phòng Ban**
- ?? T?o và qu?n lý c?u trúc phòng ban
- ????? Gán tr??ng phòng cho t?ng phòng ban
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
- ??? Mã hóa m?t kh?u b?ng SHA256
- ?? Ghi log ho?t ??ng (Audit Trail)

### ?? **Báo Cáo và Th?ng Kê**
- ?? Dashboard t?ng quan v?i các ch? s? quan tr?ng
- ?? Báo cáo nhân s? theo phòng ban, gi?i tính
- ?? Báo cáo ch?m công và th?ng kê mu?n/s?m
- ?? Báo cáo l??ng và chi phí nhân s?

## ??? Ki?n Trúc H? Th?ng

### **3-Layer Architecture**
```
??? ?? Presentation Layer (UI)
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

### **B??c 2: C?u Hình Database**

1. **T?o Database:**
   ```sql
   -- Ch?y file SQL script
   Database_QuanLyNhanSu_Fixed_Clean.sql
   ```

2. **C?p nh?t Connection String:**
   ```csharp
   // Trong file DatabaseConfig.cs
   public static string ConnectionString = 
     "Server=YOUR_SERVER;Database=QuanLyNhanSu;Integrated Security=true;";
   ```

### **B??c 3: Build và Ch?y**

**S? d?ng Visual Studio:**
1. M? `WindowsFormsApp1.sln`
2. Build Solution (Ctrl+Shift+B)
3. Run (F5)

**S? d?ng Command Line:**
```bash
cd WindowsFormsApp1
msbuild WindowsFormsApp1.csproj
cd bin\Debug
WindowsFormsApp1.exe
```

### **B??c 4: ??ng Nh?p L?n ??u**
```
Username: admin
Password: admin123
```

## ?? Database Schema

### **Tables Overview**
| Table | Mô T? | Records |
|-------|--------|---------|
| **Roles** | Vai trò ng??i dùng | 4 roles |
| **Users** | Tài kho?n ??ng nh?p | 15 accounts |
| **Departments** | Phòng ban | 5 departments |
| **Employees** | Thông tin nhân viên | 22 employees |
| **Attendance** | Ch?m công | 15 days data |
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

## ?? Tài Kho?n Test

### **?? Administrator**
- **Username**: `admin`
- **Password**: `admin123`
- **Quy?n**: Toàn quy?n h? th?ng

### **????? Managers**
- **IT Manager**: `an.nguyen` / `admin123`
- **HR Manager**: `giang.do` / `admin123`
- **Accounting Manager**: `long.phan` / `admin123`
- **Sales Manager**: `phuc.truong` / `admin123`
- **Admin Manager**: `uyen.chau` / `admin123`

### **?? HR Staff**
- **HR Specialist**: `hanh.bui` / `admin123`
- **Recruitment**: `inh.ngo` / `admin123`
- **Training**: `kim.ly` / `admin123`

### **?? Employees**
- **Developer**: `binh.tran` / `admin123`
- **Developer**: `cuong.le` / `admin123`
- **Developer**: `dung.pham` / `admin123`
- **Accountant**: `mai.ta` / `admin123`
- **Sales**: `quynh.lam` / `admin123`
- **Office**: `viet.luu` / `admin123`

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

### **Qu?n Lý Phòng Ban**
1. **T?o phòng ban**: Vào Department ? "Add New"
2. **Gán tr??ng phòng**: Ch?n Manager t? dropdown
3. **C?p nh?t thông tin**: S?a tr?c ti?p và "Save"

### **Ch?m Công**
1. **Ghi ch?m công**: Ch?n nhân viên ? Nh?p gi? vào/ra
2. **Xem báo cáo**: Filter theo ngày/tháng
3. **Xu?t báo cáo**: Click "Export" (n?u có)

## ?? Troubleshooting

### **L?i K?t N?i Database**
```
Error: Cannot connect to database
Solution: 
1. Ki?m tra SQL Server ?ã start
2. Ki?m tra connection string trong DatabaseConfig.cs
3. Ch?y l?i script t?o database
```

### **L?i ??ng Nh?p**
```
Error: Invalid username or password
Solution:
1. Ki?m tra caps lock
2. S? d?ng tài kho?n: admin/admin123
3. Ki?m tra b?ng Users trong database
```

### **L?i Upload ?nh**
```
Error: Cannot save photo
Solution:
1. Ki?m tra quy?n ghi file trong th? m?c EmployeePhotos
2. Ki?m tra ??nh d?ng ?nh (jpg, png, bmp, gif)
3. Ki?m tra dung l??ng ?nh < 5MB
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

## ????? Author & Contact

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
????? Sample Employees: 22
?? Attendance Records: 300+
```

---

## ?? Quick Links

- [?? User Manual](docs/USER_MANUAL.md)
- [?? API Documentation](docs/API_DOCS.md)
- [??? Database Schema](docs/DATABASE_SCHEMA.md)
- [?? Report Issues](https://github.com/nhandev04/cs-window-form-2/issues)
- [?? Feature Requests](https://github.com/nhandev04/cs-window-form-2/discussions)

---

**? N?u d? án này h?u ích, hãy star repo ?? ?ng h?!**

Made with ?? by [Nhan Nguyen](https://github.com/nhandev04)