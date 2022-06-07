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

            }, TrainRepository.FindTrainByName("741")),
            new Route("2", new List<Place>(){
                PlaceService.FindPlaceByName("Novi Sad"),
                PlaceService.FindPlaceByName("Beograd (Centar)"),

            }, TrainRepository.FindTrainByName("743")),
            new Route("3", new List<Place>()
            {
                PlaceService.FindPlaceByName("Novi Sad"),
                PlaceService.FindPlaceByName("Subotica")
            }, TrainRepository.FindTrainByName("743"))
        };
        

        public static Route FindRouteByName(string name)
        {
            foreach (Route r in routes) {
                if (r.Name.Equals(name))
                    return r;
            }
            return null;
        }

        public static Route SaveNewRoute(string name, List<Place> listPlaces, Train train)
		{   
            Route r = new Route(name, listPlaces, train);
            routes.Add(r);
            return r;
		}

        public static List<Route> GetRoutes()
        {
            return routes;
        }
    }
}
