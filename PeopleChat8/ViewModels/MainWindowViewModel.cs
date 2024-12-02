using CommunityToolkit.Mvvm.ComponentModel;
using PeopleChat8.Resources;
using System;

namespace PeopleChat8.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
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
        }

        private void NavigateTo(NavigationEventArgs e)
        {
            ViewModel = RoutesMap.Routes[e.Route];
        }
    }
}
