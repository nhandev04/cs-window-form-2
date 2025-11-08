using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.BLL;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Utils;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private EmployeeBLL employeeBLL;
        private DepartmentBLL departmentBLL;
        private bool isNewEmployee = false;
        private string selectedPhotoPath = null;
        private const string PHOTO_FOLDER = "EmployeePhotos";
        
        // Cache for departments to improve performance
        private List<Department> cachedDepartments = null;

        public Form1()
        {
            InitializeComponent();
            employeeBLL = new EmployeeBLL();
            departmentBLL = new DepartmentBLL();
        }

        /// <summary>
        /// Form load event - Initialize form and load data
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Load data in proper order
                LoadDepartmentDropdown(); // Load departments first
                LoadFilterDropdowns(); // Then load filter dropdowns
                LoadAllEmployees(); // Finally load employee data
                ClearForm(); // Clear form controls
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi khởi tạo form:\n\n" + ex.Message +
                              "\n\nStack Trace:\n" + ex.StackTrace,
                              "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    emp.DepartmentDisplay, // Use the display property that handles both DepartmentName and Department
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
                if (cboFilterGender.Items.Count > 0)
                {
                    cboFilterGender.SelectedIndex = 0;
                }

                // Load Department filter from database using DepartmentBLL with caching
                cboFilterDepartment.Items.Clear();
                cboFilterDepartment.Items.Add(new { Text = "-- All --", Value = (int?)null, DeptName = "" });

                // Use cached departments or load from database
                List<Department> departments = cachedDepartments ?? departmentBLL.GetActiveDepartments();
                if (cachedDepartments == null)
                {
                    cachedDepartments = departments; // Cache for future use
                }

                // Debug: Show department count for filter
                System.Diagnostics.Debug.WriteLine($"Filter: Loaded {departments.Count} departments");

                foreach (var dept in departments)
                {
                    cboFilterDepartment.Items.Add(new
                    {
                        Text = dept.DepartmentName,
                        Value = (int?)dept.Id,
                        DeptName = dept.DepartmentName
                    });
                }

                cboFilterDepartment.DisplayMember = "Text";
                cboFilterDepartment.ValueMember = "Value";

                if (cboFilterDepartment.Items.Count > 0)
                {
                    cboFilterDepartment.SelectedIndex = 0;
                }

                // Load Position filter
                cboFilterPosition.Items.Clear();
                cboFilterPosition.Items.Add("-- All --");
                List<string> positions = employeeBLL.GetPositions();
                foreach (string pos in positions)
                {
                    cboFilterPosition.Items.Add(pos);
                }
                if (cboFilterPosition.Items.Count > 0)
                {
                    cboFilterPosition.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load bộ lọc:\n\n" + ex.Message +
                              "\n\nStack Trace:\n" + ex.StackTrace,
                              "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load departments for employee form dropdown
        /// </summary>
        private void LoadDepartmentDropdown()
        {
            try
            {
                // Ensure cboDepartment exists
                if (cboDepartment == null)
                {
                    MessageBox.Show("ComboBox phòng ban chưa được khởi tạo!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                cboDepartment.Items.Clear();

                // Use cached departments or load from database
                List<Department> departments = cachedDepartments ?? departmentBLL.GetActiveDepartments();
                if (cachedDepartments == null)
                {
                    cachedDepartments = departments; // Cache for future use
                }

                // Debug: Show department count
                System.Diagnostics.Debug.WriteLine($"Loaded {departments.Count} departments from database");

                cboDepartment.Items.Add(new { Text = "-- Chọn phòng ban --", Value = (int?)null, DeptName = "" });

                foreach (var dept in departments)
                {
                    cboDepartment.Items.Add(new
                    {
                        Text = dept.DepartmentName,
                        Value = (int?)dept.Id,
                        DeptName = dept.DepartmentName
                    });
                }

                cboDepartment.DisplayMember = "Text";
                cboDepartment.ValueMember = "Value";

                // Only set SelectedIndex if items exist
                if (cboDepartment.Items.Count > 0)
                {
                    cboDepartment.SelectedIndex = 0;
                }

                // Debug: Show total items in combobox
                System.Diagnostics.Debug.WriteLine($"cboDepartment now has {cboDepartment.Items.Count} items");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh sách phòng ban:\n\n" + ex.Message +
                              "\n\nStack Trace:\n" + ex.StackTrace,
                              "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtSalary.Text = employee.Salary.ToString();

            // Set department dropdown
            SetDepartmentDropdown(employee.DepartmentId, employee.DepartmentDisplay);

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
        /// Set department dropdown selection
        /// </summary>
        private void SetDepartmentDropdown(int? departmentId, string departmentName)
        {
            // Check if cboDepartment exists and has items
            if (cboDepartment == null || cboDepartment.Items.Count == 0)
            {
                return;
            }

            if (departmentId.HasValue)
            {
                // Find by ID first (preferred method)
                for (int i = 0; i < cboDepartment.Items.Count; i++)
                {
                    try
                    {
                        var item = cboDepartment.Items[i] as dynamic;
                        if (item != null && item.Value != null)
                        {
                            // Safely convert the Value to int? to handle both int and int? cases
                           int? itemValue = null;
       if (item.Value is int intVal)
    {
     itemValue = intVal;
         }
      else if (item.Value is int?)
       {
   itemValue = item.Value;
        }

     if (itemValue.HasValue && itemValue.Value == departmentId.Value)
       {
          cboDepartment.SelectedIndex = i;
         return;
           }
 }
   }
    catch (Exception)
  {
      // Continue to next item if this one fails
       continue;
         }
  }
  }

            // Fallback: Find by department name
   if (!string.IsNullOrEmpty(departmentName))
            {
    for (int i = 0; i < cboDepartment.Items.Count; i++)
   {
       try
       {
 var item = cboDepartment.Items[i] as dynamic;
         if (item != null && item.DeptName != null && item.DeptName.ToString() == departmentName)
     {
      cboDepartment.SelectedIndex = i;
         return;
      }
         }
        catch (Exception)
  {
    // Continue to next item if this one fails
        continue;
       }
    }
 }

// Default to "-- Chọn phòng ban --" (index 0)
 if (cboDepartment.Items.Count > 0)
 {
   cboDepartment.SelectedIndex = 0;
  }
   }

        /// <summary>
        /// Clear form controls
        /// </summary>
        private void ClearForm()
        {
            txtId.Text = "";
            txtFullName.Text = "";
  
       // Only set SelectedIndex if items exist
    if (cboGender.Items.Count > 0)
       {
 cboGender.SelectedIndex = -1;
   }
    
            dtpDateOfBirth.Value = DateTime.Now.AddYears(-25);
    txtPosition.Text = "";
          
// Only reset department dropdown if it has items
  if (cboDepartment != null && cboDepartment.Items.Count > 0)
     {
      cboDepartment.SelectedIndex = 0; // Reset to "-- Chọn phòng ban --"
        }
     
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

            // Get department from dropdown with improved null checking
   try
            {
      var selectedDept = cboDepartment.SelectedItem as dynamic;
                if (selectedDept != null && selectedDept.Value != null)
                {
             // Safely convert the Value to int? to handle both int and int? cases
      int? departmentIdValue = null;
 if (selectedDept.Value is int intValue)
           {
                  departmentIdValue = intValue;
     }
           else if (selectedDept.Value is int?)
      {
        departmentIdValue = selectedDept.Value;
        }

      if (departmentIdValue.HasValue && departmentIdValue.Value > 0)
       {
     employee.DepartmentId = departmentIdValue.Value;
           employee.DepartmentName = selectedDept.DeptName?.ToString();
 employee.Department = selectedDept.DeptName?.ToString(); // For backward compatibility
          }
          else
           {
            employee.DepartmentId = null;
     employee.DepartmentName = null;
     employee.Department = null;
 }
     }
     else
        {
   employee.DepartmentId = null;
          employee.DepartmentName = null;
        employee.Department = null;
     }
    }
    catch (Exception)
 {
     // Fallback: set to null if any error occurs
                employee.DepartmentId = null;
        employee.DepartmentName = null;
      employee.Department = null;
         }

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
                        
                        // Clear cache and reload data
             cachedDepartments = null;
   LoadAllEmployees();
     LoadFilterDropdowns();
              LoadDepartmentDropdown();
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
                       
         // Clear cache and reload data
            cachedDepartments = null;
             LoadAllEmployees();
     LoadFilterDropdowns();
    LoadDepartmentDropdown();
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
            // Clear department cache to force reload from database
            cachedDepartments = null;
            
    LoadAllEmployees();
     LoadFilterDropdowns();
   LoadDepartmentDropdown();
        ClearForm();
     txtSearch.Text = "";
     
      // Only set SelectedIndex if items exist for filter ComboBoxes
    if (cboFilterGender.Items.Count > 0)
       {
 cboFilterGender.SelectedIndex = 0;
   }
  if (cboFilterDepartment.Items.Count > 0)
     {
   cboFilterDepartment.SelectedIndex = 0;
        }
       if (cboFilterPosition.Items.Count > 0)
 {
     cboFilterPosition.SelectedIndex = 0;
 }
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

               // Handle department filter with object structure
    string department = null;
           var selectedDept = cboFilterDepartment.SelectedItem as dynamic;
              if (selectedDept != null)
     {
       try
 {
   if (selectedDept.DeptName != null && !string.IsNullOrEmpty(selectedDept.DeptName.ToString()))
           {
    department = selectedDept.DeptName.ToString();
   }
               }
       catch (Exception)
      {
      // Fallback to simple string if object structure fails
    string deptString = cboFilterDepartment.SelectedItem?.ToString();
          if (deptString != "-- All --") department = deptString;
         }
    }

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
      
// Only set SelectedIndex if items exist
      if (cboFilterGender.Items.Count > 0)
       {
   cboFilterGender.SelectedIndex = 0;
  }
        if (cboFilterDepartment.Items.Count > 0)
     {
   cboFilterDepartment.SelectedIndex = 0;
     }
       if (cboFilterPosition.Items.Count > 0)
 {
     cboFilterPosition.SelectedIndex = 0;
 }
       
    LoadAllEmployees();
        }

        /// <summary>
        /// Export Excel button click - Export current employee list to Excel
        /// </summary>
      private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
 // Get current employee list from DataGridView
          List<Employee> employeesToExport = GetCurrentEmployeeList();
     
     if (employeesToExport == null || employeesToExport.Count == 0)
                {
            MessageBox.Show("Không có dữ liệu nhân viên để xuất!", "Thông báo", 
          MessageBoxButtons.OK, MessageBoxIcon.Information);
         return;
            }

        // Show progress
   this.Cursor = Cursors.WaitCursor;
            
              // Export to Excel
    ExcelHelper.ExportEmployeesToExcel(employeesToExport);
      }
       catch (Exception ex)
       {
   MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Lỗi", 
MessageBoxButtons.OK, MessageBoxIcon.Error);
   }
            finally
   {
    this.Cursor = Cursors.Default;
 }
   }

   /// <summary>
        /// Get current employee list from DataGridView
        /// </summary>
        private List<Employee> GetCurrentEmployeeList()
        {
            List<Employee> employees = new List<Employee>();
  
            try
         {
          foreach (DataGridViewRow row in dgvEmployees.Rows)
{
   if (row.Cells["ID"].Value != null)
        {
           int employeeId = Convert.ToInt32(row.Cells["ID"].Value);
            Employee emp = employeeBLL.GetEmployeeById(employeeId);
      if (emp != null)
       {
       employees.Add(emp);
    }
          }
          }
            }
            catch (Exception ex)
            {
       MessageBox.Show($"Lỗi khi lấy danh sách nhân viên: {ex.Message}", "Lỗi", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      
   return employees;
        }
    }
}
