using System;
using System.Collections.Generic;

namespace PeopleChat8.Models;

public partial class AuthDto(string userLogin, byte[] userPassword, UserDto? userData = null)
{
    public string userLogin { get; set; } = userLogin;
    public byte[] userPassword { get; set; } = userPassword;
    public UserDto? userData { get; set; } = userData;
}
