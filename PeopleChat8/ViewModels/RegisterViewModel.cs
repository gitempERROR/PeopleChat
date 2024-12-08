using CommunityToolkit.Mvvm.ComponentModel;
using PeopleChat8.Models;
using PeopleChat8.Resources;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

namespace PeopleChat8.ViewModels
{
	public partial class RegisterViewModel : ViewModelBase
	{
        [ObservableProperty]
        private string login = "";

        [ObservableProperty]
        private string password = "";

        [ObservableProperty]
        private string repeatPassword = "";

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

        partial void OnRepeatPasswordChanged(string value)
        {
            if (RepeatPassword.Length <= passwordMaxLenght)
            {
                RepeatPassword = value;
            }
            CalculateEnabledButton();
        }

        private void CalculateEnabledButton()
        {
            IsEnabled = (Login.Length > 0 && Password.Length > 0 && RepeatPassword == Password);
        }

        public void NavigateToAuth()
        {
            NavigateToRoute(new NavigationEventArgs(RouteNames.Auth));
        }

        public void NavigateToRegisterUser()
        {
            NavigateToRoute(new NavigationEventArgs(RouteNames.RegisterUser));
        }

        public void Register()
        {
            NavigateToRegisterUser();
        }
    }
}