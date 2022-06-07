using System;
using System.Collections.Generic;
using System.Text;

namespace HCI_Projekat.model
{
    public class TrainTicket
    {
        public Route route { get; set; }

        public DateTime dateTime { get; set; }

        public int wagonNum { get; set; }

        public string seat { get; set; }

        public string clientUsername { get; set; }

        public TrainTicket()
        {

        }

        public TrainTicket(Route route, DateTime dateTime, int wagonNum, string seat, string clientUsername)
        {
            this.route = route;
            this.dateTime = dateTime;
            this.wagonNum = wagonNum;
            this.seat = seat;
            this.clientUsername = clientUsername;
        }

    }
}
