using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHocSinh.Model
{
    public class Regulation : ModelBase
    {
        private int? _minAge;
        public int? MinAge
        {
            get { return _minAge; }
            set { _minAge = value; OnPropertyChanged(); }
        }

        private int? _maxAge;
        public int? MaxAge
        {
            get { return _maxAge; }
            set { _maxAge = value; OnPropertyChanged(); }
        }

        private int? _maxClassSize;
        public int? MaxClassSize
        {
            get { return _maxClassSize; }
            set { _maxClassSize = value; OnPropertyChanged(); }
        }

        private decimal? _passGPA;
        public decimal? PassGPA
        {
            get { return _passGPA; }
            set { _passGPA = value; OnPropertyChanged(); }
        }
        private decimal? _passingGPAPerSubject;
        public decimal? PassingGPAPerSubject
        {
            get { return _passingGPAPerSubject; }
            set { _passingGPAPerSubject = value; OnPropertyChanged(); }
        }
        private short? _SchoolYear;
        public short? SchoolYear
        {
            get { return _SchoolYear; }
            set { _SchoolYear = value; OnPropertyChanged(); }
        }
    }

}
