using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHocSinh.Model
{
    public class Regulation : ModelBase
    {
        private byte _minAge;
        public byte MinAge
        {
            get { return _minAge; }
            set { _minAge = value; OnPropertyChanged(); }
        }

        private byte _maxAge;
        public byte MaxAge
        {
            get { return _maxAge; }
            set { _maxAge = value; OnPropertyChanged(); }
        }

        private byte _maxClassSize;
        public byte MaxClassSize
        {
            get { return _maxClassSize; }
            set { _maxClassSize = value; OnPropertyChanged(); }
        }

        private decimal _passGPD;
        public decimal PassGPD
        {
            get { return _passGPD; }
            set { _passGPD = value; OnPropertyChanged(); }
        }
    }

}
