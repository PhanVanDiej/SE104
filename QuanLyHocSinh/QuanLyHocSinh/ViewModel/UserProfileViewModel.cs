﻿using Microsoft.Data.SqlClient;
using QuanLyHocSinh.Model;
using QuanLyHocSinh.Resources;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;
using UserControl = System.Windows.Controls.UserControl;
namespace QuanLyHocSinh.ViewModel
{
    public class UserProfileViewModel : ViewModelBase
    {
        private string _oldHashedPass;
        public string OldHashedPass
        {
            get { return _oldHashedPass; }
            set { _oldHashedPass = value; OnPropertyChanged(); }
        }
        private string _oldPass;
        public string OldPass
        {
            get { return _oldPass; }
            set { _oldPass = value; OnPropertyChanged(); }
        }
        private string _newPass;
        public string NewPass
        {
            get { return _newPass; }
            set { _newPass = value; OnPropertyChanged(); }
        }
        private string _confirmNewPass;
        public string ConfirmNewPass
        {
            get { return _confirmNewPass; }
            set { _confirmNewPass = value; OnPropertyChanged(); }
        }
        private string _fullName;

        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; OnPropertyChanged(); }
        }
        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }
        public ICommand OldPasswordChangedCommand { get; set; }
        public ICommand NewPasswordChangedCommand { get; set; }
        public ICommand ConfirmNewPasswordChangedCommand { get; set; }
        public ICommand SaveNewEmailCommand { get; set; }
        public ICommand SaveNewPassCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public UserProfileViewModel()
        {
            OldHashedPass = OldPass = NewPass = ConfirmNewPass = FullName = Email = string.Empty;
            LoadData();
            OldPasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { OldPass = p.Password; });
            NewPasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { NewPass = p.Password; });
            ConfirmNewPasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { ConfirmNewPass = p.Password; });
            SaveNewEmailCommand = new RelayCommand<object>((p) => CanSaveNewEmail(), (p) => SaveNewEmail());
            SaveNewPassCommand = new RelayCommand<object>((p) => CanSaveNewPass(), (p) => SaveNewPass());
            LogoutCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => LogOut(p));
        }

        private void LoadData()
        {
            if (string.IsNullOrEmpty(CurrentUser.Instance.UserId)) return;
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Pass, FullName, Email FROM USERS WHERE ID = @ID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", CurrentUser.Instance.UserId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                OldHashedPass = reader.GetString(0);
                                FullName = reader.GetString(1);
                                Email = reader.GetString(2);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private bool CanSaveNewEmail()
        {
            if (!EmailCheck.Validate(Email)) return false;
            return true;
        }
        private bool CanSaveNewPass()
        {
            if (string.IsNullOrEmpty(OldPass) || !PasswordManager.VerifyPassword(OldPass, OldHashedPass))
                return false;
            if (string.IsNullOrEmpty(NewPass) || string.IsNullOrEmpty(ConfirmNewPass) || NewPass != ConfirmNewPass)
                return false;
            return true;
        }
        private void SaveNewEmail()
        {
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE USERS SET FullName = @FullName, Email = @Email WHERE ID = @ID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", CurrentUser.Instance.UserId);
                        command.Parameters.AddWithValue("@FullName", FullName);
                        command.Parameters.AddWithValue("@Email", Email);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Đã cập nhật thông tin!");
                        }
                        else
                        {
                            MessageBox.Show("Lỗi khi cập nhật thông tin!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void SaveNewPass()
        {
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE USERS SET Pass = @Pass WHERE ID = @ID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", CurrentUser.Instance.UserId);
                        command.Parameters.AddWithValue("@NewPass", PasswordManager.HashPassword(NewPass));
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Đã cập nhật mật khẩu");
                        }
                        else
                        {
                            MessageBox.Show("Lỗi khi cập nhật mật khẩu!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void LogOut(UserControl p)
        {
            DialogResult dialog = MessageBox.Show("Bạn có muốn Đăng xuất tài khoản?", "", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.No) return;
            var returnLogin = new Login();
            var curWindow = Window.GetWindow(p);
            if (curWindow != null)
            {
                CurrentUser.Instance.UserId = null;
                CurrentUser.Instance.Access = null;
                Window window = curWindow as Window;
                window.Close();
            }
            returnLogin.Show();
        }
    }
}
