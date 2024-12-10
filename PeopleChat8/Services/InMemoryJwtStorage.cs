using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleChat8.Interface;
using PeopleChat8.Resources;

namespace PeopleChat8.Services
{
    public class InMemoryJwtStorage : IJwtStorage
    {
        private static InMemoryJwtStorage? _instance;
        private string? _token;

        private InMemoryJwtStorage() { } // Приватный конструктор

        public static InMemoryJwtStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InMemoryJwtStorage();
                }
                return _instance;
            }
        }

        public void SaveToken(JwtEventArgs e)
        {
            _token = e.Jwt;
        }

        public string? GetToken()
        {
            return _token;
        }

        public void RemoveToken()
        {
            _token = null;
        }
    }
}
