using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Models
{
    public class UserDto
    {
        public int Id { get; set; }

        public string UserFirstname { get; set; } = null!;

        public string UserLastname { get; set; } = null!;

        public byte[]? Image { get; set; }

        public DateOnly? BirthDate { get; set; }

        public string Gender { get; set; } = "";

        public int NotReadMessages { get; set; }
    }
}
