using Microsoft.AspNetCore.SignalR.Client;
using PeopleChat8.Interface;
using PeopleChat8.Resources;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PeopleChat8.Services
{
    internal class HubService : IHubService
    {
        public delegate void MessageHandler(MessageEventArgs e);
        public event MessageHandler? messageEvent;

        private static HubService? _instance;

        private HubConnection? _connection;

        public static HubService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HubService();
                }
                return _instance!;
            }
        }

        public async Task<bool> Connect()
        {
            InMemoryUserStorage storage = InMemoryUserStorage.Instance;
            _connection = new HubConnectionBuilder()
                .WithUrl(HttpRoutes.MessageHub, options =>
                {
                    options.Headers.Add("userId", storage.GetUser()!.Id.ToString());
                })
                .Build();

            _connection.On<string>("ReceiveMessage", (message) =>
            {
                messageEvent!(new MessageEventArgs(message));
            });

            try
            {
                await _connection.StartAsync();
                Console.WriteLine("Подключено к SignalR Hub.");
                await Task.Delay(Timeout.Infinite);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения: {ex.Message}");
                return false;
            }
            finally
            {
                await _connection.StopAsync();
            }
        }
    }
}
