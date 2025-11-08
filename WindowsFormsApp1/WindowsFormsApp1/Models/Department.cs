using System;

namespace WindowsFormsApp1.Models
{
    /// <summary>
    /// Department entity - Phòng ban
    /// </summary>
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public int? ManagerId { get; set; }
        public string ManagerName { get; set; }  // For display - loaded from Employees table
        public string Description { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
        public int EmployeeCount { get; set; }  // Calculated property
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        // Display properties
        public string StatusDisplay => IsActive ? "Hoạt động" : "Ngưng hoạt động";
        public string EstablishedDateDisplay => EstablishedDate?.ToString("dd/MM/yyyy") ?? "";
        public string ManagerDisplay => string.IsNullOrEmpty(ManagerName) ? "Chưa có" : ManagerName;
    }
}
