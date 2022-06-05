using HCI_Projekat.database;
using HCI_Projekat.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.services
{
    class RouteService
    {
        public static Route FindRouteByName(string name)
        {
            return RouteRepository.FindRouteByName(name);
        }

        public static List<Route> GetAllRoutes()
        {
            return RouteRepository.GetRoutes();
        }

        internal static Route FindRouteByAttr(string selectedFrom, string selectedTo)
        {
            return RouteRepository.FindRouteByAttr(selectedFrom, selectedTo);
        }
    }
}
