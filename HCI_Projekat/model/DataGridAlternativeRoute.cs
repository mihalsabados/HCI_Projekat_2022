using System;
using System.Collections.Generic;
using System.Text;

namespace HCI_Projekat.model
{
    public class DataGridAlternativeRoute
    {
        public string StartingPlace { get; set; }

        public string MiddlePlace { get; set; }

        public string DestinationPlace { get; set; }

        public DataGridAlternativeRoute()
        {

        }

        public DataGridAlternativeRoute(string startingPlace, string middlePlace, string destinationPlace)
        {
            StartingPlace = startingPlace;
            MiddlePlace = middlePlace;
            DestinationPlace = destinationPlace;
        }
    }
}
