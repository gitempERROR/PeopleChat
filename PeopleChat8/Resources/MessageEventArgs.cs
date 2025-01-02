using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Resources
{
    public class MessageEventArgs : EventArgs
    {
        public string Message;
        public MessageEventArgs(string message)
        {
            Message = message;
        }
    }
}
