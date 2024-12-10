using PeopleChat8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Interface
{
    public interface IAuthStorage
    {
        AuthDto? GetAuthDto();
        void SaveAuthDto(AuthDto authDto);
        void RemoveAuthDto();
    }
}
