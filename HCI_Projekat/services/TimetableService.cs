using HCI_Projekat.database;
using HCI_Projekat.model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HCI_Projekat.services
{
    class TimetableService
    {
        public static Timetable FindTimetableById(int id)
        {
            return TimetableRepository.FindTimetableById(id);
        }

        public static List<Timetable> FindTimetablesByRoute(String routeId, DayOfWeekTimetable day)
        {
            return TimetableRepository.FindTimetableByRoute(routeId, day);
        }

        public static List<Timetable> GetAllTimetables()
        {
            return TimetableRepository.GetTimetables();
        }

        internal static bool DeleteTimetable(Route route, DateTime startDateTime, DateTime endDateTime, DayOfWeekTimetable weekDay)
        {
            return TimetableRepository.DeleteTimetable(route, startDateTime, endDateTime, weekDay);
        }

        internal static bool checkIfTimetableCanCreate(Route route, string selectedDay, DateTime fromTime, DateTime toTime)
        {
            return TimetableRepository.checkIfTimetableCanCreate(route, selectedDay, fromTime, toTime);

        }

        internal static bool checkIfTimetableCanEdit(Route route, DayOfWeekTimetable selectedDay, DateTime fromTime, DateTime toTime, DataGridTimetable selectedTimetable)
        {
            return TimetableRepository.checkIfTimetableCanEdit(route, selectedDay, fromTime, toTime, selectedTimetable);

        }

        internal static void Edit(DataGridTimetable selectedTimetable, Route route, DateTime fromTime, DateTime toTime, DayOfWeekTimetable weekDay)
        {
            TimetableRepository.Edit(selectedTimetable, route, fromTime, toTime, weekDay);

        }
    }
}
