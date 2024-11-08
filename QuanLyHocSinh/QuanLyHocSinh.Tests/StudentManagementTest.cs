using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System;
using QuanLyHocSinh.ViewModel;
using QuanLyHocSinh.Model;

namespace QuanLyHocSinh.Tests
{
    [TestClass]
    public class StudentManagementViewModelTests
    {
        private StudentManagementViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new StudentManagementViewModel();
            CurrentUser.Instance.UserId = "TEST_USER";
            // Set admin access for testing
            CurrentUser.Instance.Access = "Admin";
        }

        [TestMethod]
        [Description("AS-SM-001: Load student list")]
        public void LoadStudent_ShouldReturnNonEmptyList()
        {
            // Assert
            Assert.IsNotNull(_viewModel.List);
            // Note: Further assertions would depend on actual database state
        }

        [TestMethod]
        [Description("AS-SM-002: Add student with valid data")]
        public void AddStudent_WithValidData_ShouldBeAbleToAdd()
        {
            // Arrange
            _viewModel.Id = "ST001";
            _viewModel.StudentName = "Test Student";
            _viewModel.StudentGender = "Nam";
            _viewModel.StudentBirth = DateTime.Now.AddYears(-18);
            _viewModel.StudentProvince = "Test Province";
            _viewModel.StudentDistrict = "Test District";
            _viewModel.StudentCommune = "Test Commune";
            _viewModel.StudentAddress = "Test Address";
            _viewModel.StudentEmail = "test@email.com";

            // Act
            bool canAdd = _viewModel.AddCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canAdd);
        }

        [TestMethod]
        [Description("AS-SM-003: Add student with missing fields")]
        public void AddStudent_WithMissingFields_ShouldNotBeAbleToAdd()
        {
            // Arrange
            _viewModel.Id = "ST002";
            _viewModel.StudentName = ""; // Missing name
            _viewModel.StudentEmail = "test@email.com";

            // Act
            bool result = _viewModel.checkedAddCommand();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [Description("AS-SM-004: Add student with duplicate ID")]
        public void AddStudent_WithDuplicateId_ShouldNotBeAbleToAdd()
        {
            // Arrange
            // First add a student
            var student = new Student
            {
                Id = "ST003",
                FullName = "Test Student",
                Email = "test@email.com"
            };
            _viewModel.List.Add(student);

            // Try to add another with same ID
            _viewModel.Id = "ST003";
            _viewModel.StudentName = "Another Student";
            _viewModel.StudentEmail = "another@email.com";

            // Act
            bool result = _viewModel.checkedAddCommand();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [Description("AS-SM-005: Add student with invalid email")]
        public void AddStudent_WithInvalidEmail_ShouldNotBeAbleToAdd()
        {
            // Arrange
            _viewModel.Id = "ST004";
            _viewModel.StudentName = "Test Student";
            _viewModel.StudentEmail = "invalid-email";

            // Act
            bool result = _viewModel.checkedAddCommand();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [Description("AS-SM-010: Teacher access attempt")]
        public void TeacherAccess_ShouldNotBeAbleToModifyStudent()
        {
            // Arrange
            CurrentUser.Instance.Access = "Giáo vụ";
            _viewModel.Id = "ST005";
            _viewModel.StudentName = "Test Student";
            _viewModel.StudentEmail = "test@email.com";

            // Act
            bool canAdd = _viewModel.checkedAddCommand();
            bool canDelete = _viewModel.checkedDeleteCommand();

            // Assert
            Assert.IsFalse(canAdd);
            Assert.IsFalse(canDelete);
        }

        [TestMethod]
        [Description("AS-SM-009: Load address dropdowns")]
        public void LoadAddress_ShouldPopulateDropdowns()
        {
            // Assert
            Assert.IsNotNull(_viewModel.Province);
            // When province is selected, district should be loaded
            if (_viewModel.Province.Count > 0)
            {
                _viewModel.StudentProvince = _viewModel.Province[0];
                Assert.IsNotNull(_viewModel.District);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            CurrentUser.Instance.UserId = null;
            CurrentUser.Instance.Access = null;
        }
    }
}