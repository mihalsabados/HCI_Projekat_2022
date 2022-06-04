using System;
using System.Collections.Generic;
using System.Text;

namespace HCI_Projekat.model
{
    public class DataGridTimetable
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DayOfWeekTimetable weekDay { get; set; }
        public TimeSpan Duration { get; set; }

        public DataGridTimetable()
        {

        }

        public DataGridTimetable(DateTime startDateTime, DateTime endDateTime, DayOfWeekTimetable weekDay, TimeSpan duration)
        {
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            this.weekDay = weekDay;
            Duration = duration;
        }
    }
}
