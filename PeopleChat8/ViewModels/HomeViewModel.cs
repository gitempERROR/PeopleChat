using CommunityToolkit.Mvvm.ComponentModel;
using PeopleChat8.Interface;
using PeopleChat8.Models;
using PeopleChat8.Resources;
using PeopleChat8.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PeopleChat8.ViewModels
{
	public partial class HomeViewModel : ViewModelBase, IUpdateViewModel
    {
		private UserService _userService = UserService.Instance;
		private InMemoryJwtStorage inMemoryJwtStorage = InMemoryJwtStorage.Instance;

		public HomeViewModel() { 
			users = [];
		}

		public void NavigateToSettings()
		{
			NavigateToRoute(new Resources.NavigationEventArgs(RouteNames.Settings));
		}

		[ObservableProperty]
		private List<MenuElement> users;

		public async void Update()
		{
			string? Jwt = inMemoryJwtStorage.GetToken();
            if (Jwt != null)
            {
                List<UserDto> userDtos = await _userService.GetUserList(Jwt);
                Users = userDtos.Select(userDto => new MenuElement(userDto)).ToList();
            }
		}
	}

	public partial class MenuElement : ObservableObject
	{
		public MenuElement(UserDto userData) 
		{
			UserData = userData;
		}

        [ObservableProperty]
        private UserDto userData;
	};
}