using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHocSinh.Model
{
    public class Student : ModelBase
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; OnPropertyChanged(); }
        }

        private string _gender;
        public string Gender
        {
            get { return _gender; }
            set { _gender = value; OnPropertyChanged(); }
        }

        private DateTime _dateOfBirth;
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; OnPropertyChanged(); }
        }

        private string _province;
        public string Province
        {
            get { return _province; }
            set { _province = value; OnPropertyChanged(); }
        }

        private string _district;
        public string District
        {
            get { return _district; }
            set { _district = value; OnPropertyChanged(); }
        }

        private string _commune;
        public string Commune
        {
            get { return _commune; }
            set { _commune = value; OnPropertyChanged(); }
        }

        private string _addictiveAddress;
        public string AddictiveAddress
        {
            get { return _addictiveAddress; }
            set { _addictiveAddress = value; OnPropertyChanged(); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }
    }

}
