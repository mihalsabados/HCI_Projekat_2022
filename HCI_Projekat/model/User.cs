using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.model
{

    public enum UserType {  MANAGER, CLIENT }

    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }

        public User(string firstName, string lastName, string username, string password, UserType userType)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
            UserType = userType;
        }

        public User() { }

        public List<Reservation> Reservations { get; set; }
    }
}
