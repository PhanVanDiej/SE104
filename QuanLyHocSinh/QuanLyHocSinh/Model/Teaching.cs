using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHocSinh.Model
{
    public class Teaching : ModelBase
    {
        private string _teacherId;
        public string TeacherId
        {
            get { return _teacherId; }
            set { _teacherId = value; OnPropertyChanged(); }
        }

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

        private int? _term;
        public int? Term
        {
            get { return _term; }
            set { _term = value; OnPropertyChanged(); }
        }
    }

}
