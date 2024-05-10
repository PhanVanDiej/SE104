using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using QuanLyHocSinh.Resources;
using Microsoft.Data.SqlClient;
using QuanLyHocSinh.View;

namespace QuanLyHocSinh.ViewModel
{
    public class LoginViewModel: ViewModelBase
    {
        //Fields
        private string _ID;
        private string _password;
        public bool isLogin { get; private set; } = false;

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
        //Command
        public ICommand OpenForgotPassCommand { get; set; }  
        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand SignUpCommand { get; set; }

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
            SignUpCommand = new RelayCommand<object>((p) => true, (p) => { SignUpView newSignUp = new SignUpView(); newSignUp.Show(); });
        }
        private void ForgotPasswordCommand()
        {
            ForgotPasswordView newView= new ForgotPasswordView();
            newView.Show();
        }
        private void Login(Window p)
        {
            //Test
            MainPageAppView app=new MainPageAppView();
            p.Close();
            app.Show();
            return;
            //-----------------------------
            if (p == null) { return; }
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    // Example: Execute a query
                    string query = "SELECT Pass FROM USERS WHERE ID = @ID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@ID", ID);
                        // Execute the command
                        string? storedHashedPassword = command.ExecuteScalar() as string;
                        if (storedHashedPassword != null && PasswordManager.VerifyPassword(Password, storedHashedPassword))
                        {
                            isLogin = true;
                            p.Close();
                        }
                        else
                        {
                            MessageBox.Show("Thông tin đăng nhập không chính xác!");
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
