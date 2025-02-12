﻿using PeopleChat8.ViewModels;
using System;
using System.Collections.Generic;

namespace PeopleChat8.Resources
{
    public class RouteMap
    {
        public RouteMap() 
        {
            Routes = new()
            {
                { RouteNames.Auth,         new AuthViewModel() },
                { RouteNames.Register,     new RegisterViewModel() },
                { RouteNames.Home,         new HomeViewModel() },
                { RouteNames.RegisterUser, new RegisterUserViewModel() },
                { RouteNames.Settings,     new SettingsViewModel() },
            };
        }
        public Dictionary<String, ViewModelBase> Routes { get; }
    }

    public static class RouteNames
    {
        public static readonly String Auth         = "Auth";
        public static readonly String Register     = "Register";
        public static readonly String Home         = "Home";
        public static readonly String RegisterUser = "RegisterUser";
        public static readonly String Settings     = "Settings";
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
