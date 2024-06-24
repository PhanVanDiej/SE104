using Microsoft.Data.SqlClient;
using QuanLyHocSinh.Model;
using QuanLyHocSinh.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;
namespace QuanLyHocSinh.ViewModel
{
    internal class RegulationManagementViewModel:ViewModelBase
    {
        private short? _SchoolYear { get; set; }
        public short? SchoolYear
        {
            get { return _SchoolYear; }
            set { _SchoolYear = value; OnPropertyChanged(nameof(SchoolYear)); }
        }
        private int? _MinAge { get; set; }
        public int? MinAge
        {
            get { return _MinAge; }
            set { _MinAge = value; OnPropertyChanged(nameof(MinAge)); }
        }
        private int? _MaxAge { get; set; }
        public int? MaxAge
        {
            get { return _MaxAge; }
            set { _MaxAge = value; OnPropertyChanged(nameof(MaxAge)); }
        }
        private int? _MaxClassSize { get; set; }
        public int? MaxClassSize
        {
            get { return _MaxClassSize; }
            set { _MaxClassSize = value; OnPropertyChanged(nameof(MaxClassSize)); }
        }
        private decimal? _passingGPA;
        public decimal? PassingGPA
        {
            get { return _passingGPA; }
            set { _passingGPA = value; OnPropertyChanged(nameof(PassingGPA)); }
        }
        private decimal? _passingGPAPerSubject { get; set; }
        public decimal? PassingGPAPerSubject
        {
            get { return _passingGPAPerSubject; }
            set { _passingGPAPerSubject = value; OnPropertyChanged(nameof(PassingGPAPerSubject)); }
        }
        private ObservableCollection<Regulation> _List {  get; set; }
        public ObservableCollection<Regulation> List
        {
            get { return _List; }
            set { _List = value; OnPropertyChanged(nameof(List)); }
        }
        private Regulation _SelectedItem { get; set; }
        public Regulation SelectedItem
        {
            get { return _SelectedItem; }
            set 
            { 
                _SelectedItem = value; 
                OnPropertyChanged(nameof(SelectedItem)); 
                if(SelectedItem != null)
                {
                    SchoolYear = SelectedItem.SchoolYear;
                    MinAge=SelectedItem.MinAge;
                    MaxAge=SelectedItem.MaxAge;
                    MaxClassSize = SelectedItem.MaxClassSize;
                    PassingGPA = SelectedItem.PassGPA;
                    PassingGPAPerSubject=SelectedItem.PassingGPAPerSubject;
                }
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public RegulationManagementViewModel()
        {
            LoadData();
            AddCommand = new RelayCommand<object>((p) => { return true; }, (p) => AddRegulationCommand());
            EditCommand=new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null) return false;
                
                foreach (var item in List)
                {
                    if (item.SchoolYear == SchoolYear) return true;
                }
                return false;
            }, (p) => EditRegulationCommand());
            DeleteCommand=new RelayCommand<object>((p)=>
            {
                
                if (SelectedItem == null) return false;
                foreach (var item in List)
                {
                    if (item.SchoolYear == SchoolYear) return true;
                }
                return false;
            },
            (p) => DeleteRegulationCommand());
        }
        private void LoadData()
        {
            var data=new ObservableCollection<Regulation>();
            using (SqlConnection connection=new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT SCHOOLYEAR, MINAGE, MAXAGE, MAXCLASSSIZE, PASSINGGPA,PASSINGGPAPERSUBJECT FROM REGULATION", connection);
                using (var reader=command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(new Regulation
                        {
                            SchoolYear = reader.IsDBNull(0) ? null : reader.GetInt16(0),
                            MinAge = reader.IsDBNull(1) ? null : reader.GetByte(1),
                            MaxAge = reader.IsDBNull(2) ? null : reader.GetByte(2),
                            MaxClassSize = reader.IsDBNull(3) ? null : reader.GetByte(3),
                            PassGPA = reader.IsDBNull(4) ? null : reader.GetDecimal(4),
                            PassingGPAPerSubject = reader.IsDBNull(5) ? null : reader.GetDecimal(5),
                        });
                    }
                }
            }
            List=data;
        }
        private void AddRegulationCommand()
        {
            if (!checkedAddCommand()) return;
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                connection.Open ();
                var command = new SqlCommand("INSERT INTO REGULATION (SCHOOLYEAR, MINAGE, MAXAGE, MAXCLASSSIZE, PASSINGGPA,PASSINGGPAPERSUBJECT) " +
                    "VALUES (@SchoolYear, @MinAge, @MaxAge, @MaxClassSize, @PassingGPA, @PassingGPAPerSubject)", connection);
                command.Parameters.AddWithValue("@SchoolYear", SchoolYear);
                command.Parameters.AddWithValue("@MinAge", MinAge);
                command.Parameters.AddWithValue("@MaxAge", MaxAge);
                command.Parameters.AddWithValue("@MaxClassSize", MaxClassSize);
                command.Parameters.AddWithValue("@PassingGPA", PassingGPA);
                command.Parameters.AddWithValue("@PassingGPAPerSubject", PassingGPAPerSubject);

                int rowAffected = command.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    List.Add(new Regulation
                    {
                        SchoolYear = _SchoolYear,
                        MinAge = _MinAge,
                        MaxAge = _MaxAge,
                        MaxClassSize = _MaxClassSize,
                        PassGPA = _passingGPA,
                        PassingGPAPerSubject = _passingGPAPerSubject,
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

            if (CurrentUser.Instance.Access != "Quản trị viên") { MessageBox.Show("Bạn không có quyền làm điều này!"); return false; }
            if (SchoolYear==null) { MessageBox.Show("Thông tin năm học bị thiếu!");return false;}
            if(MinAge==null) { MessageBox.Show("Thông tin Tuổi tối thiểu bị thiếu!"); return false; }
            if (MaxAge == null) { MessageBox.Show("Thông tin Tuổi tối đa bị thiếu!"); return false; }
            if(MaxClassSize==null) { MessageBox.Show("Thông tin Sĩ số lớp tối đa bị thiếu!"); return false; }
            if(PassingGPA==null) { MessageBox.Show("Thông tin điểm Đạt bị thiếu!"); return false; }
            if (PassingGPAPerSubject == null) { MessageBox.Show("Thông tin điểm Đạt qua môn bị thiếu!"); return false; }
            
            foreach(var item in List)
            {
                if (SchoolYear == item.SchoolYear) { MessageBox.Show("Dữ liệu cài đặt cho Năm học đã tồn tại. Vui lòng chỉnh sửa dữ liệu có sẵn."); return false; }
            }

            return true;
        }
        private void EditRegulationCommand()
        {
            if (!checkedEditCommand()) return;
            using (SqlConnection connection=new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE REGULATION SET MINAGE=@MinAge, MaxAge=@MaxAge, MAXCLASSSIZE=@MaxClassSize, PASSINGGPA=@PassingGPA, PASSINGGPAPERSUBJECT=@PassingGPAPerSubject WHERE SCHOOLYEAR=@SchoolYear", connection);
                command.Parameters.AddWithValue("@SchoolYear", SchoolYear);
                command.Parameters.AddWithValue("@MinAge", MinAge);
                command.Parameters.AddWithValue("@MaxAge", MaxAge);
                command.Parameters.AddWithValue("@MaxClassSize", MaxClassSize);
                command.Parameters.AddWithValue("@PassingGPA", PassingGPA);
                command.Parameters.AddWithValue("@PassingGPAPerSubject", PassingGPAPerSubject);

                int rowAffected = command.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    var update = List.FirstOrDefault(item => item.SchoolYear == SchoolYear);
                    if (update != null)
                    {
                        update.MinAge = MinAge;
                        update.MaxAge = MaxAge;
                        update.MaxClassSize = MaxClassSize;
                        update.PassingGPAPerSubject = PassingGPAPerSubject;
                        update.PassGPA = PassingGPA;
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
            if (CurrentUser.Instance.Access != "Quản trị viên") { MessageBox.Show("Bạn không có quyền làm điều này!"); return false; }

            if (SchoolYear == null) { MessageBox.Show("Thông tin năm học bị thiếu!"); return false; }
            if (MinAge == null) { MessageBox.Show("Thông tin Tuổi tối thiểu bị thiếu!"); return false; }
            if (MaxAge == null) { MessageBox.Show("Thông tin Tuổi tối đa bị thiếu!"); return false; }
            if (MaxClassSize == null) { MessageBox.Show("Thông tin Sĩ số lớp tối đa bị thiếu!"); return false; }
            if (PassingGPA == null) { MessageBox.Show("Thông tin điểm Đạt bị thiếu!"); return false; }
            if (PassingGPAPerSubject == null) { MessageBox.Show("Thông tin điểm Đạt qua môn bị thiếu!"); return false; }

            foreach (var item in List)
            {
                if (SchoolYear == item.SchoolYear)
                    return true;
            }
            return false;
        }

        private void DeleteRegulationCommand()
        {
            if (!checkedDeleteCommand()) return;
            using(SqlConnection connection=new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command =new SqlCommand("DELETE REGULATION WHERE SCHOOLYEAR=@SchoolYear", connection);
                command.Parameters.AddWithValue("@SchoolYear", SchoolYear);

                int rowAffected = command.ExecuteNonQuery();
                if(rowAffected > 0)
                {
                    foreach(var item in List)
                        if(SchoolYear == item.SchoolYear)
                        {
                            List.Remove(item);
                            return;
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
            if (CurrentUser.Instance.Access != "Quản trị viên") { MessageBox.Show("Bạn không có quyền làm điều này!"); return false; }

            DialogResult dialog =MessageBox.Show("Xóa dữ liệu được chọn?","",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes) { return true; }
            return false;
        }
    }
}
