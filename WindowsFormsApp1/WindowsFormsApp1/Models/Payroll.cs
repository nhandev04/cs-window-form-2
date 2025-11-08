using System;

namespace WindowsFormsApp1.Models
{
    /// <summary>
    /// Payroll entity - Bảng lương
    /// </summary>
    public class Payroll
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }  // For display
        public string EmployeeName { get; set; }  // For display
        public string DepartmentName { get; set; }  // For display
        public int PayrollMonth { get; set; }
        public int PayrollYear { get; set; }

        // Salary components
        public decimal BaseSalary { get; set; }
        public int WorkingDays { get; set; }
        public int StandardDays { get; set; }

        // Income
        public decimal Allowance { get; set; }
        public decimal Bonus { get; set; }
        public decimal OvertimeHours { get; set; }
        public decimal OvertimePay { get; set; }

        // Deductions
        public decimal Penalty { get; set; }
        public decimal SocialInsurance { get; set; }
        public decimal HealthInsurance { get; set; }
        public decimal UnemploymentIns { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal PersonalDeduction { get; set; }
        public decimal DependentDeduction { get; set; }
        public decimal IncomeTax { get; set; }

        // Totals
        public decimal TotalIncome { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal NetSalary { get; set; }

        public string Notes { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidDate { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        // Display properties
        public string MonthYearDisplay => $"Tháng {PayrollMonth:00}/{PayrollYear}";
        public string StatusDisplay => IsPaid ? "Đã trả" : "Chưa trả";
        public string PaidDateDisplay => PaidDate?.ToString("dd/MM/yyyy") ?? "";

        // Formatted currency
        public string BaseSalaryDisplay => BaseSalary.ToString("N0") + " đ";
        public string AllowanceDisplay => Allowance.ToString("N0") + " đ";
        public string BonusDisplay => Bonus.ToString("N0") + " đ";
        public string OvertimePayDisplay => OvertimePay.ToString("N0") + " đ";
        public string PenaltyDisplay => Penalty.ToString("N0") + " đ";
        public string TotalIncomeDisplay => TotalIncome.ToString("N0") + " đ";
        public string TotalDeductionDisplay => TotalDeduction.ToString("N0") + " đ";
        public string NetSalaryDisplay => NetSalary.ToString("N0") + " đ";

        // Calculated property
        public decimal ActualSalary => (BaseSalary / StandardDays) * WorkingDays;
    }
}
