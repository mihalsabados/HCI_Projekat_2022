using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HCI_Projekat.model;
using HCI_Projekat.database;

namespace HCI_Projekat.services
{
    public static class UserService
    {
        public static User Login(string username, string password)
        {
            User user = UserRepository.FindUserByUsername(username);
            if (user == null)
                return null;
            if (user.Password.Equals(password))
                return user;
            return null;
        }

        public static User findUserByUsername(string username)
        {
            return UserRepository.FindUserByUsername(username);
        }

        public static User RegisterNewUser(string firstName, string lastName, string username, string password)
		{
            User user = UserRepository.SaveNewUser(firstName, lastName, username, password);
            return user;
        }

    }
}
