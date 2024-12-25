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
        }

        [ObservableProperty]
        private UserDto userData;

        [ObservableProperty]
        private Bitmap? image;
    };
}
