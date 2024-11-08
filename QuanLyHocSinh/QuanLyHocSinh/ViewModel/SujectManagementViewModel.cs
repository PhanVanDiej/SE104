using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using QuanLyHocSinh.Model;
using Microsoft.Data.SqlClient;
using QuanLyHocSinh.Resources;
using DocumentFormat.OpenXml.VariantTypes;
using System.Windows;

namespace QuanLyHocSinh.ViewModel
{
    public class SubjectManagementViewModel : ViewModelBase
    {
        private ObservableCollection<Subject> _list;
        private Subject _selectedItem;
        private string _newSubjectId;
        private string _newSubjectName;

        public ObservableCollection<Subject> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(nameof(List)); }
        }

        public Subject SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                if (SelectedItem != null)
                {
                    NewSubjectId = SelectedItem.Id;
                    NewSubjectName = SelectedItem.SubjectName;
                }
            }
        }

        public string NewSubjectId
        {
            get { return _newSubjectId; }
            set { _newSubjectId = value; OnPropertyChanged(nameof(NewSubjectId)); }
        }

        public string NewSubjectName
        {
            get { return _newSubjectName; }
            set { _newSubjectName = value; OnPropertyChanged(nameof(NewSubjectName)); }
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public SubjectManagementViewModel()
        {
            List = new ObservableCollection<Subject>();
            LoadData(); // Load initial data (for example)
            AddCommand = new RelayCommand<object>((p) =>
            {
                return CanAddExecute();
            }, (p) => AddExecute());
            EditCommand = new RelayCommand<Object>((p) =>
            {
                return CanEditExecute();
            }, (p) => EditExecute());
            DeleteCommand = new RelayCommand<object>((p) =>
            {
                return CanDeleteExecute();
            }, (p) => DeleteExecute());
        }


        private void LoadData()
        {
            var data = new ObservableCollection<Subject>();
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT ID, SUBJECTNAME FROM SUBJECTS", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(new Subject
                        {
                            Id = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                            SubjectName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1)
                        });
                    }
                }
            }
            List = data;
        }

        private void AddExecute()
        {
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO SUBJECTS (ID,SUBJECTNAME) VALUES (@Id, @SubjectName)", connection);
                command.Parameters.AddWithValue("@Id", NewSubjectId);
                command.Parameters.AddWithValue("@SubjectName", NewSubjectName);

                int rowAffected = command.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    List.Add(new Subject
                    {
                        Id = NewSubjectId,
                        SubjectName = NewSubjectName,
                    });
                }
                else
                {
                    MessageBox.Show("Đã xảy ra lỗi khi thêm dữ liệu!");
                }
            }
        }

        private bool CanAddExecute()
        {
            if (string.IsNullOrEmpty(NewSubjectId)||string.IsNullOrEmpty(NewSubjectName))
                return false;
            foreach (var item in List)
            {
                if (item.Id == NewSubjectId)
                {
                    return false;
                }
            }
            return true;
        }

        private void EditExecute()
        {
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE SUBJECTS SET SUBJECTNAME=@SubjectName WHERE ID=@Id", connection);
                command.Parameters.AddWithValue("SubjectName", NewSubjectName);
                command.Parameters.AddWithValue("Id", NewSubjectId);

                int rowAffected = command.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    var item = List.FirstOrDefault(c => c.Id == NewSubjectId);
                    if (item != null)
                    {
                        item.SubjectName = NewSubjectName;
                    }
                }
                else
                {
                    MessageBox.Show("Đã xảy ra lỗi khi cập nhật dữ liệu!");
                }
            }
        }

        private bool CanEditExecute()
        {
            foreach (var item in List)
            {
                if (item.Id == NewSubjectId)
                {
                    return true;
                }
            }
            return false;
        }

        private void DeleteExecute()
        {
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE SUBJECTS WHERE ID=@Id", connection);
                command.Parameters.AddWithValue("@Id", NewSubjectId);

                try
                {
                    int rowAffected = command.ExecuteNonQuery();
                    if (rowAffected > 0)
                    {
                        List.Remove(SelectedItem); // Remove directly using SelectedItem
                        SelectedItem = null; // Clear selection after successful delete
                    }
                    else
                    {
                        MessageBox.Show(
                            "Không tìm thấy môn học để xóa!",
                            "Thông báo",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                }
                catch (SqlException ex)
                {
                    // Handle foreign key violation (error number 547)
                    if (ex.Number == 547)
                    {
                        MessageBox.Show(
                            "Không thể xóa môn học này vì đã được giảng dạy ",
                            "Lỗi",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                    // Handle other SQL-specific errors
                    else
                    {
                        MessageBox.Show(
                            $"Lỗi khi xóa môn học: {ex.Message}",
                            "Lỗi",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }

        private bool CanDeleteExecute()
        {
            foreach (var item in List)
            {
                if (item.Id == NewSubjectId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
