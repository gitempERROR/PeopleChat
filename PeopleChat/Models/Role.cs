﻿using System;
using System.Collections.Generic;

namespace PeopleChat.Models;

public partial class Role
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Auth> Auths { get; set; } = new List<Auth>();
}
