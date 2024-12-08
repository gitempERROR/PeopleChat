using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Resources
{
    public class JwtEventArgs : EventArgs
    {
        public string Jwt;
        public JwtEventArgs(string jwt) { 
            Jwt = jwt;
        }
    }
}
