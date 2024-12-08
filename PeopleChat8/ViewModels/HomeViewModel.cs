using CommunityToolkit.Mvvm.ComponentModel;
using PeopleChat8.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PeopleChat8.ViewModels
{
	public partial class HomeViewModel : ViewModelBase
	{
		public HomeViewModel() { 
			users = [];
		}

		[ObservableProperty]
		private List<MenuElement> users;
	}

	public partial class MenuElement : ObservableObject
	{
		public MenuElement(User userData) 
		{
			UserData = userData;
		}

        [ObservableProperty]
        private User userData;
	};
}