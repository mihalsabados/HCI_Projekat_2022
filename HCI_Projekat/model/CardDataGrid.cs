using System;
using System.Collections.Generic;
using System.Text;

namespace HCI_Projekat.model
{
    public class CardDataGrid
    {
        public String RouteName { get; set; }
        public DateTime DateTimeName { get; set; }
        public String PlaceInTrain { get; set; }
        public double Price { get; set; }

        public CardDataGrid() { }

        public CardDataGrid(String rr, DateTime dt, int wagonNumber,
            int seatNumber, double price)
        {
            RouteName = rr;
            DateTimeName = dt;
            PlaceInTrain = wagonNumber + "/" + seatNumber;
            Price = price;
        }
    }

}
