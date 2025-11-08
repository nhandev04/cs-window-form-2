using System;
using System.Windows.Forms;
using WindowsFormsApp1.BLL;
using WindowsFormsApp1.DAL;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Utils;

namespace WindowsFormsApp1
{
    public partial class FormLogin : Form
    {
        private UserBLL userBLL;

        public FormLogin()
        {
            InitializeComponent();
            userBLL = new UserBLL();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            // Set default focus
            txtUsername.Focus();

            // Test connection
            TestDatabaseConnection();
        }

        private void TestDatabaseConnection()
        {
            try
            {
                using (var conn = new System.Data.SqlClient.SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    lblStatus.Text = "Kết nối database thành công!";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Lỗi kết nối database!";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show($"Không thể kết nối đến database:\n\n{ex.Message}\n\nVui lòng kiểm tra:\n" +
                              "1. SQL Server đang chạy\n" +
                              "2. Database 'QuanLyNhanSu' đã được tạo\n" +
                              "3. Connection string trong App.config",
                              "Lỗi Kết Nối",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            try
            {
                btnLogin.Enabled = false;
                btnLogin.Text = "Đang đăng nhập...";
                this.Cursor = Cursors.WaitCursor;

                string message;
                User user = userBLL.Login(username, password, out message);

                if (user != null)
                {
                    // Log audit trail
                    try
                    {
                        AuditLogDAL auditDAL = new AuditLogDAL();
                        auditDAL.AddLog("Users", user.Id, "LOGIN", null, null, null,
                                       $"Đăng nhập thành công từ {SecurityHelper.GetMachineName()}",
                                       user.Username,
                                       SecurityHelper.GetLocalIPAddress(),
                                       SecurityHelper.GetMachineName());
                    }
                    catch { } // Ignore audit errors

                    MessageBox.Show(message, "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Open main form
                    this.Hide();
                    FormMain mainForm = new FormMain();
                    mainForm.FormClosed += (s, args) => {
                        this.Show();
                        ClearForm();
                    };
                    mainForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show(message, "Đăng nhập thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Đăng nhập";
                this.Cursor = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn thoát?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void ClearForm()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Press Enter to login
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(sender, e);
                e.Handled = true;
            }
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Press Enter to move to password
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtPassword.Focus();
                e.Handled = true;
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }
    }
}
