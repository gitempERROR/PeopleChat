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
using System.Media;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Reflection;

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
        private ObservableCollection<MenuElement> users;

        [ObservableProperty]
        private ObservableCollection<MenuElement> displayedUsers;

        [ObservableProperty]
        private MenuElement? selectedUser;

        [ObservableProperty]
        private string fullname;

        [ObservableProperty]
        private bool userSelected = false;

        [ObservableProperty]
        private ObservableCollection<MessageElement> messages;

        [ObservableProperty]
        private Bitmap? selectedUserImage;

        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private string filter = "";

        [ObservableProperty]
        private ObservableCollection<string> genderList = ["Male", "Female", "Нет фильтра"];

        [ObservableProperty]
        private string gender = "Нет фильтра";

        [ObservableProperty]
        private ObservableCollection<string> sortList = ["А->Я", "Я->А", "Не сортировать"];

        [ObservableProperty]
        private string sort = "Не сортировать";

        private async Task UpdateUsers(string Jwt)
        {
            UserDto userDto = inMemoryUserStorage.GetUser()!;
            List<UserDto> userDtos = await _userService.GetUserList(Jwt, userDto.Id);
            Users = new ObservableCollection<MenuElement>(userDtos.Select(userDto => new MenuElement(userDto)).ToList());
            Users = new ObservableCollection<MenuElement>(Users.Where(userDto => userDto.UserData.Id != inMemoryUserStorage.GetUser()!.Id).ToList());
            if (selectedUserId != -1)
            {
                SelectedUser = Users.Where(userDto => userDto.UserData.Id == selectedUserId).ToList().FirstOrDefault();
            }
        }

        public async void OnMessageEvent(MessageEventArgs a)
        {
            int idNumber = int.Parse(a.Message);
            string? Jwt = inMemoryJwtStorage.GetToken();
            if (SelectedUser != null && idNumber == SelectedUser!.UserData.Id)
            {
                await UpdateMessages(Jwt!);
            }
            else
            {
                MenuElement? user = Users.Where(userDto => userDto.UserData.Id == idNumber).ToList().FirstOrDefault();
                if (user != null) {
                    user.NotificationCount = user.NotificationCount + 1;
                }
                PlayEmbeddedSound("PeopleChat8.Assets.meet-message-sound-1.wav");
            }
        }

        public static void PlayEmbeddedSound(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (SoundPlayer player = new SoundPlayer(stream))
                    {
                        player.Play(); // Проигрывание звука
                    }
                }
            }
        }

        public async void Update()
        {
            string? Jwt = inMemoryJwtStorage.GetToken();
            if (Jwt != null)
            {
                await UpdateUsers(Jwt);
            }

            UserDto currentUser = inMemoryUserStorage.GetUser()!;
            currentUserImage = currentUser.Image != null ? new Bitmap(new MemoryStream(currentUser.Image)) : null;
            if (!isUpdating)
            {
                isUpdating = true;
                UpdateUsersCycle();
            }
            exited = false;
        }

        public async Task Exit()
        {
            inMemoryJwtStorage.RemoveToken();
            inMemoryUserStorage.RemoveUser();
            exited = true;
            await HubService.Instance.StopConnection();
            NavigateToAuth();
        }

        partial void OnGenderChanged(string value)
        {
            Gender = value;
            ApplyFilters();
        }

        partial void OnUsersChanged(ObservableCollection<MenuElement> value)
        {
            Users = value;
            ApplyFilters();
        }

        partial void OnSelectedUserChanged(MenuElement? value)
        {
            string? Jwt = inMemoryJwtStorage.GetToken();
            if (value == null)
            {
                return;
            }
            SelectedUser = value;
            SelectedUser.NotificationCount = 0;
            Fullname = SelectedUser!.UserData.UserFirstname + " " + SelectedUser.UserData.UserLastname;
            UserSelected = true;
            SelectedUserImage = SelectedUser?.Image;
            if (selectedUserId == value.UserData.Id)
            {
                return;
            }
            selectedUserId = SelectedUser.UserData.Id;
            UpdateMessages(Jwt!);
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
            DisplayedUsers = new ObservableCollection<MenuElement>(Users.Where(userDto => (userDto.UserData.UserFirstname + " " + userDto.UserData.UserLastname).Contains(Filter)).ToList());
            if (Gender != "Нет фильтра")
            {
                DisplayedUsers = new ObservableCollection<MenuElement>(DisplayedUsers.Where(userDto => userDto.UserData.Gender == Gender).ToList());
            }
            if (Sort != "Не сортировать")
            {
                switch (Sort)
                {
                    case "А->Я":
                        DisplayedUsers = new ObservableCollection<MenuElement>(DisplayedUsers.OrderBy(userDto => userDto.UserData.UserFirstname).ToList());
                        break;
                    case "Я->А":
                        DisplayedUsers = new ObservableCollection<MenuElement>(DisplayedUsers.OrderByDescending(userDto => userDto.UserData.UserFirstname).ToList());
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

        public async Task SendMessage()
        {
            string? Jwt = inMemoryJwtStorage.GetToken();
            UserDto? userDto = inMemoryUserStorage.GetUser();
            MessageDto? newMessage;
            if (Jwt != null && UserSelected && userDto != null)
            {
                int id = SelectedUser!.UserData.Id;
                newMessage = await _messageService.SendMessage(
                    Jwt,
                    userDto.Id,
                    id,
                    Message
                );
                if (newMessage != null)
                {
                    MessageElement newMessageElement = new MessageElement(newMessage, ref currentUserImage);
                    Messages.Add(newMessageElement);
                }
            }
            Message = "";
        }

        private async Task UpdateMessages(string Jwt)
        {
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
                Messages = new ObservableCollection<MessageElement>(newMessages);
            }
        }

        private async Task UpdateUsersCycle()
        {
            while (true)
            {
                string? Jwt = InMemoryJwtStorage.Instance.GetToken();
                if (Jwt == null)
                {
                    return;
                }
                await UpdateUsers(Jwt);
                await Task.Delay(15000);
                if (exited)
                {
                    return;
                }
            }
        }
    }
}