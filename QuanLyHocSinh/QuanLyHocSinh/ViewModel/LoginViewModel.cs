using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using QuanLyHocSinh.Resources;
using Microsoft.Data.SqlClient;

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
        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        // Delegate to store the close window action
        private Action closeAction;
        //Constructor
        public LoginViewModel()
        {
            ID = "";
            Password = "";
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
        }

        private void Login(Window p)
        {
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

        // Function to set the close action from the view
        public void SetCloseAction(Action close)
        {
            closeAction = close;
        }
    }
}
