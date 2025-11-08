using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.DAL
{
    public class AttendanceDAL
    {
        public List<Attendance> GetAllAttendance()
        {
            List<Attendance> attendances = new List<Attendance>();

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT a.Id, a.EmployeeId, e.FullName as EmployeeName, e.EmployeeCode,
                                       a.AttendanceDate, a.CheckInTime, a.CheckOutTime, a.WorkingHours,
                                       a.Status, a.Notes, a.CreatedDate, a.CreatedBy
                                FROM Attendance a
                                INNER JOIN Employees e ON a.EmployeeId = e.Id
                                ORDER BY a.AttendanceDate DESC, a.CheckInTime DESC";

                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        attendances.Add(MapReaderToAttendance(reader));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving attendance: " + ex.Message);
                }
            }

            return attendances;
        }

        public List<Attendance> GetAttendanceByEmployee(int employeeId)
        {
            List<Attendance> attendances = new List<Attendance>();

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT a.Id, a.EmployeeId, e.FullName as EmployeeName, e.EmployeeCode,
                                       a.AttendanceDate, a.CheckInTime, a.CheckOutTime, a.WorkingHours,
                                       a.Status, a.Notes, a.CreatedDate, a.CreatedBy
                                FROM Attendance a
                                INNER JOIN Employees e ON a.EmployeeId = e.Id
                                WHERE a.EmployeeId = @EmployeeId
                                ORDER BY a.AttendanceDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        attendances.Add(MapReaderToAttendance(reader));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving attendance: " + ex.Message);
                }
            }

            return attendances;
        }

        public List<Attendance> GetAttendanceByDateRange(DateTime fromDate, DateTime toDate)
        {
            List<Attendance> attendances = new List<Attendance>();

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT a.Id, a.EmployeeId, e.FullName as EmployeeName, e.EmployeeCode,
                                       a.AttendanceDate, a.CheckInTime, a.CheckOutTime, a.WorkingHours,
                                       a.Status, a.Notes, a.CreatedDate, a.CreatedBy
                                FROM Attendance a
                                INNER JOIN Employees e ON a.EmployeeId = e.Id
                                WHERE a.AttendanceDate >= @FromDate AND a.AttendanceDate <= @ToDate
                                ORDER BY a.AttendanceDate DESC, a.CheckInTime DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                cmd.Parameters.AddWithValue("@ToDate", toDate.Date);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        attendances.Add(MapReaderToAttendance(reader));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving attendance: " + ex.Message);
                }
            }

            return attendances;
        }

        public Attendance GetAttendanceById(int id)
        {
            Attendance attendance = null;

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT a.Id, a.EmployeeId, e.FullName as EmployeeName, e.EmployeeCode,
                                       a.AttendanceDate, a.CheckInTime, a.CheckOutTime, a.WorkingHours,
                                       a.Status, a.Notes, a.CreatedDate, a.CreatedBy
                                FROM Attendance a
                                INNER JOIN Employees e ON a.EmployeeId = e.Id
                                WHERE a.Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        attendance = MapReaderToAttendance(reader);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving attendance: " + ex.Message);
                }
            }

            return attendance;
        }

        public Attendance GetTodayAttendance(int employeeId)
        {
            Attendance attendance = null;

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"SELECT TOP 1 a.Id, a.EmployeeId, e.FullName as EmployeeName, e.EmployeeCode,
                                       a.AttendanceDate, a.CheckInTime, a.CheckOutTime, a.WorkingHours,
                                       a.Status, a.Notes, a.CreatedDate, a.CreatedBy
                                FROM Attendance a
                                INNER JOIN Employees e ON a.EmployeeId = e.Id
                                WHERE a.EmployeeId = @EmployeeId AND a.AttendanceDate = @Today
                                ORDER BY a.CheckInTime DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                cmd.Parameters.AddWithValue("@Today", DateTime.Today);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        attendance = MapReaderToAttendance(reader);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving today's attendance: " + ex.Message);
                }
            }

            return attendance;
        }

        public int CheckIn(Attendance attendance)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"INSERT INTO Attendance (EmployeeId, AttendanceDate, CheckInTime, Status, Notes, CreatedBy)
                                VALUES (@EmployeeId, @AttendanceDate, @CheckInTime, @Status, @Notes, @CreatedBy);
                                SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmployeeId", attendance.EmployeeId);
                cmd.Parameters.AddWithValue("@AttendanceDate", attendance.AttendanceDate);
                cmd.Parameters.AddWithValue("@CheckInTime", attendance.CheckInTime);
                cmd.Parameters.AddWithValue("@Status", attendance.Status ?? "Present");
                cmd.Parameters.AddWithValue("@Notes", (object)attendance.Notes ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", (object)attendance.CreatedBy ?? DBNull.Value);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error checking in: " + ex.Message);
                }
            }
        }

        public int CheckOut(int attendanceId, DateTime checkOutTime)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"UPDATE Attendance
                                SET CheckOutTime = @CheckOutTime,
                                    WorkingHours = DATEDIFF(MINUTE, CheckInTime, @CheckOutTime) / 60.0
                                WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", attendanceId);
                cmd.Parameters.AddWithValue("@CheckOutTime", checkOutTime);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error checking out: " + ex.Message);
                }
            }
        }

        public int UpdateAttendance(Attendance attendance)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = @"UPDATE Attendance
                                SET AttendanceDate = @AttendanceDate,
                                    CheckInTime = @CheckInTime,
                                    CheckOutTime = @CheckOutTime,
                                    WorkingHours = @WorkingHours,
                                    Status = @Status,
                                    Notes = @Notes
                                WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", attendance.Id);
                cmd.Parameters.AddWithValue("@AttendanceDate", attendance.AttendanceDate);
                cmd.Parameters.AddWithValue("@CheckInTime", attendance.CheckInTime);
                cmd.Parameters.AddWithValue("@CheckOutTime", (object)attendance.CheckOutTime ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@WorkingHours", (object)attendance.WorkingHours ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", attendance.Status ?? "Present");
                cmd.Parameters.AddWithValue("@Notes", (object)attendance.Notes ?? DBNull.Value);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error updating attendance: " + ex.Message);
                }
            }
        }

        public int DeleteAttendance(int id)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                string query = "DELETE FROM Attendance WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error deleting attendance: " + ex.Message);
                }
            }
        }

        private Attendance MapReaderToAttendance(SqlDataReader reader)
        {
            return new Attendance
            {
                Id = Convert.ToInt32(reader["Id"]),
                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                EmployeeName = reader["EmployeeName"].ToString(),
                EmployeeCode = reader["EmployeeCode"] != DBNull.Value ? reader["EmployeeCode"].ToString() : null,
                AttendanceDate = Convert.ToDateTime(reader["AttendanceDate"]),
                CheckInTime = Convert.ToDateTime(reader["CheckInTime"]),
                CheckOutTime = reader["CheckOutTime"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["CheckOutTime"]) : null,
                WorkingHours = reader["WorkingHours"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["WorkingHours"]) : null,
                Status = reader["Status"].ToString(),
                Notes = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() : null,
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : null
            };
        }
    }
}
