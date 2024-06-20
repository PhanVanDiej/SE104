using Microsoft.Data.SqlClient;
using QuanLyHocSinh.Model;
using QuanLyHocSinh.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;

namespace QuanLyHocSinh.ViewModel
{
    class LearningManagementViewModel:ViewModelBase
    {
        private ObservableCollection<Learning> _List { get; set; }
        public ObservableCollection<Learning> List
        {
            get { return _List; }
            set { _List = value; OnPropertyChanged(nameof(List)); }
        }

        private ObservableCollection<String> _StudentIdList { get; set; }
        public ObservableCollection<String> StudentIdList
        {
            get { return _StudentIdList; }
            set { _StudentIdList = value; OnPropertyChanged(nameof(StudentIdList)); }
        }

        private String _StudentId { get; set; }
        public String StudentId
        {
            get { return _StudentId; }
            set { _StudentId = value; OnPropertyChanged(nameof(StudentId)); }
        }

        private String _ClassId { get; set; }
        public String ClassId
        {
            get { return _ClassId; } set { _ClassId = value; OnPropertyChanged(nameof(ClassId)); }
        }
        private ObservableCollection<String> _ClassIdList { get; set; }
        public ObservableCollection<String> ClassIdList
        {
            get { return _ClassIdList; }
            set { _ClassIdList = value; OnPropertyChanged(nameof(ClassIdList)); }
        }
        private ObservableCollection<String> _SortClassIdList {  get; set; }
        public ObservableCollection<String> SortClassIdList
        {
            get { return _SortClassIdList; }
            set { _SortClassIdList = value;OnPropertyChanged(nameof(SortClassIdList)); }
        }

        private ObservableCollection<int> _TermList { get; set; }
        public ObservableCollection<int> TermList
        {
            get { return _TermList; }
            set { _TermList = value; OnPropertyChanged(nameof(TermList)); }

        }

        private int? _Term { get; set; }
        public int? Term
        {
            get { return _Term; }
            set { _Term = value; OnPropertyChanged(nameof(Term)); }
        }
        private Decimal? _StudentGPA {  get; set; }
        public Decimal? StudentGPA
        {
            get { return _StudentGPA; }
            set { _StudentGPA = value; OnPropertyChanged(nameof(StudentGPA)); }
        }

        private String _Note { get; set; }
        public String Note
        {
            get { return _Note;}
            set { _Note = value; OnPropertyChanged(nameof(Note));}
        }

        private Learning _SelectedItem { get; set; }
        public Learning SelectedItem
        {
            get { return _SelectedItem; }
            set 
            { 
                _SelectedItem = value; 
                OnPropertyChanged(nameof(SelectedItem)); 
                if(SelectedItem != null)
                {
                    StudentId= SelectedItem.StudentId;
                    ClassId = SelectedItem.ClassId;
                    Term = SelectedItem.Term;
                    StudentGPA = SelectedItem.GPA;
                    Note=SelectedItem.Note;
                }
            }
        }

        private String _sortClassId { get; set; }
        public String SortClassId
        {
            get { return _sortClassId; }
            set 
            { 
                _sortClassId = value; 
                OnPropertyChanged(nameof(SortClassId)); 
                if(SortClassId != null)
                {
                    LoadData();
                }
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public LearningManagementViewModel()
        {

            StudentIdList = LoadStudentId();
            LoadData();
            LoadClassIdList();
            LoadSortClassIdList();
            TermList = LoadTerm();
            AddCommand = new RelayCommand<object>((p) => true, (p) => AddLearningCommand());
            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null) return false;
                foreach (var item in List)
                {
                    if (StudentId == item.StudentId&&ClassId==item.ClassId&&Term==item.Term) return true;
                }
                return false;
            }, (p) => EditLearningCommand());
            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null) return false;
                foreach (var item in List)
                {
                    if (StudentId == item.StudentId && Term == item.Term && ClassId == item.ClassId) return true;
                }
                return false;
            }, (p) => DeleteLearningCommand());
        }
        private void LoadData()
        {
            var data=new ObservableCollection<Learning>();
            using (var connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand();
                if (SortClassId != null && SortClassId != "Tất cả")
                {
                    command = new SqlCommand("SELECT StudentId,ClassId,Term,GPA,Note FROM LEARNING WHERE ClassId=@SortClassId", connection);
                    command.Parameters.AddWithValue("@SortClassId", SortClassId);
                }
                else command = new SqlCommand("SELECT StudentId,ClassId,Term,GPA,Note FROM LEARNING", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var newLearning = new Learning
                        {
                            StudentId=reader.IsDBNull(0)? string.Empty:reader.GetString(0),
                            ClassId=reader.IsDBNull(1)? string.Empty:reader.GetString(1),
                            Term=reader.IsDBNull(2)? null:reader.GetByte(2),
                            GPA=reader.IsDBNull(3)? null:reader.GetDecimal(3),
                            Note=reader.IsDBNull(4)? string.Empty:reader.GetString(4),
                        };
                        data.Add(newLearning);
                    }
                }
            }
            List= data;
            return;
        }
        private ObservableCollection<String> LoadStudentId()
        {
            var listId=new ObservableCollection<String>();
            using (var connection=new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id from STUDENT", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        String item=reader.GetString(0);
                        listId.Add(item);
                    }
                }
            }
            return listId;
        }
        private void LoadClassIdList()
        {
            var ListId = new ObservableCollection<String>();
            using (var connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id FROM CLASS", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        String item = reader.GetString(0);
                        ListId.Add(item);
                    }
                }
            }
            ClassIdList= ListId;
        }
        private void LoadSortClassIdList()
        {
            var ListId = new ObservableCollection<String>();
            ListId.Add("Tất cả");
            using (var connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id FROM CLASS", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        String item = reader.GetString(0);
                        ListId.Add(item);
                    }
                }
            }
            SortClassIdList = ListId;
        }

        private ObservableCollection<int> LoadTerm()
        {
            var data=new ObservableCollection<int>();
            data.Add(1);
            data.Add(2);
            return data;
        }

        private void AddLearningCommand()
        {
            if (!checkedAddCommand())
                return;
            using (SqlConnection connection=new SqlConnection(Data.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO LEARNING (StudentId, ClassId, Term, GPA, Note) VALUES (@StudentId, @ClassId, @Term, @GPA, @Note)";
                    using (SqlCommand  command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentId", StudentId);
                        command.Parameters.AddWithValue("@ClassId", ClassId);
                        command.Parameters.AddWithValue("@Term", Term);
                        command.Parameters.AddWithValue("@GPA", StudentGPA);
                        command.Parameters.AddWithValue("@Note", Note==null? string.Empty:Note);

                        int rowAffected = command.ExecuteNonQuery();

                        if(rowAffected > 0)
                        {
                            List.Add(new Learning
                            {
                                StudentId = _StudentId,
                                ClassId = _ClassId,
                                Term = _Term,
                                GPA = _StudentGPA,
                                Note = _Note,
                            });
                        }
                        else
                        {
                            MessageBox.Show("Lỗi khi thêm dữ liệu!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex.Message);
                }
            }
        }
        private bool checkedAddCommand()
        {
            if (StudentId == null || StudentId == string.Empty) { MessageBox.Show("Thông tin Mã số học sinh bị thiếu!"); return false; }
            if(ClassId==null||ClassId==string.Empty) { MessageBox.Show("Thông tin mã số lớp học bị thiếu!"); return false; }
            if (Term == null) { MessageBox.Show("Thông tin học kì bị thiếu!"); return false; }
            if(StudentGPA==null) { MessageBox.Show("Thông tin GPA học sinh bị thiếu!"); return false; }
            if (StudentGPA > 10 || StudentGPA < 0) { MessageBox.Show("Thông tin GPA học sinh không hợp lệ!"); return false; }
            
            return true;
        }
        private void EditLearningCommand()
        {
            using (SqlConnection connection=new SqlConnection(Data.connectionString))
            {
                try
                {
                    connection.Open();
                    string query="UPDATE LEARNING SET GPA=@EditGPA, Note=@EditNote WHERE StudentId=@StudentId AND ClassId=@ClassId AND Term=@Term";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EditGPA",StudentGPA);
                        command.Parameters.AddWithValue("@EditNote",Note);
                        command.Parameters.AddWithValue("@StudentId",StudentId);
                        command.Parameters.AddWithValue("@ClassId",ClassId);
                        command.Parameters.AddWithValue("@Term",Term);


                        int rowAffected = command.ExecuteNonQuery();
                        if (rowAffected > 0)
                        {
                            var item = List.FirstOrDefault(Stu => Stu.StudentId == StudentId && Stu.ClassId == ClassId && Stu.Term==Term);
                            if(item != null)
                            {
                                item.GPA = StudentGPA;
                                item.Note = Note;
                            }
                            MessageBox.Show("Cập nhật thành công");
                        }
                        else
                        {
                            MessageBox.Show("Lỗi khi cập nhật dữ liệu!");
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error : " + ex.Message);
                }
            }
        }
        private bool checkedEditLearningCommand()
        {
            if (StudentId == null || StudentId == string.Empty) { MessageBox.Show("Thông tin Mã số học sinh bị thiếu!"); return false; }
            if (ClassId == null || ClassId == string.Empty) { MessageBox.Show("Thông tin mã số lớp học bị thiếu!"); return false; }
            if (Term == null) { MessageBox.Show("Thông tin học kì bị thiếu!"); return false; }
            if (StudentGPA == null) { MessageBox.Show("Thông tin GPA học sinh bị thiếu!"); return false; }
            if (StudentGPA > 10 || StudentGPA < 0) { MessageBox.Show("Thông tin GPA học sinh không hợp lệ!"); return false; }

            foreach(var item in List)
            {
                if (((item.StudentId==StudentId&&item.ClassId==ClassId))&&(StudentId!=SelectedItem.StudentId||ClassId!=SelectedItem.ClassId))
                {
                    MessageBox.Show("Phân công học tập này đã tồn tại!");
                    return false;
                }
            }
            return true;
        }
        private void DeleteLearningCommand()
        {
            if (!checkedDeleteCommand()) return;
            using(SqlConnection connection=new SqlConnection(Data.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM LEARNING WHERE StudentId=@StudentId AND ClassId=@ClassId AND Term=@Term";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentId", StudentId);
                        command.Parameters.AddWithValue("@ClassId", ClassId);
                        command.Parameters.AddWithValue("@Term", Term);

                        int rowAffected = command.ExecuteNonQuery();
                        if (rowAffected > 0)
                        {
                            foreach (var item in List)
                            {
                                if (item.StudentId == StudentId && item.ClassId == ClassId && item.Term == Term)
                                {
                                    List.Remove(item);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Đã xảy ra lỗi khi xóa dữ liệu!");
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error : " + ex.Message);
                }
            }
        }
        private bool checkedDeleteCommand()
        {
            DialogResult result = MessageBox.Show("Xóa dữ liệu được chọn?", "", MessageBoxButtons.YesNo);
            if(result==DialogResult.Yes) { return true; }
            return false;
        }
    }
}
