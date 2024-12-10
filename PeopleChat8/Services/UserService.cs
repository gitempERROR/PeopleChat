using PeopleChat8.Interface;
using PeopleChat8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using PeopleChat8.Resources;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace PeopleChat8.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private static UserService? _instance;

        public static UserService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserService();
                }
                return _instance!;
            }
        }

        public async Task<List<UserDto>> GetUserList(string Jwt)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Jwt);

            HttpResponseMessage response;
            string route = HttpRoutes.UserList;

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                response = await _httpClient.GetAsync(route);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                return [];
            }

            List<UserDto> deserializedResponse = await response.Content.ReadFromJsonAsync<List<UserDto>>()?? [];

            return deserializedResponse;
        }

        public void UpdateUserData(string Jwt, UserDto userDto) 
        {
            
        }
    }
}
