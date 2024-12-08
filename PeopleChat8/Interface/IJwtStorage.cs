using PeopleChat8.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Interface
{
    public interface IJwtStorage
    {
        string GetToken();
        void SaveToken(JwtEventArgs e);
        void RemoveToken();
    }
}
