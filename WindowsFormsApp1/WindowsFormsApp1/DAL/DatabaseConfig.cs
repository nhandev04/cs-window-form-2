using System.Configuration;

namespace WindowsFormsApp1.DAL
{
    /// <summary>
    /// Database configuration helper class
    /// Provides connection string from App.config
    /// </summary>
    public static class DatabaseConfig
    {
        /// <summary>
        /// Gets the connection string from App.config
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["EmployeeDBConnection"].ConnectionString;
            }
        }
    }
}
