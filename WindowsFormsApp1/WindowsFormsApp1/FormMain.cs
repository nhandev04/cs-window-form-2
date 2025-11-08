using System;
using System.Windows.Forms;
using WindowsFormsApp1.Utils;

namespace WindowsFormsApp1
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsLoggedIn)
            {
                MessageBox.Show("Vui lòng đăng nhập!", "Thông Báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            this.Text = $"Employee Management System - {SessionManager.FullName} ({SessionManager.Role})";
            this.WindowState = FormWindowState.Maximized;

            ConfigureMenuByRole();
        }

        private void ConfigureMenuByRole()
        {
            if (SessionManager.Role == "Employee")
            {
                menuItemEmployees.Enabled = false;
                menuItemDepartments.Enabled = false;
                menuItemAttendance.Text = "My Attendance";
            }
        }

        private void menuItemEmployees_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Form1());
        }

        private void menuItemDepartments_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormDepartment());
        }

        private void menuItemAttendance_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormAttendance());
        }

        private void menuItemLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác Nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SessionManager.Logout();

                FormLogin loginForm = new FormLogin();
                loginForm.Show();
                this.Close();
            }
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát?",
                "Xác Nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Employee Management System\n\n" +
                "Version: 1.0\n" +
                "Developed with C# Windows Forms\n\n" +
                $"Current User: {SessionManager.FullName}\n" +
                $"Role: {SessionManager.Role}",
                "About",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void OpenChildForm(Form childForm)
        {
            foreach (Form form in this.MdiChildren)
            {
                if (form.GetType() == childForm.GetType())
                {
                    form.Activate();
                    childForm.Dispose();
                    return;
                }
            }

            childForm.MdiParent = this;
            childForm.Show();
        }
    }
}
