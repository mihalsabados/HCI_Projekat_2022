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
    }
}
