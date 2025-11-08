using System;

namespace WindowsFormsApp1.Models
{
    /// <summary>
    /// Employee entity class representing an employee in the system
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Primary key - Unique identifier for employee
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee code - Mã nhân viên (e.g., NV001, NV002)
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Full name of the employee
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gender of the employee (Male/Female/Other)
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Job position/title
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Department ID (Foreign Key)
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Department name (for display - loaded from Departments table)
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Department name (alternative field for backward compatibility)
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Monthly salary
        /// </summary>
        public decimal Salary { get; set; }

        /// <summary>
        /// Hire date - Ngày vào làm
        /// </summary>
        public DateTime? HireDate { get; set; }

        /// <summary>
        /// Employee status (Active, OnLeave, Resigned)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Employee photo path (local file path)
        /// </summary>
        public string PhotoPath { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Created by user
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Updated date
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Updated by user
        /// </summary>
        public string UpdatedBy { get; set; }

        // Display properties

        /// <summary>
        /// Calculated property for age
        /// </summary>
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        /// <summary>
        /// Status display with Vietnamese
        /// </summary>
        public string StatusDisplay
        {
            get
            {
                switch (Status)
                {
                    case "Active": return "Đang làm việc";
                    case "OnLeave": return "Nghỉ phép";
                    case "Resigned": return "Đã nghỉ việc";
                    default: return Status;
                }
            }
        }

        /// <summary>
        /// Hire date display
        /// </summary>
        public string HireDateDisplay => HireDate?.ToString("dd/MM/yyyy") ?? "";

        /// <summary>
        /// Get display department name
        /// </summary>
        public string DepartmentDisplay => DepartmentName ?? Department ?? "Chưa phân bổ";
    }
}
