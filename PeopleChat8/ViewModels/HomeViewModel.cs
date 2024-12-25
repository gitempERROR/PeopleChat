using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using PeopleChat8.Interface;
using PeopleChat8.Models;
using PeopleChat8.Resources;
using PeopleChat8.Services;
using PeopleChatAPI.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PeopleChat8.ViewModels
{
    public partial class HomeViewModel : ViewModelBase, IUpdateViewModel
    {
        private readonly UserService         _userService        = UserService.Instance;
        private readonly MessageService      _messageService     = MessageService.Instance;
        private readonly InMemoryJwtStorage  inMemoryJwtStorage  = InMemoryJwtStorage.Instance;
        private readonly InMemoryUserStorage inMemoryUserStorage = InMemoryUserStorage.Instance;

        private Bitmap? currentUserImage;

        public HomeViewModel()
        {
            users = [];
        }

        public void NavigateToSettings()
        {
            NavigateToRoute(new Resources.NavigationEventArgs(RouteNames.Settings));
        }


        public void NavigateToAuth()
        {
            NavigateToRoute(new Resources.NavigationEventArgs(RouteNames.Auth));
        }

        [ObservableProperty]
        private List<MenuElement> users;

        [ObservableProperty]
        private MenuElement? selectedUser;

        [ObservableProperty]
        private string fullname;

        [ObservableProperty]
        private bool userSelected = false;

        [ObservableProperty]
        private List<MessageElement> messages;

        [ObservableProperty]
        private Bitmap? selectedUserImage;

        public async void Update()
        {
            string? Jwt = inMemoryJwtStorage.GetToken();
            if (Jwt != null)
            {
                List<UserDto> userDtos = await _userService.GetUserList(Jwt);
                Users = userDtos.Select(userDto => new MenuElement(userDto)).ToList();
                Users = Users.Where(userDto => userDto.UserData.Id != inMemoryUserStorage.GetUser()!.Id).ToList();
            }

            UserDto currentUser = inMemoryUserStorage.GetUser()!;
            currentUserImage = currentUser.Image != null ? new Bitmap(new MemoryStream(currentUser.Image)) : null;
        }

        public void Exit()
        {
            inMemoryJwtStorage.RemoveToken();
            inMemoryUserStorage.RemoveUser();
            NavigateToAuth();
        }

        partial void OnSelectedUserChanged(MenuElement? value)
        {
            SelectedUser = value;
            Fullname = SelectedUser!.UserData.UserFirstname + " " + SelectedUser.UserData.UserLastname;
            UserSelected = true;
            SelectedUserImage = SelectedUser?.Image;
            UpdateMessages();
        }

        private async void UpdateMessages()
        {
            string? Jwt = inMemoryJwtStorage.GetToken();
            UserDto? userDto = inMemoryUserStorage.GetUser();
            List<MessageElement> newMessages = new();
            if (Jwt != null && UserSelected && userDto != null)
            {
                List<MessageDto> messageDtos = await _messageService.GetMessageList(Jwt, userDto.Id, SelectedUser.UserData.Id);
                foreach (MessageDto messageDto in messageDtos)
                {
                    if (messageDto.SenderId == userDto.Id)
                    {
                        MessageElement newMessage = new MessageElement(messageDto, ref currentUserImage);
                        newMessages.Add(newMessage);
                    }
                    else
                    {
                        MessageElement newMessage = new MessageElement(messageDto, ref selectedUserImage);
                        newMessages.Add(newMessage);
                    }
                }
                Messages = newMessages;
            }
        }
    }
}