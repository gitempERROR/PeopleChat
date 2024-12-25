using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using PeopleChatAPI.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Models
{
    public partial class MessageElement: ObservableObject
    {
        public MessageElement(MessageDto message, ref Bitmap? userImage)
        {
            messageData = message;
            image = userImage;
        }

        [ObservableProperty]
        private MessageDto messageData;

        [ObservableProperty]
        private Bitmap? image;
    }
}
