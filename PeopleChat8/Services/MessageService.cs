using PeopleChat8.Interface;
using PeopleChat8.Resources;
using PeopleChatAPI.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;

namespace PeopleChat8.Services
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private static MessageService? _instance;

        public static MessageService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MessageService();
                }
                return _instance!;
            }
        }

        public async Task<List<MessageDto>> GetMessageList(String Jwt, int currentUserID, int userID)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Jwt);

            HttpResponseMessage response;
            HttpContent content;

            string jsonUserData;
            string route = HttpRoutes.MessageList;

            IdDto request = new(currentUserID, userID);

            try
            {
                jsonUserData = JsonSerializer.Serialize(request);
                content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");

                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                response = await _httpClient.PostAsync(route, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                return [];
            }

            List<MessageDto> deserializedResponse = await response.Content.ReadFromJsonAsync<List<MessageDto>>() ?? [];

            return deserializedResponse;
        }

        public async Task<MessageDto?> SendMessage(String Jwt, int currentUserID, int userID, string messageContent)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Jwt);

            HttpResponseMessage response;
            HttpContent content;

            string jsonData;
            string route = HttpRoutes.MessageSend;

            MessageDto request = new() { 
                Id = 0, 
                SenderId = currentUserID, 
                ReceaverId = userID, 
                MessageContent = messageContent 
            };

            try
            {
                jsonData = JsonSerializer.Serialize(request);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                response = await _httpClient.PostAsync(route, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                return null;
            }

            MessageDto deserializedResponse = await response.Content.ReadFromJsonAsync<MessageDto>() ?? new MessageDto();

            return deserializedResponse;
        }
    }
}
