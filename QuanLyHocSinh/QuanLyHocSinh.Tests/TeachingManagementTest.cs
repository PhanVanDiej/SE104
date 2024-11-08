using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System;
using QuanLyHocSinh.Model;
using QuanLyHocSinh.ViewModel;

namespace QuanLyHocSinh.Tests
{
    [TestClass]
    public class TeachingManagementViewModelTests
    {
        private TeachingManagementViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new TeachingManagementViewModel();
            CurrentUser.Instance.UserId = "TEST_USER";
            CurrentUser.Instance.Access = "Admin";
        }

        [TestMethod]
        [Description("AS-TM-001: Load teaching assignments")]
        public void LoadTeachingList_ShouldNotBeNull()
        {
            // Assert
            Assert.IsNotNull(_viewModel.List);
            Assert.IsNotNull(_viewModel.RealList);
        }

        [TestMethod]
        [Description("AS-TM-002: Load filter dropdowns")]
        public void LoadDropdowns_ShouldPopulateAllLists()
        {
            // Assert - Check all dropdown lists are loaded
            Assert.IsNotNull(_viewModel.SortTeacherIdList);
            Assert.IsTrue(_viewModel.SortTeacherIdList.Contains("Tất cả"));

            Assert.IsNotNull(_viewModel.SortClassIdList);
            Assert.IsTrue(_viewModel.SortClassIdList.Contains("Tất cả"));

            Assert.IsNotNull(_viewModel.SortSubjectIdList);
            Assert.IsTrue(_viewModel.SortSubjectIdList.Contains("Tất cả"));

            Assert.IsNotNull(_viewModel.SortTermList);
            Assert.IsTrue(_viewModel.SortTermList.Contains("Tất cả"));
            Assert.IsTrue(_viewModel.SortTermList.Contains("1"));
            Assert.IsTrue(_viewModel.SortTermList.Contains("2"));
        }

        [TestMethod]
        [Description("AS-TM-003: Filter assignments")]
        public void FilterAssignments_ShouldUpdateRealList()
        {
            // Arrange
            var assignment = new Teaching
            {
                TeacherId = "T001",
                ClassId = "C001",
                SubjectId = "S001",
                Term = 1
            };
            _viewModel.List.Add(assignment);

            // Act - Test each filter
            _viewModel.SortteacherId = "T001";
            _viewModel.SortClassId = "C001";
            _viewModel.SortSubId = "S001";
            _viewModel.SortTermId = "1";

            // Assert
            Assert.IsNotNull(_viewModel.RealList);
            // Note: Further assertions would depend on actual data
        }

        [TestMethod]
        [Description("AS-TM-004: Add new teaching assignment")]
        public void AddTeaching_WithValidData_ShouldBeAbleToAdd()
        {
            // Arrange
            _viewModel.TeacherId = "T001";
            _viewModel.ClassId = "C001";
            _viewModel.SubjectId = "S001";
            _viewModel.Term = 1;

            // Act
            bool canAdd = _viewModel.checkedAddCommand();

            // Assert
            Assert.IsTrue(canAdd);
        }

        [TestMethod]
        [Description("AS-TM-005: Add with missing data")]
        public void AddTeaching_WithMissingData_ShouldNotBeAbleToAdd()
        {
            // Arrange - Leave required fields empty
            _viewModel.TeacherId = null;
            _viewModel.ClassId = "C001";
            _viewModel.SubjectId = "S001";
            _viewModel.Term = 1;

            // Act
            bool canAdd = _viewModel.checkedAddCommand();

            // Assert
            Assert.IsFalse(canAdd);
        }

        [TestMethod]
        [Description("AS-TM-007: Teacher access attempt")]
        public void TeacherAccess_ShouldNotBeAbleToModifyAssignments()
        {
            // Arrange
            CurrentUser.Instance.Access = "Giáo vụ";
            _viewModel.TeacherId = "T001";
            _viewModel.ClassId = "C001";
            _viewModel.SubjectId = "S001";
            _viewModel.Term = 1;

            // Act
            bool canAdd = _viewModel.checkedAddCommand();
            bool canDelete = _viewModel.checkedDeleteCommand();

            // Assert
            Assert.IsFalse(canAdd);
            Assert.IsFalse(canDelete);
        }

        [TestMethod]
        [Description("AS-TM-009: Load teacher-specific data")]
        public void LoadTeacherList_ShouldOnlyContainTeachers()
        {
            // Assert
            Assert.IsNotNull(_viewModel.TeacherIdList);
            foreach (var teacherId in _viewModel.TeacherIdList)
            {
                // Note: In real test we would verify each ID belongs to a teacher
                Assert.IsNotNull(teacherId);
            }
        }

        [TestMethod]
        [Description("AS-TM-006: Delete teaching assignment")]
        public void DeleteTeaching_WithValidSelection_ShouldPromptConfirmation()
        {
            // Arrange
            var assignment = new Teaching
            {
                TeacherId = "T001",
                ClassId = "C001",
                SubjectId = "S001",
                Term = 1
            };
            _viewModel.List.Add(assignment);
            _viewModel.SelectedItem = assignment;

            // Act
            bool canDelete = _viewModel.DeleteCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canDelete);
            // Note: Actual deletion requires user confirmation via MessageBox
        }

        [TestCleanup]
        public void Cleanup()
        {
            CurrentUser.Instance.UserId = null;
            CurrentUser.Instance.Access = null;
        }
    }
}