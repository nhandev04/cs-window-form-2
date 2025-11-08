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
    public partial class FormAttendance : Form
    {
        private AttendanceBLL attendanceBLL;
        private EmployeeDAL employeeDAL;
        private int currentEmployeeId = 0;

        public FormAttendance()
        {
            InitializeComponent();
            attendanceBLL = new AttendanceBLL();
            employeeDAL = new EmployeeDAL();
        }

        private void FormAttendance_Load(object sender, EventArgs e)
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

                this.Text = $"Attendance Management - User: {SessionManager.FullName}";

                LoadEmployees();
                LoadAllAttendance();

                dtpFrom.Value = DateTime.Today.AddDays(-30);
                dtpTo.Value = DateTime.Today;

                CheckPermissions();
                CheckTodayStatus();
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
                btnDelete.Enabled = false;
            }
        }

        private void LoadEmployees()
        {
            try
            {
                List<Employee> employees = employeeDAL.GetAllEmployees();

                cboEmployee.Items.Clear();
                cboEmployee.Items.Add(new { Text = "-- Tất cả nhân viên --", Value = 0 });

                foreach (var emp in employees.Where(e => e.Status == "Active"))
                {
                    cboEmployee.Items.Add(new
                    {
                        Text = $"{emp.FullName} ({emp.EmployeeCode ?? emp.Id.ToString()})",
                        Value = emp.Id
                    });
                }

                cboEmployee.DisplayMember = "Text";
                cboEmployee.ValueMember = "Value";
                cboEmployee.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh sách nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAllAttendance()
        {
            try
            {
                List<Attendance> attendances = attendanceBLL.GetAllAttendance();
                BindAttendanceToGrid(attendances);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách chấm công: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindAttendanceToGrid(List<Attendance> attendances)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Employee Code", typeof(string));
            dt.Columns.Add("Employee Name", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Check In", typeof(string));
            dt.Columns.Add("Check Out", typeof(string));
            dt.Columns.Add("Work Hours", typeof(string));
            dt.Columns.Add("Status", typeof(string));

            foreach (var att in attendances)
            {
                dt.Rows.Add(
                    att.Id,
                    att.EmployeeCode ?? "",
                    att.EmployeeName,
                    att.AttendanceDate.ToString("dd/MM/yyyy"),
                    att.CheckInTime.ToString("HH:mm"),
                    att.CheckOutTime?.ToString("HH:mm") ?? "--:--",
                    att.WorkingHours.HasValue ? $"{att.WorkingHours.Value:F2}h" : "-",
                    att.Status
                );
            }

            dgvAttendance.DataSource = dt;

            if (dgvAttendance.Columns.Count > 0)
            {
                dgvAttendance.Columns["ID"].Width = 50;
                dgvAttendance.Columns["Employee Code"].Width = 100;
                dgvAttendance.Columns["Employee Name"].Width = 150;
                dgvAttendance.Columns["Date"].Width = 90;
                dgvAttendance.Columns["Check In"].Width = 80;
                dgvAttendance.Columns["Check Out"].Width = 80;
                dgvAttendance.Columns["Work Hours"].Width = 80;
                dgvAttendance.Columns["Status"].Width = 80;
            }

            lblTotal.Text = $"Tổng: {attendances.Count} bản ghi";
        }

        private void CheckTodayStatus()
        {
            try
            {
                var selectedEmp = cboEmployee.SelectedItem as dynamic;
                if (selectedEmp != null && selectedEmp.Value > 0)
                {
                    currentEmployeeId = selectedEmp.Value;

                    Attendance today = attendanceBLL.GetTodayAttendance(currentEmployeeId);

                    if (today == null)
                    {
                        lblStatus.Text = "Chưa check-in hôm nay";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        btnCheckIn.Enabled = true;
                        btnCheckOut.Enabled = false;
                    }
                    else if (today.CheckOutTime == null)
                    {
                        lblStatus.Text = $"Đã check-in lúc {today.CheckInTime:HH:mm}";
                        lblStatus.ForeColor = System.Drawing.Color.Orange;
                        btnCheckIn.Enabled = false;
                        btnCheckOut.Enabled = true;
                    }
                    else
                    {
                        lblStatus.Text = $"Đã check-out lúc {today.CheckOutTime?.ToString("HH:mm")} ({today.WorkingHours:F2}h)";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                        btnCheckIn.Enabled = false;
                        btnCheckOut.Enabled = false;
                    }
                }
                else
                {
                    lblStatus.Text = "Chọn nhân viên để check-in/out";
                    lblStatus.ForeColor = System.Drawing.Color.Gray;
                    btnCheckIn.Enabled = false;
                    btnCheckOut.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckTodayStatus();
            FilterAttendance();
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentEmployeeId == 0)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên.", "Cảnh Báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string message;
                if (attendanceBLL.CheckIn(currentEmployeeId, out message, txtNotes.Text.Trim()))
                {
                    MessageBox.Show(message, "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAllAttendance();
                    FilterAttendance();
                    CheckTodayStatus();
                    txtNotes.Clear();
                }
                else
                {
                    MessageBox.Show(message, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentEmployeeId == 0)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên.", "Cảnh Báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string message;
                if (attendanceBLL.CheckOut(currentEmployeeId, out message))
                {
                    MessageBox.Show(message, "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAllAttendance();
                    FilterAttendance();
                    CheckTodayStatus();
                }
                else
                {
                    MessageBox.Show(message, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            FilterAttendance();
        }

        private void FilterAttendance()
        {
            try
            {
                List<Attendance> attendances;

                var selectedEmp = cboEmployee.SelectedItem as dynamic;
                int empId = selectedEmp != null ? selectedEmp.Value : 0;

                if (empId > 0)
                {
                    attendances = attendanceBLL.GetAttendanceByEmployee(empId);
                }
                else
                {
                    attendances = attendanceBLL.GetAttendanceByDateRange(dtpFrom.Value, dtpTo.Value);
                }

                attendances = attendances.Where(a => a.AttendanceDate >= dtpFrom.Value.Date && a.AttendanceDate <= dtpTo.Value.Date).ToList();

                BindAttendanceToGrid(attendances);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lọc: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAllAttendance();
            cboEmployee.SelectedIndex = 0;
            dtpFrom.Value = DateTime.Today.AddDays(-30);
            dtpTo.Value = DateTime.Today;
            txtNotes.Clear();
            CheckTodayStatus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAttendance.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn bản ghi cần xóa.", "Cảnh Báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa bản ghi chấm công này?",
                "Xác Nhận Xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(dgvAttendance.SelectedRows[0].Cells["ID"].Value);
                    string message;

                    if (attendanceBLL.DeleteAttendance(id, out message))
                    {
                        MessageBox.Show(message, "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllAttendance();
                        FilterAttendance();
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
