using PeopleChat8.Interface;
using PeopleChat8.Models;
using PeopleChat8.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public event CurrentUserHandler userEvent;

        public delegate void JwtHandler(JwtEventArgs e);
        public event JwtHandler jwtEvent;

        private readonly HttpClient _httpClient = new HttpClient();
        private string jwt = "";
        public UserDto CurrentUser { get; set; } = null!;

        public async Task<bool> Auth(AuthDto loginData, bool register = false)
        {
            HttpResponseMessage response;
            HttpContent content;

            string jsonLoginData;
            string route = register ? HttpRoutes.Register : HttpRoutes.Login;

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                jsonLoginData = JsonSerializer.Serialize(loginData);
                content = new StringContent(jsonLoginData, Encoding.UTF8, "application/json");

                response = await _httpClient.PostAsync(route, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                return false;
            }

            AuthResponseDto? deserializedResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            if (deserializedResponse != null)
            {
                jwt = deserializedResponse.AccessToken;
                CurrentUser = deserializedResponse.UserData;

                userEvent(new UserEventArgs(CurrentUser));
                jwtEvent(new JwtEventArgs(jwt));
                return true;
            }

            return false;
        }
    }
}
