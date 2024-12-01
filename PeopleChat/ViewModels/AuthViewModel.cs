using CommunityToolkit.Mvvm.ComponentModel;
using PeopleChat.Resources;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace PeopleChat.ViewModels
{
	public partial class AuthViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string login = "";

        [ObservableProperty]
        private string password = "";

        public void NavigateToRegister()
        {
            NavigateToRoute(new NavigationEventArgs(RouteNames.Register));
        }
    }
}