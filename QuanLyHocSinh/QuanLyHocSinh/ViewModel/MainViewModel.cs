using System.Windows;
using System.Windows.Input;
using QuanLyHocSinh.View;

namespace QuanLyHocSinh.ViewModel
{
    public class MainViewModel: ViewModelBase
    {
        public ICommand LoadedWindowCommand { get; set; }
        public MainViewModel() 
        {
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                if (p == null)
                    return;
                p.Hide();
                Login loginWindow = new Login();
                loginWindow.ShowDialog();

                if (loginWindow.DataContext == null)
                    return;
                var loginVM = loginWindow.DataContext as LoginViewModel;
                if (loginVM != null)
                {
                    if (loginVM.isLogin)
                    {
                        p.Show();
                    }
                    else
                    {
                        p.Close();
                    }
                }
            }
              );
        }
    }
}
