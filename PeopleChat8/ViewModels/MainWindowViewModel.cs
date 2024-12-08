using CommunityToolkit.Mvvm.ComponentModel;
using PeopleChat8.Models;
using PeopleChat8.Resources;
using PeopleChat8.Services;
using System;

namespace PeopleChat8.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private AuthService authService;
        private InMemoryJwtStorage inMemoryJwtStorage;
        private InMemoryUserStorage inMemoryUserStorage;

        public MainWindowViewModel()
        {
            authService = new AuthService();
            inMemoryJwtStorage = InMemoryJwtStorage.Instance;
            inMemoryUserStorage = InMemoryUserStorage.Instance;

            authService.jwtEvent += inMemoryJwtStorage.SaveToken;
            authService.userEvent += inMemoryUserStorage.SaveUser;

            ViewModel = RoutesMap.Routes[RouteNames.Auth];

            if (ViewModel is AuthViewModel authViewModel) {
                authViewModel.AuthService = authService;
            }

            foreach (var viewModel in RoutesMap.Routes.Values) 
            {
                viewModel.Navigate += NavigateTo;
            }
        }

        private readonly RouteMap RoutesMap = new();

        [ObservableProperty]
        public ViewModelBase viewModel;

        partial void OnViewModelChanged(ViewModelBase value) 
        { 
            ViewModel = value;
        }

        private void NavigateTo(NavigationEventArgs e)
        {
            ViewModel = RoutesMap.Routes[e.Route];
        }
    }
}
