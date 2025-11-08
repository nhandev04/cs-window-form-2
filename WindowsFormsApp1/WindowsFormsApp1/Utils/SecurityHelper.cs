using System;
using System.Security.Cryptography;
using System.Text;

namespace WindowsFormsApp1.Utils
{
    /// <summary>
    /// Security Helper - Mã hóa mật khẩu
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
        /// Hash password using SHA256
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <returns>Hashed password</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder result = new StringBuilder();
                foreach (byte b in hash)
                {
                    result.Append(b.ToString("X2"));
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// Verify password against hash
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <param name="hash">Stored hash</param>
        /// <returns>True if match</returns>
        public static bool VerifyPassword(string password, string hash)
        {
            string hashedInput = HashPassword(password);
            return string.Equals(hashedInput, hash, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Get client machine name
        /// </summary>
        public static string GetMachineName()
        {
            try
            {
                return Environment.MachineName;
            }
            catch
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// Get local IP address
        /// </summary>
        public static string GetLocalIPAddress()
        {
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "127.0.0.1";
            }
            catch
            {
                return "127.0.0.1";
            }
        }
    }
}
