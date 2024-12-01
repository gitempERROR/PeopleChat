using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat.Models
{
    public static class DBContexManager
    {
        private static PeopleChatContext context = new();

        public static void CreateNewUser(string userLogin, Byte[] userPassword)
        {
            Auth auth = new Auth();
            auth.UserLogin = userLogin;
            auth.UserPassword = userPassword;
            auth.RoleId = context.Roles.Where(x => x.RoleName == "user").ToList()[0].Id;
            context.Auths.Add(auth);
            context.SaveChanges();

            User user = new User();
            user.UserFirstname = "";
            user.UserLastname = "";
            user.AuthId = auth.Id;
            context.SaveChanges();
        }
    }
}
