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
    }
}
