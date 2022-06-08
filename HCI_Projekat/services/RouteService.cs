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
        public static Route FindRouteFromToPlace(string fromTo)
        {
            foreach (Route route in GetAllRoutes())
            {
                if (route.FromTo.Equals(fromTo))
            return route;
            }
            return null;
        }

        internal static List<Tuple<Route, Route>> checkIfAlternateRouteExists(string selectedFromRoute, string selectedToRoute)
        {
            List<Tuple<Route, Route>> routes = new List<Tuple<Route, Route>>();
            foreach (Route route in GetAllRoutes())
            {
                if (route.places[0].Name.Equals(selectedFromRoute))
                {
                    var destination = route.places[route.places.Count - 1].Name;
                    foreach (var route2 in GetAllRoutes())
                    {
                        if (route2.places[0].Name.Equals(destination) && route2.places[route2.places.Count - 1].Name.Equals(selectedToRoute))
                        {
                            routes.Add(Tuple.Create(route, route2));
                        }
                    }
                }
            }
            return routes;
        }
        public static List<Place> GetAllDistinctPlacesForStartRoute()
        {
            List<Place> retVal = new List<Place>();
            HashSet<String> set = new HashSet<String>();

            foreach (Route route in GetAllRoutes())
            {
                if (!set.Contains(route.places[0].Name))
                {
                    set.Add(route.places[0].Name);
                    retVal.Add(route.places[0]);
                }
            }
            return retVal;
        }
        public static List<Place> GetAllDistinctPlacesForEndRoute()
        {
            List<Place> retVal = new List<Place>();
            HashSet<String> set = new HashSet<String>();

            foreach (Route route in GetAllRoutes())
            {
                if (!set.Contains(route.places[^1].Name))
                {
                    set.Add(route.places[^1].Name);
                    retVal.Add(route.places[^1]);
                }
            }
            return retVal;
        }

        

        internal static Route FindRouteByAttr(string selectedFrom, string selectedTo)
        {
            return RouteRepository.FindRouteByAttr(selectedFrom, selectedTo);
        }

        public static bool CheckIfSomeRouteUsePlaceWithName(string placeName)
        {
            foreach (Route r in GetAllRoutes())
            {
                foreach (Place p in r.places)
                {
                    if (p.Name.Equals(placeName))
                        return true;
                }
            }
            return false;
        }

        internal static bool DeleteRouteByName(string name)
        {
            return RouteRepository.DeleteRouteByName(name);
        }

        public static Route AddNewRoute(List<Place> places, Train train)
        {
            return RouteRepository.SaveNewRoute(places, train);
        }

        internal static bool CheckIfRouteWithPlacesAlreadyExist(List<Place> places)
        {
            foreach (Route route in GetAllRoutes())
            {
                int counter = 0;
                if (route.places.Count == places.Count)
                {
                    for (int i = 0; i < places.Count; ++i)
                    {
                        if (route.places[i].Name.Equals(places[i].Name))
                        {
                            counter++;
                        }
                    }
                    if (counter == places.Count)
                        return true;
                }
            }
            return false;
        }

        internal static void ChangeRouteByName(string name, Train train, List<Place> places)
        {
            Route route = FindRouteByName(name);
            route.RouteTrain = train;
            route.places = places;
        }
    }
}
