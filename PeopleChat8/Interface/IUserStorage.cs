using PeopleChat8.Models;
using PeopleChat8.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Interface
{
    internal interface IUserStorage
    {
        UserDto? GetUser();
        void SaveUser(UserEventArgs e);
        void RemoveUser();
    }
}
