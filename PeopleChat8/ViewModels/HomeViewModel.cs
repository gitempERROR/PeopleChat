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
using System.Threading;
using System.Threading.Tasks;

namespace PeopleChat8.ViewModels
{
    public partial class HomeViewModel : ViewModelBase, IUpdateViewModel
    {
        private readonly UserService         _userService        = UserService.Instance;
        private readonly MessageService      _messageService     = MessageService.Instance;
        private readonly InMemoryJwtStorage  inMemoryJwtStorage  = InMemoryJwtStorage.Instance;
        private readonly InMemoryUserStorage inMemoryUserStorage = InMemoryUserStorage.Instance;
        
        private bool isUpdating = false;
        private bool exited = false;
        private Bitmap? currentUserImage;
        private int selectedUserId = -1;

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
        private List<MenuElement> displayedUsers;

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

        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private string filter = "";

        [ObservableProperty]
        private List<string> genderList = ["Male", "Female", "Нет фильтра"];

        [ObservableProperty]
        private string gender = "Нет фильтра";

        [ObservableProperty]
        private List<string> sortList = ["А->Я", "Я->А", "Не сортировать"];

        [ObservableProperty]
        private string sort = "Не сортировать";

        private async void UpdateUsers(string Jwt)
        {
            List<UserDto> userDtos = await _userService.GetUserList(Jwt);
            Users = userDtos.Select(userDto => new MenuElement(userDto)).ToList();
            Users = Users.Where(userDto => userDto.UserData.Id != inMemoryUserStorage.GetUser()!.Id).ToList();
            if (selectedUserId != -1)
            {
                SelectedUser = Users.Where(userDto => userDto.UserData.Id == selectedUserId).ToList().FirstOrDefault();
            }
        }

        public async void onMessageEvent(MessageEventArgs a)
        {
            int idNumber = int.Parse(a.Message);
            if (idNumber == SelectedUser.UserData.Id)
            {
                UpdateMessages();
            }
        }

        public async void Update()
        {
            string? Jwt = inMemoryJwtStorage.GetToken();
            if (Jwt != null)
            {
                UpdateUsers(Jwt);
            }

            UserDto currentUser = inMemoryUserStorage.GetUser()!;
            currentUserImage = currentUser.Image != null ? new Bitmap(new MemoryStream(currentUser.Image)) : null;
            if (!isUpdating)
            {
                isUpdating = true;
            }
            exited = false;
        }

        public void Exit()
        {
            inMemoryJwtStorage.RemoveToken();
            inMemoryUserStorage.RemoveUser();
            exited = true;
            NavigateToAuth();
        }

        partial void OnGenderChanged(string value)
        {
            Gender = value;
            ApplyFilters();
        }

        partial void OnUsersChanged(List<MenuElement> value)
        {
            Users = value;
            ApplyFilters();
        }

        partial void OnSelectedUserChanged(MenuElement? value)
        {
            if (value != null)
            {
                SelectedUser = value;
                selectedUserId = SelectedUser.UserData.Id;
                Fullname = SelectedUser!.UserData.UserFirstname + " " + SelectedUser.UserData.UserLastname;
                UserSelected = true;
                SelectedUserImage = SelectedUser?.Image;
                UpdateMessages();
                ApplyFilters();
            }
        }

        partial void OnFilterChanged(string value)
        {
            Filter = value;
            ApplyFilters();
        }

        partial void OnSortChanged(string value)
        {
            Sort = value;
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            DisplayedUsers = Users.Where(userDto => (userDto.UserData.UserFirstname + " " + userDto.UserData.UserLastname).Contains(Filter)).ToList();
            if (Gender != "Нет фильтра")
            {
                DisplayedUsers = DisplayedUsers.Where(userDto => userDto.UserData.Gender == Gender).ToList();
            }
            if (Sort != "Не сортировать")
            {
                switch (Sort)
                {
                    case "А->Я":
                        DisplayedUsers = DisplayedUsers.OrderBy(userDto => userDto.UserData.UserFirstname).ToList();
                        break;
                    case "Я->А":
                        DisplayedUsers = DisplayedUsers.OrderByDescending(userDto => userDto.UserData.UserFirstname).ToList();
                        break;
                    default:
                        break;
                }
            }
            foreach (MenuElement userDto in DisplayedUsers)
            {
                if (userDto.UserData.Gender == "")
                {
                    userDto.UserData.Gender = "Не указано";
                }
            }
        }

        public async void SendMessage()
        {
            string? Jwt = inMemoryJwtStorage.GetToken();
            UserDto? userDto = inMemoryUserStorage.GetUser();
            MessageDto? newMessage;
            List<MessageElement> newMessages = new();
            foreach(MessageElement messageElement in Messages)
            {
                newMessages.Add(messageElement);
            }
            if (Jwt != null && UserSelected && userDto != null)
            {
                newMessage = await _messageService.SendMessage(
                    Jwt,
                    userDto.Id,
                    SelectedUser!.UserData.Id,
                    Message
                );
                if (newMessage != null)
                {
                    MessageElement newMessageElement = new MessageElement(newMessage, ref currentUserImage);
                    newMessages.Add(newMessageElement);
                }
            }
            Messages = newMessages;
            Message = "";
        }

        private async void UpdateMessages()
        {
            string? Jwt = inMemoryJwtStorage.GetToken();
            UserDto? userDto = inMemoryUserStorage.GetUser();
            List<MessageElement> newMessages = new();
            if (Jwt != null && UserSelected && userDto != null && SelectedUser != null)
            {
                List<MessageDto> messageDtos = await _messageService.GetMessageList(Jwt, userDto.Id, SelectedUser!.UserData.Id);
                foreach (MessageDto messageDto in messageDtos)
                {
                    if (messageDto.SenderId == userDto.Id)
                    {
                        MessageElement newMessageElement = new MessageElement(messageDto, ref currentUserImage);
                        newMessages.Add(newMessageElement);
                    }
                    else
                    {
                        MessageElement newMessageElement = new MessageElement(messageDto, ref selectedUserImage);
                        newMessages.Add(newMessageElement);
                    }
                }
                Messages = newMessages;
            }
        }

        private async void UpdateUsersCycle()
        {
            while (true)
            {
                string Jwt = InMemoryJwtStorage.Instance.GetToken();
                if (Jwt != null)
                {
                    UpdateUsers(Jwt);
                    await Task.Delay(3000);
                    if (exited)
                    {
                        return;
                    }
                }
            }
        }
    }
}