using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using PeopleChat.Resources;
using System;

namespace PeopleChat.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        public delegate void NavigationHandler(NavigationEventArgs e);
        public event NavigationHandler Navigate;

        protected virtual void NavigateToRoute(NavigationEventArgs e)
        {
            Navigate(e);
        }
    }
}
