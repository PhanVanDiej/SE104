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
    internal class TeachingManagementViewModel:ViewModelBase
    {
        private ObservableCollection<Teaching> _List { get; set; }
        public ObservableCollection<Teaching> List
        {
            get { return _List; }
            set { _List = value; OnPropertyChanged(nameof(List)); }
        }

        private Teaching _SelectedItem { get; set; }
        public Teaching SelectedItem
        {
            get { return _SelectedItem; }
            set 
            { 
                _SelectedItem = value; 
                OnPropertyChanged(nameof(SelectedItem));
                if(SelectedItem != null)
                {
                    TeacherId = SelectedItem.TeacherId;
                    ClassId=SelectedItem.ClassId;
                    SubjectId= SelectedItem.SubjectId;
                    Term=SelectedItem.Term;
                }
            }
        }

        private ObservableCollection<Teaching> _RealList { get; set; }
        public ObservableCollection<Teaching> RealList
        {
            get { return _RealList; }
            set { _RealList = value; OnPropertyChanged(nameof(RealList)); }
        }

        private ObservableCollection<String> _TeacherIdList { get; set; }
        public ObservableCollection<String> TeacherIdList
        {
            get { return _TeacherIdList; }
            set { _TeacherIdList = value; OnPropertyChanged(nameof(TeacherIdList)); }
        }
        private ObservableCollection<String> _SortTeacherIdList { get; set; }
        public ObservableCollection<String> SortTeacherIdList
        {
            get { return _SortTeacherIdList; }
            set { _SortTeacherIdList = value; OnPropertyChanged(nameof(SortTeacherIdList)); }
        }

        private string _sortteacherId {  get; set; }
        public string SortteacherId
        {
            get { return _sortteacherId; }
            set { _sortteacherId = value; OnPropertyChanged(nameof(SortteacherId)); if(SortteacherId!=null) LoadViewData(); }
        }
        private Teaching _SelectedTeaching { get; set; }
        public Teaching SelectedTeaching
        {
            get { return _SelectedTeaching; }
            set { _SelectedTeaching = value; OnPropertyChanged(nameof(SelectedTeaching));}
        }
        
        private String _TeacherId { get; set; }
        public String TeacherId
        {
            get { return _TeacherId; }
            set { _TeacherId = value; OnPropertyChanged(nameof(TeacherId)); }
        }

        private ObservableCollection<String> _ClassIdList { get; set; }
        public ObservableCollection<String> ClassIdList
        {
            get { return _ClassIdList; }
            set { _ClassIdList = value; OnPropertyChanged(nameof(ClassIdList)); }
        }
        private ObservableCollection<String> _SortClassIdList { get; set; }
        public ObservableCollection<String> SortClassIdList
        {
            get { return _SortClassIdList; }
            set { _SortClassIdList = value; OnPropertyChanged(nameof(SortClassIdList)); }
        }
        private string _sortClassId { get; set; }
        public string SortClassId
        {
            get { return _sortClassId; }
            set { _sortClassId = value; OnPropertyChanged(nameof(SortClassId)); if(SortClassId!=null) LoadViewData(); }
        }
        private String _ClassId { get; set; }
        public String ClassId
        {
            get { return _ClassId; }
            set { _ClassId=value; OnPropertyChanged(nameof(ClassId));}
        }

        private ObservableCollection<String> _SubjectIdList { get; set; }
        public ObservableCollection<String> SubjectIdList
        {
            get { return _SubjectIdList; }
            set { _SubjectIdList = value; OnPropertyChanged(nameof(SubjectIdList)); }
        }
        private ObservableCollection<String> _SortSubjectIdList { get; set; }
        public ObservableCollection<String> SortSubjectIdList
        {
            get { return _SortSubjectIdList; }
            set { _SortSubjectIdList = value; OnPropertyChanged(nameof(SortSubjectIdList)); }
        }
        private string _sortSubId { get; set; }
        public string SortSubId
        {
            get { return _sortSubId; }
            set { _sortSubId = value; OnPropertyChanged(nameof(SortSubId)); if(SortSubId!=null) LoadViewData(); }
        }
        private String _SubjectId;
        public String SubjectId
        {
            get { return _SubjectId; }
            set { _SubjectId = value; OnPropertyChanged(nameof(SubjectId)); }
        }

        private ObservableCollection<int> _TermList {  get; set; }
        public ObservableCollection<int> TermList
        {
            get { return _TermList; }
            set { _TermList = value; OnPropertyChanged(nameof(TermList));}
        }
        private ObservableCollection<string> _SortTermList { get; set; }
        public ObservableCollection<string> SortTermList
        {
            get { return _SortTermList; }
            set { _SortTermList = value; OnPropertyChanged(nameof(SortTermList)); }
        }
        private string _sortTermId { get; set; }
        public string SortTermId
        {
            get { return _sortTermId; }
            set { _sortTermId = value; OnPropertyChanged(nameof(SortTermId)); if(SortTermId!=null) LoadViewData(); }
        }
        private int? _Term {  get; set; }
        public int? Term
        {
            get { return _Term; }
            set { _Term = value; OnPropertyChanged(nameof(Term)); }
        }

        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand {  get; set; }
        public TeachingManagementViewModel()
        {
            LoadData();
            LoadViewData();
            LoadTeacherIdList();
            LoadClassID();
            LoadSubjectId();
            LoadTermList();
            LoadSortTeacherIdList();
            LoadSortClassID();
            LoadSortSubjectId();
            LoadSortTermList();

            AddCommand = new RelayCommand<object>((p) => { return true; }, (p) => AddTeachingCommand());
            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null) return false;
                
                return true;
            }, (p) => DeleteTeachingCommand());
        }
        private void LoadData()
        {
            var data = new ObservableCollection<Teaching>();
            using(var connection=new SqlConnection(Data.connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT TeacherID, ClassID, SubjectID, Term FROM TEACHING",connection);
                    using(var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new Teaching
                            {
                                TeacherId = reader.IsDBNull(0) ? string.Empty : reader.GetString(0), 
                                ClassId = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                SubjectId = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Term = reader.IsDBNull(3) ? null : reader.GetByte(3),
                            };
                            if(item != null)
                                data.Add(item);
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error : " + ex.Message);
                }
            }
            List = data;
            return;
        }
        private void LoadViewData()
        {
            var viewData=new ObservableCollection<Teaching>();
            foreach(var item in List)
            {
                if (SortteacherId != item.TeacherId && SortteacherId!="Tất cả" && SortteacherId!=null) continue;
                if(SortClassId!=item.ClassId && SortClassId!="Tất cả" && SortClassId!=null) continue;
                if(SortSubId != item.SubjectId && SortSubId != "Tất cả" && SortSubId != null) continue;
                if(SortTermId!=item.Term.ToString() && SortTermId!="Tất cả" && SortTermId!=null) continue;
                var itemData = new Teaching();
                itemData.TeacherId = item.TeacherId;
                itemData.SubjectId = item.SubjectId;
                itemData.Term = item.Term;
                itemData.ClassId = item.ClassId;
                viewData.Add(itemData);
            }
            RealList = viewData;
        }
        private void LoadTeacherIdList()
        {
            var ListId = new ObservableCollection<String>();
            using(SqlConnection connection=new SqlConnection(Data.connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT DISTINCT ID FROM USERS WHERE ACCESS='Giáo viên'", connection);
                    using(var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ListId.Add(reader.GetString(0));
                        }
                    }
                    TeacherIdList = ListId;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error : " + ex.Message);
                }
            }
        }
        private void LoadClassID()
        {
            var ListId = new ObservableCollection<String>();
            using (var connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT DISTINCT Id FROM CLASS", connection);
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
        private void LoadSubjectId()
        {
            var ListId = new ObservableCollection<String>();
            using (SqlConnection connection=new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id FROM SUBJECTS",connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ListId.Add(reader.GetString(0));
                    }
                }
            }
            SubjectIdList = ListId;
        }
        private void LoadTermList()
        {
            var ListId = new ObservableCollection<int>();

            ListId.Add(1);
            ListId.Add(2);
            TermList = ListId;
        }
        private void LoadSortTeacherIdList()
        {
            var ListId = new ObservableCollection<String>();
            ListId.Add("Tất cả");
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT DISTINCT ID FROM USERS WHERE ACCESS='Giáo viên'", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ListId.Add(reader.GetString(0));
                    }
                }
            }
            SortTeacherIdList = ListId;
        }
        private void LoadSortClassID()
        {
            var ListId = new ObservableCollection<String>();
            ListId.Add("Tất cả");
            using (var connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT DISTINCT Id FROM CLASS", connection);
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
        private void LoadSortSubjectId()
        {
            var ListId = new ObservableCollection<String>();
            ListId.Add("Tất cả");
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id FROM SUBJECTS", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ListId.Add(reader.GetString(0));
                    }
                }
            }
            SortSubjectIdList = ListId;
        }
        private void LoadSortTermList()
        {
            var ListId = new ObservableCollection<string>();
            ListId.Add("Tất cả");
            ListId.Add("1");
            ListId.Add("2");
            SortTermList = ListId;
        }
        private void AddTeachingCommand()
        {
            if (!checkedAddCommand())
                return;
            using (SqlConnection connection=new SqlConnection(Data.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO TEACHING (TeacherID, ClassID, SubjectID, Term) VALUES (@TeacherID,@ClassID,@SubjectID,@Term)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TeacherID", TeacherId);
                        command.Parameters.AddWithValue("@ClassID", ClassId);
                        command.Parameters.AddWithValue("@SubjectId", SubjectId);
                        command.Parameters.AddWithValue("@Term", Term);

                        int rowAffected = command.ExecuteNonQuery();
                        if (rowAffected > 0)
                        {
                            List.Add(new Teaching
                            {
                                TeacherId = _TeacherId,
                                ClassId = _ClassId,
                                SubjectId = _SubjectId,
                                Term = _Term,
                            });
                        }
                        else
                        {
                            MessageBox.Show("Đã xảy ra lỗi khi thêm dữ liệu!");
                        }
                    }
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error : "+ ex.Message);
                }
            }
            LoadViewData();
        }
        private bool checkedAddCommand()
        {
            if (CurrentUser.Instance.Access == "Giáo viên") { MessageBox.Show("Bạn không có quyền làm điều này!"); return false; }
            if (TeacherId == null) { MessageBox.Show("Thông tin Mã giáo viên bị thiếu!"); return false; }
            if (ClassId == null) { MessageBox.Show("Thông tin Mã lớp bị thiếu!"); return false; }
            if(SubjectId == null) { MessageBox.Show("Thông tin Mã môn học bị thiếu!"); return false; }
            if(Term == null) { MessageBox.Show("Thông tin học kì bị thiếu!"); return false; }
            return true;
        }
        private void DeleteTeachingCommand()
        {
            if (!checkedDeleteCommand()) return;
            using(SqlConnection connection=new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE TEACHING WHERE TeacherID=@TeacherId AND ClassID=@ClassId AND SubjectId=@SubId AND Term=@Term", connection);
                command.Parameters.AddWithValue("@TeacherId", TeacherId);
                command.Parameters.AddWithValue("@ClassId", ClassId);
                command.Parameters.AddWithValue("@SubId", SubjectId);
                command.Parameters.AddWithValue("@Term", Term);

                int rowAffected=command.ExecuteNonQuery();

                if(rowAffected > 0)
                {
                    foreach (var item in List)
                    {
                        if (item.TeacherId == TeacherId &&
                            item.ClassId == ClassId &&
                            item.SubjectId == SubjectId &&
                            item.Term == Term) { 
                            List.Remove(item);
                            break;
                        }
                    }
                    LoadViewData();
                }
                else
                {
                    MessageBox.Show("Đã xảy ra lỗi khi xóa dữ liệu!");
                }
            }
        }
        private bool checkedDeleteCommand()
        {
            if (CurrentUser.Instance.Access == "Giáo viên") { MessageBox.Show("Bạn không có quyền làm điều này!"); return false; }
            DialogResult dialog = MessageBox.Show("Xóa dữ liệu được chọn?","", MessageBoxButtons.YesNo);
            if(dialog == DialogResult.Yes) { return true; }

            return false;
        }
    }
}
