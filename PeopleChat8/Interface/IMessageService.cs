using PeopleChat8.Models;
using PeopleChatAPI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Interface
{
    public interface IMessageService
    {
        public Task<List<MessageDto>> GetMessageList(String Jwt, int currentUserID, int userID);
        public Task<MessageDto> SendMessage(String Jwt, int currentUserID, int userID, string messageContent);
    }
}
