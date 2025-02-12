﻿using PeopleChat8.Interface;
using PeopleChat8.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using PeopleChat8.Resources;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Reflection.Metadata;

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

        public async Task<List<UserDto>> GetUserList(string Jwt, int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Jwt);

            HttpResponseMessage response;
            string route = HttpRoutes.UserList;


            try
            {
                _httpClient.DefaultRequestHeaders.Add("userId", id.ToString()); // Добавляем userId в header

                response = await _httpClient.GetAsync(route);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return [];
            }

            List<UserDto> deserializedResponse = await response.Content.ReadFromJsonAsync<List<UserDto>>() ?? [];

            _httpClient.DefaultRequestHeaders.Remove("userId"); // Удаляем header после получения данных

            return deserializedResponse;
        }

        public async Task<Boolean> UpdateUserData(string Jwt, UserDto userDto) 
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Jwt);

            HttpResponseMessage response;
            HttpContent content;

            string jsonUserData;
            string route = HttpRoutes.UserUpdate;

            try
            {
                jsonUserData = JsonSerializer.Serialize(userDto);
                content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                response = await _httpClient.PostAsync(route, content);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception) 
            { 
                return false; 
            }
        }
    }
}
