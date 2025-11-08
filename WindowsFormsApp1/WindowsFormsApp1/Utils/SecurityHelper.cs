using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using WindowsFormsApp1.Models;
using System.Diagnostics;
using System.Linq;

namespace WindowsFormsApp1.Utils
{
  /// <summary>
    /// Security Helper - Mã hóa mật khẩu
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
   /// Hash password using SHA256
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <returns>Hashed password</returns>
    public static string HashPassword(string password)
      {
            if (string.IsNullOrEmpty(password))
 return string.Empty;

      using (SHA256 sha256 = SHA256.Create())
   {
        byte[] bytes = Encoding.UTF8.GetBytes(password);
  byte[] hash = sha256.ComputeHash(bytes);

     StringBuilder result = new StringBuilder();
         foreach (byte b in hash)
       {
            result.Append(b.ToString("X2"));
       }

     return result.ToString();
            }
    }

        /// <summary>
  /// Verify password against hash
        /// </summary>
        /// <param name="password">Plain text password</param>
    /// <param name="hash">Stored hash</param>
        /// <returns>True if match</returns>
 public static bool VerifyPassword(string password, string hash)
        {
    string hashedInput = HashPassword(password);
         return string.Equals(hashedInput, hash, StringComparison.OrdinalIgnoreCase);
        }

   /// <summary>
        /// Get client machine name
        /// </summary>
   public static string GetMachineName()
        {
            try
            {
            return Environment.MachineName;
    }
            catch
    {
     return "Unknown";
       }
  }

        /// <summary>
  /// Get local IP address
        /// </summary>
        public static string GetLocalIPAddress()
        {
            try
            {
         var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
             foreach (var ip in host.AddressList)
  {
      if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
           {
      return ip.ToString();
            }
      }
                return "127.0.0.1";
       }
        catch
            {
  return "127.0.0.1";
            }
    }
    }

    /// <summary>
    /// Utility class for Excel export functionality using CSV format for compatibility
  /// </summary>
    public static class ExcelHelper
    {
        /// <summary>
        /// Export Employee list to Excel (CSV format for compatibility)
        /// </summary>
        public static void ExportEmployeesToExcel(List<Employee> employees, string fileName = null)
        {
            try
            {
        if (employees == null || employees.Count == 0)
   {
      MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
      MessageBoxButtons.OK, MessageBoxIcon.Information);
           return;
     }

         // Get save file path
                string filePath = GetSaveFilePath(fileName ?? "DanhSachNhanVien", "csv");
           if (string.IsNullOrEmpty(filePath)) return;

     // Create CSV content
            StringBuilder csvContent = new StringBuilder();

    // Add title and date
    csvContent.AppendLine("DANH SÁCH NHÂN VIÊN");
                csvContent.AppendLine($"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
        csvContent.AppendLine($"Người xuất: {SessionManager.FullName ?? "System"}");
 csvContent.AppendLine(); // Empty line

   // Add headers
                csvContent.AppendLine("STT,Mã NV,Họ Tên,Giới Tính,Ngày Sinh,Tuổi,Chức Vụ,Phòng Ban,Lương (VND),Trạng Thái");

         // Add data
           for (int i = 0; i < employees.Count; i++)
    {
         var emp = employees[i];
    csvContent.AppendLine($"{i + 1}," +
          $"{EscapeCsvField(emp.EmployeeCode ?? "")}," +
             $"{EscapeCsvField(emp.FullName)}," +
         $"{EscapeCsvField(emp.Gender)}," +
$"{emp.DateOfBirth:dd/MM/yyyy}," +
            $"{emp.Age}," +
      $"{EscapeCsvField(emp.Position)}," +
          $"{EscapeCsvField(emp.DepartmentDisplay ?? "")}," +
      $"{emp.Salary:N0}," +
            $"{EscapeCsvField(emp.Status)}");
            }

         // Add summary
        csvContent.AppendLine(); // Empty line
      csvContent.AppendLine($"TỔNG CỘNG: {employees.Count} nhân viên");
    csvContent.AppendLine($"Tổng lương: {employees.Sum(e => e.Salary):N0} VND");
          csvContent.AppendLine($"Lương trung bình: {employees.Average(e => e.Salary):N0} VND");

    // Write to file with UTF-8 BOM encoding for Excel compatibility
  var utf8WithBom = new UTF8Encoding(true);
              File.WriteAllText(filePath, csvContent.ToString(), utf8WithBom);

           MessageBox.Show($"Xuất Excel thành công!\n\n" +
    $"File: {Path.GetFileName(filePath)}\n" +
    $"Số bản ghi: {employees.Count}\n" +
     $"Đường dẫn: {filePath}",
     "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Open file location
                Process.Start("explorer.exe", "/select,\"" + filePath + "\"");
            }
            catch (Exception ex)
            {
    MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Lỗi",
        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Export Department list to Excel (CSV format)
        /// </summary>
        public static void ExportDepartmentsToExcel(List<Department> departments, string fileName = null)
        {
      try
 {
     if (departments == null || departments.Count == 0)
       {
   MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
  MessageBoxButtons.OK, MessageBoxIcon.Information);
           return;
           }

    // Get save file path
       string filePath = GetSaveFilePath(fileName ?? "DanhSachPhongBan", "csv");
      if (string.IsNullOrEmpty(filePath)) return;

        // Create CSV content
         StringBuilder csvContent = new StringBuilder();

     // Add title and date
                csvContent.AppendLine("DANH SÁCH PHÒNG BAN");
      csvContent.AppendLine($"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
    csvContent.AppendLine($"Người xuất: {SessionManager.FullName ?? "System"}");
   csvContent.AppendLine(); // Empty line

     // Add headers
        csvContent.AppendLine("STT,Mã Phòng Ban,Tên Phòng Ban,Trưởng Phòng,Số Nhân Viên,Địa Điểm,Điện Thoại,Email,Trạng Thái,Ngày Thành Lập");

 // Add data
      for (int i = 0; i < departments.Count; i++)
     {
        var dept = departments[i];
   csvContent.AppendLine($"{i + 1}," +
    $"{EscapeCsvField(dept.DepartmentCode ?? "")}," +
     $"{EscapeCsvField(dept.DepartmentName)}," +
   $"{EscapeCsvField(dept.ManagerName ?? "Chưa có")}," +
            $"{dept.EmployeeCount}," +
      $"{EscapeCsvField(dept.Location ?? "")}," +
        $"{EscapeCsvField(dept.PhoneNumber ?? "")}," +
       $"{EscapeCsvField(dept.Email ?? "")}," +
      $"{EscapeCsvField(dept.IsActive ? "Hoạt động" : "Không hoạt động")}," +
              $"{(dept.EstablishedDate?.ToString("dd/MM/yyyy") ?? "")}");
                }

      // Add summary
 csvContent.AppendLine(); // Empty line
         csvContent.AppendLine($"TỔNG CỘNG: {departments.Count} phòng ban");
     csvContent.AppendLine($"Tổng nhân viên: {departments.Sum(d => d.EmployeeCount)} người");
  csvContent.AppendLine($"Phòng ban hoạt động: {departments.Count(d => d.IsActive)}");
          csvContent.AppendLine($"Phòng ban ngưng hoạt động: {departments.Count(d => !d.IsActive)}");

      // Write to file with UTF-8 BOM encoding
           var utf8WithBom = new UTF8Encoding(true);
              File.WriteAllText(filePath, csvContent.ToString(), utf8WithBom);

        MessageBox.Show($"Xuất Excel thành công!\n\n" +
          $"File: {Path.GetFileName(filePath)}\n" +
         $"Số phòng ban: {departments.Count}\n" +
         $"Đường dẫn: {filePath}",
        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

              // Open file location
        Process.Start("explorer.exe", "/select,\"" + filePath + "\"");
        }
    catch (Exception ex)
            {
      MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Lỗi",
      MessageBoxButtons.OK, MessageBoxIcon.Error);
 }
        }

        /// <summary>
        /// Export Attendance list to Excel (CSV format)
        /// </summary>
        public static void ExportAttendanceToExcel(List<Attendance> attendanceList, DateTime fromDate, DateTime toDate, string fileName = null)
   {
       try
            {
        if (attendanceList == null || attendanceList.Count == 0)
                {
            MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
        MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
         }

           // Get save file path
  string defaultFileName = fileName ?? $"BangChamCong_{fromDate:ddMMyyyy}_{toDate:ddMMyyyy}";
        string filePath = GetSaveFilePath(defaultFileName, "csv");
         if (string.IsNullOrEmpty(filePath)) return;

    // Create CSV content
       StringBuilder csvContent = new StringBuilder();

 // Add title
                csvContent.AppendLine($"BẢNG CHẤM CÔNG TỪ {fromDate:dd/MM/yyyy} ĐẾN {toDate:dd/MM/yyyy}");
            csvContent.AppendLine($"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
       csvContent.AppendLine($"Người xuất: {SessionManager.FullName ?? "System"}");
                csvContent.AppendLine(); // Empty line

             // Add headers
    csvContent.AppendLine("STT,Mã NV,Họ Tên,Phòng Ban,Ngày,Giờ Vào,Giờ Ra,Số Giờ Làm,Giờ Tăng Ca,Đi Muộn (Phút),Trạng Thái,Ghi Chú");

// Add data
        for (int i = 0; i < attendanceList.Count; i++)
       {
        var att = attendanceList[i];
             csvContent.AppendLine($"{i + 1}," +
          $"{EscapeCsvField(att.EmployeeCode ?? "")}," +
          $"{EscapeCsvField(att.EmployeeName ?? "")}," +
      $"{EscapeCsvField(att.DepartmentName ?? "")}," +
  $"{att.AttendanceDate:dd/MM/yyyy}," +
      $"{(att.CheckInTime != DateTime.MinValue ? att.CheckInTime.ToString("HH:mm") : "")}," +
            $"{(att.CheckOutTime?.ToString("HH:mm") ?? "")}," +
    $"{(att.WorkingHours?.ToString("0.0") ?? "0")}," +
             $"{(att.OvertimeHours?.ToString("0.0") ?? "0")}," +
                 $"{(att.LateMinutes ?? 0)}," +
          $"{EscapeCsvField(GetAttendanceStatusText(att.Status))}," +
     $"{EscapeCsvField(att.Notes ?? "")}");
         }

     // Add summary
 csvContent.AppendLine(); // Empty line
        csvContent.AppendLine("THỐNG KÊ TỔNG HỢP:");
            csvContent.AppendLine($"Tổng bản ghi: {attendanceList.Count}");
          csvContent.AppendLine($"Số ngày làm việc: {attendanceList.Count(a => a.Status == "Present" || a.Status == "Late")}");
           csvContent.AppendLine($"Số ngày vắng: {attendanceList.Count(a => a.Status == "Absent")}");
          csvContent.AppendLine($"Số ngày đi muộn: {attendanceList.Count(a => a.IsLate == true)}");
     csvContent.AppendLine($"Tổng giờ làm: {attendanceList.Sum(a => a.WorkingHours ?? 0):0.0} giờ");
         csvContent.AppendLine($"Tổng giờ tăng ca: {attendanceList.Sum(a => a.OvertimeHours ?? 0):0.0} giờ");
             csvContent.AppendLine($"Tổng phút đi muộn: {attendanceList.Sum(a => a.LateMinutes ?? 0)} phút");

      // Write to file with UTF-8 BOM encoding
           var utf8WithBom = new UTF8Encoding(true);
      File.WriteAllText(filePath, csvContent.ToString(), utf8WithBom);

     MessageBox.Show($"Xuất Excel thành công!\n\n" +
            $"File: {Path.GetFileName(filePath)}\n" +
       $"Số bản ghi: {attendanceList.Count}\n" +
  $"Khoảng thời gian: {fromDate:dd/MM/yyyy} - {toDate:dd/MM/yyyy}\n" +
     $"Đường dẫn: {filePath}",
       "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

    // Open file location
   Process.Start("explorer.exe", "/select,\"" + filePath + "\"");
            }
   catch (Exception ex)
{
                MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Lỗi",
                  MessageBoxButtons.OK, MessageBoxIcon.Error);
   }
        }

        /// <summary>
        /// Get file path for saving Excel file
        /// </summary>
        private static string GetSaveFilePath(string defaultFileName, string extension = "xlsx")
 {
        using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
       if (extension.ToLower() == "csv")
 {
       saveDialog.Filter = "CSV Files (*.csv)|*.csv|Excel Files (*.xlsx)|*.xlsx|Excel 97-2003 (*.xls)|*.xls";
          saveDialog.DefaultExt = "csv";
     }
else
                {
          saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|Excel 97-2003 (*.xls)|*.xls|CSV Files (*.csv)|*.csv";
           saveDialog.DefaultExt = "xlsx";
  }

    saveDialog.FilterIndex = 1;
   saveDialog.FileName = $"{defaultFileName}_{DateTime.Now:yyyyMMdd_HHmmss}";
                saveDialog.AddExtension = true;
     saveDialog.Title = "Chọn nơi lưu file Excel";

    // Set default save location to Desktop
 saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

    if (saveDialog.ShowDialog() == DialogResult.OK)
        {
           return saveDialog.FileName;
       }
      }

            return string.Empty;
        }

        /// <summary>
        /// Convert attendance status to Vietnamese text
        /// </summary>
        private static string GetAttendanceStatusText(string status)
        {
    switch (status?.ToLower())
          {
         case "present":
return "Có mặt";
      case "late":
        return "Đi muộn";
     case "absent":
   return "Vắng mặt";
           case "leave":
           return "Nghỉ phép";
    default:
        return status ?? "";
         }
        }

        /// <summary>
      /// Escape CSV field to handle commas, quotes, and newlines
        /// </summary>
      private static string EscapeCsvField(string field)
      {
  if (string.IsNullOrEmpty(field))
      return "";

 // If field contains comma, quote, or newline, wrap in quotes and escape internal quotes
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
        {
       // Escape quotes by doubling them
      field = field.Replace("\"", "\"\"");
      // Wrap in quotes
            return "\"" + field + "\"";
      }

  return field;
        }
    }
}
