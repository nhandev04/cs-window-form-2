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
                string query = @"SELECT a.Id, a.EmployeeId, 
         ISNULL(e.FullName, 'Unknown') as EmployeeName, 
    e.EmployeeCode,
        d.DepartmentName, 
         a.AttendanceDate, 
     a.CheckInTime, 
         a.CheckOutTime, 
  a.WorkingHours,
      ISNULL(a.Status, 'Present') as Status, 
        a.Notes, 
       ISNULL(a.CreatedDate, GETDATE()) as CreatedDate, 
     a.CreatedBy
    FROM Attendance a
       INNER JOIN Employees e ON a.EmployeeId = e.Id
      LEFT JOIN Departments d ON e.DepartmentId = d.Id
          WHERE a.CheckInTime IS NOT NULL
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
                string query = @"SELECT a.Id, a.EmployeeId, 
        ISNULL(e.FullName, 'Unknown') as EmployeeName, 
      e.EmployeeCode,
   d.DepartmentName, 
                   a.AttendanceDate, 
        a.CheckInTime, 
      a.CheckOutTime, 
    a.WorkingHours,
        ISNULL(a.Status, 'Present') as Status, 
 a.Notes, 
       ISNULL(a.CreatedDate, GETDATE()) as CreatedDate, 
        a.CreatedBy
          FROM Attendance a
           INNER JOIN Employees e ON a.EmployeeId = e.Id
   LEFT JOIN Departments d ON e.DepartmentId = d.Id
        WHERE a.EmployeeId = @EmployeeId AND a.CheckInTime IS NOT NULL
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
            string query = @"SELECT a.Id, a.EmployeeId, 
    ISNULL(e.FullName, 'Unknown') as EmployeeName, 
             e.EmployeeCode,
             d.DepartmentName, 
          a.AttendanceDate, 
             a.CheckInTime, 
          a.CheckOutTime, 
a.WorkingHours,
           ISNULL(a.Status, 'Present') as Status, 
  a.Notes, 
        ISNULL(a.CreatedDate, GETDATE()) as CreatedDate, 
   a.CreatedBy
         FROM Attendance a
    INNER JOIN Employees e ON a.EmployeeId = e.Id
  LEFT JOIN Departments d ON e.DepartmentId = d.Id
       WHERE a.AttendanceDate >= @FromDate AND a.AttendanceDate <= @ToDate
  AND a.CheckInTime IS NOT NULL
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
                string query = @"SELECT a.Id, a.EmployeeId, 
   ISNULL(e.FullName, 'Unknown') as EmployeeName, 
          e.EmployeeCode,
   d.DepartmentName, 
         a.AttendanceDate, 
a.CheckInTime, 
           a.CheckOutTime, 
  a.WorkingHours,
       ISNULL(a.Status, 'Present') as Status, 
            a.Notes, 
            ISNULL(a.CreatedDate, GETDATE()) as CreatedDate, 
        a.CreatedBy
          FROM Attendance a
    INNER JOIN Employees e ON a.EmployeeId = e.Id
   LEFT JOIN Departments d ON e.DepartmentId = d.Id
     WHERE a.Id = @Id AND a.CheckInTime IS NOT NULL";

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
                string query = @"SELECT TOP 1 a.Id, a.EmployeeId, 
         ISNULL(e.FullName, 'Unknown') as EmployeeName, 
     e.EmployeeCode,
      d.DepartmentName, 
              a.AttendanceDate, 
            a.CheckInTime, 
               a.CheckOutTime, 
   a.WorkingHours,
         ISNULL(a.Status, 'Present') as Status, 
a.Notes, 
       ISNULL(a.CreatedDate, GETDATE()) as CreatedDate, 
    a.CreatedBy
          FROM Attendance a
  INNER JOIN Employees e ON a.EmployeeId = e.Id
          LEFT JOIN Departments d ON e.DepartmentId = d.Id
          WHERE a.EmployeeId = @EmployeeId AND a.AttendanceDate = @Today
           AND a.CheckInTime IS NOT NULL
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
          try
      {
  var attendance = new Attendance();
       
    // Safe conversion for each field
       try { attendance.Id = Convert.ToInt32(reader["Id"]); } 
         catch (Exception ex) { throw new Exception($"Error converting Id: {ex.Message}"); }
            
         try { attendance.EmployeeId = Convert.ToInt32(reader["EmployeeId"]); } 
     catch (Exception ex) { throw new Exception($"Error converting EmployeeId: {ex.Message}"); }
          
    try { attendance.EmployeeName = reader["EmployeeName"] != DBNull.Value ? reader["EmployeeName"].ToString() : "Unknown"; } 
         catch (Exception ex) { throw new Exception($"Error converting EmployeeName: {ex.Message}"); }
            
        try { attendance.EmployeeCode = reader["EmployeeCode"] != DBNull.Value ? reader["EmployeeCode"].ToString() : null; } 
    catch (Exception ex) { throw new Exception($"Error converting EmployeeCode: {ex.Message}"); }
     
         try { attendance.DepartmentName = reader["DepartmentName"] != DBNull.Value ? reader["DepartmentName"].ToString() : ""; } 
      catch (Exception ex) { throw new Exception($"Error converting DepartmentName: {ex.Message}"); }
      
   try { attendance.AttendanceDate = Convert.ToDateTime(reader["AttendanceDate"]); } 
           catch (Exception ex) { throw new Exception($"Error converting AttendanceDate: {ex.Message}"); }
                
    // CheckInTime should never be null due to WHERE clause, but add safety check
         try { 
     if (reader["CheckInTime"] != DBNull.Value)
          attendance.CheckInTime = Convert.ToDateTime(reader["CheckInTime"]);
       else
              throw new Exception("CheckInTime is unexpectedly null");
        } 
         catch (Exception ex) { throw new Exception($"Error converting CheckInTime: {ex.Message}"); }
     
     try { attendance.CheckOutTime = reader["CheckOutTime"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["CheckOutTime"]) : null; } 
       catch (Exception ex) { throw new Exception($"Error converting CheckOutTime: {ex.Message}"); }
        
 try { attendance.WorkingHours = reader["WorkingHours"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["WorkingHours"]) : null; } 
           catch (Exception ex) { throw new Exception($"Error converting WorkingHours: {ex.Message}"); }
    
             try { attendance.Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : "Present"; } 
      catch (Exception ex) { throw new Exception($"Error converting Status: {ex.Message}"); }
         
      try { attendance.Notes = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() : null; } 
         catch (Exception ex) { throw new Exception($"Error converting Notes: {ex.Message}"); }
      
      try { attendance.CreatedDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : DateTime.Now; } 
       catch (Exception ex) { throw new Exception($"Error converting CreatedDate: {ex.Message}"); }
     
   try { attendance.CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : null; } 
           catch (Exception ex) { throw new Exception($"Error converting CreatedBy: {ex.Message}"); }
         
 return attendance;
      }
  catch (Exception ex)
         {
   throw new Exception($"Error mapping attendance data: {ex.Message}");
       }
      }
    }
}
