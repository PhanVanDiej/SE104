using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;
using QuanLyHocSinh.Component;
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
        private string _currentTime;
        public string CurrentTime
        {
            get { return _currentTime; }
            set { _currentTime = value; OnPropertyChanged(); }
        }
        public ICommand ReturnMainMenuCommand {  get; set; }
        public ICommand AddStudentCommand {  get; set; }
        public ICommand LearningManagementCommand { get; set; }
        public ICommand TeachingManagementCommand { get ; set; }
        public ICommand ScoreInputCommand { get; set; }
        public ICommand ManagementCommand { get; set; }
        public ICommand UserManagementCommand { get; set; }
        public ICommand UserProfileCommand { get; set; }

        public MainPageAppViewModel() 
        {
            CurPageView = new MainMenuView();
            ReturnMainMenuCommand = new RelayCommand<object>((p) => true, (p) => CurPageView = new MainMenuView());
            AddStudentCommand =new RelayCommand<object>((p)=>true,(p)=> CurPageView=new StudentManagementView());
            LearningManagementCommand=new RelayCommand<object>((p)=>true,(p)=>CurPageView=new LearningManagmentView());
            TeachingManagementCommand = new RelayCommand<object>((p)=>true,(p)=>CurPageView=new TeachingManagementView());
            ScoreInputCommand = new RelayCommand<object>((p) => true, (p) => CurPageView = new ScoreInputView());
            ManagementCommand =new RelayCommand<object>((p)=>true,(p)=>CurPageView = new ManagementView());
            UserManagementCommand = new RelayCommand<object>((p) => true, (p) => CurPageView = new UserManagementView());
            UserProfileCommand = new RelayCommand<object>((p) => true, (p) => CurPageView = new UserProfileView());

            DispatcherTimer LiveTime = new DispatcherTimer();
            LiveTime.Interval = TimeSpan.FromSeconds(1);
            LiveTime.Tick += timer_Tick;
            LiveTime.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            string tempTime;
            tempTime = DateTime.Now.ToString("dd/MM/yy");
            tempTime = tempTime + " , " + DateTime.Now.ToString("HH:mm:ss");
            CurrentTime = tempTime;
        }

    }
    
}
