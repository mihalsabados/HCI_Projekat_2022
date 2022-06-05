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

        internal static bool DeleteTimetable(Route route, DateTime startDateTime, DateTime endDateTime, DayOfWeekTimetable weekDay)
        {
            foreach (Timetable t in timetables)
            {
                if (weekDay.Equals(t.weekDay) && route.Name.Equals(t.RouteForTimetable.Name) && t.StartDateTime.Hour == startDateTime.Hour && t.StartDateTime.Minute == startDateTime.Minute &&
                    t.EndDateTime.Hour == endDateTime.Hour && t.EndDateTime.Minute == endDateTime.Minute)
                {
                    timetables.Remove(t);
                    return true;
                }
            }
            return false;
        }


        internal static bool checkIfTimetableCanEdit(Route route, DayOfWeekTimetable dayOfWeekTimetable, DateTime fromTime, DateTime toTime, DataGridTimetable selectedTimetable)
        {
            foreach (Timetable t in timetables)
            {
                if (dayOfWeekTimetable.Equals(t.weekDay) && route.Name.Equals(t.RouteForTimetable.Name) && t.StartDateTime.Hour == selectedTimetable.StartDateTime.Hour && t.StartDateTime.Minute == selectedTimetable.StartDateTime.Minute &&
                   t.EndDateTime.Hour == selectedTimetable.EndDateTime.Hour && t.EndDateTime.Minute == selectedTimetable.EndDateTime.Minute)
                    continue;
                DateTime newDateFrom = new DateTime(t.StartDateTime.Year, t.StartDateTime.Month, t.StartDateTime.Day,
                                                    fromTime.Hour, fromTime.Minute, 0);
                DateTime newDateTo = new DateTime(t.EndDateTime.Year, t.EndDateTime.Month, t.EndDateTime.Day,
                                                    toTime.Hour, toTime.Minute, 0);
                if (t.RouteForTimetable.Name.Equals(route.Name) && dayOfWeekTimetable.Equals(t.weekDay))
                {
                    if (newDateFrom >= t.StartDateTime && newDateTo <= t.EndDateTime)
                        return false;
                    if (newDateFrom <= t.StartDateTime && newDateTo >= t.EndDateTime)
                        return false;
                    if (newDateFrom <= t.StartDateTime && newDateTo > t.StartDateTime && newDateTo <= t.EndDateTime)
                        return false;
                    if (newDateFrom >= t.StartDateTime && newDateFrom <= t.EndDateTime)
                        return false;
                    if (newDateTo >= t.StartDateTime && newDateTo <= t.EndDateTime)
                        return false;
                    else return true;
                }
            }
            return true;
        }

        internal static void Edit(DataGridTimetable selectedTimetable, Route route, DateTime fromTime, DateTime toTime, DayOfWeekTimetable weekDay)
        {
            foreach (Timetable t in timetables)
            {
                if (weekDay.Equals(t.weekDay) && route.Name.Equals(t.RouteForTimetable.Name) && t.StartDateTime.Hour == selectedTimetable.StartDateTime.Hour && t.StartDateTime.Minute == selectedTimetable.StartDateTime.Minute &&
                    t.EndDateTime.Hour == selectedTimetable.EndDateTime.Hour && t.EndDateTime.Minute == selectedTimetable.EndDateTime.Minute)
                {
                    t.StartDateTime = new DateTime(fromTime.Year, fromTime.Month, fromTime.Day, fromTime.Hour, fromTime.Minute, 0);
                    t.EndDateTime = new DateTime(toTime.Year, toTime.Month, toTime.Day, toTime.Hour, toTime.Minute, 0);
                    t.Duration = t.EndDateTime.Subtract(t.StartDateTime);
                    break;
                }
            }
        }

        internal static bool checkIfTimetableCanCreate(Route route, string selectedDay, DateTime fromTime, DateTime toTime)
        {
            DayOfWeekTimetable dayOfWeekTimetable;
            if (selectedDay.Equals("Radni dan"))
                dayOfWeekTimetable = DayOfWeekTimetable.WORK;
            else if (selectedDay.Equals("Subota"))
                dayOfWeekTimetable = DayOfWeekTimetable.SATURDAY;
            else dayOfWeekTimetable = DayOfWeekTimetable.SUNDAY;


            foreach (Timetable t in timetables)
            {
                DateTime newDateFrom = new DateTime(t.StartDateTime.Year, t.StartDateTime.Month, t.StartDateTime.Day,
                                                    fromTime.Hour, fromTime.Minute, 0);
                DateTime newDateTo = new DateTime(t.EndDateTime.Year, t.EndDateTime.Month, t.EndDateTime.Day,
                                                    toTime.Hour, toTime.Minute, 0);
                if (t.RouteForTimetable.Name.Equals(route.Name) && dayOfWeekTimetable.Equals(t.weekDay))
                {
                    if (newDateFrom >= t.StartDateTime && newDateTo <= t.EndDateTime)
                        return false;
                    if (newDateFrom <= t.StartDateTime && newDateTo >= t.EndDateTime)
                        return false;
                    if (newDateFrom <= t.StartDateTime && newDateTo > t.StartDateTime && newDateTo <= t.EndDateTime)
                        return false;
                    if (newDateFrom >= t.StartDateTime && newDateFrom <= t.EndDateTime)
                        return false;
                    if (newDateTo >= t.StartDateTime && newDateTo <= t.EndDateTime)
                        return false;
                    else return true;
                }
            }
            return true;
        }

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
