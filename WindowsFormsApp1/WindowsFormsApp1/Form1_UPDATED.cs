using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.BLL;
using WindowsFormsApp1.DAL;     // NEW: Added for DepartmentDAL
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Utils;   // NEW: Added for SessionManager

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private EmployeeBLL employeeBLL;
        private DepartmentDAL departmentDAL;  // NEW: For loading departments
        private bool isNewEmployee = false;
        private string selectedPhotoPath = null;
        private const string PHOTO_FOLDER = "EmployeePhotos";

        public Form1()
        {
            InitializeComponent();
            employeeBLL = new EmployeeBLL();
            departmentDAL = new DepartmentDAL();  // NEW
        }

        /// <summary>
        /// Form load event - Initialize form and load data
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // NEW: Check login first
                if (!SessionManager.IsLoggedIn)
                {
                    MessageBox.Show("Vui lòng đăng nhập trước!", "Thông Báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                // NEW: Show current user in title bar
                this.Text = $"Employee Management - User: {SessionManager.FullName} ({SessionManager.Role})";

                // NEW: Check permissions and disable buttons accordingly
                CheckPermissions();

                // NEW: Load departments into ComboBox
                LoadDepartments();

                // NEW: Load status options
                LoadStatusOptions();

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
        /// NEW: Check user permissions and enable/disable buttons
        /// </summary>
        private void CheckPermissions()
        {
            // Admin and HR can do everything
            if (SessionManager.IsAdmin || SessionManager.IsHR)
            {
                // Full access
                return;
            }

            // Managers can view and edit, but not delete
            if (SessionManager.Role == "Manager")
            {
                btnDelete.Enabled = false;
            }

            // Regular employees can only view
            if (SessionManager.Role == "Employee")
            {
                btnNew.Enabled = false;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                btnUploadPhoto.Enabled = false;

                // Make all input controls readonly
                txtEmployeeCode.ReadOnly = true;  // NEW control
                txtFullName.ReadOnly = true;
                cboGender.Enabled = false;
                dtpDateOfBirth.Enabled = false;
                txtEmail.ReadOnly = true;  // NEW control
                txtPhoneNumber.ReadOnly = true;  // NEW control
                txtAddress.ReadOnly = true;  // NEW control
                txtPosition.ReadOnly = true;
                cboDepartment.Enabled = false;  // NEW control (replaces txtDepartment)
                txtSalary.ReadOnly = true;
                dtpHireDate.Enabled = false;  // NEW control
                cboStatus.Enabled = false;  // NEW control
                txtNotes.ReadOnly = true;  // NEW control
            }
        }

        /// <summary>
        /// NEW: Load departments into ComboBox
        /// NOTE: Requires cboDepartment (ComboBox) in Designer
        /// </summary>
        private void LoadDepartments()
        {
            try
            {
                List<Department> departments = departmentDAL.GetActiveDepartments();

                cboDepartment.Items.Clear();

                // Add empty option
                cboDepartment.Items.Add(new { Text = "-- Chọn phòng ban --", Value = (int?)null });

                // Add all departments
                foreach (var dept in departments)
                {
                    cboDepartment.Items.Add(new { Text = dept.DepartmentName, Value = dept.Id });
                }

                cboDepartment.DisplayMember = "Text";
                cboDepartment.ValueMember = "Value";
                cboDepartment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load phòng ban: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// NEW: Load status options into ComboBox
        /// NOTE: Requires cboStatus (ComboBox) in Designer
        /// </summary>
        private void LoadStatusOptions()
        {
            cboStatus.Items.Clear();
            cboStatus.Items.Add("Active");
            cboStatus.Items.Add("OnLeave");
            cboStatus.Items.Add("Resigned");
            cboStatus.SelectedIndex = 0;  // Default to Active
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
                                  "Database_QuanLyNhanSu_Full.sql",
                                  "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                BindEmployeesToGrid(employees);

                // Update status in title bar
                this.Text = $"Employee Management - User: {SessionManager.FullName} ({SessionManager.Role}) - {employees.Count} nhân viên";
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
        /// UPDATED: Added new columns for Employee Code, Email, Phone, Status, Hire Date
        /// </summary>
        private void BindEmployeesToGrid(List<Employee> employees)
        {
            // Create a DataTable for binding with formatted data
            DataTable dt = new DataTable();

            // Existing columns
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Employee Code", typeof(string));  // NEW
            dt.Columns.Add("Full Name", typeof(string));
            dt.Columns.Add("Gender", typeof(string));
            dt.Columns.Add("Age", typeof(int));
            dt.Columns.Add("Email", typeof(string));          // NEW
            dt.Columns.Add("Phone", typeof(string));          // NEW
            dt.Columns.Add("Position", typeof(string));
            dt.Columns.Add("Department", typeof(string));     // Now uses DepartmentName from JOIN
            dt.Columns.Add("Salary", typeof(string));
            dt.Columns.Add("Status", typeof(string));         // NEW
            dt.Columns.Add("Hire Date", typeof(string));      // NEW

            foreach (var emp in employees)
            {
                dt.Rows.Add(
                    emp.Id,
                    emp.EmployeeCode,                         // NEW
                    emp.FullName,
                    emp.Gender,
                    emp.Age,
                    emp.Email,                                // NEW
                    emp.PhoneNumber,                          // NEW
                    emp.Position,
                    emp.DepartmentDisplay,                    // Uses DepartmentName from JOIN or legacy Department
                    emp.Salary.ToString("N0") + " đ",
                    emp.StatusDisplay,                        // NEW - Vietnamese display
                    emp.HireDateDisplay                       // NEW - Formatted date
                );
            }

            dgvEmployees.DataSource = dt;

            // Auto-size columns
            if (dgvEmployees.Columns.Count > 0)
            {
                dgvEmployees.Columns["ID"].Width = 40;
                dgvEmployees.Columns["Employee Code"].Width = 100;  // NEW
                dgvEmployees.Columns["Full Name"].Width = 150;
                dgvEmployees.Columns["Gender"].Width = 60;
                dgvEmployees.Columns["Age"].Width = 40;
                dgvEmployees.Columns["Email"].Width = 150;          // NEW
                dgvEmployees.Columns["Phone"].Width = 100;          // NEW
                dgvEmployees.Columns["Position"].Width = 120;
                dgvEmployees.Columns["Department"].Width = 120;
                dgvEmployees.Columns["Salary"].Width = 100;
                dgvEmployees.Columns["Status"].Width = 80;          // NEW
                dgvEmployees.Columns["Hire Date"].Width = 90;       // NEW
            }
        }

        /// <summary>
        /// Load filter dropdown values
        /// UPDATED: Added Status filter
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

                // NEW: Load Status filter
                // NOTE: Requires cboFilterStatus (ComboBox) in Designer
                cboFilterStatus.Items.Clear();
                cboFilterStatus.Items.Add("-- All --");
                cboFilterStatus.Items.Add("Active");
                cboFilterStatus.Items.Add("OnLeave");
                cboFilterStatus.Items.Add("Resigned");
                cboFilterStatus.SelectedIndex = 0;
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
        /// UPDATED: Added mapping for all new fields
        /// NOTE: New controls required in Designer:
        /// - txtEmployeeCode (TextBox)
        /// - txtEmail (TextBox)
        /// - txtPhoneNumber (TextBox)
        /// - txtAddress (TextBox, multiline)
        /// - cboDepartment (ComboBox) - REPLACES txtDepartment
        /// - cboStatus (ComboBox)
        /// - dtpHireDate (DateTimePicker)
        /// - txtNotes (TextBox, multiline)
        /// </summary>
        private void LoadEmployeeToForm(Employee employee)
        {
            // Existing fields
            txtId.Text = employee.Id.ToString();
            txtFullName.Text = employee.FullName;
            cboGender.SelectedItem = employee.Gender;
            dtpDateOfBirth.Value = employee.DateOfBirth;
            txtPosition.Text = employee.Position;
            txtSalary.Text = employee.Salary.ToString();

            // NEW FIELDS
            txtEmployeeCode.Text = employee.EmployeeCode ?? "";
            txtEmail.Text = employee.Email ?? "";
            txtPhoneNumber.Text = employee.PhoneNumber ?? "";
            txtAddress.Text = employee.Address ?? "";
            txtNotes.Text = employee.Notes ?? "";

            // NEW: Department ComboBox (replaces txtDepartment)
            if (employee.DepartmentId.HasValue)
            {
                // Select the department in ComboBox by matching DepartmentId
                bool found = false;
                foreach (var item in cboDepartment.Items)
                {
                    var deptItem = item as dynamic;
                    if (deptItem != null && deptItem.Value == employee.DepartmentId)
                    {
                        cboDepartment.SelectedItem = item;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    cboDepartment.SelectedIndex = 0;  // Default to "-- Chọn phòng ban --"
                }
            }
            else
            {
                cboDepartment.SelectedIndex = 0;
            }

            // NEW: Hire Date
            if (employee.HireDate.HasValue)
            {
                dtpHireDate.Value = employee.HireDate.Value;
            }
            else
            {
                dtpHireDate.Value = DateTime.Now;
            }

            // NEW: Status
            string status = employee.Status ?? "Active";
            if (cboStatus.Items.Contains(status))
            {
                cboStatus.SelectedItem = status;
            }
            else
            {
                cboStatus.SelectedIndex = 0;  // Default to Active
            }

            // Load photo from file path (existing code)
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
        /// UPDATED: Added clearing for all new fields
        /// </summary>
        private void ClearForm()
        {
            // Existing fields
            txtId.Text = "";
            txtFullName.Text = "";
            cboGender.SelectedIndex = -1;
            dtpDateOfBirth.Value = DateTime.Now.AddYears(-25);
            txtPosition.Text = "";
            txtSalary.Text = "";

            // NEW FIELDS
            txtEmployeeCode.Text = "";
            txtEmail.Text = "";
            txtPhoneNumber.Text = "";
            txtAddress.Text = "";
            txtNotes.Text = "";

            cboDepartment.SelectedIndex = 0;  // Select "-- Chọn phòng ban --"
            cboStatus.SelectedIndex = 0;      // Default to Active
            dtpHireDate.Value = DateTime.Now;

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
        /// UPDATED: Added extraction for all new fields
        /// </summary>
        private Employee GetEmployeeFromForm()
        {
            Employee employee = new Employee();

            // Existing fields
            if (!string.IsNullOrWhiteSpace(txtId.Text))
            {
                employee.Id = Convert.ToInt32(txtId.Text);
            }

            employee.FullName = txtFullName.Text.Trim();
            employee.Gender = cboGender.SelectedItem?.ToString() ?? "";
            employee.DateOfBirth = dtpDateOfBirth.Value;
            employee.Position = txtPosition.Text.Trim();

            decimal salary = 0;
            if (decimal.TryParse(txtSalary.Text, out salary))
            {
                employee.Salary = salary;
            }

            employee.PhotoPath = selectedPhotoPath;

            // NEW FIELDS
            employee.EmployeeCode = txtEmployeeCode.Text.Trim();
            employee.Email = txtEmail.Text.Trim();
            employee.PhoneNumber = txtPhoneNumber.Text.Trim();
            employee.Address = txtAddress.Text.Trim();
            employee.Notes = txtNotes.Text.Trim();

            // NEW: Get DepartmentId from ComboBox
            var selectedDept = cboDepartment.SelectedItem as dynamic;
            if (selectedDept != null && selectedDept.Value != null)
            {
                employee.DepartmentId = selectedDept.Value;
            }

            // NEW: Hire Date
            employee.HireDate = dtpHireDate.Value;

            // NEW: Status
            employee.Status = cboStatus.SelectedItem?.ToString() ?? "Active";

            return employee;
        }

        /// <summary>
        /// New button click - Prepare form for new employee
        /// </summary>
        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            isNewEmployee = true;
            txtEmployeeCode.Focus();  // Focus on Employee Code for new entry
        }

        /// <summary>
        /// Save button click - Add or update employee
        /// NOTE: Audit logging is now handled automatically in EmployeeBLL
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
                    // CreatedBy will be set automatically from SessionManager in BLL
                    if (employeeBLL.AddEmployee(employee, out message))
                    {
                        MessageBox.Show(message, "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllEmployees();
                        LoadFilterDropdowns();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    // Update existing employee
                    // UpdatedBy will be set automatically from SessionManager in BLL
                    // Audit trail will be logged automatically in BLL
                    if (employeeBLL.UpdateEmployee(employee, out message))
                    {
                        MessageBox.Show(message, "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllEmployees();
                        LoadFilterDropdowns();
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Delete button click - Delete selected employee
        /// NOTE: Audit logging is now handled automatically in EmployeeBLL
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa.", "Cảnh Báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa nhân viên này?\n\n" +
                "Tên: " + txtFullName.Text + "\n" +
                "Mã NV: " + txtEmployeeCode.Text,
                "Xác Nhận Xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int employeeId = Convert.ToInt32(txtId.Text);
                    string message;

                    // Audit trail will be logged automatically in BLL
                    if (employeeBLL.DeleteEmployee(employeeId, out message))
                    {
                        MessageBox.Show(message, "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllEmployees();
                        LoadFilterDropdowns();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa nhân viên: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Refresh button click - Reload all data
        /// UPDATED: Reset Status filter as well
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
            cboFilterStatus.SelectedIndex = 0;  // NEW
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
        /// NOTE: Search now includes Employee Code, Email, Phone in EmployeeDAL
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
                    // Search will now include Employee Code, Email, Phone
                    List<Employee> employees = employeeBLL.SearchEmployees(searchText);
                    BindEmployeesToGrid(employees);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Filter changed event - applies when any filter dropdown changes
        /// UPDATED: Added Status filter
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

                // NEW: Status filter
                string status = cboFilterStatus.SelectedItem?.ToString();
                if (status == "-- All --") status = null;

                // Apply filters (including status)
                if (gender == null && department == null && position == null && status == null)
                {
                    LoadAllEmployees();
                }
                else
                {
                    List<Employee> employees = employeeBLL.FilterEmployees(gender, department, position, status);
                    BindEmployeesToGrid(employees);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lọc dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Clear filter button click
        /// UPDATED: Reset Status filter as well
        /// </summary>
        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            cboFilterGender.SelectedIndex = 0;
            cboFilterDepartment.SelectedIndex = 0;
            cboFilterPosition.SelectedIndex = 0;
            cboFilterStatus.SelectedIndex = 0;  // NEW
            LoadAllEmployees();
        }
    }
}
