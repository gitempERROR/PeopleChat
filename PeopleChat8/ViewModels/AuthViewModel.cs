using CommunityToolkit.Mvvm.ComponentModel;
using PeopleChat8.Interface;
using PeopleChat8.Models;
using PeopleChat8.Resources;
using PeopleChat8.Services;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace PeopleChat8.ViewModels
{
    public partial class AuthViewModel : ViewModelBase, IUpdateViewModel
    {
        private AuthService? authService = AuthService.Instance;

        [ObservableProperty]
        private string login = "";

        [ObservableProperty]
        private string password = "";

        [ObservableProperty]
        private bool isEnabled = false;

        private readonly int loginMaxLenght = 30;
        private readonly int passwordMaxLenght = 150;

        partial void OnLoginChanged(string value)
        {
            if (Login.Length <= loginMaxLenght)
            {
                Login = value;
            }
            CalculateEnabledButton();
        }

        partial void OnPasswordChanged(string value)
        {
            if (Password.Length <= passwordMaxLenght)
            {
                Password = value;
            }
            CalculateEnabledButton();
        }

        private void CalculateEnabledButton()
        {
            IsEnabled = (Login.Length > 0 && Password.Length > 0);
        }

        public void NavigateToRegister()
        {
            NavigateToRoute(new NavigationEventArgs(RouteNames.Register));
        }

        public void NavigateToHome()
        {
            NavigateToRoute(new NavigationEventArgs(RouteNames.Home));
        }

        public async void LogIntoProgram()
        {
            byte[] EncryptedPassword = Encryption.Encrypt(Password);

            if (authService == null)
                return;

            AuthDto loginData = new
            (
                Login,
                EncryptedPassword
            );

            bool result = await authService.Auth(loginData);

            if (result)
            {
                NavigateToHome();
            }
        }

        public void Update() { }
    }
}