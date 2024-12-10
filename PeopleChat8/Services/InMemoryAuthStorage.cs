using PeopleChat8.Interface;
using PeopleChat8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Services
{
    public class InMemoryAuthStorage : IAuthStorage
    {
        private static InMemoryAuthStorage? _instance;
        private AuthDto? _authDto;

        public static InMemoryAuthStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InMemoryAuthStorage();
                }
                return _instance!;
            }
        }

        public AuthDto? GetAuthDto()
        {
            return _authDto;
        }

        public void SaveAuthDto(AuthDto authDto)
        {
            _authDto = authDto;
        }

        public void RemoveAuthDto()
        {
            _authDto = null;
        }
    }
}
