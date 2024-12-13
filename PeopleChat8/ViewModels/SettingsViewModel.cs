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

        [ObservableProperty]
        private DateTime currentUserBirthdate;

        public void Update()
        {
            CurrentUser = InMemoryUserStorage.Instance.GetUser()!;
            CurrentUserImage = CurrentUser.Image != null? new Bitmap(new MemoryStream(CurrentUser.Image)) : null;
            DateOnly birthDate;
            if (CurrentUser.BirthDate != null)
            {
                birthDate = (DateOnly)CurrentUser.BirthDate;
                CurrentUserBirthdate = birthDate.ToDateTime(TimeOnly.MinValue);
            }
        }

        public void NavigateToHome()
        {
            NavigateToRoute(new NavigationEventArgs(RouteNames.Home));
        }

        partial void OnCurrentUserBirthdateChanged(DateTime value)
        {
            CurrentUserBirthdate = value;
        }

        public async void Save()
        {
            CurrentUser!.BirthDate = CurrentUserBirthdate != null? DateOnly.FromDateTime((DateTime)CurrentUserBirthdate!) : null;
            InMemoryUserStorage.Instance.SaveUser(CurrentUser!);
            string jwt = InMemoryJwtStorage.Instance.GetToken()!;
            await UserService.Instance.UpdateUserData(jwt, CurrentUser!);
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