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
            new Place("Beograd (Centar)", 44, 20),
            new Place("Novi Sad", 45.27, 19.807),
            new Place("Subotica", 5,40)
        };


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
