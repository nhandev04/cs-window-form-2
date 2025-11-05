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
        /// Get all employees from database
        /// </summary>
        /// <returns>List of all employees</returns>
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = "SELECT Id, FullName, Gender, DateOfBirth, Position, Department, Salary, PhotoPath FROM Employees ORDER BY Id";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FullName = reader["FullName"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                            Position = reader["Position"].ToString(),
                            Department = reader["Department"].ToString(),
                            Salary = Convert.ToDecimal(reader["Salary"]),
                            PhotoPath = reader["PhotoPath"] != DBNull.Value ? reader["PhotoPath"].ToString() : null
                        });
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
                string query = "SELECT Id, FullName, Gender, DateOfBirth, Position, Department, Salary, PhotoPath FROM Employees WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FullName = reader["FullName"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                            Position = reader["Position"].ToString(),
                            Department = reader["Department"].ToString(),
                            Salary = Convert.ToDecimal(reader["Salary"]),
                            PhotoPath = reader["PhotoPath"] != DBNull.Value ? reader["PhotoPath"].ToString() : null
                        };
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
                string query = @"INSERT INTO Employees (FullName, Gender, DateOfBirth, Position, Department, Salary, PhotoPath)
                                VALUES (@FullName, @Gender, @DateOfBirth, @Position, @Department, @Salary, @PhotoPath)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@FullName", employee.FullName);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                cmd.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                cmd.Parameters.AddWithValue("@Position", employee.Position);
                cmd.Parameters.AddWithValue("@Department", employee.Department);
                cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                cmd.Parameters.AddWithValue("@PhotoPath", !string.IsNullOrEmpty(employee.PhotoPath) ? (object)employee.PhotoPath : DBNull.Value);

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
                                SET FullName = @FullName,
                                    Gender = @Gender,
                                    DateOfBirth = @DateOfBirth,
                                    Position = @Position,
                                    Department = @Department,
                                    Salary = @Salary,
                                    PhotoPath = @PhotoPath
                                WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", employee.Id);
                cmd.Parameters.AddWithValue("@FullName", employee.FullName);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                cmd.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                cmd.Parameters.AddWithValue("@Position", employee.Position);
                cmd.Parameters.AddWithValue("@Department", employee.Department);
                cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                cmd.Parameters.AddWithValue("@PhotoPath", !string.IsNullOrEmpty(employee.PhotoPath) ? (object)employee.PhotoPath : DBNull.Value);

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
        /// Search employees by name or department
        /// </summary>
        /// <param name="searchText">Search text</param>
        /// <returns>List of matching employees</returns>
        public List<Employee> SearchEmployees(string searchText)
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT Id, FullName, Gender, DateOfBirth, Position, Department, Salary, PhotoPath
                                FROM Employees
                                WHERE FullName LIKE @SearchText OR Department LIKE @SearchText
                                ORDER BY Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FullName = reader["FullName"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                            Position = reader["Position"].ToString(),
                            Department = reader["Department"].ToString(),
                            Salary = Convert.ToDecimal(reader["Salary"]),
                            PhotoPath = reader["PhotoPath"] != DBNull.Value ? reader["PhotoPath"].ToString() : null
                        });
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
        /// Filter employees by gender, department, or position
        /// </summary>
        /// <param name="gender">Gender filter (null or empty for no filter)</param>
        /// <param name="department">Department filter (null or empty for no filter)</param>
        /// <param name="position">Position filter (null or empty for no filter)</param>
        /// <returns>List of filtered employees</returns>
        public List<Employee> FilterEmployees(string gender = null, string department = null, string position = null)
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = "SELECT Id, FullName, Gender, DateOfBirth, Position, Department, Salary, PhotoPath FROM Employees WHERE 1=1";

                if (!string.IsNullOrEmpty(gender))
                    query += " AND Gender = @Gender";
                if (!string.IsNullOrEmpty(department))
                    query += " AND Department = @Department";
                if (!string.IsNullOrEmpty(position))
                    query += " AND Position = @Position";

                query += " ORDER BY Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(gender))
                    cmd.Parameters.AddWithValue("@Gender", gender);
                if (!string.IsNullOrEmpty(department))
                    cmd.Parameters.AddWithValue("@Department", department);
                if (!string.IsNullOrEmpty(position))
                    cmd.Parameters.AddWithValue("@Position", position);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FullName = reader["FullName"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                            Position = reader["Position"].ToString(),
                            Department = reader["Department"].ToString(),
                            Salary = Convert.ToDecimal(reader["Salary"]),
                            PhotoPath = reader["PhotoPath"] != DBNull.Value ? reader["PhotoPath"].ToString() : null
                        });
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
                string query = "SELECT DISTINCT Department FROM Employees ORDER BY Department";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        departments.Add(reader["Department"].ToString());
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
    }
}
