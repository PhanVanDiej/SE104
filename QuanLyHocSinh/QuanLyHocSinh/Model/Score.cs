using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHocSinh.Model
{
    public class Score : ModelBase
    {
        private string _classId;
        public string ClassId
        {
            get { return _classId; }
            set { _classId = value; OnPropertyChanged(); }
        }

        private string _subjectId;
        public string SubjectId
        {
            get { return _subjectId; }
            set { _subjectId = value; OnPropertyChanged(); }
        }

        private string _studentId;
        public string StudentId
        {
            get { return _studentId; }
            set { _studentId = value; OnPropertyChanged(); }
        }

        private byte _term;
        public byte Term
        {
            get { return _term; }
            set { _term = value; OnPropertyChanged(); }
        }

        private decimal _miniTest;
        public decimal MiniTest
        {
            get { return _miniTest; }
            set { _miniTest = value; OnPropertyChanged(); }
        }

        private decimal _midTermTest;
        public decimal MidTermTest
        {
            get { return _midTermTest; }
            set { _midTermTest = value; OnPropertyChanged(); }
        }

        private decimal _finalTermTest;
        public decimal FinalTermTest
        {
            get { return _finalTermTest; }
            set { _finalTermTest = value; OnPropertyChanged(); }
        }

        private decimal _average;
        public decimal Average
        {
            get { return _average; }
            set { _average = value; OnPropertyChanged(); }
        }
    }

}
