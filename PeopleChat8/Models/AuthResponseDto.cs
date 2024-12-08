using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Models
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = null!;
        public UserDto UserData { get; set; } = null!;
    }
}
