using QuanLyHocSinh.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyHocSinh.ViewModel
{
    internal class SignUpViewModel:ViewModelBase
    {
        private object _curSignUpView;
        public object curSignUpView
        {
            get { return _curSignUpView; }
            set { _curSignUpView = value; OnPropertyChanged(); }
        }
        public ICommand GoFrontSignUpCommand {  get; set; }
        public ICommand GoBackSignUpBox1Command { get; set; }
        public SignUpViewModel() 
        {
            curSignUpView = new SignUpBox1();
            GoFrontSignUpCommand = new RelayCommand<object>((p) => true, (p) => GoFrontSignUp(p));
            GoBackSignUpBox1Command = new RelayCommand<object>((p) => true, (p) => GoBackSignUp(p));
        }
        private void GoFrontSignUp(object obj)
        {
            curSignUpView = new SignUpBox2();
        }
        private void GoBackSignUp(object obj)
        {
            curSignUpView = new SignUpBox1();
        }
    }
}
