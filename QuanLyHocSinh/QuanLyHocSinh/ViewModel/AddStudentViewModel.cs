using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace QuanLyHocSinh.ViewModel
{
    internal class AddStudentViewModel:ViewModelBase
    {

        private BitmapImage _studentAvatarSource { get; set; }
        public BitmapImage StudentAvatarSource
        {
            get { return _studentAvatarSource; }
            set { _studentAvatarSource = value; OnPropertyChanged(nameof(StudentAvatarSource)); }
        }
        public ICommand BrowseImageCommand { get; set; }
        public AddStudentViewModel()
        {
            BrowseImageCommand = new RelayCommand<object>((p) => true, (p) => LoadStudentAvatar());
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(@"/Resources/Images/UploadAvatar.png", UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            StudentAvatarSource = bitmapImage;

        }
        private void LoadStudentAvatar()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*",
                    Title = "Select an image file"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string imagePath = openFileDialog.FileName;

                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(imagePath);
                    bitmapImage.EndInit();
                    StudentAvatarSource = bitmapImage;
                }
            }
            catch (Exception ex) { MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
