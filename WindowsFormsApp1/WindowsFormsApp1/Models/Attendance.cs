using System;

namespace WindowsFormsApp1.Models
{
    /// <summary>
    /// Attendance entity class representing employee attendance records
    /// </summary>
    public class Attendance
    {
        /// <summary>
        /// Primary key - Unique identifier for attendance record
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee ID (Foreign Key)
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Employee name (for display - loaded from Employees table)
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// Employee code (for display - loaded from Employees table)
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Attendance date
        /// </summary>
        public DateTime AttendanceDate { get; set; }

        /// <summary>
        /// Check-in time
        /// </summary>
        public DateTime CheckInTime { get; set; }

        /// <summary>
        /// Check-out time (nullable)
        /// </summary>
        public DateTime? CheckOutTime { get; set; }

        /// <summary>
        /// Working hours calculated
        /// </summary>
        public decimal? WorkingHours { get; set; }

        /// <summary>
        /// Attendance status (Present, Late, Absent, etc.)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Notes about the attendance
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Created by user
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Updated date
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Updated by user
        /// </summary>
        public string UpdatedBy { get; set; }

        // Display properties

        /// <summary>
        /// Working hours display with hours format
        /// </summary>
        public string WorkingHoursDisplay => WorkingHours.HasValue ? $"{WorkingHours.Value:F2}h" : "-";

        /// <summary>
        /// Status display with Vietnamese
        /// </summary>
        public string StatusDisplay
        {
            get
            {
                switch (Status)
                {
                    case "Present": return "Có mặt";
                    case "Late": return "Đi muộn";
                    case "Absent": return "Vắng mặt";
                    case "OnLeave": return "Nghỉ phép";
                    default: return Status;
                }
            }
        }

        /// <summary>
        /// Attendance date display
        /// </summary>
        public string AttendanceDateDisplay => AttendanceDate.ToString("dd/MM/yyyy");

        /// <summary>
        /// Check in time display
        /// </summary>
        public string CheckInTimeDisplay => CheckInTime.ToString("HH:mm");

        /// <summary>
        /// Check out time display
        /// </summary>
        public string CheckOutTimeDisplay => CheckOutTime?.ToString("HH:mm") ?? "--:--";
    }
}
