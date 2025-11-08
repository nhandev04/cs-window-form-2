using System;

namespace WindowsFormsApp1.Models
{
    /// <summary>
    /// User entity - Người dùng hệ thống
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }  // For display - loaded from Roles table
        public int? EmployeeId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        // Display properties
        public string StatusDisplay => IsActive ? "Hoạt động" : "Bị khóa";
        public string LastLoginDisplay => LastLogin?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa đăng nhập";
    }

    /// <summary>
    /// Role entity - Vai trò người dùng
    /// </summary>
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
