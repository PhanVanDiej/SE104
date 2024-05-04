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
        private string _username;
        private string _password;
        public bool isLogin { get; private set; } = false;

        //Properties
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
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
            Username = "";
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
                    string query = "SELECT 1 FROM TB WHERE Username = @Username AND Pass = @Password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@Username", Username);
                        command.Parameters.AddWithValue("@Password", Password);

                        // Execute the command
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            isLogin = true;
                            p.Close();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect login information!");
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
