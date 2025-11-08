namespace WindowsFormsApp1
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuItemManagement = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemEmployees = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDepartments = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAttendance = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            //
            // menuStrip
            //
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemManagement,
            this.menuItemSystem,
            this.menuItemHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1200, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            //
            // menuItemManagement
            //
            this.menuItemManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemEmployees,
            this.menuItemDepartments,
            this.menuItemAttendance});
            this.menuItemManagement.Name = "menuItemManagement";
            this.menuItemManagement.Size = new System.Drawing.Size(90, 20);
            this.menuItemManagement.Text = "Management";
            //
            // menuItemEmployees
            //
            this.menuItemEmployees.Name = "menuItemEmployees";
            this.menuItemEmployees.Size = new System.Drawing.Size(180, 22);
            this.menuItemEmployees.Text = "Employees";
            this.menuItemEmployees.Click += new System.EventHandler(this.menuItemEmployees_Click);
            //
            // menuItemDepartments
            //
            this.menuItemDepartments.Name = "menuItemDepartments";
            this.menuItemDepartments.Size = new System.Drawing.Size(180, 22);
            this.menuItemDepartments.Text = "Departments";
            this.menuItemDepartments.Click += new System.EventHandler(this.menuItemDepartments_Click);
            //
            // menuItemAttendance
            //
            this.menuItemAttendance.Name = "menuItemAttendance";
            this.menuItemAttendance.Size = new System.Drawing.Size(180, 22);
            this.menuItemAttendance.Text = "Attendance";
            this.menuItemAttendance.Click += new System.EventHandler(this.menuItemAttendance_Click);
            //
            // menuItemSystem
            //
            this.menuItemSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemLogout,
            this.menuItemExit});
            this.menuItemSystem.Name = "menuItemSystem";
            this.menuItemSystem.Size = new System.Drawing.Size(57, 20);
            this.menuItemSystem.Text = "System";
            //
            // menuItemLogout
            //
            this.menuItemLogout.Name = "menuItemLogout";
            this.menuItemLogout.Size = new System.Drawing.Size(180, 22);
            this.menuItemLogout.Text = "Logout";
            this.menuItemLogout.Click += new System.EventHandler(this.menuItemLogout_Click);
            //
            // menuItemExit
            //
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.Size = new System.Drawing.Size(180, 22);
            this.menuItemExit.Text = "Exit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            //
            // menuItemHelp
            //
            this.menuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAbout});
            this.menuItemHelp.Name = "menuItemHelp";
            this.menuItemHelp.Size = new System.Drawing.Size(44, 20);
            this.menuItemHelp.Text = "Help";
            //
            // menuItemAbout
            //
            this.menuItemAbout.Name = "menuItemAbout";
            this.menuItemAbout.Size = new System.Drawing.Size(180, 22);
            this.menuItemAbout.Text = "About";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            //
            // statusStrip
            //
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 678);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1200, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            //
            // toolStripStatusLabel
            //
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Ready";
            //
            // FormMain
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Employee Management System";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuItemManagement;
        private System.Windows.Forms.ToolStripMenuItem menuItemEmployees;
        private System.Windows.Forms.ToolStripMenuItem menuItemDepartments;
        private System.Windows.Forms.ToolStripMenuItem menuItemAttendance;
        private System.Windows.Forms.ToolStripMenuItem menuItemSystem;
        private System.Windows.Forms.ToolStripMenuItem menuItemLogout;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
    }
}
