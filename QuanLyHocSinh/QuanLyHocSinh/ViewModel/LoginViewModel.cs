using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using QuanLyHocSinh.Resources;
using Microsoft.Data.SqlClient;
using QuanLyHocSinh.View;
using QuanLyHocSinh.Model;
using DocumentFormat.OpenXml.Math;
namespace QuanLyHocSinh.ViewModel
{
    public class LoginViewModel: ViewModelBase
    {
        //Fields
        private string _ID;
        private string _password;

        //Properties
        public string ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        public bool SimulateDatabaseError { get; set; }
        //Command
        public ICommand OpenForgotPassCommand { get; set; }  
        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        //Constructor
        public LoginViewModel()
        {
            ID = "";
            Password = "";
            LoginCommand = new RelayCommand<Window>((p) => {
                if (string.IsNullOrEmpty(ID) || string.IsNullOrEmpty(Password))
                    return false;
                return true;
            },
                (p) => { Login(p); });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            OpenForgotPassCommand=new RelayCommand<object>((p) => true, (p)=> ForgotPasswordCommand());
        }
        private void ForgotPasswordCommand()
        {
            ForgotPasswordView newView= new ForgotPasswordView();
            newView.ShowDialog();
        }
        public bool CheckLogin()
        {
            if (SimulateDatabaseError)
                return false;
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    // Example: Execute a query
                    string query = "SELECT Pass, Access FROM USERS WHERE ID = @ID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", ID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHashedPassword = reader.GetString(0);
                                string access = reader.GetString(1);

                                if (PasswordManager.VerifyPassword(Password, storedHashedPassword))
                                {
                                    CurrentUser.Instance.UserId = ID;
                                    CurrentUser.Instance.Access = access;
                                    return true;
                                }
                                else
                                {
                                    MessageBox.Show("Thông tin đăng nhập không chính xác!");
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Thông tin đăng nhập không chính xác!");
                                return false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return false;
                }
            }
        }
        private void Login(Window p)
        {
            if (CheckLogin())
            {
                MainPageAppView app = new();
                p.Close();
                app.Show();
            }
        }
    }
}