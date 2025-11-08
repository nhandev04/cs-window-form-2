using System;
using System.Collections.Generic;
using WindowsFormsApp1.DAL;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Utils;

namespace WindowsFormsApp1.BLL
{
    public class DepartmentBLL
    {
        private DepartmentDAL departmentDAL;

        public DepartmentBLL()
        {
            departmentDAL = new DepartmentDAL();
        }

        public bool ValidateDepartment(Department department, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(department.DepartmentName))
            {
                errorMessage = "Tên phòng ban không được để trống.";
                return false;
            }

            if (department.DepartmentName.Length < 2 || department.DepartmentName.Length > 100)
            {
                errorMessage = "Tên phòng ban phải có từ 2-100 ký tự.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(department.Description) && department.Description.Length > 500)
            {
                errorMessage = "Mô tả không được quá 500 ký tự.";
                return false;
            }

            return true;
        }

        public List<Department> GetAllDepartments()
        {
            try
            {
                return departmentDAL.GetAllDepartments();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách phòng ban: " + ex.Message);
            }
        }

        public List<Department> GetActiveDepartments()
        {
            try
            {
                return departmentDAL.GetActiveDepartments();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách phòng ban: " + ex.Message);
            }
        }

        public Department GetDepartmentById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID phòng ban không hợp lệ.");
            }

            try
            {
                return departmentDAL.GetDepartmentById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin phòng ban: " + ex.Message);
            }
        }

        public bool AddDepartment(Department department, out string message)
        {
            if (!ValidateDepartment(department, out message))
                return false;

            if (departmentDAL.DepartmentNameExists(department.DepartmentName))
            {
                message = $"Phòng ban '{department.DepartmentName}' đã tồn tại.";
                return false;
            }

            try
            {
                department.CreatedBy = SessionManager.Username;
                department.IsActive = true;

                int result = departmentDAL.AddDepartment(department);

                if (result > 0)
                {
                    AuditHelper.Log("Departments", 0, "INSERT",
                                   $"Thêm phòng ban: {department.DepartmentName}");

                    message = "Thêm phòng ban thành công.";
                    return true;
                }
                else
                {
                    message = "Không thể thêm phòng ban.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi khi thêm phòng ban: " + ex.Message;
                return false;
            }
        }

        public bool UpdateDepartment(Department department, out string message)
        {
            if (department.Id <= 0)
            {
                message = "ID phòng ban không hợp lệ.";
                return false;
            }

            if (!ValidateDepartment(department, out message))
                return false;

            if (departmentDAL.DepartmentNameExists(department.DepartmentName, department.Id))
            {
                message = $"Phòng ban '{department.DepartmentName}' đã tồn tại.";
                return false;
            }

            try
            {
                Department oldDept = departmentDAL.GetDepartmentById(department.Id);

                if (oldDept == null)
                {
                    message = "Không tìm thấy phòng ban.";
                    return false;
                }

                department.UpdatedBy = SessionManager.Username;

                int result = departmentDAL.UpdateDepartment(department);

                if (result > 0)
                {
                    if (oldDept.DepartmentName != department.DepartmentName)
                    {
                        AuditHelper.LogFieldChange("Departments", department.Id, "DepartmentName",
                                                  oldDept.DepartmentName, department.DepartmentName);
                    }
                    if (oldDept.Description != department.Description)
                    {
                        AuditHelper.LogFieldChange("Departments", department.Id, "Description",
                                                  oldDept.Description, department.Description);
                    }
                    if (oldDept.ManagerId != department.ManagerId)
                    {
                        AuditHelper.LogFieldChange("Departments", department.Id, "ManagerId",
                                                  oldDept.ManagerId?.ToString() ?? "NULL",
                                                  department.ManagerId?.ToString() ?? "NULL");
                    }

                    message = "Cập nhật phòng ban thành công.";
                    return true;
                }
                else
                {
                    message = "Không thể cập nhật phòng ban.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi khi cập nhật phòng ban: " + ex.Message;
                return false;
            }
        }

        public bool DeleteDepartment(int id, out string message)
        {
            if (id <= 0)
            {
                message = "ID phòng ban không hợp lệ.";
                return false;
            }

            try
            {
                Department department = departmentDAL.GetDepartmentById(id);

                if (department == null)
                {
                    message = "Không tìm thấy phòng ban.";
                    return false;
                }

                if (department.EmployeeCount > 0)
                {
                    message = $"Không thể xóa phòng ban '{department.DepartmentName}' vì còn {department.EmployeeCount} nhân viên.";
                    return false;
                }

                int result = departmentDAL.DeleteDepartment(id);

                if (result > 0)
                {
                    AuditHelper.Log("Departments", id, "DELETE",
                                   $"Xóa phòng ban: {department.DepartmentName}");

                    message = "Xóa phòng ban thành công.";
                    return true;
                }
                else
                {
                    message = "Không thể xóa phòng ban.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi khi xóa phòng ban: " + ex.Message;
                return false;
            }
        }

        public bool ToggleStatus(int id, out string message)
        {
            try
            {
                Department dept = departmentDAL.GetDepartmentById(id);
                if (dept == null)
                {
                    message = "Không tìm thấy phòng ban.";
                    return false;
                }

                dept.IsActive = !dept.IsActive;
                dept.UpdatedBy = SessionManager.Username;

                int result = departmentDAL.UpdateDepartment(dept);

                if (result > 0)
                {
                    string status = dept.IsActive ? "Active" : "Inactive";
                    AuditHelper.Log("Departments", id, "UPDATE",
                                   $"Thay đổi trạng thái phòng ban '{dept.DepartmentName}' → {status}");

                    message = $"Đã {(dept.IsActive ? "kích hoạt" : "vô hiệu hóa")} phòng ban.";
                    return true;
                }
                else
                {
                    message = "Không thể thay đổi trạng thái.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi: " + ex.Message;
                return false;
            }
        }
    }
}
