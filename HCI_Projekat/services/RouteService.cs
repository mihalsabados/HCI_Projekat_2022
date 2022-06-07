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

        public static Route checkIfRouteExists(string selectedFromRoute, string selectedToRoute)
        {
            foreach (Route route in GetAllRoutes())
            {
                if (route.places[0].Name.Equals(selectedFromRoute) && route.places[route.places.Count - 1].Name.Equals(selectedToRoute))
                    return route;
            }
            return null;
        }

        internal static List<Tuple<Route, Route>> checkIfAlternateRouteExists(string selectedFromRoute, string selectedToRoute)
        {
            List<Tuple<Route,Route>> routes = new List<Tuple<Route, Route>>();
            foreach (Route route in GetAllRoutes())
            {
                if (route.places[0].Name.Equals(selectedFromRoute))
                {
                    var destination = route.places[route.places.Count - 1].Name;
                    foreach (var route2 in GetAllRoutes())
                    {
                        if(route2.places[0].Name.Equals(destination) && route2.places[route2.places.Count - 1].Name.Equals(selectedToRoute))
                        {
                            routes.Add(Tuple.Create(route, route2));
                        }
                    }
                }
            }
            return routes;
        }
    }
}
