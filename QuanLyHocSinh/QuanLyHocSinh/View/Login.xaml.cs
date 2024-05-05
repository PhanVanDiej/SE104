using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QuanLyHocSinh
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    IDPlaceHolder.Opacity = 0.5;
                }
                else
                {
                    IDPlaceHolder.Opacity = 0;
                }
            }

        }

        private void FloatingPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                if (string.IsNullOrEmpty(passwordBox.Password))
                {
                    PassPlaceHolder.Opacity = 0.5;
                }
                else
                {
                    PassPlaceHolder.Opacity = 0;
                }
            }
        }
    }
}
