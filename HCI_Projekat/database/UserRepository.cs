using HCI_Projekat.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HCI_Projekat.database
{
    public static class UserRepository
    {
        private static List<User> users = new List<User>()
        {
            new User("mika", "mikic", "mika123", "123", UserType.MANAGER),
            new User("pera", "peric", "pera123", "123", UserType.CLIENT),
            new User("jova", "jovic", "jova123", "123", UserType.CLIENT)
        };
        

        public static User FindUserByUsername(string username)
        {
            foreach (User u in users) {
                if (u.Username.Equals(username))
                    return u;
            }
            return null;
        }

        public static User SaveNewUser(string firstName, string lastName, string username, string password)
		{   
            User u = new User(firstName, lastName, username, password, UserType.CLIENT);
            users.Add(u);
            return u;
		}

    }
}
