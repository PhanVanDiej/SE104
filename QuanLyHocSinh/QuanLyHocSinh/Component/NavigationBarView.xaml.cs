using QuanLyHocSinh.Model;
using System.Windows.Controls;

namespace QuanLyHocSinh.Component
{
    /// <summary>
    /// Interaction logic for NavigationBarView.xaml
    /// </summary>
    public partial class NavigationBarView : UserControl
    {
        public NavigationBarView()
        {
            InitializeComponent();
            UserIdTextBlock.Text = CurrentUser.Instance.UserId;
            AccessTextBlock.Text = CurrentUser.Instance.Access;
        }
    }
}
