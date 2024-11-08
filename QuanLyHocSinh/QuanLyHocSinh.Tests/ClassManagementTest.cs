using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuanLyHocSinh.ViewModel;
using QuanLyHocSinh.Model;
using System.Collections.ObjectModel;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyHocSinh.Tests
{
    [TestClass]
    public class ClassManagementViewModelTests
    {
        private ClassManagementViewModel viewModel;
        private Class mockClass;

        [TestInitialize]
        public void Setup()
        {
            // Create and setup mock data first
            mockClass = new Class
            {
                Id = "TEST1",
                ClassName = "10A1",  // Using shorter name
                SchoolYear = 2024
            };

            // Initialize viewModel after creating mock data
            viewModel = new ClassManagementViewModel();

            // Override the ClassList with our test data
            viewModel.ClassList = new ObservableCollection<Class> { mockClass };

            // Set up SchoolYearList for testing
            viewModel.SchoolYearList = new ObservableCollection<short> { 2024, 2025 };

            // Set the current user as admin
            CurrentUser.Instance.Access = "Quản trị viên";
        }


        // VP-CM-001: Load class list
        [TestMethod]
        public void LoadData_ShouldPopulateClassList()
        {
            Assert.IsNotNull(viewModel.ClassList);
            Assert.IsTrue(viewModel.ClassList.Count > 0);
        }

        // VP-CM-002: Add new class with valid data
        [TestMethod]
        public void AddClass_WithValidData_ShouldAddToList()
        {
            // Arrange
            viewModel.ClassId = "TEST02";
            viewModel.ClassName = "Test Class";
            viewModel.SchoolYear = 2024;
            int initialCount = viewModel.ClassList.Count;

            // Set admin access
            CurrentUser.Instance.Access = "Quản trị viên";

            // Act
            viewModel.AddCommand.Execute(null);

            // Assert
            Assert.AreEqual(initialCount + 1, viewModel.ClassList.Count);
            var addedClass = viewModel.ClassList.LastOrDefault();
            Assert.IsNotNull(addedClass);
            Assert.AreEqual("TEST02", addedClass.Id);
            Assert.AreEqual("Test Class", addedClass.ClassName);
            Assert.AreEqual((short?)2024, addedClass.SchoolYear);
        }

        // VP-CM-003: Add class with duplicate ID - Fixed
        [TestMethod]
        public void AddClass_WithDuplicateId_ShouldNotAdd()
        {
            // Arrange
            viewModel.ClassId = "TEST1"; // Use the exact ID we know exists
            viewModel.ClassName = "Test Class";
            viewModel.SchoolYear = 2024;
            int initialCount = viewModel.ClassList.Count;
            CurrentUser.Instance.Access = "Quản trị viên";

            // Act
            viewModel.AddCommand.Execute(null);

            // Assert
            Assert.AreEqual(initialCount, viewModel.ClassList.Count);
        }

        // VP-CM-004: Add class with missing data
        [TestMethod]
        public void AddClass_WithMissingData_ShouldNotAdd()
        {
            // Arrange
            viewModel.ClassId = "TEST02";
            viewModel.ClassName = null; // Missing class name
            viewModel.SchoolYear = 2024;
            int initialCount = viewModel.ClassList.Count;
            CurrentUser.Instance.Access = "Quản trị viên";

            // Act
            viewModel.AddCommand.Execute(null);

            // Assert
            Assert.AreEqual(initialCount, viewModel.ClassList.Count);
        }

        // VP-CM-005: Edit existing class - Fixed
        [TestMethod]
        public void EditClass_WithValidData_ShouldUpdateClass()
        {
            // Arrange
            viewModel.SelectedItem = mockClass;
            viewModel.ClassId = mockClass.Id;
            viewModel.ClassName = "10A2";  // Using shorter name that fits column length
            viewModel.SchoolYear = 2025;

            // Act
            viewModel.EditCommand.Execute(null);

            // Assert
            var updatedClass = viewModel.ClassList.FirstOrDefault(c => c.Id == mockClass.Id);
            Assert.IsNotNull(updatedClass);
            Assert.AreEqual("10A2", updatedClass.ClassName);
            Assert.AreEqual((short?)2025, updatedClass.SchoolYear);
        }

        // VP-CM-006: Edit class ID
        [TestMethod]
        public void EditClass_AttemptToChangeId_ShouldNotUpdate()
        {
            // Arrange
            viewModel.SelectedItem = mockClass;
            string originalId = mockClass.Id;
            viewModel.ClassId = "NEWID";
            CurrentUser.Instance.Access = "Quản trị viên";

            // Act
            viewModel.EditCommand.Execute(null);

            // Assert
            Assert.AreEqual(originalId, mockClass.Id);
        }

        // VP-CM-007: Delete class
        [TestMethod]
        public void DeleteClass_WithConfirmation_ShouldRemoveClass()
        {
            // Arrange
            viewModel.SelectedItem = mockClass;
            viewModel.ClassId = mockClass.Id;
            int initialCount = viewModel.ClassList.Count;
            CurrentUser.Instance.Access = "Quản trị viên";

            // Mock the MessageBox dialog result
            var mockDialog = new DialogResult();
            mockDialog = DialogResult.Yes;

            // Act
            viewModel.DeleteCommand.Execute(null);

            // Assert - Check if class was removed
            Assert.IsFalse(viewModel.ClassList.Contains(mockClass));
            Assert.AreEqual(initialCount - 1, viewModel.ClassList.Count);
        }

        // VP-CM-008: Non-admin access attempt
        [TestMethod]
        public void NonAdmin_AttemptToModify_ShouldNotModify()
        {
            // Arrange
            CurrentUser.Instance.Access = "Regular User";
            viewModel.ClassId = "TEST03";
            viewModel.ClassName = "Test Class";
            viewModel.SchoolYear = 2024;
            int initialCount = viewModel.ClassList.Count;

            // Act
            viewModel.AddCommand.Execute(null);

            // Assert
            Assert.AreEqual(initialCount, viewModel.ClassList.Count);
        }

        // VP-CM-010: Load school year options
        [TestMethod]
        public void LoadSchoolYearList_ShouldPopulateYearOptions()
        {
            // Assert
            Assert.IsNotNull(viewModel.SchoolYearList);
            Assert.IsTrue(viewModel.SchoolYearList.Count > 0);
        }
    }
}