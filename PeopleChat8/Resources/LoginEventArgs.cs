using PeopleChat8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Resources
{
    public class LoginEventArgs : EventArgs
    {
        public AuthDto LoginData;
        public LoginEventArgs(AuthDto loginData)
        {
            LoginData = loginData;
        }
    }
}
