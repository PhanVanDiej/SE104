using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHocSinh.Model
{
    public class Class : ModelBase
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private string _className;
        public string ClassName
        {
            get { return _className; }
            set { _className = value; OnPropertyChanged(); }
        }

        private short _schoolYear;
        public short SchoolYear
        {
            get { return _schoolYear; }
            set { _schoolYear = value; OnPropertyChanged(); }
        }

        private string _homeroomTeacherId;
        public string HomeroomTeacherId
        {
            get { return _homeroomTeacherId; }
            set { _homeroomTeacherId = value; OnPropertyChanged(); }
        }
    }

}
