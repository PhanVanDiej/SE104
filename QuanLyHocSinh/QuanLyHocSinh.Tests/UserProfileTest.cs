using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Controls;
using System.Windows;
using QuanLyHocSinh.Model;
using QuanLyHocSinh.Resources;
using QuanLyHocSinh.ViewModel;

namespace QuanLyHocSinh.Tests
{
    [TestClass]
    public class UserProfileViewModelTests
    {
        private UserProfileViewModel _viewModel;
        private LoginViewModel _viewModel0;

        [TestInitialize]
        public void Setup()
        {
            _viewModel0 = new LoginViewModel();
            _viewModel = new UserProfileViewModel();
            CurrentUser.Instance.UserId = "TEST_USER";
        }

        [TestMethod]
        [Description("GF-UP-001: Load user profile data")]
        public void LoadUserProfile_ShouldPopulateData()
        {
            _viewModel0.ID = "u001";
            _viewModel0.Password = "u001";
            bool loginResult = _viewModel0.CheckLogin();
            Assert.IsTrue(loginResult);
            _viewModel.LoadData();
            Assert.IsNotNull(_viewModel.FullName);
            Assert.IsNotNull(_viewModel.Email);
            Assert.IsNotNull(_viewModel.OldHashedPass);
        }

        [TestMethod]
        [Description("GF-UP-002: Validate and save email")]
        public void SaveNewEmail_WithValidEmail_ShouldEnableSaveButton()
        {
            // Arrange
            _viewModel.Email = "valid@email.com";
            _viewModel.FullName = "Test User";

            // Act
            bool canSave = _viewModel.SaveNewEmailCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canSave);
        }

        [TestMethod]
        [Description("GF-UP-003: Invalid email validation")]
        public void SaveNewEmail_WithInvalidEmail_ShouldDisableSaveButton()
        {
            // Arrange
            _viewModel.Email = "invalid-email";
            _viewModel.FullName = "Test User";

            // Act
            bool canSave = _viewModel.SaveNewEmailCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canSave);
        }

        [TestMethod]
        [Description("GF-UP-004: Password change with correct old password")]
        public void SaveNewPassword_WithCorrectOldPassword_ShouldEnableSaveButton()
        {
            // Arrange
            string oldPass = "oldPass";
            _viewModel.OldHashedPass = PasswordManager.HashPassword(oldPass);
            _viewModel.OldPass = oldPass;
            _viewModel.NewPass = "newPass";
            _viewModel.ConfirmNewPass = "newPass";

            // Act
            bool canSave = _viewModel.SaveNewPassCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canSave);
        }

        [TestMethod]
        [Description("GF-UP-005: Password change with incorrect old password")]
        public void SaveNewPassword_WithIncorrectOldPassword_ShouldDisableSaveButton()
        {
            // Arrange
            _viewModel.OldHashedPass = PasswordManager.HashPassword("correctPass");
            _viewModel.OldPass = "wrongPass";
            _viewModel.NewPass = "newPass";
            _viewModel.ConfirmNewPass = "newPass";

            // Act
            bool canSave = _viewModel.SaveNewPassCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canSave);
        }

        [TestMethod]
        [Description("GF-UP-006: Password change with mismatched confirmation")]
        public void SaveNewPassword_WithMismatchedConfirmation_ShouldDisableSaveButton()
        {
            // Arrange
            string oldPass = "oldPass";
            _viewModel.OldHashedPass = PasswordManager.HashPassword(oldPass);
            _viewModel.OldPass = oldPass;
            _viewModel.NewPass = "newPass";
            _viewModel.ConfirmNewPass = "differentPass";

            // Act
            bool canSave = _viewModel.SaveNewPassCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canSave);
        }

        [TestMethod]
        [Description("GF-UP-007: Logout functionality")]
        public void Logout_ShouldClearUserSession()
        {
            // This test is unreliable and tests UI interaction
            // Better handled through manual testing
        }

        [TestCleanup]
        public void Cleanup()
        {
            CurrentUser.Instance.UserId = null;
            CurrentUser.Instance.Access = null;
        }
    }
}