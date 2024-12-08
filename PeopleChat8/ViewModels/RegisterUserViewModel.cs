using CommunityToolkit.Mvvm.ComponentModel;
using PeopleChat8.Resources;
using System;
using System.Collections.Generic;

namespace PeopleChat8.ViewModels
{
	public partial class RegisterUserViewModel : ViewModelBase
	{
        [ObservableProperty]
        private string firstName = "";

        [ObservableProperty]
        private string lastName = "";

        [ObservableProperty]
        private bool isEnabled = false;

        [ObservableProperty]
        private byte[]? image = null;

        [ObservableProperty]
        private List<string> genderList = ["Male", "Female"];

        [ObservableProperty]
        private string gender = "";

        private readonly int firstNameMaxLenght = 50;
        private readonly int lastNameMaxLenght = 50;

        partial void OnFirstNameChanged(string value)
        {
            if (FirstName.Length <= firstNameMaxLenght)
            {
                FirstName = value;
            }
            CalculateEnabledButton();
        }

        partial void OnLastNameChanged(string value)
        {
            if (LastName.Length <= lastNameMaxLenght)
            {
                LastName = value;
            }
            CalculateEnabledButton();
        }

        private void CalculateEnabledButton()
        {
            IsEnabled = (FirstName.Length > 0 && LastName.Length > 0);
        }

        public void NavigateToAuth()
        {
            NavigateToRoute(new NavigationEventArgs(RouteNames.Auth));
        }

        public void NavigateToHome()
        {
            NavigateToRoute(new NavigationEventArgs(RouteNames.Home));
        }

        public void Register()
        {
        }
    }
}