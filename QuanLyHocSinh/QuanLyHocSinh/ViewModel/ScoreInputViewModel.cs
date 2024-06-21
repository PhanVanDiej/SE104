using Microsoft.Data.SqlClient;
using QuanLyHocSinh.Model;
using QuanLyHocSinh.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;
using Timer = System.Timers.Timer;

namespace QuanLyHocSinh.ViewModel
{
    internal class ScoreInputViewModel : ViewModelBase
    {
        private ObservableCollection<string> _ClassIdList { get; set; }
        public ObservableCollection<string> ClassIdList
        {
            get { return _ClassIdList; }
            set { _ClassIdList = value; OnPropertyChanged(nameof(ClassIdList)); }
        }
        private String _ClassId { get; set; }
        public String ClassId
        {
            get { return _ClassId; }
            set { _ClassId = value; OnPropertyChanged(nameof(ClassId)); }
        }
        private ObservableCollection<String> _SubjectIdList { get; set; }
        public ObservableCollection<String> SubjectIdList
        {
            get { return _SubjectIdList; }
            set { _SubjectIdList = value; OnPropertyChanged(nameof(SubjectIdList)); }
        }
        private String _SubjectId { get; set; }
        public String SubjectId
        {
            get { return _SubjectId; }
            set { _SubjectId = value; OnPropertyChanged(nameof(SubjectId)); }
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
        private ObservableCollection<int> _TermList { get; set; }
        public ObservableCollection<int> TermList
        {
            get { return _TermList; }
            set { _TermList = value; OnPropertyChanged(nameof(_TermList)); }
        }
        private int? _Term { get; set; }
        public int? Term
        {
            get { return _Term; }
            set { _Term = value; OnPropertyChanged(nameof(Term)); }
        }
        private decimal? _MiniTest { get; set; }
        public decimal? MiniTest
        {
            get { return _MiniTest; }
            set { _MiniTest = value; OnPropertyChanged(nameof(MiniTest)); }
        }
        private decimal? _midTermTest { get; set; }
        public decimal? MidTermTest
        {
            get { return _midTermTest; }
            set { _midTermTest = value; OnPropertyChanged(nameof(MidTermTest)); }
        }
        private decimal? _finalTermTest { get; set; }
        public decimal? FinalTermTest
        {
            get { return _finalTermTest; }
            set { _finalTermTest = value; OnPropertyChanged(nameof(FinalTermTest)); }
        }
        private ObservableCollection<String> _YearList { get; set; }
        public ObservableCollection<String> YearList
        {
            get { return _YearList; }
            set { _YearList = value; OnPropertyChanged(nameof(YearList)); }
        }
        private String _SortYear { get; set; }
        public String SortYear
        {
            get { return _SortYear; }
            set { _SortYear = value; OnPropertyChanged(nameof(SortYear)); if (SortYear != null) { LoadData(); } }
        }
        private ObservableCollection<String> _SortClassIdList { get; set; }
        public ObservableCollection<String> SortClassIdList
        {
            get { return _SortClassIdList; }
            set { _SortClassIdList = value; OnPropertyChanged(nameof(SortClassIdList)); }
        }
        private String _SortClassId {  get; set; }  
        public String SortClassId
        {
            get { return _SortClassId; }
            set { _SortClassId = value; OnPropertyChanged(nameof(SortClassId)); if (SortClassId != null) { LoadViewData(); } }
        }
        private ObservableCollection<String>_SortTermList { get; set; }
        public ObservableCollection<String> SortTermList
        {
            get { return _SortTermList; }
            set { _SortTermList = value; OnPropertyChanged(nameof(SortTermList)); }
        }
        private String _SortTerm {  get; set; }
        public String SortTerm
        {
            get { return _SortTerm; }
            set { _SortTerm = value; OnPropertyChanged(nameof(SortTerm)); if (SortTerm != null) { LoadViewData(); } }
        }
        private ObservableCollection<Score> _List { get; set; }
        public ObservableCollection<Score> List
        {
            get { return _List; }
            set { _List = value; OnPropertyChanged(nameof(_List)); }
        }
        private ObservableCollection<Score> _ViewList { get; set; }
        public ObservableCollection<Score> ViewList
        {
            get { return _ViewList; }
            set { _ViewList = value; OnPropertyChanged(nameof(ViewList)); }
        }
        private Score _SelectedItem {  get; set; }
        public Score SelectedItem
        {
            get { return _SelectedItem; }
            set 
            { 
                _SelectedItem = value; 
                OnPropertyChanged(nameof(SelectedItem));
                if (SelectedItem != null)
                {
                    ClassId = SelectedItem.ClassId;
                    SubjectId = SelectedItem.SubjectId;
                    StudentId = SelectedItem.StudentId;
                    Term = SelectedItem.Term;
                    MiniTest = SelectedItem.MiniTest;
                    MidTermTest = SelectedItem.MidTermTest;
                    FinalTermTest = SelectedItem.FinalTermTest;
                }
            }
        }
        public ICommand EditCommand {  get; set; }

        public ScoreInputViewModel() 
        {
            LoadData();
            LoadClassIdList();
            LoadStudentIdList();
            LoadSubjectIdList();
            LoadTermList();
            LoadSchoolYear();
            LoadSortClassIdList();
            LoadSortTermList();

            Timer myTimer = new Timer();
            myTimer.Elapsed += new ElapsedEventHandler(DisplayTimeEvent);
            myTimer.Interval = 3000; // 1000 ms is one second
            myTimer.Start();

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null) return false;
                foreach (var item in List)
                {
                    if (item.ClassId == SelectedItem.ClassId && item.SubjectId == SelectedItem.SubjectId && item.StudentId == SelectedItem.StudentId && item.Term == SelectedItem.Term) return true;
                }
                return false;
            }, (p) => EditScoreCommand());
        }
       private void DisplayTimeEvent(object source, ElapsedEventArgs e)
        {
            LoadData();
        }
        private void LoadClassIdList()
        {
            var ListId = new ObservableCollection<String>();
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id FROM CLASS", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ListId.Add(reader.GetString(0));
                    }
                }
            }
            ClassIdList = ListId;
        }
        private void LoadSubjectIdList()
        {
            var ListId=new ObservableCollection<String>();
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
        private void LoadStudentIdList()
        {
            var ListId = new ObservableCollection<String>();
            using (SqlConnection connection= new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT ID FROM STUDENT", connection);
                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ListId.Add(reader.GetString(0));
                    }
                }
            }
            StudentIdList = ListId;
        }
        private void LoadTermList()
        {
            var List=new ObservableCollection<int>();
            List.Add(1);
            List.Add(2);
            TermList = List;
        }
        private void LoadSchoolYear()
        {
            var list = new ObservableCollection<String>();
            list.Add("Tất cả");
            using (SqlConnection connection=new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT DISTINCT SCHOOLYEAR FROM CLASS", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetInt16(0).ToString());
                    }
                }
            }
            YearList = list;
        }
        private void LoadSortClassIdList()
        {
            var ListId = new ObservableCollection<String>();
            ListId.Add("Tất cả");
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id FROM CLASS", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ListId.Add(reader.GetString(0));
                    }
                }
            }
            SortClassIdList = ListId;
        }
        private void LoadSortTermList()
        {
            var list = new ObservableCollection<String>();
            list.Add("Tất cả");
            list.Add("1");
            list.Add("2");
            SortTermList = list;
        }
        public void LoadData()
        {
            var data=new ObservableCollection<Score>();
            string userId = CurrentUser.Instance.UserId;
            using (SqlConnection connection= new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand();
                if (SortYear != null && SortYear != "Tất cả")
                {
                    command = new SqlCommand("SELECT SCORE.CLASSID,SCORE.SUBJECTID,SCORE.STUDENTID,SCORE.TERM,MINITEST,MIDTERMTEST,FINALTERMTEST,AVERAGE, ISPASS FROM SCORE " +
                                            "JOIN CLASS ON CLASS.ID=SCORE.CLASSID " +
                                            "JOIN TEACHING ON TEACHING.CLASSID=SCORE.CLASSID AND TEACHING.SUBJECTID=SCORE.SUBJECTID AND TEACHING.TERM=SCORE.TERM " +
                                            "WHERE CLASS.SCHOOLYEAR=@SortYear AND TEACHING.TEACHERID=@TeacherId", connection);
                    command.Parameters.AddWithValue("@SortYear", SortYear);
                    command.Parameters.AddWithValue("@TeacherId", userId);
                }
                else
                {
                    command = new SqlCommand("SELECT SCORE.CLASSID,SCORE.SUBJECTID,SCORE.STUDENTID,SCORE.TERM,MINITEST,MIDTERMTEST,FINALTERMTEST,AVERAGE,ISPASS FROM SCORE " +
                        "JOIN TEACHING ON TEACHING.CLASSID=SCORE.CLASSID AND TEACHING.SUBJECTID=SCORE.SUBJECTID AND TEACHING.TERM=SCORE.TERM WHERE TEACHING.TEACHERID=@TeacherId", connection);
                    command.Parameters.AddWithValue("@TeacherId", userId);
                }
                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Score
                        {
                            ClassId = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                            SubjectId = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            StudentId = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Term = reader.IsDBNull(3) ? null : reader.GetByte(3),
                            MiniTest = reader.IsDBNull(4) ? null : reader.GetDecimal(4),
                            MidTermTest = reader.IsDBNull(5) ? null : reader.GetDecimal(5),
                            FinalTermTest = reader.IsDBNull(6) ? null : reader.GetDecimal(6),
                            Average = reader.IsDBNull(7) ? null : reader.GetDecimal(7),
                            IsPass = reader.IsDBNull(8)?string.Empty : reader.GetString(8),
                        };
                        data.Add(item);
                    }
                }
            } 
            List=data;
            LoadViewData();
        }
        private void LoadViewData()
        {
            var viewdata = new ObservableCollection<Score>();
            foreach(var item in List)
            {
                if (item.ClassId != SortClassId && SortClassId != "Tất cả" && SortClassId != null) continue;
                if (item.Term.ToString() != SortTerm && SortTerm != "Tất cả" && SortTerm != null) continue;
                
                viewdata.Add(new Score
                {
                    ClassId = item.ClassId,
                    Term = item.Term,
                    SubjectId = item.SubjectId,
                    StudentId = item.StudentId,
                    MiniTest = item.MiniTest,
                    MidTermTest = item.MidTermTest,
                    FinalTermTest = item.FinalTermTest,
                    Average = item.Average,
                    IsPass = item.IsPass,
                });
            }
            ViewList = viewdata;
        }
       
       
        private void EditScoreCommand()
        {
            if (!checkedEditCommand()) return;
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE SCORE SET MINITEST=@MiniTest, MIDTERMTEST=@MidTermTest, FINALTERMTEST=@FinalTermTest WHERE STUDENTID=@StudentId AND SUBJECTID=@SubjectId AND CLASSID=@ClassId AND TERM=@Term", connection);
                command.Parameters.AddWithValue("@ClassId", ClassId);
                command.Parameters.AddWithValue("@SubjectId", SubjectId);
                command.Parameters.AddWithValue("@StudentId", StudentId);
                command.Parameters.AddWithValue("@Term", Term);
                command.Parameters.AddWithValue("@MiniTest", MiniTest);
                command.Parameters.AddWithValue("@MidTermTest", MidTermTest);
                command.Parameters.AddWithValue("@FinalTermTest", FinalTermTest);

                int rowAffect=command.ExecuteNonQuery();
                if(rowAffect > 0)
                {
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Đã xảy ra lỗi khi cập nhật dữ liệu!");
                }
            }
        }
        private bool checkedEditCommand()
        {
            if (MiniTest < 0 || MiniTest > 10) { MessageBox.Show("Điểm số phải ≥ 0 và ≤ 10."); return false; }
            if (MidTermTest < 0 || MidTermTest > 10) { MessageBox.Show("Điểm số phải ≥ 0 và ≤ 10."); return false; }
            if (FinalTermTest < 0 || FinalTermTest > 10) { MessageBox.Show("Điểm số phải ≥ 0 và ≤ 10."); return false; }

            foreach (var item in List)
            {
                if (item.StudentId == StudentId && item.ClassId == ClassId && item.SubjectId == SubjectId && item.Term == Term)
                {
                    return true;
                }
            }
            MessageBox.Show("Thông tin dữ liệu không tồn tại hoặc đã bị xóa.");
            return false;
        }
    }
}
