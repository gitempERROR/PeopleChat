using PeopleChat8.Interface;
using PeopleChat8.Models;
using PeopleChat8.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Services
{
    public class InMemoryUserStorage : IUserStorage
    {
        private static InMemoryUserStorage _instance;
        private UserDto? user;

        private InMemoryUserStorage() { }

        public static InMemoryUserStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InMemoryUserStorage();
                }
                return _instance;
            }
        }

        public UserDto? GetUser()
        {
            return user;
        }

        public void SaveUser(UserEventArgs e)
        {
            user = e.UserData;
        }

        public void RemoveUser()
        {
            user = null;
        }
    }
}
