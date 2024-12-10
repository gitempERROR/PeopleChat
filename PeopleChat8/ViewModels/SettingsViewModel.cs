using System;
using System.Collections.Generic;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using System.IO;
using System.Threading.Tasks;
using PeopleChat8.Interface;
using Avalonia;
using PeopleChat8.Services;
using PeopleChat8.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using PeopleChat8.Resources;

namespace PeopleChat8.ViewModels
{
    public partial class SettingsViewModel : ViewModelBase, IUpdateViewModel
    {
        [ObservableProperty]
        private UserDto? currentUser;

        [ObservableProperty]
        private Bitmap? currentUserImage;

        public void Update()
        {
            CurrentUser = InMemoryUserStorage.Instance.GetUser()!;
        }

        public void NavigateToHome()
        {
            NavigateToRoute(new NavigationEventArgs(RouteNames.Home));
        }

        public void Save()
        {
            InMemoryUserStorage.Instance.SaveUser(CurrentUser!);
        }

        public async Task Image()
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } provider)
                throw new NullReferenceException("Missing StorageProvider instance.");

            var files = await provider.OpenFilePickerAsync(
                new FilePickerOpenOptions()
                {
                    Title = "Выберите файл с изображением пользователя",
                    AllowMultiple = false
                }
                );

            await using var readStream = await files[0].OpenReadAsync();
            byte[] buffer = new byte[10000000];
            var bytes = readStream.ReadAtLeast(buffer, 1);

            Array.Resize(ref buffer, bytes);
            CurrentUser!.Image = buffer;
            CurrentUserImage = new Bitmap(new MemoryStream(CurrentUser.Image));
        }
    }
}