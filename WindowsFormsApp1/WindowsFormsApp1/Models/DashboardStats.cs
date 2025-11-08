using System.Collections.Generic;

namespace WindowsFormsApp1.Models
{
    /// <summary>
    /// Dashboard Statistics - Thống kê cho dashboard
    /// </summary>
    public class DashboardStats
    {
        // KPIs - Key Performance Indicators
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int OnLeaveEmployees { get; set; }
        public int ResignedEmployees { get; set; }
        public int NewEmployeesThisMonth { get; set; }
        public int NewEmployeesThisYear { get; set; }
        public decimal TotalPayrollThisMonth { get; set; }
        public decimal TotalPayrollLastMonth { get; set; }
        public decimal AverageSalary { get; set; }
        public int TotalDepartments { get; set; }
        public int ActiveDepartments { get; set; }

        // Attendance stats
        public int TotalAttendanceToday { get; set; }
        public int LateToday { get; set; }
        public int AbsentToday { get; set; }
        public decimal AverageWorkingHours { get; set; }

        // Chart data
        public Dictionary<string, int> EmployeesByDepartment { get; set; }
        public Dictionary<string, int> EmployeesByGender { get; set; }
        public Dictionary<string, int> EmployeesByAgeGroup { get; set; }
        public Dictionary<string, decimal> PayrollByMonth { get; set; }
        public Dictionary<string, int> AttendanceByStatus { get; set; }
        public Dictionary<string, int> NewEmployeesByMonth { get; set; }

        public DashboardStats()
        {
            EmployeesByDepartment = new Dictionary<string, int>();
            EmployeesByGender = new Dictionary<string, int>();
            EmployeesByAgeGroup = new Dictionary<string, int>();
            PayrollByMonth = new Dictionary<string, decimal>();
            AttendanceByStatus = new Dictionary<string, int>();
            NewEmployeesByMonth = new Dictionary<string, int>();
        }
    }

    /// <summary>
    /// System Settings - Cài đặt hệ thống
    /// </summary>
    public class SystemSetting
    {
        public int Id { get; set; }
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
        public string DataType { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
