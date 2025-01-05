using PeopleChat8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Interface
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetUserList(string Jwt, int id);
        public Task<Boolean> UpdateUserData(String Jwt, UserDto userData);
    }
}
