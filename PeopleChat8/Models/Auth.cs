using System;
using System.Collections.Generic;

namespace PeopleChat8.Models;

public partial class Auth
{
    public int Id { get; set; }

    public string UserLogin { get; set; } = null!;

    public byte[] UserPassword { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
