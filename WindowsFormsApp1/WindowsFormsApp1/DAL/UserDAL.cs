using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.DAL
{
    /// <summary>
    /// Data Access Layer for User operations
    /// </summary>
    public class UserDAL
    {
        /// <summary>
        /// Authenticate user login
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="passwordHash">Hashed password</param>
        /// <returns>User object if found, null otherwise</returns>
        public User AuthenticateUser(string username, string passwordHash)
        {
            User user = null;

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT u.Id, u.Username, u.PasswordHash, u.FullName, u.Email,
                                       u.RoleId, r.RoleName, u.EmployeeId, u.IsActive, u.LastLogin,
                                       u.CreatedDate, u.CreatedBy
                                FROM Users u
                                INNER JOIN Roles r ON u.RoleId = r.Id
                                WHERE u.Username = @Username AND u.PasswordHash = @PasswordHash";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Username = reader["Username"].ToString(),
                            PasswordHash = reader["PasswordHash"].ToString(),
                            FullName = reader["FullName"].ToString(),
                            Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            RoleName = reader["RoleName"].ToString(),
                            EmployeeId = reader["EmployeeId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["EmployeeId"]) : null,
                            IsActive = Convert.ToBoolean(reader["IsActive"]),
                            LastLogin = reader["LastLogin"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["LastLogin"]) : null,
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : null
                        };
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error authenticating user: " + ex.Message);
                }
            }

            return user;
        }

        /// <summary>
        /// Update last login time
        /// </summary>
        public void UpdateLastLogin(int userId)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = "UPDATE Users SET LastLogin = GETDATE() WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", userId);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error updating last login: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Get all users
        /// </summary>
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT u.Id, u.Username, u.PasswordHash, u.FullName, u.Email,
                                       u.RoleId, r.RoleName, u.EmployeeId, u.IsActive, u.LastLogin,
                                       u.CreatedDate, u.CreatedBy
                                FROM Users u
                                INNER JOIN Roles r ON u.RoleId = r.Id
                                ORDER BY u.Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Username = reader["Username"].ToString(),
                            PasswordHash = reader["PasswordHash"].ToString(),
                            FullName = reader["FullName"].ToString(),
                            Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            RoleName = reader["RoleName"].ToString(),
                            EmployeeId = reader["EmployeeId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["EmployeeId"]) : null,
                            IsActive = Convert.ToBoolean(reader["IsActive"]),
                            LastLogin = reader["LastLogin"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["LastLogin"]) : null,
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : null
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving users: " + ex.Message);
                }
            }

            return users;
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        public List<Role> GetAllRoles()
        {
            List<Role> roles = new List<Role>();

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = "SELECT Id, RoleName, Description, CreatedDate FROM Roles ORDER BY Id";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        roles.Add(new Role
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            RoleName = reader["RoleName"].ToString(),
                            Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving roles: " + ex.Message);
                }
            }

            return roles;
        }

        /// <summary>
        /// Add new user
        /// </summary>
        public int AddUser(User user)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"INSERT INTO Users (Username, PasswordHash, FullName, Email, RoleId, EmployeeId, IsActive, CreatedBy)
                                VALUES (@Username, @PasswordHash, @FullName, @Email, @RoleId, @EmployeeId, @IsActive, @CreatedBy)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@Email", (object)user.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@RoleId", user.RoleId);
                cmd.Parameters.AddWithValue("@EmployeeId", (object)user.EmployeeId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
                cmd.Parameters.AddWithValue("@CreatedBy", (object)user.CreatedBy ?? DBNull.Value);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error adding user: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Update user
        /// </summary>
        public int UpdateUser(User user)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"UPDATE Users
                                SET Username = @Username,
                                    FullName = @FullName,
                                    Email = @Email,
                                    RoleId = @RoleId,
                                    EmployeeId = @EmployeeId,
                                    IsActive = @IsActive
                                WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", user.Id);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@Email", (object)user.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@RoleId", user.RoleId);
                cmd.Parameters.AddWithValue("@EmployeeId", (object)user.EmployeeId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", user.IsActive);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error updating user: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Change password
        /// </summary>
        public int ChangePassword(int userId, string newPasswordHash)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = "UPDATE Users SET PasswordHash = @PasswordHash WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", userId);
                cmd.Parameters.AddWithValue("@PasswordHash", newPasswordHash);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error changing password: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Delete user
        /// </summary>
        public int DeleteUser(int userId)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = "DELETE FROM Users WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", userId);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error deleting user: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Check if username exists
        /// </summary>
        public bool UsernameExists(string username, int? excludeUserId = null)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                if (excludeUserId.HasValue)
                    query += " AND Id != @ExcludeId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                if (excludeUserId.HasValue)
                    cmd.Parameters.AddWithValue("@ExcludeId", excludeUserId.Value);

                try
                {
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error checking username: " + ex.Message);
                }
            }
        }
    }
}
