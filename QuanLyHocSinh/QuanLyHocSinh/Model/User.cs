namespace QuanLyHocSinh.Model
{
    public class User: ModelBase
    {
		private string _id;

		public string id
		{
			get { return _id; }
			set { id = value; OnPropertyChanged(); }
		}
        private string? _pass;

		public string pass
		{
			get { return _pass; }
			set { _pass = value; OnPropertyChanged(); }
		}
		private string _fullName;

		public string FullName
		{
			get { return _fullName; }
			set { _fullName = value; OnPropertyChanged(); }
		}
		private string _email;

		public string Email
		{
			get { return _email; }
			set { _email = value; OnPropertyChanged(); }
		}
		private string _access;

		public string Access
		{
			get { return _access; }
			set { _access = value; OnPropertyChanged(); }
		}
	}
}
