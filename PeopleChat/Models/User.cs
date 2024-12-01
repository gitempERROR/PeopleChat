using System;
using System.Collections.Generic;

namespace PeopleChat.Models;

public partial class User
{
    public int Id { get; set; }

    public int? GenderId { get; set; }

    public int? AuthId { get; set; }

    public string UserFirstname { get; set; } = null!;

    public string UserLastname { get; set; } = null!;

    public byte[]? Image { get; set; }

    public DateOnly? BirthDate { get; set; }

    public virtual Auth? Auth { get; set; }

    public virtual Gender? Gender { get; set; }

    public virtual ICollection<Message> MessageReceavers { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();
}
