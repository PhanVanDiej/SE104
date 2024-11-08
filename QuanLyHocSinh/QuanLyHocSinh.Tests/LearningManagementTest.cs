using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System;
using QuanLyHocSinh.Model;
using QuanLyHocSinh.ViewModel;

namespace QuanLyHocSinh.Tests
{
    [TestClass]
    public class LearningManagementViewModelTests
    {
        private LearningManagementViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new LearningManagementViewModel();
            CurrentUser.Instance.UserId = "TEST_USER";
            // Set admin access for testing
            CurrentUser.Instance.Access = "Admin";
        }

        [TestMethod]
        [Description("AS-LM-001: Load learning list")]
        public void LoadLearningList_ShouldNotBeNull()
        {
            // Assert
            Assert.IsNotNull(_viewModel.List);
        }

        [TestMethod]
        [Description("AS-LM-002: Load dropdown data")]
        public void LoadDropdowns_ShouldPopulateAllLists()
        {
            // Assert
            Assert.IsNotNull(_viewModel.StudentIdList);
            Assert.IsNotNull(_viewModel.ClassIdList);
            Assert.IsNotNull(_viewModel.TermList);
            Assert.IsTrue(_viewModel.TermList.Contains(1));
            Assert.IsTrue(_viewModel.TermList.Contains(2));
            Assert.IsNotNull(_viewModel.SortClassIdList);
            Assert.IsTrue(_viewModel.SortClassIdList.Contains("Tất cả"));
        }

        [TestMethod]
        [Description("AS-LM-003: Add new learning record")]
        public void AddLearning_WithValidData_ShouldBeAbleToAdd()
        {
            // Arrange
            _viewModel.StudentId = "ST001";
            _viewModel.ClassId = "CLS001";
            _viewModel.Term = 1;
            _viewModel.Note = "Test Note";

            // Act
            bool canAdd = _viewModel.checkedAddCommand();

            // Assert
            Assert.IsTrue(canAdd);
        }

        [TestMethod]
        [Description("AS-LM-004: Add record with missing data")]
        public void AddLearning_WithMissingData_ShouldNotBeAbleToAdd()
        {
            // Arrange - leave required fields empty
            _viewModel.StudentId = "";
            _viewModel.ClassId = "CLS001";
            _viewModel.Term = 1;

            // Act
            bool canAdd = _viewModel.checkedAddCommand();

            // Assert
            Assert.IsFalse(canAdd);
        }

        [TestMethod]
        [Description("AS-LM-005: Filter by class")]
        public void FilterByClass_ShouldUpdateList()
        {
            // Arrange
            string testClass = "CLS001";
            _viewModel.SortClassId = testClass;

            // Assert
            Assert.IsNotNull(_viewModel.List);
            // Note: Further assertions would depend on actual database content
        }

        [TestMethod]
        [Description("AS-LM-006: Edit learning record")]
        public void EditLearning_WithValidData_ShouldBeAbleToEdit()
        {
            // Arrange
            var learningRecord = new Learning
            {
                StudentId = "ST001",
                ClassId = "CLS001",
                Term = 1,
                Note = "Original Note"
            };

            _viewModel.List.Add(learningRecord);
            _viewModel.SelectedItem = learningRecord;
            _viewModel.Note = "Updated Note";

            // Act
            bool canEdit = _viewModel.checkedEditLearningCommand();

            // Assert
            Assert.IsTrue(canEdit);
        }

        [TestMethod]
        [Description("AS-LM-007: Edit duplicate record")]
        public void EditLearning_ToDuplicate_ShouldNotBeAllowed()
        {
            // Arrange
            var existingRecord = new Learning
            {
                StudentId = "ST001",
                ClassId = "CLS001",
                Term = 1
            };
            var selectedRecord = new Learning
            {
                StudentId = "ST002",
                ClassId = "CLS002",
                Term = 1
            };

            _viewModel.List.Add(existingRecord);
            _viewModel.List.Add(selectedRecord);

            // Try to edit second record to match first
            _viewModel.SelectedItem = selectedRecord;
            _viewModel.StudentId = "ST001";
            _viewModel.ClassId = "CLS001";
            _viewModel.Term = 1;

            // Act
            bool canEdit = _viewModel.checkedEditLearningCommand();

            // Assert
            Assert.IsFalse(canEdit);
        }

        [TestMethod]
        [Description("AS-LM-010: Teacher access attempt")]
        public void TeacherAccess_ShouldNotBeAbleToModifyRecords()
        {
            // Arrange
            CurrentUser.Instance.Access = "Giáo vụ";
            _viewModel.StudentId = "ST001";
            _viewModel.ClassId = "CLS001";
            _viewModel.Term = 1;

            // Act
            bool canAdd = _viewModel.checkedAddCommand();
            bool canDelete = _viewModel.checkedDeleteCommand();
            bool canEdit = _viewModel.checkedEditLearningCommand();

            // Assert
            Assert.IsFalse(canAdd);
            Assert.IsFalse(canDelete);
            Assert.IsFalse(canEdit);
        }

        [TestCleanup]
        public void Cleanup()
        {
            CurrentUser.Instance.UserId = null;
            CurrentUser.Instance.Access = null;
        }
    }
}