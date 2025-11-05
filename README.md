# 🏢 Employee Management System (Hệ Thống Quản Lý Nhân Sự)

Ứng dụng quản lý nhân sự hoàn chỉnh bằng **C# Windows Forms** với **SQL Server**, hỗ trợ đầy đủ CRUD, tìm kiếm, lọc và quản lý ảnh.

---

## 📋 Mục Lục

- [⚠️ BẢO MẬT - ĐỌC TRƯỚC KHI PUSH GITHUB](#️-bảo-mật---đọc-trước-khi-push-github)
- [Tính Năng](#-tính-năng)
- [Công Nghệ](#-công-nghệ)
- [Cài Đặt Nhanh](#-cài-đặt-nhanh)
- [Cấu Trúc Database](#-cấu-trúc-database)
- [Hướng Dẫn Sử Dụng](#-hướng-dẫn-sử-dụng)
- [Xử Lý Lỗi](#-xử-lý-lỗi-thường-gặp)
- [Scripts SQL](#-scripts-sql-có-sẵn)

---

## ⚠️ BẢO MẬT - ĐỌC TRƯỚC KHI PUSH GITHUB

### 🔴 THÔNG TIN NHẠY CẢM

Project này có các file chứa thông tin nhạy cảm **KHÔNG NÊN** push lên GitHub public:

1. **`WindowsFormsApp1/WindowsFormsApp1/App.config`**
   - Chứa connection string với **password SQL Server**
   - ✅ Đã được thêm vào `.gitignore`

2. **Thư mục build outputs:**
   - `bin/`, `obj/`, `.vs/`, `.claude/`
   - ✅ Đã được thêm vào `.gitignore`

3. **Ảnh nhân viên:**
   - `EmployeePhotos/` - chứa ảnh cá nhân
   - ✅ Đã được thêm vào `.gitignore`

### ✅ ĐÃ BẢO VỆ

File **`.gitignore`** đã được tạo sẵn để bảo vệ các file nhạy cảm. Khi bạn commit, các file sau **KHÔNG** được push:

```
✅ App.config (chứa password)
✅ .vs/ (Visual Studio settings)
✅ .claude/ (Claude Code settings)
✅ bin/, obj/ (Build outputs)
✅ EmployeePhotos/ (Ảnh nhân viên)
```

### 📝 CẤU HÌNH CHO NGƯỜI KHÁC

File **`App.config.example`** đã được tạo để hướng dẫn cấu hình:

**Người clone project cần làm:**

1. Copy `App.config.example` thành `App.config`:
   ```bash
   cd WindowsFormsApp1/WindowsFormsApp1/
   copy App.config.example App.config
   ```

2. Sửa `App.config` với thông tin SQL Server của họ:
   ```xml
   <connectionString>
     Server=localhost;
     Database=QuanLyNhanSu;
     User Id=sa;
     Password=PASSWORD_CUA_BAN;  <!-- ⚠️ Sửa đây -->
     TrustServerCertificate=True;
   </connectionString>
   ```

### 🚨 TRƯỚC KHI COMMIT

**LUÔN LUÔN kiểm tra:**

```bash
# Xem file nào sẽ được commit
git status

# Xem nội dung sẽ commit (tìm password)
git diff

# Kiểm tra App.config có bị track không
git check-ignore WindowsFormsApp1/WindowsFormsApp1/App.config
# ✅ Phải hiển thị: .gitignore:79:**/App.config
```

**KHÔNG BAO GIỜ commit nếu thấy:**
- ❌ `Password=` trong git diff
- ❌ File `App.config` trong git status
- ❌ Folder `EmployeePhotos/` với ảnh thật

---

## ✨ Tính Năng

### CRUD Hoàn Chỉnh
- ✅ **Thêm** nhân viên mới với đầy đủ thông tin
- ✅ **Xem** danh sách nhân viên trong DataGridView
- ✅ **Sửa** thông tin nhân viên (click vào row)
- ✅ **Xóa** nhân viên (có xác nhận)

### Tìm Kiếm & Lọc
- 🔍 **Tìm kiếm real-time** theo tên hoặc phòng ban
- 🎯 **Lọc** theo:
  - Giới tính (Male/Female/Other)
  - Phòng ban (IT, HR, Finance, Sales, Marketing...)
  - Chức vụ (Developer, Manager, Director...)
- 🔄 **Clear filters** để reset tất cả bộ lọc

### Quản Lý Ảnh
- 📸 **Upload ảnh** nhân viên (JPG, PNG, BMP, GIF)
- 💾 **Lưu ảnh** vào folder local `EmployeePhotos`
- 🔗 **Lưu đường dẫn** vào database (không lưu binary)
- 🖼️ **Hiển thị ảnh** tự động khi chọn nhân viên

### Giao Diện
- 🎨 **UI hiện đại** với màu sắc phân biệt chức năng
- 📱 **Responsive** - co giãn theo kích thước cửa sổ
- 🖥️ **Split layout** - form bên trái, danh sách bên phải
- ⌨️ **Keyboard friendly** - tab order hợp lý

### Validation & Bảo Mật
- ✔️ **Validate đầy đủ** tất cả input
- 🛡️ **SQL Injection protection** với parameterized queries
- ⚠️ **Error handling** với thông báo tiếng Việt
- 📊 **Business logic** tách biệt trong BLL

---

## 🛠️ Công Nghệ

| Component | Technology |
|-----------|-----------|
| Framework | .NET Framework 4.7.2 |
| Language | C# |
| UI | Windows Forms |
| Database | SQL Server (2016+) |
| Data Access | ADO.NET |
| Architecture | 3-Tier (DAL, BLL, UI) |

---

## 🚀 Cài Đặt Nhanh

### Bước 1: Chuẩn Bị

**Yêu cầu:**
- ✅ Visual Studio 2019/2022
- ✅ SQL Server 2016+ (LocalDB, Express hoặc Full)
- ✅ .NET Framework 4.7.2+

### Bước 2: Tạo Database

**Mở SQL Server Management Studio (SSMS):**

1. Kết nối với server của bạn:
   - Server: `localhost` (hoặc `.` hoặc `.\SQLEXPRESS`)
   - Authentication: SQL Server Authentication
   - Login: `sa`
   - Password: `123456789`

2. Chạy script tạo database:

```sql
-- Option 1: Database trống với 10 nhân viên mẫu
-- Chạy file: TaoDatabase_QuanLyNhanSu.sql

-- Option 2: Database với 20 nhân viên đầy đủ (KHUYẾN NGHỊ)
-- Chạy file: SeedData_Reset_Va_Them_20.sql
```

**Trong SSMS:**
```
File > Open > File...
→ Chọn: SeedData_Reset_Va_Them_20.sql
→ Nhấn F5 (Execute)
```

### Bước 3: Cấu Hình Connection String

File `App.config` đã được cấu hình sẵn:

```xml
<connectionStrings>
    <add name="EmployeeDBConnection"
         connectionString="Server=localhost;Database=QuanLyNhanSu;User Id=sa;Password=123456789;TrustServerCertificate=True;"
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

**Nếu dùng SQL Server khác:**
- SQL Server Express: `Server=.\SQLEXPRESS;...`
- Windows Auth: `Server=localhost;...;Integrated Security=True;`

### Bước 4: Build & Run

```
1. Mở: WindowsFormsApp1\WindowsFormsApp1.sln
2. Build: Ctrl + Shift + B
3. Run: F5
4. ✅ Ứng dụng khởi động với 20 nhân viên!
```

---

## 📊 Cấu Trúc Database

### Bảng Employees

```sql
CREATE TABLE Employees (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,      -- Họ tên đầy đủ
    Gender NVARCHAR(10) NOT NULL,          -- Male/Female/Other
    DateOfBirth DATE NOT NULL,             -- Ngày sinh
    Position NVARCHAR(50) NOT NULL,        -- Chức vụ
    Department NVARCHAR(50) NOT NULL,      -- Phòng ban
    Salary DECIMAL(18,2) NOT NULL,         -- Lương (VNĐ)
    PhotoPath NVARCHAR(500) NULL           -- Đường dẫn ảnh
);

-- Indexes để tăng tốc tìm kiếm
CREATE INDEX IX_Employees_Department ON Employees(Department);
CREATE INDEX IX_Employees_Position ON Employees(Position);
```

### Quy Tắc Validation

| Field | Rule |
|-------|------|
| FullName | 2-100 ký tự, bắt buộc |
| Gender | Male/Female/Other, bắt buộc |
| DateOfBirth | Tuổi 18-100, bắt buộc |
| Position | Max 50 ký tự, bắt buộc |
| Department | Max 50 ký tự, bắt buộc |
| Salary | > 0, bắt buộc |
| PhotoPath | NULL hoặc đường dẫn hợp lệ |

---

## 📖 Hướng Dẫn Sử Dụng

### 1️⃣ Thêm Nhân Viên Mới

```
1. Nhấn nút "New" (màu xanh dương)
2. Điền thông tin:
   - Họ Tên *
   - Giới Tính * (chọn dropdown)
   - Ngày Sinh * (chọn từ calendar)
   - Chức Vụ *
   - Phòng Ban *
   - Lương * (số, VNĐ)
   - Ảnh (tùy chọn)
3. Nhấn "Save" (màu xanh lá)
4. ✅ Nhân viên được thêm vào database
```

### 2️⃣ Sửa Thông Tin Nhân Viên

```
1. Click vào dòng nhân viên trong bảng
   → Thông tin hiển thị bên trái
   → Dòng được highlight màu xanh
2. Sửa các field bạn muốn
3. Nhấn "Save"
4. ✅ Thông tin được cập nhật
```

**3 cách chọn nhân viên:**
- Click 1 lần vào dòng
- Double-click vào dòng
- Click vào bất kỳ cell nào trong dòng

### 3️⃣ Upload Ảnh Nhân Viên

```
1. Chọn nhân viên (mới hoặc có sẵn)
2. Nhấn "Upload Photo"
3. Chọn file ảnh (JPG, PNG, BMP, GIF)
4. Ảnh hiển thị ngay trong PictureBox
5. Nhấn "Save"
6. ✅ Ảnh lưu vào: bin\Debug\EmployeePhotos\
```

**Lưu ý về ảnh:**
- Ảnh được copy vào folder local
- Đường dẫn lưu trong database
- Khuyến nghị: < 2MB, JPG/PNG
- Tỷ lệ tốt nhất: 3:4 (ảnh chân dung)

### 4️⃣ Xóa Nhân Viên

```
1. Chọn nhân viên cần xóa
2. Nhấn "Delete" (màu đỏ)
3. Xác nhận trong hộp thoại
4. ✅ Nhân viên bị xóa khỏi database
```

### 5️⃣ Tìm Kiếm

```
Gõ vào ô "Search":
- Tìm theo tên: "Nguyễn"
- Tìm theo phòng ban: "IT"
- Kết quả hiển thị real-time
```

### 6️⃣ Lọc Dữ Liệu

```
Sử dụng 3 dropdown:
- Gender: Male/Female/Other
- Department: IT/HR/Finance/Sales/Marketing
- Position: Developer/Manager/Director...

Có thể kết hợp nhiều filter!
Nhấn "Clear Filters" để reset.
```

### 7️⃣ Làm Mới

```
Nhấn "Refresh" (màu xám) để:
- Tải lại dữ liệu từ database
- Xóa tất cả filter và search
- Reset form nhập liệu
```

---

## ⚠️ Xử Lý Lỗi Thường Gặp

### ❌ Lỗi 1: "Cannot open database 'QuanLyNhanSu'"

**Nguyên nhân:** Database chưa được tạo

**Giải pháp:**
```sql
-- Chạy trong SSMS:
File > Open > TaoDatabase_QuanLyNhanSu.sql
→ F5 (Execute)

-- Hoặc tạo database thủ công:
CREATE DATABASE QuanLyNhanSu;
```

### ❌ Lỗi 2: "Login failed for user 'sa'"

**Nguyên nhân:** Password sai hoặc sa bị disabled

**Giải pháp:**

**A. Enable SQL Server Authentication:**
```
1. SSMS > Chuột phải Server > Properties
2. Security > SQL Server and Windows Authentication mode
3. OK > Restart SQL Server
```

**B. Enable user sa:**
```
1. SSMS > Security > Logins > sa > Properties
2. Tab Status:
   - Permission to connect: Grant ✅
   - Login: Enabled ✅
3. Tab General: Set password = 123456789
4. OK
```

**C. Restart SQL Server:**
```
- Chuột phải Server name > Restart
- Hoặc: services.msc > SQL Server > Restart
```

### ❌ Lỗi 3: "Invalid column name 'Photo'"

**Nguyên nhân:** Database có cấu trúc cũ (dùng Photo thay vì PhotoPath)

**Giải pháp:**
```sql
-- Chạy script update:
File > Open > UpdateDatabase_PhotoPath.sql
→ F5
```

### ❌ Lỗi 4: "A network-related error occurred"

**Nguyên nhân:** SQL Server không chạy hoặc sai server name

**Giải pháp:**

**A. Kiểm tra SQL Server đang chạy:**
```
1. Windows + R > services.msc
2. Tìm "SQL Server (MSSQLSERVER)" hoặc "SQL Server (SQLEXPRESS)"
3. Nếu Stopped → Chuột phải > Start
```

**B. Thử các server name khác trong App.config:**
```xml
<!-- Thử 1: localhost -->
Server=localhost;...

<!-- Thử 2: Dot -->
Server=.;...

<!-- Thử 3: SQL Express -->
Server=.\SQLEXPRESS;...

<!-- Thử 4: IP -->
Server=127.0.0.1;...
```

### ❌ Lỗi 5: "Không thể hiển thị ảnh"

**Nguyên nhân:** File ảnh bị xóa hoặc di chuyển

**Giải pháp:**
```
1. Upload lại ảnh mới cho nhân viên đó
2. Hoặc kiểm tra folder: bin\Debug\EmployeePhotos\
3. Copy ảnh vào folder nếu còn backup
```

### ❌ Lỗi 6: "Click vào bảng không được"

**Nguyên nhân:** Chưa có dữ liệu hoặc code chưa build

**Giải pháp:**
```
1. Kiểm tra title bar: "Employee Management System - X nhân viên"
2. Nếu 0 nhân viên → Chạy SeedData_Reset_Va_Them_20.sql
3. Build lại: Ctrl + Shift + B
4. Run lại: F5
```

---

## 📜 Scripts SQL Có Sẵn

### 1. `TaoDatabase_QuanLyNhanSu.sql`
**Mục đích:** Tạo database + bảng + 10 nhân viên mẫu
**Dùng khi:** Lần đầu cài đặt

```sql
-- Tạo database QuanLyNhanSu
-- Tạo bảng Employees
-- Thêm 10 nhân viên mẫu
-- Tạo indexes
```

### 2. `SeedData_20NhanVien.sql`
**Mục đích:** Thêm 20 nhân viên vào database hiện tại
**Dùng khi:** Muốn thêm nhiều dữ liệu test

```sql
-- Thêm 20 nhân viên (KHÔNG xóa dữ liệu cũ)
-- IT: 8 người
-- HR: 3 người
-- Finance: 3 người
-- Sales: 4 người
-- Marketing: 2 người
```

### 3. `SeedData_Reset_Va_Them_20.sql` ⭐ KHUYẾN NGHỊ
**Mục đích:** Reset database và thêm 20 nhân viên mới
**Dùng khi:** Muốn bắt đầu lại với dữ liệu sạch

```sql
-- ⚠️ XÓA TOÀN BỘ dữ liệu cũ
-- Reset ID về 1
-- Thêm 20 nhân viên mới với thông tin đầy đủ
-- Hiển thị thống kê
```

### 4. `UpdateDatabase_PhotoPath.sql`
**Mục đích:** Update database từ cấu trúc cũ (Photo) sang mới (PhotoPath)
**Dùng khi:** Nâng cấp từ version cũ

```sql
-- Kiểm tra cột Photo có tồn tại
-- Thêm cột PhotoPath
-- Xóa cột Photo
-- Hiển thị cấu trúc mới
```

### 5. `KiemTraKetNoi.sql`
**Mục đích:** Kiểm tra database và dữ liệu
**Dùng khi:** Debug hoặc verify setup

```sql
-- Kiểm tra SQL Server version
-- Kiểm tra database tồn tại
-- Kiểm tra bảng Employees
-- Hiển thị cấu trúc bảng
-- Đếm số nhân viên
-- Hiển thị dữ liệu mẫu
```

### 6. `DatabaseScript.sql`
**Mục đích:** Script gốc (tương tự TaoDatabase_QuanLyNhanSu.sql)
**Dùng khi:** Backup hoặc reference

---

## 📁 Cấu Trúc Project

```
cs-window-form-2/
│
├── WindowsFormsApp1/
│   ├── WindowsFormsApp1.sln              # Solution file
│   └── WindowsFormsApp1/
│       ├── Models/
│       │   └── Employee.cs               # Entity class
│       ├── DAL/
│       │   ├── DatabaseConfig.cs         # Connection helper
│       │   └── EmployeeDAL.cs            # Data access layer
│       ├── BLL/
│       │   └── EmployeeBLL.cs            # Business logic
│       ├── Form1.cs                      # Main form logic
│       ├── Form1.Designer.cs             # UI design code
│       ├── Form1.resx                    # Form resources
│       ├── Program.cs                    # Entry point
│       ├── App.config                    # Configuration
│       └── bin/Debug/
│           └── EmployeePhotos/           # Photo storage (auto-created)
│
├── TaoDatabase_QuanLyNhanSu.sql          # Create DB + 10 employees
├── SeedData_20NhanVien.sql               # Add 20 employees
├── SeedData_Reset_Va_Them_20.sql         # Reset + 20 employees ⭐
├── UpdateDatabase_PhotoPath.sql          # Update DB structure
├── KiemTraKetNoi.sql                     # Test connection
└── README.md                             # This file

```

---

## 🎯 Kiến Trúc 3 Tầng

### 1. **Data Access Layer (DAL)**
- File: `DAL/EmployeeDAL.cs`
- Chức năng: Tất cả thao tác database
- Công nghệ: ADO.NET (SqlConnection, SqlCommand)
- Methods:
  - `GetAllEmployees()` - Lấy danh sách
  - `GetEmployeeById(int)` - Lấy 1 nhân viên
  - `AddEmployee(Employee)` - Thêm mới
  - `UpdateEmployee(Employee)` - Cập nhật
  - `DeleteEmployee(int)` - Xóa
  - `SearchEmployees(string)` - Tìm kiếm
  - `FilterEmployees(...)` - Lọc
  - `GetDepartments()` - Lấy danh sách phòng ban
  - `GetPositions()` - Lấy danh sách chức vụ

### 2. **Business Logic Layer (BLL)**
- File: `BLL/EmployeeBLL.cs`
- Chức năng: Validation, business rules
- Validation:
  - Kiểm tra required fields
  - Validate độ dài string
  - Kiểm tra tuổi (18-100)
  - Validate salary > 0
  - Error messages tiếng Việt

### 3. **User Interface (UI)**
- File: `Form1.cs`, `Form1.Designer.cs`
- Chức năng: Giao diện người dùng
- Controls:
  - DataGridView: Hiển thị danh sách
  - TextBox: Nhập liệu
  - ComboBox: Dropdown (Gender, Filters)
  - DateTimePicker: Chọn ngày
  - PictureBox: Hiển thị ảnh
  - Buttons: New, Save, Delete, Refresh

---

## 💡 Tips & Tricks

### 🔥 Performance

1. **Database Indexes**: Đã tạo sẵn indexes cho Department và Position
2. **Connection Pooling**: Mặc định enabled trong connection string
3. **Photo Size**: Nên < 2MB để load nhanh

### 🎨 Customize

**Thay đổi màu nút:**
```csharp
// Form1.Designer.cs
btnSave.BackColor = Color.FromArgb(46, 204, 113); // Green
btnDelete.BackColor = Color.FromArgb(231, 76, 60); // Red
btnNew.BackColor = Color.FromArgb(52, 152, 219);   // Blue
```

**Thêm validation mới:**
```csharp
// BLL/EmployeeBLL.cs > ValidateEmployee()
if (employee.Salary < 5000000)
{
    errorMessage = "Lương phải >= 5,000,000 VNĐ";
    return false;
}
```

**Thêm field mới:**
```sql
-- 1. Thêm cột vào database
ALTER TABLE Employees ADD Email NVARCHAR(100);

-- 2. Thêm property vào Models/Employee.cs
public string Email { get; set; }

-- 3. Update DAL queries
-- 4. Thêm control vào Form
```

### 🔐 Security

- ✅ Parameterized queries (SQL injection protection)
- ✅ Input validation tại BLL
- ✅ Read-only DataGridView
- ✅ Error handling toàn bộ code

### 📦 Backup & Restore

**Backup database:**
```sql
BACKUP DATABASE QuanLyNhanSu
TO DISK = 'C:\Backup\QuanLyNhanSu.bak';
```

**Backup photos:**
```
Copy folder: bin\Debug\EmployeePhotos\
```

---

## 🎓 Học Từ Project Này

Project này demonstrate:
- ✅ 3-tier architecture
- ✅ ADO.NET database operations
- ✅ Windows Forms UI design
- ✅ Data validation patterns
- ✅ File I/O (ảnh)
- ✅ CRUD operations
- ✅ Search & filter logic
- ✅ Error handling
- ✅ Connection string management

---

## 🆘 Cần Trợ Giúp?

### Kiểm Tra Trước:

1. ✅ SQL Server đang chạy?
2. ✅ Database QuanLyNhanSu đã tạo?
3. ✅ Bảng Employees có dữ liệu?
4. ✅ Connection string trong App.config đúng?
5. ✅ Project build thành công?

### Debug Steps:

```
1. Chạy KiemTraKetNoi.sql trong SSMS
   → Kiểm tra database OK

2. Build project: Ctrl + Shift + B
   → Xem Output window có lỗi không

3. Run: F5
   → Xem thông báo lỗi chi tiết

4. Kiểm tra title bar: "X nhân viên"
   → Nếu 0 → Chạy SeedData_Reset_Va_Them_20.sql
```

---

## 📝 Change Log

### Version 1.0 (Current)
- ✅ CRUD operations hoàn chỉnh
- ✅ Search & filter
- ✅ Photo management (lưu local)
- ✅ 3-tier architecture
- ✅ Full validation
- ✅ Error handling tiếng Việt
- ✅ 20+ nhân viên seed data

---

## 📄 License

Project này được tạo ra cho mục đích học tập và thương mại.

---

## 🎉 Hoàn Tất!

**Bây giờ bạn có thể:**
1. ✅ Chạy SeedData_Reset_Va_Them_20.sql
2. ✅ Mở solution trong Visual Studio
3. ✅ Build & Run (F5)
4. ✅ Thêm/Sửa/Xóa nhân viên
5. ✅ Upload ảnh
6. ✅ Tìm kiếm và lọc dữ liệu

**Chúc bạn thành công!** 🚀

---

**Ngày tạo:** 2025-01-05
**Framework:** .NET Framework 4.7.2
**Database:** SQL Server 2016+
**Language:** C# + SQL
