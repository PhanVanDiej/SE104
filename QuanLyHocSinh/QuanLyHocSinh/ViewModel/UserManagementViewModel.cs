using Microsoft.Data.SqlClient;
using QuanLyHocSinh.Model;
using QuanLyHocSinh.Resources;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace QuanLyHocSinh.ViewModel
{
    public class UserManagementViewModel: ViewModelBase
    {
        private ObservableCollection<User> _List;
        public ObservableCollection<User> List { get => _List; set { _List = value; OnPropertyChanged(); } }
        private User _SelectedItem;
        public User SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    ID = SelectedItem.ID;
                    FullName = SelectedItem.FullName;
                    Password = SelectedItem.Password;
                    Email = SelectedItem.Email;
                    Access = SelectedItem.Access;
                }
            }
        }
        private string _ID;
        public string ID { get => _ID; set { _ID = value; OnPropertyChanged(); } }
        private string _FullName;
        public string FullName { get => _FullName; set { _FullName = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }
        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { _Email = value; OnPropertyChanged(); }
        }
        private string _Access;

        public string Access
        {
            get { return _Access; }
            set { _Access = value; OnPropertyChanged(); }
        }
        private ObservableCollection<string> _accessLevels;
        public ObservableCollection<string> AccessLevels
        {
            get { return _accessLevels; }
            set { _accessLevels = value; OnPropertyChanged(); }
        }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public UserManagementViewModel() 
        {
            AccessLevels = LoadAccessLevels();
            List = LoadData();
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            AddCommand = new RelayCommand<object>((p) =>
            {
                if (!EmailCheck.Validate(Email)) return false;
                foreach (var item in List)
                {
                    if (ID == item.ID)
                        return false;
                }
                return true;
            }, (p) =>
            {
                AddData();
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                if (!EmailCheck.Validate(Email)) return false;
                foreach (var item in List)
                {
                    if (item.ID == SelectedItem.ID) return true;
                }
                return false;
            }, (p) =>
            {
                EditData();
            });
            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                foreach (var item in List)
                {
                    if (item.ID == SelectedItem.ID) return true;
                }
                return false;
            }, (p) =>
            {
                DeleteData();
            });
        }

        private ObservableCollection<string> LoadAccessLevels()
        {
            var data = new ObservableCollection<string>();
            data.Add("Quản trị viên");
            data.Add("Phó hiệu trưởng");
            data.Add("Giáo vụ");
            data.Add("Giáo viên");
            return data;
        }

        private ObservableCollection<User> LoadData()
        {
            var data = new ObservableCollection<User>();
            using (var connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT ID, Pass, FullName, Email, Access FROM USERS", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new User
                        {
                            ID = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                            Password = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            FullName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Access = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        };
                        data.Add(user);
                    }
                }
            }
            return data;
        }
        private void AddData()
        {
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO USERS (ID, PASS, FULLNAME, EMAIL, ACCESS) VALUES (@ID, @Password, @FullName, @Email, @Access)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", ID);
                        command.Parameters.AddWithValue("@Password", PasswordManager.HashPassword(Password));
                        command.Parameters.AddWithValue("@FullName", FullName);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Access", Access);

                        int rowsAffected = command.ExecuteNonQuery();

                        //if (rowsAffected > 0)
                        //{
                        //    MessageBox.Show("Thêm thành công!");
                        //}
                        //else
                        //{
                        //    MessageBox.Show("Lỗi khi thêm dữ liệu!"); 
                        //}
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            List.Add(new User
            {
                ID = _ID,
                Password = PasswordManager.HashPassword(_Password),
                FullName = _FullName,
                Email = _Email,
                Access = _Access,
            });
        }
        private void EditData()
        {
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE USERS SET PASS =  @Password, FULLNAME = @FullName, EMAIL = @Email, ACCESS = @Access WHERE ID = @ID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", ID);
                        command.Parameters.AddWithValue("@Password", PasswordManager.HashPassword(Password));
                        command.Parameters.AddWithValue("@FullName", FullName);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Access", Access);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            //MessageBox.Show("Cập nhật thành công!");
                            var item = List.FirstOrDefault(u => u.ID == ID);
                            if (item != null)
                            {
                                item.Password = Password;
                                item.FullName = FullName;
                                item.Email = Email;
                                item.Access = Access;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Lỗi khi cập nhật dữ liệu!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void DeleteData()
        {
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "DELETE FROM USERS WHERE ID = @ID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", SelectedItem.ID);

                        int rowsAffected = command.ExecuteNonQuery();

                        //if (rowsAffected > 0)
                        //{
                        //    MessageBox.Show("Xóa thành công");
                        //}
                        //else
                        //{
                        //    MessageBox.Show("Lỗi khi xóa dữ liệu!");
                        //}
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
            foreach (var item in List)
            {
                if (item.ID == SelectedItem.ID)
                {
                    List.Remove(item);
                    return;
                }
            }
        }
    }
}
