using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.DAL
{
    /// <summary>
    /// Data Access Layer for Employee operations
    /// Handles all database interactions for Employee entity
    /// </summary>
    public class EmployeeDAL
    {
        /// <summary>
        /// Get all employees from database with Department info
        /// </summary>
        /// <returns>List of all employees</returns>
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT e.Id, e.EmployeeCode, e.FullName, e.Gender, e.DateOfBirth,
                                       e.Email, e.PhoneNumber, e.Address, e.Position,
                                       e.DepartmentId, d.DepartmentName, e.Department as LegacyDepartment,
                                       e.Salary, e.HireDate, e.Status, e.PhotoPath, e.Notes,
                                       e.CreatedDate, e.CreatedBy, e.UpdatedDate, e.UpdatedBy
                                FROM Employees e
                                LEFT JOIN Departments d ON e.DepartmentId = d.Id
                                ORDER BY e.Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        employees.Add(MapReaderToEmployee(reader));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving employees: " + ex.Message);
                }
            }

            return employees;
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee object or null if not found</returns>
        public Employee GetEmployeeById(int id)
        {
            Employee employee = null;

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT e.Id, e.EmployeeCode, e.FullName, e.Gender, e.DateOfBirth,
                                       e.Email, e.PhoneNumber, e.Address, e.Position,
                                       e.DepartmentId, d.DepartmentName, e.Department as LegacyDepartment,
                                       e.Salary, e.HireDate, e.Status, e.PhotoPath, e.Notes,
                                       e.CreatedDate, e.CreatedBy, e.UpdatedDate, e.UpdatedBy
                                FROM Employees e
                                LEFT JOIN Departments d ON e.DepartmentId = d.Id
                                WHERE e.Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        employee = MapReaderToEmployee(reader);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving employee: " + ex.Message);
                }
            }

            return employee;
        }

        /// <summary>
        /// Add new employee to database
        /// </summary>
        /// <param name="employee">Employee object to add</param>
        /// <returns>Number of rows affected</returns>
        public int AddEmployee(Employee employee)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"INSERT INTO Employees (
                                    EmployeeCode, FullName, Gender, DateOfBirth,
                                    Email, PhoneNumber, Address, Position,
                                    DepartmentId, Department, Salary, HireDate, Status,
                                    PhotoPath, Notes, CreatedBy
                                ) VALUES (
                                    @EmployeeCode, @FullName, @Gender, @DateOfBirth,
                                    @Email, @PhoneNumber, @Address, @Position,
                                    @DepartmentId, @Department, @Salary, @HireDate, @Status,
                                    @PhotoPath, @Notes, @CreatedBy
                                )";

                SqlCommand cmd = new SqlCommand(query, conn);
                AddEmployeeParameters(cmd, employee);
                cmd.Parameters.AddWithValue("@CreatedBy", (object)employee.CreatedBy ?? DBNull.Value);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error adding employee: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Update existing employee in database
        /// </summary>
        /// <param name="employee">Employee object with updated data</param>
        /// <returns>Number of rows affected</returns>
        public int UpdateEmployee(Employee employee)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"UPDATE Employees
                                SET EmployeeCode = @EmployeeCode,
                                    FullName = @FullName,
                                    Gender = @Gender,
                                    DateOfBirth = @DateOfBirth,
                                    Email = @Email,
                                    PhoneNumber = @PhoneNumber,
                                    Address = @Address,
                                    Position = @Position,
                                    DepartmentId = @DepartmentId,
                                    Department = @Department,
                                    Salary = @Salary,
                                    HireDate = @HireDate,
                                    Status = @Status,
                                    PhotoPath = @PhotoPath,
                                    Notes = @Notes,
                                    UpdatedDate = GETDATE(),
                                    UpdatedBy = @UpdatedBy
                                WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", employee.Id);
                AddEmployeeParameters(cmd, employee);
                cmd.Parameters.AddWithValue("@UpdatedBy", (object)employee.UpdatedBy ?? DBNull.Value);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error updating employee: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Delete employee from database
        /// </summary>
        /// <param name="id">Employee ID to delete</param>
        /// <returns>Number of rows affected</returns>
        public int DeleteEmployee(int id)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = "DELETE FROM Employees WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error deleting employee: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Search employees by name, email, phone, or department
        /// </summary>
        /// <param name="searchText">Search text</param>
        /// <returns>List of matching employees</returns>
        public List<Employee> SearchEmployees(string searchText)
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT e.Id, e.EmployeeCode, e.FullName, e.Gender, e.DateOfBirth,
                                       e.Email, e.PhoneNumber, e.Address, e.Position,
                                       e.DepartmentId, d.DepartmentName, e.Department as LegacyDepartment,
                                       e.Salary, e.HireDate, e.Status, e.PhotoPath, e.Notes,
                                       e.CreatedDate, e.CreatedBy, e.UpdatedDate, e.UpdatedBy
                                FROM Employees e
                                LEFT JOIN Departments d ON e.DepartmentId = d.Id
                                WHERE e.FullName LIKE @SearchText
                                   OR e.Email LIKE @SearchText
                                   OR e.PhoneNumber LIKE @SearchText
                                   OR e.Department LIKE @SearchText
                                   OR d.DepartmentName LIKE @SearchText
                                   OR e.Position LIKE @SearchText
                                   OR e.EmployeeCode LIKE @SearchText
                                ORDER BY e.Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        employees.Add(MapReaderToEmployee(reader));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error searching employees: " + ex.Message);
                }
            }

            return employees;
        }

        /// <summary>
        /// Filter employees by gender, department, position, or status
        /// </summary>
        /// <param name="gender">Gender filter (null or empty for no filter)</param>
        /// <param name="department">Department filter (null or empty for no filter)</param>
        /// <param name="position">Position filter (null or empty for no filter)</param>
        /// <param name="status">Status filter (null or empty for no filter)</param>
        /// <returns>List of filtered employees</returns>
        public List<Employee> FilterEmployees(string gender = null, string department = null, string position = null, string status = null)
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT e.Id, e.EmployeeCode, e.FullName, e.Gender, e.DateOfBirth,
                                       e.Email, e.PhoneNumber, e.Address, e.Position,
                                       e.DepartmentId, d.DepartmentName, e.Department as LegacyDepartment,
                                       e.Salary, e.HireDate, e.Status, e.PhotoPath, e.Notes,
                                       e.CreatedDate, e.CreatedBy, e.UpdatedDate, e.UpdatedBy
                                FROM Employees e
                                LEFT JOIN Departments d ON e.DepartmentId = d.Id
                                WHERE 1=1";

                if (!string.IsNullOrEmpty(gender))
                    query += " AND e.Gender = @Gender";
                if (!string.IsNullOrEmpty(department))
                    query += " AND (e.Department = @Department OR d.DepartmentName = @Department)";
                if (!string.IsNullOrEmpty(position))
                    query += " AND e.Position = @Position";
                if (!string.IsNullOrEmpty(status))
                    query += " AND e.Status = @Status";

                query += " ORDER BY e.Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(gender))
                    cmd.Parameters.AddWithValue("@Gender", gender);
                if (!string.IsNullOrEmpty(department))
                    cmd.Parameters.AddWithValue("@Department", department);
                if (!string.IsNullOrEmpty(position))
                    cmd.Parameters.AddWithValue("@Position", position);
                if (!string.IsNullOrEmpty(status))
                    cmd.Parameters.AddWithValue("@Status", status);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        employees.Add(MapReaderToEmployee(reader));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error filtering employees: " + ex.Message);
                }
            }

            return employees;
        }

        /// <summary>
        /// Get distinct departments for filter dropdown
        /// </summary>
        /// <returns>List of department names</returns>
        public List<string> GetDepartments()
        {
            List<string> departments = new List<string>();

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT DISTINCT ISNULL(d.DepartmentName, e.Department) as DeptName
                                FROM Employees e
                                LEFT JOIN Departments d ON e.DepartmentId = d.Id
                                WHERE ISNULL(d.DepartmentName, e.Department) IS NOT NULL
                                ORDER BY DeptName";

                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        departments.Add(reader["DeptName"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving departments: " + ex.Message);
                }
            }

            return departments;
        }

        /// <summary>
        /// Get distinct positions for filter dropdown
        /// </summary>
        /// <returns>List of position names</returns>
        public List<string> GetPositions()
        {
            List<string> positions = new List<string>();

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = "SELECT DISTINCT Position FROM Employees ORDER BY Position";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        positions.Add(reader["Position"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving positions: " + ex.Message);
                }
            }

            return positions;
        }

        /// <summary>
        /// Check if employee code exists
        /// </summary>
        public bool EmployeeCodeExists(string employeeCode, int? excludeId = null)
        {
            if (string.IsNullOrEmpty(employeeCode))
                return false;

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = "SELECT COUNT(*) FROM Employees WHERE EmployeeCode = @EmployeeCode";
                if (excludeId.HasValue)
                    query += " AND Id != @ExcludeId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmployeeCode", employeeCode);
                if (excludeId.HasValue)
                    cmd.Parameters.AddWithValue("@ExcludeId", excludeId.Value);

                try
                {
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error checking employee code: " + ex.Message);
                }
            }
        }

        // ============================================
        // HELPER METHODS
        // ============================================

        /// <summary>
        /// Map SqlDataReader to Employee object
        /// </summary>
        private Employee MapReaderToEmployee(SqlDataReader reader)
        {
            return new Employee
            {
                Id = Convert.ToInt32(reader["Id"]),
                EmployeeCode = reader["EmployeeCode"] != DBNull.Value ? reader["EmployeeCode"].ToString() : null,
                FullName = reader["FullName"].ToString(),
                Gender = reader["Gender"].ToString(),
                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                PhoneNumber = reader["PhoneNumber"] != DBNull.Value ? reader["PhoneNumber"].ToString() : null,
                Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null,
                Position = reader["Position"].ToString(),
                DepartmentId = reader["DepartmentId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["DepartmentId"]) : null,
                DepartmentName = reader["DepartmentName"] != DBNull.Value ? reader["DepartmentName"].ToString() : null,
                Department = reader["LegacyDepartment"] != DBNull.Value ? reader["LegacyDepartment"].ToString() : null,
                Salary = Convert.ToDecimal(reader["Salary"]),
                HireDate = reader["HireDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["HireDate"]) : null,
                Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : "Active",
                PhotoPath = reader["PhotoPath"] != DBNull.Value ? reader["PhotoPath"].ToString() : null,
                Notes = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() : null,
                CreatedDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : DateTime.Now,
                CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : null,
                UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["UpdatedDate"]) : null,
                UpdatedBy = reader["UpdatedBy"] != DBNull.Value ? reader["UpdatedBy"].ToString() : null
            };
        }

        /// <summary>
        /// Add common employee parameters to SqlCommand
        /// </summary>
        private void AddEmployeeParameters(SqlCommand cmd, Employee employee)
        {
            cmd.Parameters.AddWithValue("@EmployeeCode", (object)employee.EmployeeCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FullName", employee.FullName);
            cmd.Parameters.AddWithValue("@Gender", employee.Gender);
            cmd.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
            cmd.Parameters.AddWithValue("@Email", (object)employee.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PhoneNumber", (object)employee.PhoneNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", (object)employee.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Position", employee.Position);
            cmd.Parameters.AddWithValue("@DepartmentId", (object)employee.DepartmentId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Department", (object)employee.Department ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Salary", employee.Salary);
            cmd.Parameters.AddWithValue("@HireDate", (object)employee.HireDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", (object)employee.Status ?? "Active");
            cmd.Parameters.AddWithValue("@PhotoPath", (object)employee.PhotoPath ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Notes", (object)employee.Notes ?? DBNull.Value);
        }
    }
}
