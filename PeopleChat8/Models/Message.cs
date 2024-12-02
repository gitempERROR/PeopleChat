using System;
using System.Collections.Generic;

namespace PeopleChat8.Models;

public partial class Message
{
    public int Id { get; set; }

    public int SenderId { get; set; }

    public int ReceaverId { get; set; }

    public virtual User Receaver { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
