using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHocSinh.Model
{
    public class Subject : ModelBase
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private string _subjectName;
        public string SubjectName
        {
            get { return _subjectName; }
            set { _subjectName = value; OnPropertyChanged(); }
        }

        private decimal _passingScore;
        public decimal PassingScore
        {
            get { return _passingScore; }
            set { _passingScore = value; OnPropertyChanged(); }
        }

        private string _chiefTeacherId;
        public string ChiefTeacherId
        {
            get { return _chiefTeacherId; }
            set { _chiefTeacherId = value; OnPropertyChanged(); }
        }
    }

}
