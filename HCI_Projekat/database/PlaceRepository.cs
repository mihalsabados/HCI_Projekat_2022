using HCI_Projekat.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.database
{
    class PlaceRepository
    {
        private static List<Place> places = new List<Place>()
        {
            new Place("Beograd (Centar)", 44.8178131, 20.4568974),
            new Place("Novi Sad", 45.2551338, 19.8451756),
            new Place("Sombor", 45.773979, 19.118759), 
            new Place("Kisač", 45.347900, 19.728190),
            new Place("Ruma", 45.007889, 19.822540), 
            new Place("Subotica", 46.096958, 19.657539),
            new Place("Niš (Centar)", 43.316872, 21.894501),
            new Place("Zrenjanin", 45.383442882724424, 20.396730192158543),
            new Place("Novi Beograd", 44.817188716187026, 20.390020344181387),
            new Place("Kraljevo", 43.72611554624095, 20.696088017270164),
            new Place("Kragujevac",44.0130251, 20.91925),
            new Place("Smederevo", 44.6651048, 20.9271115),
            new Place("Leskovac", 42.9951304, 21.9464271),
            new Place("Požarevac", 44.6199926, 21.1854),
            new Place("Čačak", 43.8914332, 20.3491624),
            new Place("Kruševac", 43.58264, 21.3264811),
            new Place("Bački Petrovac", 45.3600646, 19.5914331),
            new Place("Odžaci", 45.5058662, 19.2597368),
            new Place("Kikinda", 45.8297838, 20.4653942),
            new Place("Bečej", 45.6153745, 20.0490085),
            new Place("Zrenjanin", 45.3802683, 20.3907614),
            new Place("Jagodina", 43.9794256, 21.2607688),
        };

        internal static void AddNewPlace(Place newPlace)
        {
            places.Add(newPlace);
        }

        public static bool DeletePlaceByName(string name)
        {
            for (int i = 0; i < places.Count; ++i)
            {
                if (places[i].Name.Equals(name))
                {
                    return places.Remove(places[i]);
                }
            }
            return false; 
        }

        public static List<Place> GetAllPlaces()
        {
            return places;
        }

        public static Place FindPlaceByName(string name)
        {
            foreach (Place p in places)
            {
                if (p.Name.Equals(name))
                    return p;
            }
            return null;
        }
    }
}
