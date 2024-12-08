using System;
using System.Collections.Generic;

namespace PeopleChat8.Models;

public partial class Role
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<AuthDto> Auths { get; set; } = new List<AuthDto>();
}
