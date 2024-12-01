using PeopleChat.ViewModels;
using System;
using System.Collections.Generic;

namespace PeopleChat.Resources
{
    public class RouteMap
    {
        public RouteMap() 
        {
            Routes = new()
            {
                { RouteNames.Auth,     new AuthViewModel() },
                { RouteNames.Register, new RegisterViewModel() },
            };
        }
        public Dictionary<String, ViewModelBase> Routes { get; }
    }

    public static class RouteNames
    {
        public static readonly String Auth = "Auth";
        public static readonly String Register = "Register";
    }

    public class NavigationEventArgs : EventArgs
    {
        public string Route;
        public NavigationEventArgs(string route)
        {
            Route = route;
        }
    }
}
