using Microsoft.Data.SqlClient;
using QuanLyHocSinh.Model;
using QuanLyHocSinh.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Navigation;

namespace QuanLyHocSinh.ViewModel
{
    internal class ClassManagementViewModel:ViewModelBase
    {
        private String _ClassId {  get; set; }
        public String ClassId
        {
            get { return _ClassId; }
            set { _ClassId = value; OnPropertyChanged(nameof(ClassId)); }
        }

        private String _ClassName { get; set; }
        public String ClassName
        {
            get { return _ClassName; }
            set { _ClassName = value; OnPropertyChanged(nameof(ClassName)); }
        }

        private ObservableCollection<short> _SchoolYearList { get; set; }
        public ObservableCollection<short> SchoolYearList
        {
            get { return _SchoolYearList; }
            set { _SchoolYearList = value; OnPropertyChanged(nameof(SchoolYearList)); }
        }
        private short? _SchoolYear { get; set; }
        public short? SchoolYear
        {
            get { return _SchoolYear; }
            set { _SchoolYear = value; OnPropertyChanged(nameof(SchoolYear)); }
        }
        private ObservableCollection<Class> _ClassList { get; set; }
        public ObservableCollection<Class> ClassList
        {
            get { return _ClassList; }
            set { _ClassList = value; OnPropertyChanged(nameof(ClassList)); }
        }
        private Class _SelectedItem { get; set; }
        public Class SelectedItem
        {
            get { return _SelectedItem; }
            set { 
                _SelectedItem = value; 
                OnPropertyChanged(nameof(SelectedItem)); 
                if(SelectedItem != null)
                {
                    ClassId= SelectedItem.Id;
                    ClassName= SelectedItem.ClassName;
                    SchoolYear= SelectedItem.SchoolYear;
                }
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand {  get; set; }

        public ClassManagementViewModel()
        {
            LoadData();
            LoadSchoolYearList();
            AddCommand = new RelayCommand<object>((p) => 
            { 
                if (CurrentUser.Instance.Access != "Quản trị viên" ||CurrentUser.Instance.Access!="Phó hiệu trưởng") 
                { 
                    MessageBox.Show("Bạn không có quyền làm điều này."); 
                    return false; 
                } return true; }, (p) => AddClassCommand());
            EditCommand = new RelayCommand<Object>((p) =>
            {
                if (SelectedItem == null) return false;
                if (CurrentUser.Instance.Access != "Quản trị viên" || CurrentUser.Instance.Access != "Phó hiệu trưởng")
                {
                    MessageBox.Show("Bạn không có quyền làm điều này.");
                    return false;
                }
                foreach (var item in ClassList)
                {
                    if (item.Id == ClassId) return true;
                }
                return false;
            }, (p) => EditClassCommand());
            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null) return false;
                if (CurrentUser.Instance.Access != "Quản trị viên" || CurrentUser.Instance.Access != "Phó hiệu trưởng")
                {
                    MessageBox.Show("Bạn không có quyền làm điều này.");
                    return false;
                }
                foreach (var item in ClassList)
                {
                    if (item.Id == ClassId) return true;
                }
                return false;
            }, (p) => DeleteClassCommand());
        }
        private void LoadData()
        {
            var data=new ObservableCollection<Class>();
            using(SqlConnection connection=new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT ID, CLASSNAME, SCHOOLYEAR FROM CLASS", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(new Class
                        {
                            Id = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                            ClassName=reader.IsDBNull(1)? string.Empty: reader.GetString(1),
                            SchoolYear=reader.IsDBNull(2)? null: reader.GetInt16(2),
                        });
                    }
                }
            }
            ClassList = data;
        }
        private void LoadSchoolYearList()
        {
            var list = new ObservableCollection<short>();
            using (SqlConnection connection=new SqlConnection(Data.connectionString)) 
            {
                connection.Open();
                var command = new SqlCommand("SELECT SCHOOLYEAR FROM REGULATION", connection);
                using ( var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetInt16(0));
                    }
                }
            }
            SchoolYearList=list;
        }
        private void AddClassCommand()
        {
            if (!checkedAddCommand()) return;
            using (SqlConnection connection=new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO CLASS (ID, CLASSNAME, SCHOOLYEAR) VALUES (@ClassId, @ClassName,@SchoolYear)", connection);
                command.Parameters.AddWithValue("@ClassId", ClassId);
                command.Parameters.AddWithValue("@ClassName", ClassName);
                command.Parameters.AddWithValue("@SchoolYear", SchoolYear);

                int rowAffected = command.ExecuteNonQuery();
                if(rowAffected > 0)
                {
                    ClassList.Add(new Class
                    {
                        Id = ClassId,
                        ClassName = ClassName,
                        SchoolYear = SchoolYear,
                    });
                }
                else
                {
                    MessageBox.Show("Đã xảy ra lỗi khi thêm dữ liệu!");
                }
            }
        }
        private bool checkedAddCommand()
        {
            if (ClassId == null) { MessageBox.Show("Thông tin mã lớp bị thiếu!"); return false; }
            if(ClassName==null) { MessageBox.Show("Thông tin tên lớp bị thiếu!"); return false; }
            if (SchoolYear == null) { MessageBox.Show("Thông tin năm học của lớp bị thiếu!"); return false; }

            foreach(var item in ClassList)
            {
                if(item.Id == ClassId)
                {
                    MessageBox.Show("Lớp đã tồn tại!");
                    return false;
                }
            }
            return true;
        }
        private void EditClassCommand()
        {
            if (!checkedEditCommand()) return;
            using (SqlConnection connection=new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE CLASS SET CLASSNAME=@ClassName, SCHOOLYEAR=@SchoolYear WHERE ID=@ClassId", connection);
                command.Parameters.AddWithValue("@ClassName", ClassName);
                command.Parameters.AddWithValue("@SchoolYear", SchoolYear);
                command.Parameters.AddWithValue("@ClassId", ClassId);

                int rowAffected = command.ExecuteNonQuery();
                if (rowAffected > 0) {
                    var item = ClassList.FirstOrDefault(c => c.Id == ClassId);
                    if(item != null)
                    {
                        item.ClassName= ClassName;
                        item.SchoolYear= SchoolYear;
                    }
                }
                else
                {
                    MessageBox.Show("Đã xảy ra lỗi khi cập nhật dữ liệu!");
                }
            }
        }
        private bool checkedEditCommand()
        {
            if (ClassId == null) { MessageBox.Show("Thông tin mã lớp bị thiếu!"); return false; }
            if (ClassName == null) { MessageBox.Show("Thông tin tên lớp bị thiếu!"); return false; }
            if (SchoolYear == null) { MessageBox.Show("Thông tin năm học của lớp bị thiếu!"); return false; }

            if (ClassId != SelectedItem.Id) { MessageBox.Show("Không thể thay đổi Mã lớp!"); return false; }
            return true;
        }
        private void DeleteClassCommand()
        {
            if (!checkedDeleteCommand()) return;
            using (SqlConnection connection=new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE CLASS WHERE ID=@ClassId", connection);
                command.Parameters.AddWithValue("@ClassId", ClassId);
                
                int rowAffected=command.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    var item = ClassList.FirstOrDefault(s=>s.Id == ClassId);
                    if(item != null)
                    {
                        ClassList.Remove(item);
                    }
                }
                else
                {
                    MessageBox.Show("Đã xảy ra lỗi khi xóa dữ liệu!");
                }
            }
        }
        private bool checkedDeleteCommand()
        {
            DialogResult dialog = MessageBox.Show("Xóa dữ liệu được chọn?", "", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes) { return true; }
            return false;
        }
    }
}
