using System;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Utils
{
    /// <summary>
    /// Session Manager - Quản lý phiên đăng nhập
    /// </summary>
    public static class SessionManager
    {
        /// <summary>
        /// Current logged in user
        /// </summary>
        public static User CurrentUser { get; set; }

        /// <summary>
        /// Check if user is logged in
        /// </summary>
        public static bool IsLoggedIn => CurrentUser != null;

        /// <summary>
        /// Get current username
        /// </summary>
        public static string Username => CurrentUser?.Username ?? "Guest";

        /// <summary>
        /// Get current user's full name
        /// </summary>
        public static string FullName => CurrentUser?.FullName ?? "Guest";

        /// <summary>
        /// Get current user's role
        /// </summary>
        public static string Role => CurrentUser?.RoleName ?? "None";

        /// <summary>
        /// Check if current user is Admin
        /// </summary>
        public static bool IsAdmin => CurrentUser?.RoleName == "Admin";

        /// <summary>
        /// Check if current user is Manager
        /// </summary>
        public static bool IsManager => CurrentUser?.RoleName == "Manager";

        /// <summary>
        /// Check if current user is HR
        /// </summary>
        public static bool IsHR => CurrentUser?.RoleName == "HR";

        /// <summary>
        /// Check if current user has permission
        /// </summary>
        /// <param name="permission">Permission to check (e.g., "Employee.Edit", "Payroll.View")</param>
        /// <returns>True if has permission</returns>
        public static bool HasPermission(string permission)
        {
            if (!IsLoggedIn)
                return false;

            // Admin has all permissions
            if (IsAdmin)
                return true;

            // Parse permission string
            string[] parts = permission.Split('.');
            if (parts.Length != 2)
                return false;

            string module = parts[0];  // e.g., "Employee", "Payroll", "Department"
            string action = parts[1];  // e.g., "View", "Edit", "Delete"

            // Manager permissions
            if (IsManager)
            {
                // Can view everything
                if (action == "View")
                    return true;

                // Can edit employees and departments
                if ((module == "Employee" || module == "Department") && action == "Edit")
                    return true;

                // Can approve payroll
                if (module == "Payroll" && action == "Approve")
                    return true;

                // Cannot delete
                if (action == "Delete")
                    return false;

                return true;
            }

            // HR permissions
            if (IsHR)
            {
                // Can manage employees, attendance, and view payroll
                if (module == "Employee" || module == "Attendance")
                    return true;

                if (module == "Payroll" && action == "View")
                    return true;

                return false;
            }

            // Employee permissions
            if (CurrentUser.RoleName == "Employee")
            {
                // Can only view their own info
                if (action == "View" && module == "Employee")
                    return true;

                // Can check-in/out
                if (module == "Attendance" && (action == "CheckIn" || action == "CheckOut"))
                    return true;

                return false;
            }

            return false;
        }

        /// <summary>
        /// Clear session (logout)
        /// </summary>
        public static void Logout()
        {
            CurrentUser = null;
        }

        /// <summary>
        /// Login user
        /// </summary>
        public static void Login(User user)
        {
            CurrentUser = user;
        }
    }
}
