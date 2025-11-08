using System;
using System.Collections.Generic;
using WindowsFormsApp1.DAL;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Utils;

namespace WindowsFormsApp1.BLL
{
    public class AttendanceBLL
    {
        private AttendanceDAL attendanceDAL;

        public AttendanceBLL()
        {
            attendanceDAL = new AttendanceDAL();
        }

        public List<Attendance> GetAllAttendance()
        {
            try
            {
                return attendanceDAL.GetAllAttendance();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách chấm công: " + ex.Message);
            }
        }

        public List<Attendance> GetAttendanceByEmployee(int employeeId)
        {
            try
            {
                return attendanceDAL.GetAttendanceByEmployee(employeeId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy lịch sử chấm công: " + ex.Message);
            }
        }

        public List<Attendance> GetAttendanceByDateRange(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return attendanceDAL.GetAttendanceByDateRange(fromDate, toDate);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lọc chấm công: " + ex.Message);
            }
        }

        public Attendance GetTodayAttendance(int employeeId)
        {
            try
            {
                return attendanceDAL.GetTodayAttendance(employeeId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi kiểm tra chấm công hôm nay: " + ex.Message);
            }
        }

        public bool CheckIn(int employeeId, out string message, string notes = null)
        {
            try
            {
                Attendance today = attendanceDAL.GetTodayAttendance(employeeId);

                if (today != null)
                {
                    message = $"Bạn đã check-in hôm nay lúc {today.CheckInTime:HH:mm}.";
                    return false;
                }

                Attendance attendance = new Attendance
                {
                    EmployeeId = employeeId,
                    AttendanceDate = DateTime.Today,
                    CheckInTime = DateTime.Now,
                    Status = "Present",
                    Notes = notes,
                    CreatedBy = SessionManager.Username
                };

                int result = attendanceDAL.CheckIn(attendance);

                if (result > 0)
                {
                    AuditHelper.Log("Attendance", result, "CHECK_IN",
                                   $"Nhân viên ID {employeeId} check-in lúc {DateTime.Now:HH:mm}");

                    message = $"Check-in thành công lúc {DateTime.Now:HH:mm}!";
                    return true;
                }
                else
                {
                    message = "Không thể check-in.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi khi check-in: " + ex.Message;
                return false;
            }
        }

        public bool CheckOut(int employeeId, out string message)
        {
            try
            {
                Attendance today = attendanceDAL.GetTodayAttendance(employeeId);

                if (today == null)
                {
                    message = "Bạn chưa check-in hôm nay.";
                    return false;
                }

                if (today.CheckOutTime.HasValue)
                {
                    message = $"Bạn đã check-out hôm nay lúc {today.CheckOutTime.Value:HH:mm}.";
                    return false;
                }

                int result = attendanceDAL.CheckOut(today.Id, DateTime.Now);

                if (result > 0)
                {
                    decimal workHours = (decimal)(DateTime.Now - today.CheckInTime).TotalHours;

                    AuditHelper.Log("Attendance", today.Id, "CHECK_OUT",
                                   $"Nhân viên ID {employeeId} check-out lúc {DateTime.Now:HH:mm}. Làm việc: {workHours:F2}h");

                    message = $"Check-out thành công lúc {DateTime.Now:HH:mm}!\nTổng giờ làm: {workHours:F2} giờ";
                    return true;
                }
                else
                {
                    message = "Không thể check-out.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi khi check-out: " + ex.Message;
                return false;
            }
        }

        public bool UpdateAttendance(Attendance attendance, out string message)
        {
            try
            {
                if (attendance.CheckOutTime.HasValue)
                {
                    TimeSpan duration = attendance.CheckOutTime.Value - attendance.CheckInTime;
                    attendance.WorkingHours = (decimal)duration.TotalHours;
                }

                int result = attendanceDAL.UpdateAttendance(attendance);

                if (result > 0)
                {
                    AuditHelper.Log("Attendance", attendance.Id, "UPDATE",
                                   $"Cập nhật chấm công: Ngày {attendance.AttendanceDate:dd/MM/yyyy}");

                    message = "Cập nhật chấm công thành công.";
                    return true;
                }
                else
                {
                    message = "Không thể cập nhật chấm công.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi khi cập nhật: " + ex.Message;
                return false;
            }
        }

        public bool DeleteAttendance(int id, out string message)
        {
            try
            {
                Attendance attendance = attendanceDAL.GetAttendanceById(id);

                if (attendance == null)
                {
                    message = "Không tìm thấy bản ghi chấm công.";
                    return false;
                }

                int result = attendanceDAL.DeleteAttendance(id);

                if (result > 0)
                {
                    AuditHelper.Log("Attendance", id, "DELETE",
                                   $"Xóa chấm công: {attendance.EmployeeName} - {attendance.AttendanceDate:dd/MM/yyyy}");

                    message = "Xóa chấm công thành công.";
                    return true;
                }
                else
                {
                    message = "Không thể xóa chấm công.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi khi xóa: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Get attendance by ID
        /// </summary>
        public Attendance GetAttendanceById(int id)
        {
            try
            {
                return attendanceDAL.GetAttendanceById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin chấm công: " + ex.Message);
            }
        }
    }
}
