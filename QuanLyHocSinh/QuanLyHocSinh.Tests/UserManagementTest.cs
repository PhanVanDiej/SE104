using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuanLyHocSinh.ViewModel;
using QuanLyHocSinh.Model;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Data.SqlClient;
using QuanLyHocSinh.Resources;

namespace QuanLyHocSinh.Tests
{
    [TestClass]
    public class UserManagementViewModelTests
    {
        private UserManagementViewModel viewModel;

        [TestInitialize]
        public void Setup()
        {
            CleanupTestData();
            viewModel = new UserManagementViewModel();
            CurrentUser.Instance.Access = "Quản trị viên";
        }

        [TestCleanup]
        public void Cleanup()
        {
            CleanupTestData();
        }

        private void CleanupTestData()
        {
            using (var connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM USERS WHERE ID LIKE 'TEST%'";
                    command.ExecuteNonQuery();
                }
            }
        }

        private void AddTestUser(string id, string fullName, string email, string access)
        {
            using (var connection = new SqlConnection(Data.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO USERS (ID, PASS, FULLNAME, EMAIL, ACCESS) VALUES (@ID, @Password, @FullName, @Email, @Access)";
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@Password", "testpass123");
                    command.Parameters.AddWithValue("@FullName", fullName);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Access", access);
                    command.ExecuteNonQuery();
                }
            }
        }

        // AR-UM-001: Load user list
        [TestMethod]
        public void LoadData_ShouldPopulateUserList()
        {
            // Arrange
            AddTestUser("TEST1", "Test User", "test@email.com", "Giáo viên");

            // Act
            viewModel = new UserManagementViewModel(); // Reload to get fresh data

            // Assert
            Assert.IsNotNull(viewModel.List);
            Assert.IsTrue(viewModel.List.Any(u => u.ID == "TEST1"));
        }

        // AR-UM-002: Add new user with valid data
        [TestMethod]
        public void AddUser_WithValidData_ShouldAddToList()
        {
            // Arrange
            viewModel.ID = "TEST1";
            viewModel.Password = "pass123";
            viewModel.FullName = "Test";
            viewModel.Email = "test@email.com";
            viewModel.Access = "Giáo viên";

            // Act
            viewModel.AddCommand.Execute(null);

            // Assert
            Assert.IsTrue(viewModel.List.Any(u => u.ID == "TEST1"));
            var addedUser = viewModel.List.First(u => u.ID == "TEST1");
            Assert.AreEqual("Test", addedUser.FullName);
            Assert.AreEqual("test@email.com", addedUser.Email);
        }

        // AR-UM-003: Add user with duplicate ID
        [TestMethod]
        public void AddUser_WithDuplicateId_ShouldNotAdd()
        {
            // Arrange
            AddTestUser("TEST1", "First User", "first@email.com", "Giáo viên");
            viewModel = new UserManagementViewModel(); // Reload to get fresh data

            viewModel.ID = "TEST1"; // Duplicate ID
            viewModel.Password = "pass123";
            viewModel.FullName = "Second User";
            viewModel.Email = "second@email.com";
            viewModel.Access = "Giáo viên";

            int initialCount = viewModel.List.Count;

            // Act
            viewModel.AddCommand.Execute(null);

            // Assert
            Assert.AreEqual(initialCount, viewModel.List.Count);
        }

        // AR-UM-004: Add user with invalid email
        [TestMethod]
        public void AddUser_WithInvalidEmail_ShouldNotAdd()
        {
            // Arrange
            viewModel.ID = "TEST1";
            viewModel.Password = "pass123";
            viewModel.FullName = "Test";
            viewModel.Email = "invalid.email"; // Invalid email format
            viewModel.Access = "Giáo viên";

            // Act & Assert
            bool canExecute = viewModel.AddCommand.CanExecute(null);
            Assert.IsFalse(canExecute, "Add command should not be executable with invalid email");
        }

        // AR-UM-005: Edit existing user
        [TestMethod]
        public void EditUser_WithValidData_ShouldUpdateUser()
        {
            // Arrange
            AddTestUser("TEST1", "Original Name", "original@email.com", "Giáo viên");
            viewModel = new UserManagementViewModel(); // Reload to get fresh data

            var userToEdit = viewModel.List.First(u => u.ID == "TEST1");
            viewModel.SelectedItem = userToEdit;
            viewModel.ID = userToEdit.ID;
            viewModel.FullName = "Updated Name";
            viewModel.Email = "updated@email.com";
            viewModel.Access = "Giáo vụ";

            // Act
            viewModel.EditCommand.Execute(null);

            // Assert
            var updatedUser = viewModel.List.First(u => u.ID == "TEST1");
            Assert.AreEqual("Updated Name", updatedUser.FullName);
            Assert.AreEqual("updated@email.com", updatedUser.Email);
            Assert.AreEqual("Giáo vụ", updatedUser.Access);
        }

        // AR-UM-006: Edit user password
        [TestMethod]
        public void EditUser_WithNewPassword_ShouldUpdatePassword()
        {
            // Arrange
            AddTestUser("TEST1", "Test User", "test@email.com", "Giáo viên");
            viewModel = new UserManagementViewModel(); // Reload to get fresh data

            var userToEdit = viewModel.List.First(u => u.ID == "TEST1");
            string originalPassword = userToEdit.Password;

            viewModel.SelectedItem = userToEdit;
            viewModel.ID = userToEdit.ID;
            viewModel.Password = "newpass123";
            viewModel.FullName = userToEdit.FullName;
            viewModel.Email = userToEdit.Email;
            viewModel.Access = userToEdit.Access;

            // Act
            viewModel.EditCommand.Execute(null);

            // Assert
            var updatedUser = viewModel.List.First(u => u.ID == "TEST1");
            Assert.AreNotEqual(originalPassword, updatedUser.Password);
        }

        // AR-UM-007: Delete existing user
        [TestMethod]
        public void DeleteUser_ShouldRemoveFromList()
        {
            // Arrange
            AddTestUser("TEST1", "Test User", "test@email.com", "Giáo viên");
            viewModel = new UserManagementViewModel(); // Reload to get fresh data

            viewModel.SelectedItem = viewModel.List.First(u => u.ID == "TEST1");

            // Act
            viewModel.DeleteCommand.Execute(null);

            // Assert
            Assert.IsFalse(viewModel.List.Any(u => u.ID == "TEST1"));
        }

        // AR-UM-008: Non-admin access attempt
        [TestMethod]
        public void NonAdmin_AttemptToModify_ShouldNotModify()
        {
            // Arrange
            AddTestUser("TEST1", "Test User", "test@email.com", "Giáo viên");
            viewModel = new UserManagementViewModel(); // Reload to get fresh data

            CurrentUser.Instance.Access = "Giáo viên"; // Set non-admin access

            viewModel.ID = "TEST2";
            viewModel.Password = "pass123";
            viewModel.FullName = "New User";
            viewModel.Email = "new@email.com";
            viewModel.Access = "Giáo viên";

            int initialCount = viewModel.List.Count;

            // Act
            viewModel.AddCommand.Execute(null);

            // Assert
            Assert.AreEqual(initialCount, viewModel.List.Count);
        }

        // AR-UM-009: Load access levels
        [TestMethod]
        public void LoadAccessLevels_ShouldContainAllLevels()
        {
            // Assert
            Assert.IsNotNull(viewModel.AccessLevels);
            Assert.IsTrue(viewModel.AccessLevels.Contains("Quản trị viên"));
            Assert.IsTrue(viewModel.AccessLevels.Contains("Phó hiệu trưởng"));
            Assert.IsTrue(viewModel.AccessLevels.Contains("Giáo vụ"));
            Assert.IsTrue(viewModel.AccessLevels.Contains("Giáo viên"));
            Assert.AreEqual(4, viewModel.AccessLevels.Count);
        }
    }
}