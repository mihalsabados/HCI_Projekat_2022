using HCI_Projekat.database;
using HCI_Projekat.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.services
{
    class PlaceService
    {
        public static Place FindPlaceByName(string name)
        {
            return PlaceRepository.FindPlaceByName(name);
        }

        public static List<Place> GetAllPlaces()
        {
            return PlaceRepository.GetAllPlaces();
        }

        public static bool DeletePlaceByName(string name)
        {
            return PlaceRepository.DeletePlaceByName(name);
        }

        internal static void AddNewPlace(Place newPlace)
        {
            PlaceRepository.AddNewPlace(newPlace);
        }
    }
}
