using System.Windows.Controls;
using System.Windows.Input;
using System.Net.Mail;
using System.Net;
using System.Windows;
using QuanLyHocSinh.Resources;
using Microsoft.Data.SqlClient;

namespace QuanLyHocSinh.ViewModel
{
    public class ForgotPasswordViewModel: ViewModelBase
    {
		private string _email;
			
		public string Email
		{
			get { return _email; }
			set { _email = value; OnPropertyChanged(); }
		}
		private string _code;

		public string Code
		{
			get { return _code; }
			set { _code = value; OnPropertyChanged(); }
		}
		private string _password;

		public string Password
		{
			get { return _password; }
			set { _password = value; OnPropertyChanged(); }
		}
		private string _confirmPassword;

		public string ConfirmPassword
		{
			get { return _confirmPassword; }
			set { _confirmPassword = value; OnPropertyChanged(); }
		}
        public ICommand SendCodeCommand { get; set; }
		public ICommand SaveNewPasswordCommand { get; set;}
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand ConfirmPasswordChangedCommand { get; set; }
		public ForgotPasswordViewModel()
		{
			Email = "";
			Code = "";
			Password = "";
			ConfirmPassword = "";
            SendCodeCommand = new RelayCommand<object>((p) => {
				return EmailValidate();
            },
                (p) => { SendCode(); });
            SaveNewPasswordCommand = new RelayCommand<Window>((p) => {
                return PasswordValidate();
            },
                (p) => { SaveNewPassword(p); });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            ConfirmPasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { ConfirmPassword = p.Password; });
        }
		public bool EmailValidate()
		{
            if (!string.IsNullOrEmpty(Email))
            {
                var trimmedEmail = Email.Trim();

                if (trimmedEmail.EndsWith("."))
                {
                    return false;
                }
                try
                {
                    var addr = new MailAddress(Email);
                    return addr.Address == trimmedEmail;
                }
                catch
                {
                    return false;
                }
            }
            return false;
		}
		public void SendCode()
		{
            string recoveryCode = GenerateRandomCode();
            if (string.IsNullOrEmpty(recoveryCode)) { return; }
            try
            {
                string senderEmail = "terror.voz@gmail.com";
                string senderPassword = "itsyuxnttlcgxuxz";
                string senderDisplayName = "Quản lý học sinh";
                string subject = "Mã phục hồi mật khẩu";
                string body = $"Mã phục hồi mật khẩu của bạn là: {recoveryCode}";

                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;

                    MailAddress fromAddress = new MailAddress(senderEmail, senderDisplayName);

                    client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                    MailMessage message = new MailMessage();
                    message.From = fromAddress;
                    message.To.Add(Email);
                    message.Subject = subject;
                    message.Body = body;

                    client.Send(message);
                    MessageBox.Show("Đã gửi mã.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private string GenerateRandomCode()
        {
            Random random = new Random();
            string code = random.Next(100000, 999999).ToString();

            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE USERS SET CODE = @CODE WHERE EMAIL = @EMAIL";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@CODE", code);
                        command.Parameters.AddWithValue("@EMAIL", Email);
                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Đã tạo mã khôi phục. Vui lòng đợi.");
                        }
                        else
                        {
                            MessageBox.Show("Địa chỉ email không chính xác!");
                            return string.Empty;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            return code;
        }
        public bool PasswordValidate()
        {
            if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
                return false;
            if (Password == ConfirmPassword)
                return true;
            return false;
        }
        public void SaveNewPassword(Window p)
        {
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    // Example: Execute a query
                    string query = "UPDATE USERS SET CODE = NULL, PASS = @PASS WHERE EMAIL = @EMAIL AND CODE = @CODE";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@PASS", PasswordManager.HashPassword(Password));
                        command.Parameters.AddWithValue("@EMAIL", Email);
                        command.Parameters.AddWithValue("@CODE", Code);
                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Đã lưu mật khẩu mới");
                            p.Close();
                        }
                        else
                        {
                            MessageBox.Show("Vui lòng nhập các trường thông tin đầy đủ và chính xác!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}
