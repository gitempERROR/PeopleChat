using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleChat8.Models
{
    public static class DBContexManager
    {
        private static PeopleChatContext context = new();

        public static void CreateNewUser(string userLogin, Byte[] userPassword)
        {
            Auth auth = new Auth();
            try
            {
                auth.UserLogin = userLogin;
                auth.UserPassword = userPassword;
                auth.RoleId = context.Roles.Where(x => x.RoleName == "user").FirstOrDefault()!.Id;
                context.Auths.Add(auth);
                context.SaveChanges();
            }
            catch { }

            try
            {
                User user = new User();
                user.UserFirstname = "";
                user.UserLastname = "";
                user.AuthId = auth.Id;
                context.Users.Add(user);
                context.SaveChanges();
            }
            catch { }
        }

        public static User? LogIn(string userLogin, Byte[] userPassword) 
        {
            Auth? auth = new Auth();
            User? user = new User();
            try
            {
                auth = context.Auths.Where(x => x.UserLogin == userLogin && x.UserPassword == userPassword).FirstOrDefault();
                user = context.Users.Where(x => x.AuthId == auth.Id).FirstOrDefault();

                if (user != null)
                {
                    return user;
                }

                return null;
            }
            catch 
            {
                return null;
            }
        }

        public static List<User> GetUsers() { 
            List<User> users = [.. context.Users];
            return users;
        }
    }
}
