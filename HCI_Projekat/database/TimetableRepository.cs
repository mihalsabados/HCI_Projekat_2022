using HCI_Projekat.model;
using HCI_Projekat.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HCI_Projekat.database
{
    public static class TimetableRepository
    {
        private static List<Timetable> timetables = new List<Timetable>()
        {
            new Timetable(1, RouteService.FindRouteByName("1"), 
                new DateTime(2022, 6, 13, 4, 48, 0), new DateTime(2022, 6, 13, 5, 43, 0), DayOfWeekTimetable.WORK),
            new Timetable(2, RouteService.FindRouteByName("1"),
                new DateTime(2022, 6, 13, 6, 13, 0), new DateTime(2022, 6, 13, 7, 10, 0), DayOfWeekTimetable.SATURDAY),
            new Timetable(3, RouteService.FindRouteByName("1"),
                new DateTime(2022, 6, 13, 6, 13, 0), new DateTime(2022, 6, 13, 7, 10, 0), DayOfWeekTimetable.WORK),

        };

        internal static List<Timetable> FindTimetableByRoute(string routeId, DayOfWeekTimetable day)
        {
            List<Timetable> tts = new List<Timetable>();
            foreach (Timetable t in timetables)
            {
                if (t.RouteForTimetable.Name.Equals(routeId) && t.weekDay == day)
                    tts.Add(t);
            }
            return tts;
        }

        public static Timetable FindTimetableById(int id)
        {
            foreach (Timetable t in timetables)
            {
                if (t.Id.Equals(id))
                    return t;
            }
            return null;
        }

        public static void SaveNewRoute(string name, List<Place> listPlaces)
        {
            //Timetable r = new Timetable(name, listPlaces);
            //timetables.Add(r);
            //return r;
        }

        public static List<Timetable> GetTimetables()
        {
            return timetables;
        }
    }
}
