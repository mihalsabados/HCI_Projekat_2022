using HCI_Projekat.model;
using HCI_Projekat.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HCI_Projekat.database
{
    public static class RouteRepository
    {
        private static List<Route> routes = new List<Route>()
        {
            new Route("1", new List<Place>(){ 
                PlaceService.FindPlaceByName("Beograd (Centar)"),
                PlaceService.FindPlaceByName("Novi Sad"),

            }),
            new Route("2", new List<Place>(){
                PlaceService.FindPlaceByName("Novi Sad"),
                PlaceService.FindPlaceByName("Beograd (Centar)"),

            }),
            new Route("3", new List<Place>(){
                PlaceService.FindPlaceByName("Sombor"),
                PlaceService.FindPlaceByName("Odžaci"),
                PlaceService.FindPlaceByName("Bački Petrovac"),
                PlaceService.FindPlaceByName("Novi Sad"),
            }),
            new Route("3", new List<Place>(){
                PlaceService.FindPlaceByName("Novi Sad"),
                PlaceService.FindPlaceByName("Bački Petrovac"),
                PlaceService.FindPlaceByName("Odžaci"),
                PlaceService.FindPlaceByName("Sombor"),
            }),
            new Route("4", new List<Place>(){
                PlaceService.FindPlaceByName("Subotica"),
                PlaceService.FindPlaceByName("Beograd (Centar)"),
            }),
            new Route("5", new List<Place>(){
                PlaceService.FindPlaceByName("Beograd (Centar)"),
                PlaceService.FindPlaceByName("Subotica"),
            }),
            new Route("6", new List<Place>(){
                PlaceService.FindPlaceByName("Novi Sad"),
                PlaceService.FindPlaceByName("Smederevo"),
                PlaceService.FindPlaceByName("Jagodina"),
                PlaceService.FindPlaceByName("Niš (Centar)"),
            }),
            new Route("7", new List<Place>(){
                PlaceService.FindPlaceByName("Niš (Centar)"),
                PlaceService.FindPlaceByName("Jagodina"),
                PlaceService.FindPlaceByName("Smederevo"),
                PlaceService.FindPlaceByName("Novi Sad"),
            }),
            new Route("8", new List<Place>(){
                PlaceService.FindPlaceByName("Subotica"),
                PlaceService.FindPlaceByName("Novi Sad"),
                PlaceService.FindPlaceByName("Beograd (Centar)"),
                PlaceService.FindPlaceByName("Kragujevac"),
            }),
            new Route("9", new List<Place>(){
                PlaceService.FindPlaceByName("Kragujevac"),
                PlaceService.FindPlaceByName("Beograd (Centar)"),
                PlaceService.FindPlaceByName("Novi Sad"),
                PlaceService.FindPlaceByName("Subotica"),
            }),
            new Route("10", new List<Place>(){
                PlaceService.FindPlaceByName("Kikinda"),
                PlaceService.FindPlaceByName("Beograd (Centar)"),
            }),
            new Route("11", new List<Place>(){
                PlaceService.FindPlaceByName("Beograd (Centar)"),
                PlaceService.FindPlaceByName("Kikinda"),
            }),
            new Route("12", new List<Place>(){
                PlaceService.FindPlaceByName("Kikinda"),
                PlaceService.FindPlaceByName("Smederevo"),
            }),
            new Route("13", new List<Place>(){
                PlaceService.FindPlaceByName("Smederevo"),
                PlaceService.FindPlaceByName("Kikinda"),
            }),
            new Route("14", new List<Place>(){
                PlaceService.FindPlaceByName("Kikinda"),
                PlaceService.FindPlaceByName("Zrenjanin"),
            }),
            new Route("15", new List<Place>(){
                PlaceService.FindPlaceByName("Zrenjanin"),
                PlaceService.FindPlaceByName("Kikinda"),
            }),
            new Route("16", new List<Place>(){
                PlaceService.FindPlaceByName("Beograd (Centar)"),
                PlaceService.FindPlaceByName("Kragujevac"),
            }),
            new Route("17", new List<Place>(){
                PlaceService.FindPlaceByName("Kragujevac"),
                PlaceService.FindPlaceByName("Beograd (Centar)"),
            }),
            new Route("18", new List<Place>(){
                PlaceService.FindPlaceByName("Zrenjanin"),
                PlaceService.FindPlaceByName("Kragujevac"),
            }),
            new Route("19", new List<Place>(){
                PlaceService.FindPlaceByName("Kragujevac"),
                PlaceService.FindPlaceByName("Zrenjanin"),
            }),
            new Route("20", new List<Place>(){
                PlaceService.FindPlaceByName("Smederevo"),
                PlaceService.FindPlaceByName("Kragujevac"),
            }),
            new Route("21", new List<Place>(){
                PlaceService.FindPlaceByName("Kragujevac"),
                PlaceService.FindPlaceByName("Smederevo"),
            }),
        };

        internal static Route FindRouteByAttr(string selectedFrom, string selectedTo)
        {
            foreach (Route t in routes)
            {
                if (t.places[0].Name.Equals(selectedFrom) && t.places[t.places.Count - 1].Name.Equals(selectedTo))
                {
                    return t;
                }
            }
            return null;
        }

        public static Route FindRouteByName(string name)
        {
            foreach (Route r in routes) {
                if (r.Name.Equals(name))
                    return r;
            }
            return null;
        }

        public static Route SaveNewRoute(string name, List<Place> listPlaces)
		{   
            Route r = new Route(name, listPlaces);
            routes.Add(r);
            return r;
		}

        public static List<Route> GetRoutes()
        {
            return routes;
        }
    }
}
