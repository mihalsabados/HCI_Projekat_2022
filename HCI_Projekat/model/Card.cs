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

        public Card() { }

        public Card(Route routeForCard, DateTime dateTimeForCard, int wagonNumber, int seatNumber, double price)
        {
            RouteForCard = routeForCard;
            DateTimeForCard = dateTimeForCard;
            WagonNumber = wagonNumber;
            SeatNumber = seatNumber;
            Price = price;
        }
    }
}
