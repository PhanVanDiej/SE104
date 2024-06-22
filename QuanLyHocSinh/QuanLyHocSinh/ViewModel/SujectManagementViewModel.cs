using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using QuanLyHocSinh.Model;

namespace QuanLyHocSinh.ViewModel
{
    public class SubjectManagementViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Subject> _list;
        private Subject _selectedItem;
        private string _newSubjectId;
        private string _newSubjectName;

        public ObservableCollection<Subject> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(nameof(List)); }
        }

        public Subject SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); }
        }

        public string NewSubjectId
        {
            get { return _newSubjectId; }
            set { _newSubjectId = value; OnPropertyChanged(nameof(NewSubjectId)); }
        }

        public string NewSubjectName
        {
            get { return _newSubjectName; }
            set { _newSubjectName = value; OnPropertyChanged(nameof(NewSubjectName)); }
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public SubjectManagementViewModel()
        {
            List = new ObservableCollection<Subject>();
            InitializeCommands();
            LoadData(); // Load initial data (for example)
        }

        private void InitializeCommands()
        {
            AddCommand = new RelayCommand(AddExecute, CanAddExecute);
            EditCommand = new RelayCommand(EditExecute, CanEditExecute);
            DeleteCommand = new RelayCommand(DeleteExecute, CanDeleteExecute);
        }

        private void LoadData()
        {
            // Example: Load initial data into List (optional)
            // For demonstration purpose, you may load some initial data here
            List.Add(new Subject { Id = "M001", SubjectName = "Toán học", PassingScore = 5.0m, ChiefTeacherId = "T001" });
            List.Add(new Subject { Id = "M002", SubjectName = "Văn học", PassingScore = 4.5m, ChiefTeacherId = "T002" });
            List.Add(new Subject { Id = "M003", SubjectName = "Lịch sử", PassingScore = 4.0m, ChiefTeacherId = "T003" });
        }

        private void AddExecute(object parameter)
        {
            // Default values for PassingScore and ChiefTeacherId
            decimal defaultPassingScore;
            string defaultChiefTeacherId;

            // Logic to add new Subject
            Subject newSubject = new Subject
            {
                Id = NewSubjectId,
                SubjectName = NewSubjectName
            };


            List.Add(newSubject); // Add new Subject to ObservableCollection
            SelectedItem = newSubject; // Set the newly added Subject as SelectedItem

            // Reset NewSubjectId and NewSubjectName for next input
            NewSubjectId = string.Empty;
            NewSubjectName = string.Empty;
        }

        private bool CanAddExecute(object parameter)
        {
            // Example: Logic to determine if AddCommand can execute
            // Here, you can add validation logic if needed
            return true; // Placeholder logic
        }

        private void EditExecute(object parameter)
        {
            // Logic to edit selected Subject
            if (SelectedItem != null)
            {
                SelectedItem.Id = NewSubjectId;
                SelectedItem.SubjectName = NewSubjectName;

                // Reset NewSubjectId and NewSubjectName for next input
                NewSubjectId = string.Empty;
                NewSubjectName = string.Empty;
            }
        }

        private bool CanEditExecute(object parameter)
        {
            // Example: Logic to determine if EditCommand can execute
            return SelectedItem != null; // Only allow editing if an item is selected
        }

        private void DeleteExecute(object parameter)
        {
            // Example: Logic to delete selected Subject
            if (SelectedItem != null)
            {
                List.Remove(SelectedItem); // Remove selected item from ObservableCollection
                SelectedItem = null; // Clear the selected item
            }
        }

        private bool CanDeleteExecute(object parameter)
        {
            // Example: Logic to determine if DeleteCommand can execute
            return SelectedItem != null; // Only allow deletion if an item is selected
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // RelayCommand class for ICommand implementation (unchanged)
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
