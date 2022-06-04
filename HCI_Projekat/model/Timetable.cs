using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.model
{
    public enum DayOfWeekTimetable { WORK, SATURDAY, SUNDAY }

    public class Timetable
    {
        public int Id { get; set; }
        public Route RouteForTimetable { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DayOfWeekTimetable weekDay { get; set; }
        public TimeSpan Duration {get; set;}

        public Timetable(int id, Route r, DateTime start, DateTime end, DayOfWeekTimetable d)
        {
            Id = id;
            RouteForTimetable = r;
            StartDateTime = start;
            EndDateTime = end;
            Duration = end.Subtract(start);
            weekDay = d;
        }
        public Timetable() { }

    }

}
