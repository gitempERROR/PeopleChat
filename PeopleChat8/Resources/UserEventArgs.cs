using PeopleChat8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Resources
{
    public class UserEventArgs : EventArgs
    {
        public UserDto UserData;
        public UserEventArgs(UserDto user) 
        {
            UserData = user;
        }
    }
}
