using CommunityToolkit.Mvvm.ComponentModel;
using PeopleChat8.Interface;
using PeopleChat8.Models;
using PeopleChat8.Resources;
using PeopleChat8.Services;
using System;
using System.Collections.Generic;

namespace PeopleChat8.ViewModels
{
	public partial class RegisterUserViewModel : ViewModelBase, IUpdateViewModel
	{
        private AuthService? authService = AuthService.Instance;

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

        [ObservableProperty]
        private DateTime birthDate = DateTime.Now;

        private InMemoryAuthStorage inMemoryAuthStorage = InMemoryAuthStorage.Instance;

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
            inMemoryAuthStorage.RemoveAuthDto();
            NavigateToRoute(new NavigationEventArgs(RouteNames.Auth));
        }

        public void NavigateToHome()
        {
            inMemoryAuthStorage.RemoveAuthDto();
            NavigateToRoute(new NavigationEventArgs(RouteNames.Home));
        }

        public async void Register()
        {
            AuthDto authDto = inMemoryAuthStorage.GetAuthDto()!;
            UserDto userDto = new()
            {
                UserFirstname = FirstName,
                UserLastname = LastName,
                Gender = Gender,
                BirthDate = DateOnly.FromDateTime(BirthDate)
            };

            authDto.userData = userDto;

            if (authService == null)
                return;

            bool result = await authService.Auth(authDto, register: true);

            if (result)
            {
                NavigateToHome();
            }
        }

        public void Update() { }
    }
}