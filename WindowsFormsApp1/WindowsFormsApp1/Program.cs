using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Add error handling to catch startup errors
            try
            {
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                // Show detailed error message
                string errorMessage = "LỖI KHỞI ĐỘNG ỨNG DỤNG:\n\n" +
                                    "Lỗi: " + ex.Message + "\n\n" +
                                    "Chi tiết:\n" + ex.ToString() + "\n\n" +
                                    "Vui lòng kiểm tra:\n" +
                                    "1. SQL Server đang chạy\n" +
                                    "2. Database 'QuanLyNhanSu' đã được tạo\n" +
                                    "3. Bảng 'Employees' đã tồn tại\n" +
                                    "4. Connection string trong App.config đúng";

                MessageBox.Show(errorMessage, "Lỗi Khởi Động",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
