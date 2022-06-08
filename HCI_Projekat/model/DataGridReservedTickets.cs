using System;
using System.Collections.Generic;
using System.Text;

namespace HCI_Projekat.model
{
    public class DataGridReservedTickets
    {
        public string FromToRoute { get; set; }

        public DateTime StartTime { get; set; }

        public int WagonNum { get; set; }

        public string Seat { get; set; }

        public DataGridReservedTickets()
        {

        }

        public DataGridReservedTickets(string fromToRoute, DateTime startTime, int wagonNum, string seat)
        {
            FromToRoute = fromToRoute;
            StartTime = startTime;
            WagonNum = wagonNum;
            Seat = seat;
        }
    }
}
