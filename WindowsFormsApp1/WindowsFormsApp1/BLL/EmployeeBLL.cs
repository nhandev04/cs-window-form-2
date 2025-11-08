using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WindowsFormsApp1.DAL;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Utils;

namespace WindowsFormsApp1.BLL
{
    /// <summary>
    /// Business Logic Layer for Employee operations
    /// Handles validation and business rules
    /// </summary>
    public class EmployeeBLL
    {
        private EmployeeDAL employeeDAL;

        public EmployeeBLL()
        {
            employeeDAL = new EmployeeDAL();
        }

        /// <summary>
        /// Validate employee data before saving
        /// </summary>
        public bool ValidateEmployee(Employee employee, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Validate Employee Code
            if (!string.IsNullOrWhiteSpace(employee.EmployeeCode))
            {
                if (employee.EmployeeCode.Length > 20)
                {
                    errorMessage = "Mã nhân viên không được quá 20 ký tự.";
                    return false;
                }
            }

            // Validate Full Name
            if (string.IsNullOrWhiteSpace(employee.FullName))
            {
                errorMessage = "Họ tên không được để trống.";
                return false;
            }

            if (employee.FullName.Length < 2 || employee.FullName.Length > 100)
            {
                errorMessage = "Họ tên phải có từ 2-100 ký tự.";
                return false;
            }

            // Validate Gender
            if (string.IsNullOrWhiteSpace(employee.Gender))
            {
                errorMessage = "Giới tính không được để trống.";
                return false;
            }

            // Validate Date of Birth
            if (employee.DateOfBirth == DateTime.MinValue)
            {
                errorMessage = "Ngày sinh không hợp lệ.";
                return false;
            }

            // Check if age is reasonable (between 18 and 100)
            int age = DateTime.Today.Year - employee.DateOfBirth.Year;
            if (employee.DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;

            if (age < 18)
            {
                errorMessage = "Nhân viên phải từ 18 tuổi trở lên.";
                return false;
            }

            if (age > 100)
            {
                errorMessage = "Ngày sinh không hợp lệ (tuổi > 100).";
                return false;
            }

            // Validate Email
            if (!string.IsNullOrWhiteSpace(employee.Email))
            {
                if (!IsValidEmail(employee.Email))
                {
                    errorMessage = "Email không đúng định dạng.";
                    return false;
                }
            }

            // Validate Phone Number
            if (!string.IsNullOrWhiteSpace(employee.PhoneNumber))
            {
                if (!IsValidPhoneNumber(employee.PhoneNumber))
                {
                    errorMessage = "Số điện thoại không hợp lệ (10-11 số).";
                    return false;
                }
            }

            // Validate Position
            if (string.IsNullOrWhiteSpace(employee.Position))
            {
                errorMessage = "Chức vụ không được để trống.";
                return false;
            }

            if (employee.Position.Length > 50)
            {
                errorMessage = "Chức vụ không được quá 50 ký tự.";
                return false;
            }

            // Validate Salary
            if (employee.Salary <= 0)
            {
                errorMessage = "Lương phải lớn hơn 0.";
                return false;
            }

            if (employee.Salary > 999999999.99m)
            {
                errorMessage = "Lương quá lớn.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        public List<Employee> GetAllEmployees()
        {
            try
            {
                return employeeDAL.GetAllEmployees();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách nhân viên: " + ex.Message);
            }
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        public Employee GetEmployeeById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID nhân viên không hợp lệ.");
            }

            try
            {
                return employeeDAL.GetEmployeeById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin nhân viên: " + ex.Message);
            }
        }

        /// <summary>
        /// Add new employee
        /// </summary>
        public bool AddEmployee(Employee employee, out string message)
        {
            // Validate employee data
            if (!ValidateEmployee(employee, out message))
                return false;

            // Check employee code uniqueness
            if (!string.IsNullOrWhiteSpace(employee.EmployeeCode))
            {
                if (employeeDAL.EmployeeCodeExists(employee.EmployeeCode))
                {
                    message = $"Mã nhân viên '{employee.EmployeeCode}' đã tồn tại.";
                    return false;
                }
            }

            try
            {
                // Set created by
                employee.CreatedBy = SessionManager.Username;
                employee.Status = string.IsNullOrWhiteSpace(employee.Status) ? "Active" : employee.Status;

                // Add to database
                int result = employeeDAL.AddEmployee(employee);

                if (result > 0)
                {
                    // Log audit trail
                    AuditHelper.Log("Employees", 0, "INSERT",
                                   $"Thêm nhân viên: {employee.FullName}");

                    message = "Thêm nhân viên thành công.";
                    return true;
                }
                else
                {
                    message = "Không thể thêm nhân viên.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi khi thêm nhân viên: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Update existing employee
        /// </summary>
        public bool UpdateEmployee(Employee employee, out string message)
        {
            if (employee.Id <= 0)
            {
                message = "ID nhân viên không hợp lệ.";
                return false;
            }

            // Validate employee data
            if (!ValidateEmployee(employee, out message))
                return false;

            // Check employee code uniqueness (exclude current employee)
            if (!string.IsNullOrWhiteSpace(employee.EmployeeCode))
            {
                if (employeeDAL.EmployeeCodeExists(employee.EmployeeCode, employee.Id))
                {
                    message = $"Mã nhân viên '{employee.EmployeeCode}' đã được sử dụng.";
                    return false;
                }
            }

            try
            {
                // Get old employee for audit trail
                Employee oldEmployee = employeeDAL.GetEmployeeById(employee.Id);

                if (oldEmployee == null)
                {
                    message = "Không tìm thấy nhân viên.";
                    return false;
                }

                // Set updated by
                employee.UpdatedBy = SessionManager.Username;

                // Update database
                int result = employeeDAL.UpdateEmployee(employee);

                if (result > 0)
                {
                    // Log changes to audit trail
                    AuditHelper.LogEmployeeChanges(oldEmployee, employee);

                    message = "Cập nhật nhân viên thành công.";
                    return true;
                }
                else
                {
                    message = "Không thể cập nhật nhân viên.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi khi cập nhật nhân viên: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Delete employee
        /// </summary>
        public bool DeleteEmployee(int id, out string message)
        {
            if (id <= 0)
            {
                message = "ID nhân viên không hợp lệ.";
                return false;
            }

            try
            {
                // Get employee info before delete for audit
                Employee employee = employeeDAL.GetEmployeeById(id);

                if (employee == null)
                {
                    message = "Không tìm thấy nhân viên.";
                    return false;
                }

                // Delete from database
                int result = employeeDAL.DeleteEmployee(id);

                if (result > 0)
                {
                    // Log audit trail
                    AuditHelper.Log("Employees", id, "DELETE",
                                   $"Xóa nhân viên: {employee.FullName}");

                    message = "Xóa nhân viên thành công.";
                    return true;
                }
                else
                {
                    message = "Không thể xóa nhân viên.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi khi xóa nhân viên: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Search employees by text
        /// </summary>
        public List<Employee> SearchEmployees(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return GetAllEmployees();
            }

            try
            {
                return employeeDAL.SearchEmployees(searchText);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm: " + ex.Message);
            }
        }

        /// <summary>
        /// Filter employees by criteria
        /// </summary>
        public List<Employee> FilterEmployees(string gender, string department, string position, string status = null)
        {
            try
            {
                return employeeDAL.FilterEmployees(gender, department, position, status);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lọc dữ liệu: " + ex.Message);
            }
        }

        /// <summary>
        /// Get list of departments
        /// </summary>
        public List<string> GetDepartments()
        {
            try
            {
                return employeeDAL.GetDepartments();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách phòng ban: " + ex.Message);
            }
        }

        /// <summary>
        /// Get list of positions
        /// </summary>
        public List<string> GetPositions()
        {
            try
            {
                return employeeDAL.GetPositions();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách chức vụ: " + ex.Message);
            }
        }

        // ============================================
        // VALIDATION HELPERS
        // ============================================

        /// <summary>
        /// Validate email format
        /// </summary>
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Simple email regex pattern
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validate phone number (Vietnam format)
        /// </summary>
        private bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Remove spaces, dashes, parentheses
            string cleaned = Regex.Replace(phone, @"[\s\-\(\)]", "");

            // Check if it's 10 or 11 digits
            if (cleaned.Length < 10 || cleaned.Length > 11)
                return false;

            // Check if all characters are digits
            return Regex.IsMatch(cleaned, @"^\d+$");
        }
    }
}
