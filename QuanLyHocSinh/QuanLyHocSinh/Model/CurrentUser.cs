using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHocSinh.Model
{
    public class CurrentUser: ModelBase
    {
        private static CurrentUser _instance;
        public static CurrentUser Instance => _instance ??= new CurrentUser();

        private string _userId;
        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged();
            }
        }

        private string _access;
        public string Access
        {
            get => _access;
            set
            {
                _access = value;
                OnPropertyChanged();
            }
        }
        private CurrentUser() { }
    }
}
