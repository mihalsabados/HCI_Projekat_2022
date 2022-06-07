using HCI_Projekat.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCI_Projekat.database
{
    public static class TicketRepository
    {

        private static List<TrainTicket> tickets = new List<TrainTicket>();

        static TicketRepository(){
            DateTime dt = new DateTime(2022, 6, 13,0,0,0);
            DateTime startTime = TimetableRepository.FindTimetableById(1).StartDateTime;
            dt = dt.AddHours(startTime.Hour);
            dt = dt.AddMinutes(startTime.Minute);
            TrainTicket tt1 = new TrainTicket(RouteRepository.FindRouteByName("1"), dt, 1, "11", "pera");
            TrainTicket tt2 = new TrainTicket(RouteRepository.FindRouteByName("1"), dt, 1, "12", "pera");
            TrainTicket tt3 = new TrainTicket(RouteRepository.FindRouteByName("1"), dt, 1, "13", "jova");
            tickets.Add(tt1);
            tickets.Add(tt2);
            tickets.Add(tt3);
        }

        public static void AddNewTrainTicket(Route route, DateTime dateTime, int wagonNum, string seat, string clientUsername)
        {
            string takenSeat = (int.Parse(seat) + 11).ToString();
            TrainTicket tt = new TrainTicket(route, dateTime, wagonNum, takenSeat, clientUsername);
            tickets.Add(tt);
        }

        public static List<TrainTicket> GetTicketsForDateTimeAndWagon(Route route, DateTime dateTime, int wagonNum)
        {
            var result = from ticket in tickets
                         where ticket.route.Name == route.Name && ticket.dateTime == dateTime && ticket.wagonNum == wagonNum
                         select ticket;

            return result.ToList();
        }
    }
}
