using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System;
using QuanLyHocSinh.Model;
using QuanLyHocSinh.ViewModel;

namespace QuanLyHocSinh.Tests
{
    [TestClass]
    public class ScoreInputViewModelTests
    {
        private ScoreInputViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            // Setup the current user as if they logged in
            CurrentUser.Instance.UserId = "u002";  // Use an actual teacher ID from your database
            CurrentUser.Instance.Access = "Giáo viên";

            // Initialize the viewmodel after setting up user
            _viewModel = ScoreInputViewModel.Instance;
        }

        [TestMethod]
        [Description("TR-SM-005: Edit score with valid data")]
        public void EditScore_WithValidData_ShouldBeAbleToEdit()
        {
            // Arrange - Get an existing score from loaded data
            Assert.IsTrue(_viewModel.List.Count > 0, "No scores loaded. Check if test teacher has assigned scores.");
            _viewModel.SelectedItem = _viewModel.List[0];

            decimal originalMiniTest = _viewModel.MiniTest ?? 0;
            _viewModel.MiniTest = 8.5m;
            _viewModel.MidTermTest = 8.0m;
            _viewModel.FinalTermTest = 8.0m;

            // Act
            bool canEdit = _viewModel.checkedEditCommand();

            // Assert
            Assert.IsTrue(canEdit);

            // Cleanup - restore original value
            _viewModel.MiniTest = originalMiniTest;
        }

        [TestMethod]
        [Description("TR-SM-006: Edit score with invalid data (negative)")]
        public void EditScore_WithNegativeScore_ShouldNotBeAbleToEdit()
        {
            // Arrange
            Assert.IsTrue(_viewModel.List.Count > 0, "No scores loaded. Check if test teacher has assigned scores.");
            _viewModel.SelectedItem = _viewModel.List[0];

            decimal originalMiniTest = _viewModel.MiniTest ?? 0;
            _viewModel.MiniTest = -1;

            // Act
            bool canEdit = _viewModel.checkedEditCommand();

            // Assert
            Assert.IsFalse(canEdit);

            // Cleanup
            _viewModel.MiniTest = originalMiniTest;
        }

        [TestMethod]
        [Description("TR-SM-007: Edit score with invalid data (>10)")]
        public void EditScore_WithScoreOverTen_ShouldNotBeAbleToEdit()
        {
            // Arrange
            Assert.IsTrue(_viewModel.List.Count > 0, "No scores loaded. Check if test teacher has assigned scores.");
            _viewModel.SelectedItem = _viewModel.List[0];

            decimal originalMiniTest = _viewModel.MiniTest ?? 0;
            _viewModel.MiniTest = 11;

            // Act
            bool canEdit = _viewModel.checkedEditCommand();

            // Assert
            Assert.IsFalse(canEdit);

            // Cleanup
            _viewModel.MiniTest = originalMiniTest;
        }

        [TestMethod]
        [Description("TR-SM-008: Edit non-existent score")]
        public void EditScore_WithNonExistentData_ShouldNotBeAbleToEdit()
        {
            // Arrange
            _viewModel.StudentId = "NONEXIST";
            _viewModel.ClassId = "NONEXIST";
            _viewModel.SubjectId = "NONEXIST";
            _viewModel.Term = 1;
            _viewModel.MiniTest = 8;

            // Act
            bool canEdit = _viewModel.checkedEditCommand();

            // Assert
            Assert.IsFalse(canEdit);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Clear the user session
            CurrentUser.Instance.UserId = null;
            CurrentUser.Instance.Access = null;
        }
    }
}