using Microsoft.Data.SqlClient;
using QuanLyHocSinh.Resources;
using QuanLyHocSinh.ViewModel;

namespace QuanLyHocSinh.Tests
{
    [TestClass]
    public class SubjectManagementViewModelTests
    {
        private SubjectManagementViewModel viewModel;

        [TestInitialize]
        public void Setup()
        {
            // Add test data to database
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "INSERT INTO SUBJECTS (ID, SUBJECTNAME) VALUES ('TEST999', 'Test Subject')",
                    connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            viewModel = new SubjectManagementViewModel();
        }

        // VP-SM-001: Load subject list
        [TestMethod]
        public void LoadData_ShouldLoadInitialData()
        {
            // Assert - LoadData is called in constructor
            Assert.IsNotNull(viewModel.List);
            Assert.IsTrue(viewModel.List.Any()); // Verifies list is not empty
        }

        // VP-SM-002: Add new subject with valid data
        [TestMethod]
        public void AddCommand_WithNewValidData_ShouldAddToList()
        {
            // Arrange
            string newId = "M999"; // Use an ID we know doesn't exist
            viewModel.NewSubjectId = newId;
            viewModel.NewSubjectName = "Test Subject";
            int initialCount = viewModel.List.Count;

            // Act
            bool canExecute = viewModel.AddCommand.CanExecute(null);
            viewModel.AddCommand.Execute(null);

            // Assert
            Assert.IsTrue(canExecute, "Add command should be executable");
            Assert.IsTrue(viewModel.List.Count > initialCount, "List count should increase");
            Assert.IsNotNull(viewModel.List.FirstOrDefault(s => s.Id == newId), "New subject should be in list");
        }

        // VP-SM-003: Select subject from list
        [TestMethod]
        public void SelectItem_WhenItemSelected_ShouldUpdateFields()
        {
            // Arrange
            var firstItem = viewModel.List.FirstOrDefault();
            Assert.IsNotNull(firstItem, "Test requires at least one item in list");

            // Act
            viewModel.SelectedItem = firstItem;

            // Assert
            Assert.AreEqual(firstItem.Id, viewModel.NewSubjectId, "NewSubjectId should match selected item");
            Assert.AreEqual(firstItem.SubjectName, viewModel.NewSubjectName, "NewSubjectName should match selected item");
            Assert.IsTrue(viewModel.EditCommand.CanExecute(null), "Edit command should be enabled");
            Assert.IsTrue(viewModel.DeleteCommand.CanExecute(null), "Delete command should be enabled");
        }

        // VP-SM-004: Edit existing subject
        [TestMethod]
        public void EditCommand_WithSelectedItem_ShouldUpdateSubject()
        {
            // Arrange
            var firstItem = viewModel.List.FirstOrDefault();
            Assert.IsNotNull(firstItem, "Test requires at least one item in list");
            viewModel.SelectedItem = firstItem;
            string originalName = firstItem.SubjectName;
            viewModel.NewSubjectName = "Updated Name";

            // Act
            bool canExecute = viewModel.EditCommand.CanExecute(null);
            viewModel.EditCommand.Execute(null);

            // Assert
            Assert.IsTrue(canExecute, "Edit command should be executable");
            var updatedItem = viewModel.List.First(s => s.Id == firstItem.Id);
            Assert.AreEqual("Updated Name", updatedItem.SubjectName, "Subject name should be updated");
        }

        // VP-SM-005: Delete subject
        [TestMethod]
        public void DeleteCommand_WithSelectedItem_ShouldRemoveFromList()
        {
            // Arrange
            var itemToDelete = viewModel.List.FirstOrDefault(x => x.Id == "TEST999");
            Assert.IsNotNull(itemToDelete, "Test subject should exist in list");

            viewModel.SelectedItem = itemToDelete;
            viewModel.NewSubjectId = itemToDelete.Id; // This is needed because DeleteExecute uses NewSubjectId

            int initialCount = viewModel.List.Count;

            // Act
            bool canExecute = viewModel.DeleteCommand.CanExecute(null);
            Assert.IsTrue(canExecute, "Delete command should be executable");

            viewModel.DeleteCommand.Execute(null);

            // Assert
            Assert.AreEqual(initialCount - 1, viewModel.List.Count, "List should have one less item");
            Assert.IsNull(viewModel.List.FirstOrDefault(s => s.Id == "TEST999"), "Item should be removed from list");
            Assert.IsNull(viewModel.SelectedItem, "SelectedItem should be null after deletion");
        }

        // VP-SM-006: Add subject with empty fields
        [TestMethod]
        public void AddCommand_WithEmptyFields_ShouldNotExecute()
        {
            // Arrange
            viewModel.NewSubjectId = "";
            viewModel.NewSubjectName = "";

            // Act & Assert
            Assert.IsFalse(viewModel.AddCommand.CanExecute(null), "Add command should not execute with empty fields");
        }

        // VP-SM-007: Edit without selection
        [TestMethod]
        public void EditCommand_WithoutSelection_ShouldNotExecute()
        {
            // Arrange
            viewModel.SelectedItem = null;

            // Act & Assert
            Assert.IsFalse(viewModel.EditCommand.CanExecute(null), "Edit command should not execute without selection");
        }

        // VP-SM-008: Delete without selection
        [TestMethod]
        public void DeleteCommand_WithoutSelection_ShouldNotExecute()
        {
            // Arrange
            viewModel.SelectedItem = null;

            // Act & Assert
            Assert.IsFalse(viewModel.DeleteCommand.CanExecute(null), "Delete command should not execute without selection");
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (SqlConnection connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM SUBJECTS WHERE ID = 'M999'", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}