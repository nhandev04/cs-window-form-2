using System;

namespace WindowsFormsApp1.Models
{
    /// <summary>
    /// Employee entity class representing an employee in the system
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Primary key - Unique identifier for employee
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Full name of the employee
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gender of the employee (Male/Female/Other)
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Job position/title
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Department name
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Monthly salary
        /// </summary>
        public decimal Salary { get; set; }

        /// <summary>
        /// Employee photo path (local file path)
        /// </summary>
        public string PhotoPath { get; set; }

        /// <summary>
        /// Calculated property for age
        /// </summary>
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }
}
