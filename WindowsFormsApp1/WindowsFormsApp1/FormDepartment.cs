using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.BLL;
using WindowsFormsApp1.DAL;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Utils;

namespace WindowsFormsApp1
{
    public partial class FormDepartment : Form
    {
        private DepartmentBLL departmentBLL;
        private EmployeeDAL employeeDAL;
        private bool isNewDepartment = false;

        public FormDepartment()
        {
            InitializeComponent();
            departmentBLL = new DepartmentBLL();
            employeeDAL = new EmployeeDAL();
        }

        private void FormDepartment_Load(object sender, EventArgs e)
        {
            try
            {
                if (!SessionManager.IsLoggedIn)
                {
                    MessageBox.Show("Vui lòng đăng nhập!", "Thông Báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                this.Text = $"Department Management - User: {SessionManager.FullName} ({SessionManager.Role})";

                CheckPermissions();
                LoadManagers();
                LoadAllDepartments();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi khởi tạo form: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckPermissions()
        {
            if (SessionManager.Role == "Employee")
            {
                btnNew.Enabled = false;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                btnToggleStatus.Enabled = false;

                txtDepartmentName.ReadOnly = true;
                txtDescription.ReadOnly = true;
                cboManager.Enabled = false;
            }
            else if (SessionManager.Role == "Manager")
            {
                btnDelete.Enabled = false;
            }
        }

        private void LoadManagers()
        {
            try
            {
                List<Employee> employees = employeeDAL.GetAllEmployees();

                cboManager.Items.Clear();
                cboManager.Items.Add(new { Text = "-- Không có --", Value = (int?)null });

                foreach (var emp in employees.Where(e => e.Status == "Active"))
                {
                    cboManager.Items.Add(new { Text = $"{emp.FullName} ({emp.Position})", Value = emp.Id });
                }

                cboManager.DisplayMember = "Text";
                cboManager.ValueMember = "Value";
                cboManager.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh sách quản lý: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAllDepartments()
        {
            try
            {
                List<Department> departments = departmentBLL.GetAllDepartments();
                BindDepartmentsToGrid(departments);
                this.Text = $"Department Management - {departments.Count} phòng ban";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách phòng ban: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindDepartmentsToGrid(List<Department> departments)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Department Name", typeof(string));
            dt.Columns.Add("Manager", typeof(string));
            dt.Columns.Add("Employees", typeof(int));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Created Date", typeof(string));
            dt.Columns.Add("Created By", typeof(string));

            foreach (var dept in departments)
            {
                dt.Rows.Add(
                    dept.Id,
                    dept.DepartmentName,
                    dept.ManagerName ?? "Chưa có",
                    dept.EmployeeCount,
                    dept.IsActive ? "Active" : "Inactive",
                    dept.CreatedDate.ToString("dd/MM/yyyy"),
                    dept.CreatedBy ?? ""
                );
            }

            dgvDepartments.DataSource = dt;

            if (dgvDepartments.Columns.Count > 0)
            {
                dgvDepartments.Columns["ID"].Width = 50;
                dgvDepartments.Columns["Department Name"].Width = 200;
                dgvDepartments.Columns["Manager"].Width = 150;
                dgvDepartments.Columns["Employees"].Width = 80;
                dgvDepartments.Columns["Status"].Width = 80;
                dgvDepartments.Columns["Created Date"].Width = 100;
                dgvDepartments.Columns["Created By"].Width = 100;
            }
        }

        private void dgvDepartments_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDepartments.SelectedRows.Count > 0 && !isNewDepartment)
            {
                try
                {
                    int deptId = Convert.ToInt32(dgvDepartments.SelectedRows[0].Cells["ID"].Value);
                    Department department = departmentBLL.GetDepartmentById(deptId);
                    if (department != null)
                    {
                        LoadDepartmentToForm(department);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi load thông tin phòng ban: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvDepartments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgvDepartments.Rows.Count)
                {
                    dgvDepartments.Rows[e.RowIndex].Selected = true;
                    int deptId = Convert.ToInt32(dgvDepartments.Rows[e.RowIndex].Cells["ID"].Value);
                    Department department = departmentBLL.GetDepartmentById(deptId);
                    if (department != null)
                    {
                        LoadDepartmentToForm(department);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn phòng ban: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDepartmentToForm(Department department)
        {
            txtId.Text = department.Id.ToString();
            txtDepartmentName.Text = department.DepartmentName;
            txtDescription.Text = department.Description ?? "";
            lblEmployeeCount.Text = $"{department.EmployeeCount} nhân viên";
            lblStatus.Text = department.IsActive ? "Active" : "Inactive";

            if (department.ManagerId.HasValue)
            {
                foreach (var item in cboManager.Items)
                {
                    var managerItem = item as dynamic;
                    if (managerItem != null && managerItem.Value == department.ManagerId)
                    {
                        cboManager.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                cboManager.SelectedIndex = 0;
            }

            btnToggleStatus.Text = department.IsActive ? "Deactivate" : "Activate";
            isNewDepartment = false;
        }

        private void ClearForm()
        {
            txtId.Text = "";
            txtDepartmentName.Text = "";
            txtDescription.Text = "";
            cboManager.SelectedIndex = 0;
            lblEmployeeCount.Text = "0 nhân viên";
            lblStatus.Text = "Active";
            btnToggleStatus.Text = "Deactivate";
            isNewDepartment = false;
        }

        private Department GetDepartmentFromForm()
        {
            Department department = new Department();

            if (!string.IsNullOrWhiteSpace(txtId.Text))
            {
                department.Id = Convert.ToInt32(txtId.Text);
            }

            department.DepartmentName = txtDepartmentName.Text.Trim();
            department.Description = txtDescription.Text.Trim();

            var selectedManager = cboManager.SelectedItem as dynamic;
            if (selectedManager != null && selectedManager.Value != null)
            {
                department.ManagerId = selectedManager.Value;
            }

            return department;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            isNewDepartment = true;
            txtDepartmentName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Department department = GetDepartmentFromForm();
                string message;

                if (isNewDepartment || string.IsNullOrWhiteSpace(txtId.Text))
                {
                    if (departmentBLL.AddDepartment(department, out message))
                    {
                        MessageBox.Show(message, "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllDepartments();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    if (departmentBLL.UpdateDepartment(department, out message))
                    {
                        MessageBox.Show(message, "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllDepartments();
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu phòng ban: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Vui lòng chọn phòng ban cần xóa.", "Cảnh Báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa phòng ban '{txtDepartmentName.Text}'?\n\n" +
                $"Số nhân viên: {lblEmployeeCount.Text}",
                "Xác Nhận Xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int deptId = Convert.ToInt32(txtId.Text);
                    string message;

                    if (departmentBLL.DeleteDepartment(deptId, out message))
                    {
                        MessageBox.Show(message, "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllDepartments();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa phòng ban: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnToggleStatus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Vui lòng chọn phòng ban.", "Cảnh Báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int deptId = Convert.ToInt32(txtId.Text);
                string message;

                if (departmentBLL.ToggleStatus(deptId, out message))
                {
                    MessageBox.Show(message, "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAllDepartments();
                    Department dept = departmentBLL.GetDepartmentById(deptId);
                    if (dept != null)
                    {
                        LoadDepartmentToForm(dept);
                    }
                }
                else
                {
                    MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAllDepartments();
            LoadManagers();
            ClearForm();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text.Trim().ToLower();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    LoadAllDepartments();
                }
                else
                {
                    List<Department> allDepts = departmentBLL.GetAllDepartments();
                    List<Department> filtered = allDepts.Where(d =>
                        d.DepartmentName.ToLower().Contains(searchText) ||
                        (d.ManagerName != null && d.ManagerName.ToLower().Contains(searchText)) ||
                        (d.Description != null && d.Description.ToLower().Contains(searchText))
                    ).ToList();

                    BindDepartmentsToGrid(filtered);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
