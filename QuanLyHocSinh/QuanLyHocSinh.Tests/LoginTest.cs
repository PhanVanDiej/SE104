using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuanLyHocSinh.ViewModel;
using QuanLyHocSinh.View;
using System;

namespace QuanLyHocSinh.Tests
{
    [TestClass]
    public class LoginViewModelTests
    {
        private LoginViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new LoginViewModel();
        }

        [TestMethod]
        [TestCategory("GF-L-001")]
        public void Login_WithValidCredentials_ShouldSucceed()
        {
            // Arrange
            _viewModel.ID = "u001"; // Use a real valid ID from your database
            _viewModel.Password = "u001"; // Use a real valid password

            bool result = _viewModel.CheckLogin();
            Assert.IsTrue(result);

        }

        [TestMethod]
        [TestCategory("GF-L-002")]
        public void Login_WithEmptyFields_ShouldNotProceed()
        {
            // Arrange
            _viewModel.ID = "";
            _viewModel.Password = "";

            // Act
            bool canExecute = _viewModel.LoginCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [TestMethod]
        [TestCategory("GF-L-003")]
        public void Login_WithInvalidCredentials_ShouldFail()
        {
            // Arrange
            _viewModel.ID = "wronguser";
            _viewModel.Password = "wrongpass";

            // Act
            bool result = _viewModel.CheckLogin();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("GF-L-004")]
        public void ForgotPassword_Command_ShouldBeAvailable()
        {
            // Act
            bool canExecute = _viewModel.OpenForgotPassCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }
        [TestMethod]
        [TestCategory("GF-L-005")]
        public void Login_WithDatabaseError_ShouldReturnFalse()
        {
            // Arrange
            _viewModel.ID = "admin";
            _viewModel.Password = "admin123";
            _viewModel.SimulateDatabaseError = true; // Add this to ViewModel

            // Act
            bool result = _viewModel.CheckLogin();

            // Assert
            Assert.IsFalse(result);
        }
    }
}