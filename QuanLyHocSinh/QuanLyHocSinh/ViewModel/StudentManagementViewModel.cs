using Microsoft.Data.SqlClient;
using QuanLyHocSinh.Model;
using QuanLyHocSinh.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QuanLyHocSinh.ViewModel
{
    internal class StudentManagementViewModel:ViewModelBase
    {
        private ObservableCollection<Student> _List;
        public ObservableCollection<Student> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private Student _SelectedStudent {  get; set; }
        public Student SelectedStudent
        {
            get{ return _SelectedStudent; }
            set
            { 
                _SelectedStudent = value; 
                OnPropertyChanged(nameof(SelectedStudent));
                if(SelectedStudent!=null)
                {
                    Id= SelectedStudent.Id;
                    StudentName = SelectedStudent.FullName;
                    StudentEmail=SelectedStudent.Email;
                    if (SelectedStudent.Gender == "M") StudentGender = "Nam";
                    else if (SelectedStudent.Gender == "F") StudentGender = "Nữ";
                    else StudentGender = "Khác";
                    StudentBirth = SelectedStudent.DateOfBirth;
                    StudentProvince=SelectedStudent.Province;
                    StudentDistrict=SelectedStudent.District;
                    StudentCommune=SelectedStudent.Commune;
                    StudentAddress = SelectedStudent.AddictiveAddress;
                }
            }
        }
        private String _Id { get; set; }
        public String Id
        {
            get {  return _Id; } 
            set { _Id = value; OnPropertyChanged(nameof(Id)); } 
        }

        private String _StudentName { get; set; }
        public String StudentName
        {
            get { return _StudentName; }
            set
            {
                _StudentName = value; OnPropertyChanged(nameof(StudentName));
            }
        }

        private String _StudentGender {  get; set; }
        public String StudentGender
        {
            get { return _StudentGender; }
            set
            {
                _StudentGender = value; OnPropertyChanged(nameof(StudentGender));
            }
        }
        private ObservableCollection<String> _Gender { get; set; }
        public ObservableCollection<String> Gender
        {
            get { return _Gender; }
            set { _Gender = value; OnPropertyChanged(nameof(Gender)); }
        }
        private DateTime? _StudentBirth { get; set; }
        public DateTime? StudentBirth
        {
            get { return _StudentBirth; }
            set { _StudentBirth = value; OnPropertyChanged(nameof(StudentBirth)); }
        }

        private ObservableCollection<String> _Province { get; set; }
        public ObservableCollection<String> Province
        {
            get { return _Province; }
            set
            {
                _Province = value; OnPropertyChanged(nameof(Province));
            }
        }

        private String _StudentProvince { get;set; }
        public String StudentProvince
        {
            get { return _StudentProvince; }
            set { _StudentProvince = value; OnPropertyChanged(nameof(StudentProvince));  LoadDistrict(); }
        }

        private ObservableCollection<String> _District { get; set; }
        public ObservableCollection<String> District
        {
            get { return _District; }
            set { _District = value; OnPropertyChanged(nameof(District)); }
        }
        private String _StudentDistrict {  get; set; }
        public String StudentDistrict
        {
            get { return _StudentDistrict; }
            set { _StudentDistrict = value; OnPropertyChanged(nameof(StudentDistrict)); LoadCommune(); }
        }
        private ObservableCollection<String> _Commune { get; set; }
        public ObservableCollection<String> Commune
        {
            get { return _Commune; }
            set { _Commune = value; OnPropertyChanged(nameof(Commune)); }
        }

        private String _StudentCommune { get; set; }
        public String StudentCommune
        {
            get { return _StudentCommune; }
            set { _StudentCommune = value; OnPropertyChanged(nameof(StudentCommune));   }
        }

        private String _StudentAddress { get; set; }
        public String StudentAddress
        {
            get { return _StudentAddress; }
            set
            {
                _StudentAddress = value; OnPropertyChanged(nameof(StudentAddress));
            }
        }

        private String _StudentEmail { get;set; }
        public String StudentEmail
        {
            get { return _StudentEmail; }
            set { _StudentEmail = value; OnPropertyChanged(nameof(StudentEmail)); }
        }
        public ICommand AddCommand {  get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public StudentManagementViewModel() 
        {
            Gender = LoadGender();
            LoadProvince();
            LoadDistrict();
            LoadCommune();
            List = LoadData();

            AddCommand = new RelayCommand<object>((p) => { return true; } , (p) => AddStudent());
            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedStudent == null)
                    return false;
                
                if (!EmailCheck.Validate(StudentEmail)) return false;
                foreach (var item in List)
                {
                    if (Id == item.Id)
                        return true;
                }
                return false;
            }, (p) => EditStudent());

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedStudent == null)
                    return false;
          
                if (!EmailCheck.Validate(StudentEmail)) return false;
                foreach (var item in List)
                {
                    if (Id == item.Id)
                        return true;
                }
                return false;
            }, (p) => DeleteStudent());

        }
        private ObservableCollection<String> LoadGender()
        {
            var gender = new ObservableCollection<String>();
            gender.Add("Nam");
            gender.Add("Nữ");
            gender.Add("Khác");
            return gender;
        }
        private void LoadProvince()
        {
            var province = new ObservableCollection<String>();
            using(SqlConnection connection=new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT DISTINCT Province FROM ADDRESSES", connection);
                using (var  reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        province.Add(reader.GetString(0));
                    }
                }
            }
            Province = province;
        }
        private void LoadDistrict()
        {
            var district = new ObservableCollection<String>();

            if (StudentProvince == null) return;

            using(SqlConnection connection=new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command=new SqlCommand("SELECT DISTINCT District FROM ADDRESSES WHERE Province=@StudentProvince",connection);
                command.Parameters.AddWithValue("@StudentProvince",StudentProvince);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        district.Add(reader.GetString(0));
                    }
                }
            }
            District =district;
        }
        private void LoadCommune()
        {
            var commune = new ObservableCollection<String>();
           
            if (StudentProvince == null|| StudentDistrict==null) return;

            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT DISTINCT Commune FROM ADDRESSES WHERE Province=@StudentProvince AND District=@StudentDistrict", connection);
                command.Parameters.AddWithValue("@StudentProvince", StudentProvince);
                command.Parameters.AddWithValue("@StudentDistrict", StudentDistrict);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        commune.Add(reader.GetString(0));
                    }
                }
            }
            Commune = commune;
        }
        private ObservableCollection<Student> LoadData()
        {
            var data = new ObservableCollection<Student>();
            using (var connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT ID, FullName, Gender, DateOfBirth, Province, District, Commune, AddictiveAddress, Email FROM STUDENT", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var student = new Student()
                        {
                            Id = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                            FullName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            Gender = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            DateOfBirth = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                            Province = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            District = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            Commune = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                            AddictiveAddress = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                            Email = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        };
                        data.Add(student);
                    }
                }
                return data;
            }
        }
       
        private void AddStudent()
        {
            if (!checkedAddCommand())
            {
                return;
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(Data.connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "INSERT INTO STUDENT (ID, FullName, Gender, DateOfBirth, Province, District, Commune, AddictiveAddress, Email) VALUES (@Id, @FullName, @Gender, @DateOfBirth, @Province, @District, @Commune, @AddictiveAddress, @Email) ";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Id", Id);
                            command.Parameters.AddWithValue("@FullName", StudentName);
                            command.Parameters.AddWithValue("@Gender", StudentGender);
                            command.Parameters.AddWithValue("@DateOfBirth", StudentBirth);
                            command.Parameters.AddWithValue("@Province", StudentProvince);
                            command.Parameters.AddWithValue("@District", StudentDistrict);
                            command.Parameters.AddWithValue("@Commune", StudentCommune);
                            command.Parameters.AddWithValue("@AddictiveAddress", StudentAddress);
                            command.Parameters.AddWithValue("@Email", StudentEmail);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                List.Add(new Student
                                {
                                    Id = _Id,
                                    FullName=_StudentName,
                                    Gender=_StudentGender,
                                    DateOfBirth=_StudentBirth,
                                    Province=_StudentProvince,
                                    District=_StudentDistrict,
                                    Commune=_StudentCommune,
                                    AddictiveAddress=_StudentAddress,
                                    Email=_StudentEmail
                                });
                            }
                            else
                            {
                                MessageBox.Show("Đã xảy ra lỗi!");
                            }
                        }
                    }
                    catch(Exception ex) 
                    {
                        MessageBox.Show("Error" + ex.Message);
                    }
                }
            }
        }
        private bool checkedAddCommand()
        {

            if (CurrentUser.Instance.Access == "Giáo viên") { MessageBox.Show("Bạn không có quyền làm điều này!"); return false; }
            if (Id == null || Id == string.Empty) { MessageBox.Show("Thông tin mã học sinh bị thiếu"); return false; }
            if (StudentName == null || StudentName == string.Empty) { MessageBox.Show("Thông tin tên học sinh bị thiếu"); return false; }
            if (StudentGender == null || StudentGender == string.Empty) { MessageBox.Show("Thông tin giới tính học sinh bị thiếu"); return false; }
            if (StudentBirth == null) { MessageBox.Show("Thông tin ngày sinh học sinh bị thiếu"); return false; }
            if (StudentProvince == null || StudentProvince == string.Empty) { MessageBox.Show("Thông tin tỉnh học sinh bị thiếu"); return false; }
            if (StudentDistrict == null || StudentDistrict == string.Empty) { MessageBox.Show("Thông tin quận/huyện học sinh bị thiếu"); return false; }
            if (StudentCommune == null || StudentCommune == string.Empty) { MessageBox.Show("Thông tin xã/phường học sinh bị thiếu"); return false; }
            if (StudentAddress == null || StudentAddress == string.Empty) { MessageBox.Show("Thông tin địa chỉ học sinh bị thiếu"); return false; }
            if (StudentEmail == null || StudentEmail == string.Empty) { MessageBox.Show("Thông tin email học sinh bị thiếu"); return false; }

            if (!EmailCheck.Validate(StudentEmail)) { MessageBox.Show("Email không thể xác thực"); return false; }
            foreach (var item in List)
            {
                if (Id == item.Id)
                {
                    MessageBox.Show("Mã học sinh trùng với mã học sinh đã tồn tại");
                    return false;
                }
            }
            return true;
        }
        private void EditStudent()
        {
            if (!checkedEditCommand())
            {
                MessageBox.Show("Có lỗi xảy ra. Vui lòng cập nhật lại thông tin.");
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(Data.connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "UPDATE STUDENT SET FULLNAME=@FullName, GENDER=@Gender, DATEOFBIRTH=@DateOfBirth, PROVINCE=@Province, DISTRICT=@District, COMMUNE=@Commune, ADDICTIVEADDRESS=@AddictiveAddress, EMAIL=@Email WHERE Id = @Id"; 
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Id", Id);
                            command.Parameters.AddWithValue("@FullName", StudentName);
                            command.Parameters.AddWithValue("@Gender", StudentGender);
                            command.Parameters.AddWithValue("@DateOfBirth", StudentBirth);
                            command.Parameters.AddWithValue("@Province", StudentProvince);
                            command.Parameters.AddWithValue("@District", StudentDistrict);
                            command.Parameters.AddWithValue("@Commune", StudentCommune);
                            command.Parameters.AddWithValue("@AddictiveAddress", StudentAddress);
                            command.Parameters.AddWithValue("@Email", StudentEmail);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                //MessageBox.Show("Cập nhật thành công!");
                                var item = List.FirstOrDefault(stu => stu.Id == Id);
                                if (item != null)
                                {
                                    item.FullName = StudentName;
                                    item.Gender=StudentGender;
                                    item.DateOfBirth = StudentBirth;
                                    item.Province = StudentProvince;
                                    item.District = StudentDistrict;    
                                    item.Commune = StudentCommune;
                                    item.AddictiveAddress = StudentAddress;
                                    item.Email = StudentEmail;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Lỗi khi cập nhật thông tin học sinh!");
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Error" + ex.Message);
                    }
                }
            }
        }
        private bool checkedEditCommand()
        {

            if (CurrentUser.Instance.Access == "Giáo viên") { MessageBox.Show("Bạn không có quyền làm điều này!"); return false; }
            if (Id == null || Id == string.Empty) { MessageBox.Show("Thông tin mã học sinh bị thiếu"); return false; }
            if (StudentName == null || StudentName == string.Empty) { MessageBox.Show("Thông tin tên học sinh bị thiếu"); return false; }
            if (StudentGender == null || StudentGender == string.Empty) { MessageBox.Show("Thông tin giới tính học sinh bị thiếu"); return false; }
            if (StudentBirth == DateTime.MinValue) { MessageBox.Show("Thông tin ngày sinh học sinh bị thiếu"); return false; }
            if (StudentProvince == null || StudentProvince == string.Empty) { MessageBox.Show("Thông tin tỉnh học sinh bị thiếu"); return false; }
            if (StudentDistrict == null || StudentDistrict == string.Empty) { MessageBox.Show("Thông tin quận/huyện học sinh bị thiếu"); return false; }
            if (StudentCommune == null || StudentCommune == string.Empty) { MessageBox.Show("Thông tin xã/phường học sinh bị thiếu"); return false; }
            if (StudentAddress == null || StudentAddress == string.Empty) { MessageBox.Show("Thông tin địa chỉ học sinh bị thiếu"); return false; }
            if (StudentEmail == null || StudentEmail == string.Empty) { MessageBox.Show("Thông tin email học sinh bị thiếu"); return false; }
            return true;
        }

        private void DeleteStudent()
        {
            if (!checkedDeleteCommand())
            {
                return;
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(Data.connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "DELETE FROM STUDENT WHERE Id = @Id";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Id", Id);
                            int rowsAffected = command.ExecuteNonQuery();
                            if(rowsAffected > 0)
                            {
                                foreach(var item in List)
                                {
                                    if(item.Id == Id)
                                    {
                                        List.Remove(item);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Đã xảy ra lỗi khi xóa!");
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("ERROR" + ex.Message);
                    }
                }
            }
        }
        private bool checkedDeleteCommand()
        {
            if (CurrentUser.Instance.Access == "Giáo viên") { MessageBox.Show("Bạn không có quyền làm điều này!"); return false; }
            DialogResult dialog = MessageBox.Show("Xóa dữ liệu được chọn?", "", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes) { return true; }
            return false;
        }
    }
}
