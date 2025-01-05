using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Models
{
    public partial class MenuElement : ObservableObject
    {
        public MenuElement(UserDto userData)
        {
            UserData = userData;
            image = userData.Image != null ? new Bitmap(new MemoryStream(userData.Image)) : null;
            NotificationCount = userData.NotReadMessages;
        }

        [ObservableProperty]
        private UserDto userData;

        [ObservableProperty]
        private Bitmap? image;

        [ObservableProperty]
        private bool notification = false;

        [ObservableProperty]
        private int notificationCount = 0;

        partial void OnNotificationCountChanged(int value)
        {
            NotificationCount = value;
            if (value == 0)
            {
                Notification = false;
            }
            else
            {
                Notification = true;
            }
        }
    };
}
