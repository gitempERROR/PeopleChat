using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using PeopleChat8.Interface;
using PeopleChat8.Models;
using PeopleChat8.Resources;
using PeopleChat8.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PeopleChat8.ViewModels
{
    public partial class HomeViewModel : ViewModelBase, IUpdateViewModel
    {
        private UserService _userService = UserService.Instance;
        private InMemoryJwtStorage inMemoryJwtStorage = InMemoryJwtStorage.Instance;
        private InMemoryUserStorage inMemoryUserStorage = InMemoryUserStorage.Instance;

        public HomeViewModel()
        {
            users = [];
        }

        public void NavigateToSettings()
        {
            NavigateToRoute(new Resources.NavigationEventArgs(RouteNames.Settings));
        }


        public void NavigateToAuth()
        {
            NavigateToRoute(new Resources.NavigationEventArgs(RouteNames.Auth));
        }

        [ObservableProperty]
        private List<MenuElement> users;
        [ObservableProperty]
        private MenuElement selectedUser;
        [ObservableProperty]
        private string fullname;
        [ObservableProperty]
        private bool userSelected = false;

        public async void Update()
        {
            string? Jwt = inMemoryJwtStorage.GetToken();
            if (Jwt != null)
            {
                List<UserDto> userDtos = await _userService.GetUserList(Jwt);
                Users = userDtos.Select(userDto => new MenuElement(userDto)).ToList();
                Users = Users.Where(userDto => userDto.UserData.Id != inMemoryUserStorage.GetUser()!.Id).ToList();
            }
        }

        public void Exit()
        {
            inMemoryJwtStorage.RemoveToken();
            inMemoryUserStorage.RemoveUser();
            NavigateToAuth();
        }

        partial void OnSelectedUserChanged(MenuElement value)
        {
            SelectedUser = value;
            Fullname = SelectedUser.UserData.UserFirstname + " " + SelectedUser.UserData.UserLastname;
            UserSelected = true;
        }
    }

    public partial class MenuElement : ObservableObject
    {
        public MenuElement(UserDto userData)
        {
            UserData = userData;
            image = userData.Image != null ? new Bitmap(new MemoryStream(userData.Image)) : null;
        }

        [ObservableProperty]
        private UserDto userData;

        [ObservableProperty]
        private Bitmap? image;
    };
}