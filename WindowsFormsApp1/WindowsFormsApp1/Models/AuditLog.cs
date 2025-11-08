using System;

namespace WindowsFormsApp1.Models
{
    /// <summary>
    /// AuditLog entity - Lịch sử hoạt động
    /// </summary>
    public class AuditLog
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public int RecordId { get; set; }
        public string Action { get; set; }  // INSERT, UPDATE, DELETE, LOGIN, LOGOUT
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Description { get; set; }
        public string PerformedBy { get; set; }
        public DateTime PerformedDate { get; set; }
        public string IPAddress { get; set; }
        public string MachineName { get; set; }

        // Display properties
        public string ActionDisplay
        {
            get
            {
                switch (Action)
                {
                    case "INSERT": return "Thêm mới";
                    case "UPDATE": return "Cập nhật";
                    case "DELETE": return "Xóa";
                    case "LOGIN": return "Đăng nhập";
                    case "LOGOUT": return "Đăng xuất";
                    case "EXPORT": return "Xuất Excel";
                    case "IMPORT": return "Nhập Excel";
                    default: return Action;
                }
            }
        }

        public string TableNameDisplay
        {
            get
            {
                switch (TableName)
                {
                    case "Employees": return "Nhân viên";
                    case "Departments": return "Phòng ban";
                    case "Attendance": return "Chấm công";
                    case "Payroll": return "Bảng lương";
                    case "Users": return "Người dùng";
                    default: return TableName;
                }
            }
        }

        public string DateTimeDisplay => PerformedDate.ToString("dd/MM/yyyy HH:mm:ss");
        public string DateDisplay => PerformedDate.ToString("dd/MM/yyyy");
        public string TimeDisplay => PerformedDate.ToString("HH:mm:ss");

        public string ChangeDisplay
        {
            get
            {
                if (!string.IsNullOrEmpty(FieldName))
                {
                    return $"{FieldName}: [{OldValue}] → [{NewValue}]";
                }
                return Description ?? "";
            }
        }
    }
}
