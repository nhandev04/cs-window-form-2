using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.DAL
{
    /// <summary>
    /// Data Access Layer for Department operations
    /// </summary>
    public class DepartmentDAL
    {
        /// <summary>
        /// Get all departments
        /// </summary>
        public List<Department> GetAllDepartments()
        {
            List<Department> departments = new List<Department>();

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT d.Id, d.DepartmentCode, d.DepartmentName, d.ManagerId,
                                       e.FullName as ManagerName, d.Description, d.EstablishedDate,
                                       d.PhoneNumber, d.Email, d.Location, d.IsActive,
                                       d.CreatedDate, d.CreatedBy, d.UpdatedDate, d.UpdatedBy,
                                       (SELECT COUNT(*) FROM Employees WHERE DepartmentId = d.Id AND Status = 'Active') as EmployeeCount
                                FROM Departments d
                                LEFT JOIN Employees e ON d.ManagerId = e.Id
                                ORDER BY d.DepartmentCode";

                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        departments.Add(new Department
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            DepartmentCode = reader["DepartmentCode"].ToString(),
                            DepartmentName = reader["DepartmentName"].ToString(),
                            ManagerId = reader["ManagerId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["ManagerId"]) : null,
                            ManagerName = reader["ManagerName"] != DBNull.Value ? reader["ManagerName"].ToString() : null,
                            Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                            EstablishedDate = reader["EstablishedDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["EstablishedDate"]) : null,
                            PhoneNumber = reader["PhoneNumber"] != DBNull.Value ? reader["PhoneNumber"].ToString() : null,
                            Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                            Location = reader["Location"] != DBNull.Value ? reader["Location"].ToString() : null,
                            IsActive = Convert.ToBoolean(reader["IsActive"]),
                            EmployeeCount = Convert.ToInt32(reader["EmployeeCount"]),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : null,
                            UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["UpdatedDate"]) : null,
                            UpdatedBy = reader["UpdatedBy"] != DBNull.Value ? reader["UpdatedBy"].ToString() : null
                        });
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
        /// Get active departments only
        /// </summary>
        public List<Department> GetActiveDepartments()
        {
            List<Department> departments = new List<Department>();

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT Id, DepartmentCode, DepartmentName
                                FROM Departments
                                WHERE IsActive = 1
                                ORDER BY DepartmentName";

                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        departments.Add(new Department
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            DepartmentCode = reader["DepartmentCode"].ToString(),
                            DepartmentName = reader["DepartmentName"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving active departments: " + ex.Message);
                }
            }

            return departments;
        }

        /// <summary>
        /// Get department by ID
        /// </summary>
        public Department GetDepartmentById(int id)
        {
            Department department = null;

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT d.Id, d.DepartmentCode, d.DepartmentName, d.ManagerId,
                                       e.FullName as ManagerName, d.Description, d.EstablishedDate,
                                       d.PhoneNumber, d.Email, d.Location, d.IsActive,
                                       d.CreatedDate, d.CreatedBy, d.UpdatedDate, d.UpdatedBy
                                FROM Departments d
                                LEFT JOIN Employees e ON d.ManagerId = e.Id
                                WHERE d.Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        department = new Department
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            DepartmentCode = reader["DepartmentCode"].ToString(),
                            DepartmentName = reader["DepartmentName"].ToString(),
                            ManagerId = reader["ManagerId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["ManagerId"]) : null,
                            ManagerName = reader["ManagerName"] != DBNull.Value ? reader["ManagerName"].ToString() : null,
                            Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                            EstablishedDate = reader["EstablishedDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["EstablishedDate"]) : null,
                            PhoneNumber = reader["PhoneNumber"] != DBNull.Value ? reader["PhoneNumber"].ToString() : null,
                            Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                            Location = reader["Location"] != DBNull.Value ? reader["Location"].ToString() : null,
                            IsActive = Convert.ToBoolean(reader["IsActive"]),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : null,
                            UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["UpdatedDate"]) : null,
                            UpdatedBy = reader["UpdatedBy"] != DBNull.Value ? reader["UpdatedBy"].ToString() : null
                        };
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving department: " + ex.Message);
                }
            }

            return department;
        }

        /// <summary>
        /// Check if department name exists
        /// </summary>
        public bool DepartmentNameExists(string departmentName, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                return false;

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = "SELECT COUNT(*) FROM Departments WHERE DepartmentName = @DepartmentName";
                if (excludeId.HasValue)
                    query += " AND Id != @ExcludeId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DepartmentName", departmentName);
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
                    throw new Exception("Error checking department name: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Add new department
        /// </summary>
        public int AddDepartment(Department department)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                // Generate DepartmentCode if not provided
                if (string.IsNullOrWhiteSpace(department.DepartmentCode))
                {
                    department.DepartmentCode = GenerateNextDepartmentCode(conn);
                }

                string query = @"INSERT INTO Departments (
                                    DepartmentCode, DepartmentName, ManagerId, Description,
                                    EstablishedDate, PhoneNumber, Email, Location, IsActive,
                                    CreatedBy
                                ) VALUES (
                                    @DepartmentCode, @DepartmentName, @ManagerId, @Description,
                                    @EstablishedDate, @PhoneNumber, @Email, @Location, @IsActive,
                                    @CreatedBy
                                )";

                SqlCommand cmd = new SqlCommand(query, conn);
                AddDepartmentParameters(cmd, department);
                cmd.Parameters.AddWithValue("@CreatedBy", (object)department.CreatedBy ?? DBNull.Value);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error adding department: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Update existing department
        /// </summary>
        public int UpdateDepartment(Department department)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                // Generate DepartmentCode if not provided
                if (string.IsNullOrWhiteSpace(department.DepartmentCode))
                {
                    department.DepartmentCode = GenerateNextDepartmentCode(conn);
                }

                string query = @"UPDATE Departments
                                SET DepartmentCode = @DepartmentCode,
                                    DepartmentName = @DepartmentName,
                                    ManagerId = @ManagerId,
                                    Description = @Description,
                                    EstablishedDate = @EstablishedDate,
                                    PhoneNumber = @PhoneNumber,
                                    Email = @Email,
                                    Location = @Location,
                                    IsActive = @IsActive,
                                    UpdatedDate = GETDATE(),
                                    UpdatedBy = @UpdatedBy
                                WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", department.Id);
                AddDepartmentParameters(cmd, department);
                cmd.Parameters.AddWithValue("@UpdatedBy", (object)department.UpdatedBy ?? DBNull.Value);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error updating department: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Delete department
        /// </summary>
        public int DeleteDepartment(int id)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = "DELETE FROM Departments WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error deleting department: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Generate next department code automatically
        /// </summary>
        private string GenerateNextDepartmentCode(SqlConnection conn)
        {
            try
            {
                string query = "SELECT TOP 1 DepartmentCode FROM Departments WHERE DepartmentCode LIKE 'DEPT%' ORDER BY DepartmentCode DESC";
                SqlCommand cmd = new SqlCommand(query, conn);

                bool shouldClose = conn.State == System.Data.ConnectionState.Closed;
                if (shouldClose) conn.Open();

                object result = cmd.ExecuteScalar();

                if (shouldClose) conn.Close();

                if (result != null)
                {
                    string lastCode = result.ToString();
                    if (lastCode.StartsWith("DEPT") && lastCode.Length > 4)
                    {
                        string numberPart = lastCode.Substring(4);
                        if (int.TryParse(numberPart, out int lastNumber))
                        {
                            return $"DEPT{(lastNumber + 1):D3}";
                        }
                    }
                }

                // Default starting code
                return "DEPT001";
            }
            catch
            {
                // Fallback: use timestamp-based code
                return $"DEPT{DateTime.Now:yyyyMMddHHmmss}".Substring(0, 10);
            }
        }

        /// <summary>
        /// Add common department parameters to SqlCommand
        /// </summary>
        private void AddDepartmentParameters(SqlCommand cmd, Department department)
        {
            cmd.Parameters.AddWithValue("@DepartmentCode", department.DepartmentCode ?? "");
            cmd.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
            cmd.Parameters.AddWithValue("@ManagerId", (object)department.ManagerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", (object)department.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EstablishedDate", (object)department.EstablishedDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PhoneNumber", (object)department.PhoneNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object)department.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Location", (object)department.Location ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsActive", department.IsActive);
        }
    }
}
