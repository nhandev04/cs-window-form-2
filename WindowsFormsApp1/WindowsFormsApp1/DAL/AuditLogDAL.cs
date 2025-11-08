using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.DAL
{
    /// <summary>
    /// Data Access Layer for AuditLog operations
    /// </summary>
    public class AuditLogDAL
    {
        /// <summary>
        /// Add audit log entry
        /// </summary>
        public int AddLog(string tableName, int recordId, string action,
                         string fieldName, string oldValue, string newValue,
                         string description, string performedBy,
                         string ipAddress = null, string machineName = null)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"INSERT INTO AuditLogs (TableName, RecordId, Action, FieldName,
                                                       OldValue, NewValue, Description,
                                                       PerformedBy, IPAddress, MachineName)
                                VALUES (@TableName, @RecordId, @Action, @FieldName,
                                       @OldValue, @NewValue, @Description,
                                       @PerformedBy, @IPAddress, @MachineName)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TableName", tableName);
                cmd.Parameters.AddWithValue("@RecordId", recordId);
                cmd.Parameters.AddWithValue("@Action", action);
                cmd.Parameters.AddWithValue("@FieldName", (object)fieldName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@OldValue", (object)oldValue ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NewValue", (object)newValue ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PerformedBy", performedBy);
                cmd.Parameters.AddWithValue("@IPAddress", (object)ipAddress ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MachineName", (object)machineName ?? DBNull.Value);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Don't throw - audit logging should not break the application
                    System.Diagnostics.Debug.WriteLine("Error logging audit: " + ex.Message);
                    return 0;
                }
            }
        }

        /// <summary>
        /// Get all audit logs
        /// </summary>
        public List<AuditLog> GetAllLogs()
        {
            return GetLogs(null, null, null, null, 1000); // Last 1000 logs
        }

        /// <summary>
        /// Get audit logs with filters
        /// </summary>
        public List<AuditLog> GetLogs(string tableName = null, string performedBy = null,
                                     DateTime? fromDate = null, DateTime? toDate = null,
                                     int limit = 1000)
        {
            List<AuditLog> logs = new List<AuditLog>();

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT TOP (@Limit) Id, TableName, RecordId, Action, FieldName,
                                       OldValue, NewValue, Description, PerformedBy,
                                       PerformedDate, IPAddress, MachineName
                                FROM AuditLogs
                                WHERE 1=1";

                if (!string.IsNullOrEmpty(tableName))
                    query += " AND TableName = @TableName";

                if (!string.IsNullOrEmpty(performedBy))
                    query += " AND PerformedBy LIKE @PerformedBy";

                if (fromDate.HasValue)
                    query += " AND PerformedDate >= @FromDate";

                if (toDate.HasValue)
                    query += " AND PerformedDate <= @ToDate";

                query += " ORDER BY PerformedDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Limit", limit);

                if (!string.IsNullOrEmpty(tableName))
                    cmd.Parameters.AddWithValue("@TableName", tableName);

                if (!string.IsNullOrEmpty(performedBy))
                    cmd.Parameters.AddWithValue("@PerformedBy", "%" + performedBy + "%");

                if (fromDate.HasValue)
                    cmd.Parameters.AddWithValue("@FromDate", fromDate.Value);

                if (toDate.HasValue)
                    cmd.Parameters.AddWithValue("@ToDate", toDate.Value.AddDays(1).AddSeconds(-1));

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        logs.Add(new AuditLog
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            TableName = reader["TableName"].ToString(),
                            RecordId = Convert.ToInt32(reader["RecordId"]),
                            Action = reader["Action"].ToString(),
                            FieldName = reader["FieldName"] != DBNull.Value ? reader["FieldName"].ToString() : null,
                            OldValue = reader["OldValue"] != DBNull.Value ? reader["OldValue"].ToString() : null,
                            NewValue = reader["NewValue"] != DBNull.Value ? reader["NewValue"].ToString() : null,
                            Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                            PerformedBy = reader["PerformedBy"].ToString(),
                            PerformedDate = Convert.ToDateTime(reader["PerformedDate"]),
                            IPAddress = reader["IPAddress"] != DBNull.Value ? reader["IPAddress"].ToString() : null,
                            MachineName = reader["MachineName"] != DBNull.Value ? reader["MachineName"].ToString() : null
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving audit logs: " + ex.Message);
                }
            }

            return logs;
        }

        /// <summary>
        /// Get logs for specific record
        /// </summary>
        public List<AuditLog> GetLogsForRecord(string tableName, int recordId)
        {
            List<AuditLog> logs = new List<AuditLog>();

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT Id, TableName, RecordId, Action, FieldName,
                                       OldValue, NewValue, Description, PerformedBy,
                                       PerformedDate, IPAddress, MachineName
                                FROM AuditLogs
                                WHERE TableName = @TableName AND RecordId = @RecordId
                                ORDER BY PerformedDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TableName", tableName);
                cmd.Parameters.AddWithValue("@RecordId", recordId);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        logs.Add(new AuditLog
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            TableName = reader["TableName"].ToString(),
                            RecordId = Convert.ToInt32(reader["RecordId"]),
                            Action = reader["Action"].ToString(),
                            FieldName = reader["FieldName"] != DBNull.Value ? reader["FieldName"].ToString() : null,
                            OldValue = reader["OldValue"] != DBNull.Value ? reader["OldValue"].ToString() : null,
                            NewValue = reader["NewValue"] != DBNull.Value ? reader["NewValue"].ToString() : null,
                            Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                            PerformedBy = reader["PerformedBy"].ToString(),
                            PerformedDate = Convert.ToDateTime(reader["PerformedDate"]),
                            IPAddress = reader["IPAddress"] != DBNull.Value ? reader["IPAddress"].ToString() : null,
                            MachineName = reader["MachineName"] != DBNull.Value ? reader["MachineName"].ToString() : null
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving audit logs: " + ex.Message);
                }
            }

            return logs;
        }

        /// <summary>
        /// Delete old logs (cleanup)
        /// </summary>
        public int DeleteOldLogs(int daysToKeep = 365)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = "DELETE FROM AuditLogs WHERE PerformedDate < DATEADD(DAY, -@Days, GETDATE())";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Days", daysToKeep);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error deleting old logs: " + ex.Message);
                }
            }
        }
    }
}
