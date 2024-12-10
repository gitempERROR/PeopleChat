using CommunityToolkit.Mvvm.ComponentModel;
using PeopleChat8.Interface;
using PeopleChat8.Models;
using PeopleChat8.Resources;
using PeopleChat8.Services;
using System;

namespace PeopleChat8.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase, IUpdateViewModel
    {
        private AuthService authService;
        private UserService userService;
        private InMemoryJwtStorage inMemoryJwtStorage;
        private InMemoryUserStorage inMemoryUserStorage;
        private InMemoryAuthStorage inMemoryAuthStorage;

        public MainWindowViewModel()
        {
            authService = AuthService.Instance;
            userService = UserService.Instance;
            inMemoryJwtStorage = InMemoryJwtStorage.Instance;
            inMemoryUserStorage = InMemoryUserStorage.Instance;
            inMemoryAuthStorage = InMemoryAuthStorage.Instance;

            authService.jwtEvent += inMemoryJwtStorage.SaveToken;
            authService.userEvent += inMemoryUserStorage.SaveUser;

            ViewModel = RoutesMap.Routes[RouteNames.Auth];

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
            if (ViewModel is IUpdateViewModel updateViewModel) 
            {
                updateViewModel.Update();
            }
        }

        private void NavigateTo(NavigationEventArgs e)
        {
            ViewModel = RoutesMap.Routes[e.Route];
        }

        public void Update() { }
    }
}
