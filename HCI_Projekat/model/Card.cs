using System;
using System.Collections.Generic;
using System.Text;

namespace HCI_Projekat.model
{
    public class Card
    {
        public Route RouteForCard { get; set; }
        public DateTime DateTimeForCard { get; set; }
        public int WagonNumber { get; set; }
        public int SeatNumber { get; set; }
        public double Price { get; set; }
        public Timetable TimetableForCard { get; set; }

        public string ClientName { get; set; }

        public Card() { }

        public Card(Route routeForCard, DateTime dateTimeForCard, int wagonNumber, 
            int seatNumber, double price, Timetable t, string clientName)
        {
            RouteForCard = routeForCard;
            DateTimeForCard = dateTimeForCard;
            WagonNumber = wagonNumber;
            SeatNumber = seatNumber;
            Price = price;
            TimetableForCard = t;
            ClientName = clientName;
        }
    }
}
