using PeopleChat8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Interface
{
    public interface IAuthService
    {
        public Task<bool> Auth(AuthDto loginData, bool register = false);
    }
}
