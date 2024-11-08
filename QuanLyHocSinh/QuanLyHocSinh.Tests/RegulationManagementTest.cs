using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuanLyHocSinh.ViewModel;
using QuanLyHocSinh.Resources;
using System.Linq;
using QuanLyHocSinh.Model;

namespace QuanLyHocSinh.Tests
{
    [TestClass]
    public class RegulationManagementViewModelTests
    {
        private RegulationManagementViewModel viewModel;

        [TestInitialize]
        public void Setup()
        {
            CurrentUser.Instance.Access = "Quản trị viên";
            viewModel = new RegulationManagementViewModel();
        }

        // VP-RM-001: Load regulation list
        [TestMethod]
        public void LoadData_ShouldLoadList()
        {
            Assert.IsNotNull(viewModel.List);
        }

        // VP-RM-002: Add new regulation
        [TestMethod]
        public void AddCommand_WithValidData_ShouldExecute()
        {
            // Arrange
            viewModel.SchoolYear = 2024;
            viewModel.MinAge = 15;
            viewModel.MaxAge = 20;
            viewModel.MaxClassSize = 40;
            viewModel.PassingGPA = 5.0M;
            viewModel.PassingGPAPerSubject = 4.0M;

            // Act & Assert
            Assert.IsTrue(viewModel.AddCommand.CanExecute(null));
        }

        // VP-RM-003: Add regulation with duplicate school year
        [TestMethod]
        public void AddCommand_WithDuplicateYear_ShouldShowError()
        {
            // Arrange
            if (!viewModel.List.Any())
            {
                Assert.Inconclusive("Need existing data for this test");
                return;
            }

            var existingReg = viewModel.List.First();
            viewModel.SchoolYear = existingReg.SchoolYear;
            viewModel.MinAge = 15;
            viewModel.MaxAge = 20;
            viewModel.MaxClassSize = 40;
            viewModel.PassingGPA = 5.0M;
            viewModel.PassingGPAPerSubject = 4.0M;

            // Act
            int initialCount = viewModel.List.Count;
            viewModel.AddCommand.Execute(null);

            // Assert
            Assert.AreEqual(initialCount, viewModel.List.Count, "List count should not change");
        }

        // VP-RM-004: Add regulation with missing fields
        [TestMethod]
        public void AddCommand_WithMissingFields_ShouldShowError()
        {
            // Arrange
            viewModel.SchoolYear = null;  // Missing field
            viewModel.MinAge = 15;
            viewModel.MaxAge = 20;
            viewModel.MaxClassSize = 40;
            viewModel.PassingGPA = 5.0M;
            viewModel.PassingGPAPerSubject = 4.0M;

            // Act
            int initialCount = viewModel.List.Count;
            viewModel.AddCommand.Execute(null);

            // Assert
            Assert.AreEqual(initialCount, viewModel.List.Count, "List count should not change");
        }

        // VP-RM-007: Non-admin access
        [TestMethod]
        public void Commands_WithNonAdminAccess_ShouldShowError()
        {
            // Arrange
            CurrentUser.Instance.Access = "Giáo viên";

            // Set all required fields
            viewModel.SchoolYear = 2024;
            viewModel.MinAge = 15;
            viewModel.MaxAge = 20;
            viewModel.MaxClassSize = 40;
            viewModel.PassingGPA = 5.0M;
            viewModel.PassingGPAPerSubject = 4.0M;

            // Act
            int initialCount = viewModel.List.Count;
            viewModel.AddCommand.Execute(null);

            // Assert
            Assert.AreEqual(initialCount, viewModel.List.Count, "List count should not change for non-admin users");
        }

        // VP-RM-008: Select regulation
        [TestMethod]
        public void SelectItem_WhenSelected_ShouldUpdateFields()
        {
            // Arrange
            if (viewModel.List.Any())
            {
                var firstItem = viewModel.List.First();

                // Act
                viewModel.SelectedItem = firstItem;

                // Assert
                Assert.AreEqual(firstItem.SchoolYear, viewModel.SchoolYear);
                Assert.AreEqual(firstItem.MinAge, viewModel.MinAge);
                Assert.AreEqual(firstItem.MaxAge, viewModel.MaxAge);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            CurrentUser.Instance.Access = null;
        }
    }
}