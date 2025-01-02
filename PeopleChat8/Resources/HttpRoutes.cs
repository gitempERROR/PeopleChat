using PeopleChat8.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Resources
{
    public static class HttpRoutes
    {
        public static readonly string Login       = "http://127.0.0.1:5001/api/Auth/Login";
        public static readonly string Register    = "http://127.0.0.1:5001/api/Auth/Register";
        public static readonly string UserList    = "http://127.0.0.1:5001/api/Users";
        public static readonly string UserUpdate  = "http://127.0.0.1:5001/api/Users/Update";
        public static readonly string MessageHub  = "http://127.0.0.1:5001/chatHub";
        public static readonly string MessageList = "http://127.0.0.1:5001/api/Messages";
        public static readonly string MessageSend = "http://127.0.0.1:5001/api/Messages/Send";
    }
}
