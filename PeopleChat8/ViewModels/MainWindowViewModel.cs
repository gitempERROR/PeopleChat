using CommunityToolkit.Mvvm.ComponentModel;
using PeopleChat8.Interface;
using PeopleChat8.Models;
using PeopleChat8.Resources;
using PeopleChat8.Services;
using PeopleChat8.ViewModels;
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
        [ObservableProperty]
        public ViewModelBase? viewModelSettings;

        partial void OnViewModelChanged(ViewModelBase value) 
        {
            ViewModel = value;
        }

        partial void OnViewModelSettingsChanged(ViewModelBase? value)
        {
            ViewModelSettings = value;
        }

        private void NavigateTo(NavigationEventArgs e)
        {
            if (e.Route == RouteNames.Settings)
            {
                ViewModelSettings = RoutesMap.Routes[e.Route];
            }
            else
            {
                if (ViewModelSettings != null)
                {
                    ViewModelSettings = null;
                }
                ViewModel = RoutesMap.Routes[e.Route];
            }
            Update();
        }

        public void Update() 
        {
            if (ViewModel is IUpdateViewModel updateViewModel)
            {
                updateViewModel.Update();
            }
            if (ViewModelSettings is IUpdateViewModel updateViewModelSettings)
            {
                updateViewModelSettings.Update();
            }
        }
    }
}
