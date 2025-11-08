using System;
using WindowsFormsApp1.DAL;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Utils
{
    /// <summary>
    /// Audit Helper - Simplify audit logging
    /// </summary>
    public static class AuditHelper
    {
        private static AuditLogDAL auditDAL = new AuditLogDAL();

        /// <summary>
        /// Log action with current user info
        /// </summary>
        public static void Log(string tableName, int recordId, string action,
                              string description = null)
        {
            try
            {
                auditDAL.AddLog(
                    tableName,
                    recordId,
                    action,
                    null, // fieldName
                    null, // oldValue
                    null, // newValue
                    description,
                    SessionManager.Username,
                    SecurityHelper.GetLocalIPAddress(),
                    SecurityHelper.GetMachineName()
                );
            }
            catch
            {
                // Ignore audit errors - should not break application
            }
        }

        /// <summary>
        /// Log field change
        /// </summary>
        public static void LogFieldChange(string tableName, int recordId,
                                         string fieldName, string oldValue, string newValue)
        {
            try
            {
                auditDAL.AddLog(
                    tableName,
                    recordId,
                    "UPDATE",
                    fieldName,
                    oldValue,
                    newValue,
                    $"Changed {fieldName}",
                    SessionManager.Username,
                    SecurityHelper.GetLocalIPAddress(),
                    SecurityHelper.GetMachineName()
                );
            }
            catch
            {
                // Ignore audit errors
            }
        }

        /// <summary>
        /// Log multiple field changes for an employee
        /// </summary>
        public static void LogEmployeeChanges(Employee oldEmp, Employee newEmp)
        {
            if (oldEmp == null || newEmp == null) return;

            try
            {
                if (oldEmp.FullName != newEmp.FullName)
                    LogFieldChange("Employees", newEmp.Id, "FullName", oldEmp.FullName, newEmp.FullName);

                if (oldEmp.Gender != newEmp.Gender)
                    LogFieldChange("Employees", newEmp.Id, "Gender", oldEmp.Gender, newEmp.Gender);

                if (oldEmp.DateOfBirth != newEmp.DateOfBirth)
                    LogFieldChange("Employees", newEmp.Id, "DateOfBirth",
                                  oldEmp.DateOfBirth.ToString("dd/MM/yyyy"),
                                  newEmp.DateOfBirth.ToString("dd/MM/yyyy"));

                if (oldEmp.Position != newEmp.Position)
                    LogFieldChange("Employees", newEmp.Id, "Position", oldEmp.Position, newEmp.Position);

                if (oldEmp.DepartmentId != newEmp.DepartmentId)
                    LogFieldChange("Employees", newEmp.Id, "Department",
                                  oldEmp.DepartmentName ?? "null",
                                  newEmp.DepartmentName ?? "null");

                if (oldEmp.Salary != newEmp.Salary)
                    LogFieldChange("Employees", newEmp.Id, "Salary",
                                  oldEmp.Salary.ToString("N0"),
                                  newEmp.Salary.ToString("N0"));

                if (oldEmp.Status != newEmp.Status)
                    LogFieldChange("Employees", newEmp.Id, "Status", oldEmp.Status, newEmp.Status);
            }
            catch
            {
                // Ignore audit errors
            }
        }
    }
}
