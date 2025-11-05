using System;
using System.Collections.Generic;
using WindowsFormsApp1.DAL;
using WindowsFormsApp1.Models;

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
        /// <param name="employee">Employee to validate</param>
        /// <param name="errorMessage">Error message if validation fails</param>
        /// <returns>True if valid, false otherwise</returns>
        public bool ValidateEmployee(Employee employee, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Validate Full Name
            if (string.IsNullOrWhiteSpace(employee.FullName))
            {
                errorMessage = "Full Name is required.";
                return false;
            }

            if (employee.FullName.Length < 2 || employee.FullName.Length > 100)
            {
                errorMessage = "Full Name must be between 2 and 100 characters.";
                return false;
            }

            // Validate Gender
            if (string.IsNullOrWhiteSpace(employee.Gender))
            {
                errorMessage = "Gender is required.";
                return false;
            }

            // Validate Date of Birth
            if (employee.DateOfBirth == DateTime.MinValue)
            {
                errorMessage = "Date of Birth is required.";
                return false;
            }

            // Check if age is reasonable (between 18 and 100)
            int age = DateTime.Today.Year - employee.DateOfBirth.Year;
            if (employee.DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;

            if (age < 18)
            {
                errorMessage = "Employee must be at least 18 years old.";
                return false;
            }

            if (age > 100)
            {
                errorMessage = "Invalid date of birth. Age cannot exceed 100 years.";
                return false;
            }

            // Validate Position
            if (string.IsNullOrWhiteSpace(employee.Position))
            {
                errorMessage = "Position is required.";
                return false;
            }

            if (employee.Position.Length > 50)
            {
                errorMessage = "Position must not exceed 50 characters.";
                return false;
            }

            // Validate Department
            if (string.IsNullOrWhiteSpace(employee.Department))
            {
                errorMessage = "Department is required.";
                return false;
            }

            if (employee.Department.Length > 50)
            {
                errorMessage = "Department must not exceed 50 characters.";
                return false;
            }

            // Validate Salary
            if (employee.Salary <= 0)
            {
                errorMessage = "Salary must be greater than 0.";
                return false;
            }

            if (employee.Salary > 999999999.99m)
            {
                errorMessage = "Salary value is too large.";
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
                throw new Exception("Business layer error: " + ex.Message);
            }
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        public Employee GetEmployeeById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid employee ID.");
            }

            try
            {
                return employeeDAL.GetEmployeeById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Business layer error: " + ex.Message);
            }
        }

        /// <summary>
        /// Add new employee
        /// </summary>
        public bool AddEmployee(Employee employee, out string message)
        {
            // Validate employee data
            if (!ValidateEmployee(employee, out message))
            {
                return false;
            }

            try
            {
                int result = employeeDAL.AddEmployee(employee);
                if (result > 0)
                {
                    message = "Employee added successfully.";
                    return true;
                }
                else
                {
                    message = "Failed to add employee.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Error adding employee: " + ex.Message;
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
                message = "Invalid employee ID.";
                return false;
            }

            // Validate employee data
            if (!ValidateEmployee(employee, out message))
            {
                return false;
            }

            try
            {
                int result = employeeDAL.UpdateEmployee(employee);
                if (result > 0)
                {
                    message = "Employee updated successfully.";
                    return true;
                }
                else
                {
                    message = "Failed to update employee. Employee may not exist.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Error updating employee: " + ex.Message;
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
                message = "Invalid employee ID.";
                return false;
            }

            try
            {
                int result = employeeDAL.DeleteEmployee(id);
                if (result > 0)
                {
                    message = "Employee deleted successfully.";
                    return true;
                }
                else
                {
                    message = "Failed to delete employee. Employee may not exist.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Error deleting employee: " + ex.Message;
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
                throw new Exception("Business layer error: " + ex.Message);
            }
        }

        /// <summary>
        /// Filter employees by criteria
        /// </summary>
        public List<Employee> FilterEmployees(string gender, string department, string position)
        {
            try
            {
                return employeeDAL.FilterEmployees(gender, department, position);
            }
            catch (Exception ex)
            {
                throw new Exception("Business layer error: " + ex.Message);
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
                throw new Exception("Business layer error: " + ex.Message);
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
                throw new Exception("Business layer error: " + ex.Message);
            }
        }
    }
}
