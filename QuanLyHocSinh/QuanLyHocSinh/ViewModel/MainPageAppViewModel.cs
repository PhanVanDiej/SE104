using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QuanLyHocSinh.View;
namespace QuanLyHocSinh.ViewModel
{
    internal class MainPageAppViewModel:ViewModelBase
    {
        private object _curPageView {  get; set; }
        public object CurPageView
        {
            get { return _curPageView; }
            set { _curPageView = value; OnPropertyChanged() ; }
        }
        public ICommand AddStudentCommand {  get; set; }
        public MainPageAppViewModel() 
        {
            AddStudentCommand=new RelayCommand<object>((p)=>true,(p)=> CurPageView=new AddStudentView());
        }
    }
}
