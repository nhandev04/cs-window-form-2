using System;
using System.Collections.Generic;
using WindowsFormsApp1.DAL;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Utils;

namespace WindowsFormsApp1.BLL
{
    /// <summary>
    /// Business Logic Layer for User operations
    /// </summary>
    public class UserBLL
    {
        private UserDAL userDAL;

        public UserBLL()
        {
            userDAL = new UserDAL();
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Plain text password</param>
        /// <param name="message">Output message</param>
        /// <returns>User object if login successful, null otherwise</returns>
        public User Login(string username, string password, out string message)
        {
            message = string.Empty;

            // Validate input
            if (string.IsNullOrWhiteSpace(username))
            {
                message = "Vui lòng nhập tên đăng nhập.";
                return null;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                message = "Vui lòng nhập mật khẩu.";
                return null;
            }

            try
            {
                // Hash password
                string passwordHash = SecurityHelper.HashPassword(password);

                // Authenticate
                User user = userDAL.AuthenticateUser(username, passwordHash);

                if (user == null)
                {
                    message = "Tên đăng nhập hoặc mật khẩu không đúng.";
                    return null;
                }

                // Check if user is active
                if (!user.IsActive)
                {
                    message = "Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên.";
                    return null;
                }

                // Update last login
                userDAL.UpdateLastLogin(user.Id);

                // Set session
                SessionManager.Login(user);

                message = $"Đăng nhập thành công! Xin chào {user.FullName}.";
                return user;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi đăng nhập: " + ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Logout current user
        /// </summary>
        public void Logout()
        {
            SessionManager.Logout();
        }

        /// <summary>
        /// Get all users
        /// </summary>
        public List<User> GetAllUsers()
        {
            try
            {
                return userDAL.GetAllUsers();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách người dùng: " + ex.Message);
            }
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        public List<Role> GetAllRoles()
        {
            try
            {
                return userDAL.GetAllRoles();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách vai trò: " + ex.Message);
            }
        }

        /// <summary>
        /// Add new user
        /// </summary>
        public bool AddUser(User user, string password, out string message)
        {
            message = string.Empty;

            // Validate
            if (!ValidateUser(user, out message))
                return false;

            if (string.IsNullOrWhiteSpace(password))
            {
                message = "Mật khẩu không được để trống.";
                return false;
            }

            if (password.Length < 6)
            {
                message = "Mật khẩu phải có ít nhất 6 ký tự.";
                return false;
            }

            // Check username exists
            if (userDAL.UsernameExists(user.Username))
            {
                message = $"Tên đăng nhập '{user.Username}' đã tồn tại.";
                return false;
            }

            try
            {
                // Hash password
                user.PasswordHash = SecurityHelper.HashPassword(password);
                user.CreatedBy = SessionManager.Username;

                int result = userDAL.AddUser(user);

                if (result > 0)
                {
                    message = "Thêm người dùng thành công.";
                    return true;
                }
                else
                {
                    message = "Không thể thêm người dùng.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi khi thêm người dùng: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Update user
        /// </summary>
        public bool UpdateUser(User user, out string message)
        {
            message = string.Empty;

            // Validate
            if (user.Id <= 0)
            {
                message = "ID người dùng không hợp lệ.";
                return false;
            }

            if (!ValidateUser(user, out message))
                return false;

            // Check username exists (exclude current user)
            if (userDAL.UsernameExists(user.Username, user.Id))
            {
                message = $"Tên đăng nhập '{user.Username}' đã được sử dụng bởi người dùng khác.";
                return false;
            }

            try
            {
                int result = userDAL.UpdateUser(user);

                if (result > 0)
                {
                    message = "Cập nhật người dùng thành công.";
                    return true;
                }
                else
                {
                    message = "Không thể cập nhật người dùng.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi khi cập nhật người dùng: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Change password
        /// </summary>
        public bool ChangePassword(int userId, string oldPassword, string newPassword, out string message)
        {
            message = string.Empty;

            if (string.IsNullOrWhiteSpace(oldPassword))
            {
                message = "Vui lòng nhập mật khẩu cũ.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                message = "Vui lòng nhập mật khẩu mới.";
                return false;
            }

            if (newPassword.Length < 6)
            {
                message = "Mật khẩu mới phải có ít nhất 6 ký tự.";
                return false;
            }

            if (oldPassword == newPassword)
            {
                message = "Mật khẩu mới phải khác mật khẩu cũ.";
                return false;
            }

            try
            {
                // Verify old password
                var users = userDAL.GetAllUsers();
                var user = users.Find(u => u.Id == userId);
                if (user == null)
                {
                    message = "Không tìm thấy người dùng.";
                    return false;
                }

                string oldPasswordHash = SecurityHelper.HashPassword(oldPassword);
                if (user.PasswordHash != oldPasswordHash)
                {
                    message = "Mật khẩu cũ không đúng.";
                    return false;
                }

                // Hash new password
                string newPasswordHash = SecurityHelper.HashPassword(newPassword);

                int result = userDAL.ChangePassword(userId, newPasswordHash);

                if (result > 0)
                {
                    message = "Đổi mật khẩu thành công.";
                    return true;
                }
                else
                {
                    message = "Không thể đổi mật khẩu.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi khi đổi mật khẩu: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Reset password to default
        /// </summary>
        public bool ResetPassword(int userId, out string message)
        {
            message = string.Empty;

            try
            {
                // Default password is "123456"
                string defaultPassword = "123456";
                string passwordHash = SecurityHelper.HashPassword(defaultPassword);

                int result = userDAL.ChangePassword(userId, passwordHash);

                if (result > 0)
                {
                    message = $"Reset mật khẩu thành công. Mật khẩu mới: {defaultPassword}";
                    return true;
                }
                else
                {
                    message = "Không thể reset mật khẩu.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi khi reset mật khẩu: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Delete user
        /// </summary>
        public bool DeleteUser(int userId, out string message)
        {
            message = string.Empty;

            if (userId <= 0)
            {
                message = "ID người dùng không hợp lệ.";
                return false;
            }

            // Cannot delete yourself
            if (SessionManager.CurrentUser != null && SessionManager.CurrentUser.Id == userId)
            {
                message = "Bạn không thể xóa chính mình.";
                return false;
            }

            try
            {
                int result = userDAL.DeleteUser(userId);

                if (result > 0)
                {
                    message = "Xóa người dùng thành công.";
                    return true;
                }
                else
                {
                    message = "Không thể xóa người dùng.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi khi xóa người dùng: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Validate user data
        /// </summary>
        private bool ValidateUser(User user, out string message)
        {
            message = string.Empty;

            if (string.IsNullOrWhiteSpace(user.Username))
            {
                message = "Tên đăng nhập không được để trống.";
                return false;
            }

            if (user.Username.Length < 3 || user.Username.Length > 50)
            {
                message = "Tên đăng nhập phải có từ 3-50 ký tự.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.FullName))
            {
                message = "Họ tên không được để trống.";
                return false;
            }

            if (user.FullName.Length < 2 || user.FullName.Length > 100)
            {
                message = "Họ tên phải có từ 2-100 ký tự.";
                return false;
            }

            if (user.RoleId <= 0)
            {
                message = "Vui lòng chọn vai trò.";
                return false;
            }

            return true;
        }
    }
}
