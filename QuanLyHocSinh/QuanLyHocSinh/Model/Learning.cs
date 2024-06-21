using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHocSinh.Model
{
    public class Learning : ModelBase
    {
        private string _studentId;
        public string StudentId
        {
            get { return _studentId; }
            set { _studentId = value; OnPropertyChanged(); }
        }

        private string _classId;
        public string ClassId
        {
            get { return _classId; }
            set { _classId = value; OnPropertyChanged(); }
        }

        private int? _term;
        public int? Term
        {
            get { return _term; }
            set { _term = value; OnPropertyChanged(); }
        }

        private decimal? _gpa;
        public decimal? GPA
        {
            get { return _gpa; }
            set { _gpa = value; OnPropertyChanged(); }
        }
        private string? _note;

        public string? Note
        {
            get { return _note; }
            set { _note = value; OnPropertyChanged(); }
        }
        private string _IsPass;

        public string IsPass
        {
            get { return _IsPass; }
            set { _IsPass = value; OnPropertyChanged(); }
        }


    }

}
