using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.BLL;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private EmployeeBLL employeeBLL;
        private bool isNewEmployee = false;
        private string selectedPhotoPath = null;
        private const string PHOTO_FOLDER = "EmployeePhotos";

        public Form1()
        {
            InitializeComponent();
            employeeBLL = new EmployeeBLL();
        }

        /// <summary>
        /// Form load event - Initialize form and load data
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                LoadAllEmployees();
                LoadFilterDropdowns();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing form: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load all employees into DataGridView
        /// </summary>
        private void LoadAllEmployees()
        {
            try
            {
                List<Employee> employees = employeeBLL.GetAllEmployees();

                if (employees == null || employees.Count == 0)
                {
                    MessageBox.Show("Không có nhân viên nào trong database.\n\n" +
                                  "Vui lòng chạy script SQL để thêm dữ liệu mẫu:\n" +
                                  "TaoDatabase_QuanLyNhanSu.sql",
                                  "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                BindEmployeesToGrid(employees);

                // Update status in title bar
                this.Text = $"Employee Management System - {employees.Count} nhân viên";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách nhân viên:\n\n" + ex.Message +
                              "\n\nChi tiết:\n" + ex.ToString(),
                              "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Bind employee list to DataGridView
        /// </summary>
        private void BindEmployeesToGrid(List<Employee> employees)
        {
            // Create a DataTable for binding with formatted data
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Full Name", typeof(string));
            dt.Columns.Add("Gender", typeof(string));
            dt.Columns.Add("Age", typeof(int));
            dt.Columns.Add("Date of Birth", typeof(string));
            dt.Columns.Add("Position", typeof(string));
            dt.Columns.Add("Department", typeof(string));
            dt.Columns.Add("Salary", typeof(string));

            foreach (var emp in employees)
            {
                dt.Rows.Add(
                    emp.Id,
                    emp.FullName,
                    emp.Gender,
                    emp.Age,
                    emp.DateOfBirth.ToString("dd/MM/yyyy"),
                    emp.Position,
                    emp.Department,
                    emp.Salary.ToString("N0") + " VND"
                );
            }

            dgvEmployees.DataSource = dt;

            // Auto-size columns
            if (dgvEmployees.Columns.Count > 0)
            {
                dgvEmployees.Columns["ID"].Width = 50;
                dgvEmployees.Columns["Age"].Width = 50;
            }
        }

        /// <summary>
        /// Load filter dropdown values
        /// </summary>
        private void LoadFilterDropdowns()
        {
            try
            {
                // Load Gender filter
                cboFilterGender.Items.Clear();
                cboFilterGender.Items.Add("-- All --");
                cboFilterGender.Items.Add("Male");
                cboFilterGender.Items.Add("Female");
                cboFilterGender.Items.Add("Other");
                cboFilterGender.SelectedIndex = 0;

                // Load Department filter
                cboFilterDepartment.Items.Clear();
                cboFilterDepartment.Items.Add("-- All --");
                List<string> departments = employeeBLL.GetDepartments();
                foreach (string dept in departments)
                {
                    cboFilterDepartment.Items.Add(dept);
                }
                cboFilterDepartment.SelectedIndex = 0;

                // Load Position filter
                cboFilterPosition.Items.Clear();
                cboFilterPosition.Items.Add("-- All --");
                List<string> positions = employeeBLL.GetPositions();
                foreach (string pos in positions)
                {
                    cboFilterPosition.Items.Add(pos);
                }
                cboFilterPosition.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading filters: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// DataGridView selection changed event
        /// </summary>
        private void dgvEmployees_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0 && !isNewEmployee)
            {
                try
                {
                    // Get the selected employee ID
                    int employeeId = Convert.ToInt32(dgvEmployees.SelectedRows[0].Cells["ID"].Value);

                    // Load employee details
                    Employee employee = employeeBLL.GetEmployeeById(employeeId);
                    if (employee != null)
                    {
                        LoadEmployeeToForm(employee);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi load thông tin nhân viên:\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// DataGridView cell click event - Alternative way to select employee
        /// </summary>
        private void dgvEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore header row clicks
            if (e.RowIndex < 0) return;

            try
            {
                // Make sure row is selected
                if (e.RowIndex >= 0 && e.RowIndex < dgvEmployees.Rows.Count)
                {
                    dgvEmployees.Rows[e.RowIndex].Selected = true;

                    // Get employee ID from the clicked row
                    int employeeId = Convert.ToInt32(dgvEmployees.Rows[e.RowIndex].Cells["ID"].Value);

                    // Load employee details
                    Employee employee = employeeBLL.GetEmployeeById(employeeId);
                    if (employee != null)
                    {
                        LoadEmployeeToForm(employee);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn nhân viên:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// DataGridView double click - Same as single click for this app
        /// </summary>
        private void dgvEmployees_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Same behavior as single click
            dgvEmployees_CellClick(sender, e);
        }

        /// <summary>
        /// Load employee data to form controls
        /// </summary>
        private void LoadEmployeeToForm(Employee employee)
        {
            txtId.Text = employee.Id.ToString();
            txtFullName.Text = employee.FullName;
            cboGender.SelectedItem = employee.Gender;
            dtpDateOfBirth.Value = employee.DateOfBirth;
            txtPosition.Text = employee.Position;
            txtDepartment.Text = employee.Department;
            txtSalary.Text = employee.Salary.ToString();

            // Load photo from file path
            selectedPhotoPath = employee.PhotoPath;
            if (!string.IsNullOrEmpty(employee.PhotoPath) && File.Exists(employee.PhotoPath))
            {
                try
                {
                    // Dispose old image first
                    if (pictureBoxPhoto.Image != null)
                    {
                        pictureBoxPhoto.Image.Dispose();
                        pictureBoxPhoto.Image = null;
                    }

                    // Load image from file
                    using (var fileStream = new FileStream(employee.PhotoPath, FileMode.Open, FileAccess.Read))
                    {
                        pictureBoxPhoto.Image = Image.FromStream(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    pictureBoxPhoto.Image = null;
                    MessageBox.Show("Không thể hiển thị ảnh: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // No photo or file not found
                if (pictureBoxPhoto.Image != null)
                {
                    pictureBoxPhoto.Image.Dispose();
                }
                pictureBoxPhoto.Image = null;
            }

            isNewEmployee = false;
        }

        /// <summary>
        /// Clear form controls
        /// </summary>
        private void ClearForm()
        {
            txtId.Text = "";
            txtFullName.Text = "";
            cboGender.SelectedIndex = -1;
            dtpDateOfBirth.Value = DateTime.Now.AddYears(-25);
            txtPosition.Text = "";
            txtDepartment.Text = "";
            txtSalary.Text = "";

            // Properly dispose image before clearing
            if (pictureBoxPhoto.Image != null)
            {
                pictureBoxPhoto.Image.Dispose();
                pictureBoxPhoto.Image = null;
            }

            selectedPhotoPath = null;
            isNewEmployee = false;
        }

        /// <summary>
        /// Get employee from form controls
        /// </summary>
        private Employee GetEmployeeFromForm()
        {
            Employee employee = new Employee();

            if (!string.IsNullOrWhiteSpace(txtId.Text))
            {
                employee.Id = Convert.ToInt32(txtId.Text);
            }

            employee.FullName = txtFullName.Text.Trim();
            employee.Gender = cboGender.SelectedItem?.ToString() ?? "";
            employee.DateOfBirth = dtpDateOfBirth.Value;
            employee.Position = txtPosition.Text.Trim();
            employee.Department = txtDepartment.Text.Trim();

            decimal salary = 0;
            if (decimal.TryParse(txtSalary.Text, out salary))
            {
                employee.Salary = salary;
            }

            employee.PhotoPath = selectedPhotoPath;

            return employee;
        }

        /// <summary>
        /// New button click - Prepare form for new employee
        /// </summary>
        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            isNewEmployee = true;
            txtFullName.Focus();
        }

        /// <summary>
        /// Save button click - Add or update employee
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Employee employee = GetEmployeeFromForm();
                string message;

                if (isNewEmployee || string.IsNullOrWhiteSpace(txtId.Text))
                {
                    // Add new employee
                    if (employeeBLL.AddEmployee(employee, out message))
                    {
                        MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllEmployees();
                        LoadFilterDropdowns();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show(message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    // Update existing employee
                    if (employeeBLL.UpdateEmployee(employee, out message))
                    {
                        MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllEmployees();
                        LoadFilterDropdowns();
                    }
                    else
                    {
                        MessageBox.Show(message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving employee: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Delete button click - Delete selected employee
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Please select an employee to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this employee?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int employeeId = Convert.ToInt32(txtId.Text);
                    string message;

                    if (employeeBLL.DeleteEmployee(employeeId, out message))
                    {
                        MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllEmployees();
                        LoadFilterDropdowns();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting employee: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Refresh button click - Reload all data
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAllEmployees();
            LoadFilterDropdowns();
            ClearForm();
            txtSearch.Text = "";
            cboFilterGender.SelectedIndex = 0;
            cboFilterDepartment.SelectedIndex = 0;
            cboFilterPosition.SelectedIndex = 0;
        }

        /// <summary>
        /// Upload photo button click - Save to local folder and store path
        /// </summary>
        private void btnUploadPhoto_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                    ofd.Title = "Chọn Ảnh Nhân Viên";
                    ofd.FilterIndex = 1;
                    ofd.RestoreDirectory = true;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        // Dispose old image first
                        if (pictureBoxPhoto.Image != null)
                        {
                            pictureBoxPhoto.Image.Dispose();
                            pictureBoxPhoto.Image = null;
                        }

                        // Create EmployeePhotos folder if not exists
                        string photoFolder = Path.Combine(Application.StartupPath, PHOTO_FOLDER);
                        if (!Directory.Exists(photoFolder))
                        {
                            Directory.CreateDirectory(photoFolder);
                        }

                        // Generate unique filename
                        string extension = Path.GetExtension(ofd.FileName);
                        string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString("N").Substring(0, 8) + extension;
                        string destPath = Path.Combine(photoFolder, fileName);

                        // Copy file to EmployeePhotos folder
                        File.Copy(ofd.FileName, destPath, true);

                        // Save path
                        selectedPhotoPath = destPath;

                        // Display image
                        using (var fileStream = new FileStream(destPath, FileMode.Open, FileAccess.Read))
                        {
                            pictureBoxPhoto.Image = Image.FromStream(fileStream);
                        }

                        MessageBox.Show("Ảnh đã được tải lên thành công!\nNhấn 'Save' để lưu thông tin nhân viên.",
                            "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải ảnh: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Search textbox changed event
        /// </summary>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    LoadAllEmployees();
                }
                else
                {
                    List<Employee> employees = employeeBLL.SearchEmployees(searchText);
                    BindEmployeesToGrid(employees);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Filter changed event - applies when any filter dropdown changes
        /// </summary>
        private void FilterChanged(object sender, EventArgs e)
        {
            try
            {
                string gender = cboFilterGender.SelectedItem?.ToString();
                if (gender == "-- All --") gender = null;

                string department = cboFilterDepartment.SelectedItem?.ToString();
                if (department == "-- All --") department = null;

                string position = cboFilterPosition.SelectedItem?.ToString();
                if (position == "-- All --") position = null;

                // Apply filters
                if (gender == null && department == null && position == null)
                {
                    LoadAllEmployees();
                }
                else
                {
                    List<Employee> employees = employeeBLL.FilterEmployees(gender, department, position);
                    BindEmployeesToGrid(employees);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error applying filters: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Clear filter button click
        /// </summary>
        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            cboFilterGender.SelectedIndex = 0;
            cboFilterDepartment.SelectedIndex = 0;
            cboFilterPosition.SelectedIndex = 0;
            LoadAllEmployees();
        }
    }
}
