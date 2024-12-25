using PeopleChat8.Interface;
using PeopleChat8.Models;
using PeopleChat8.Resources;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PeopleChat8.Services
{
    public class AuthService : IAuthService
    {
        public delegate void CurrentUserHandler(UserEventArgs e);
        public event CurrentUserHandler? userEvent;

        public delegate void JwtHandler(JwtEventArgs e);
        public event JwtHandler? jwtEvent;

        private readonly HttpClient _httpClient = new HttpClient();
        private string jwt = "";
        public UserDto CurrentUser { get; set; } = null!;

        private static AuthService? _instance;

        public static AuthService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AuthService();
                }
                return _instance!;
            }
        }

        public async Task<bool> Auth(AuthDto loginData, bool register = false)
        {
            HttpResponseMessage response; // Куда помещается ответ
            HttpContent content; // Контент запроса

            string jsonLoginData; // Строка из которой делаем контент
            string route = register ? HttpRoutes.Register : HttpRoutes.Login; // Я получаю адрес эндпоинта, смотреть в свагере

            try
            {
                jsonLoginData = JsonSerializer.Serialize(loginData); // Сериализую класс с данными логина и пароля, которые называются также как поля необходимые в запросе
                content = new StringContent(jsonLoginData, Encoding.UTF8, "application/json"); // Создаю json контент

                response = await _httpClient.PostAsync(route, content); // Отправляю
                response.EnsureSuccessStatusCode(); // Проверяю на успех
            }
            catch (Exception)
            {
                return false;
            }

            AuthResponseDto? deserializedResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>(); // Десериализую
            if (deserializedResponse != null)
            {
                jwt = deserializedResponse.AccessToken;
                CurrentUser = deserializedResponse.UserData;

                userEvent!(new UserEventArgs(CurrentUser));
                jwtEvent!(new JwtEventArgs(jwt));
                return true;
            }

            return false;
        }
    }
}
