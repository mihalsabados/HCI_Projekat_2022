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
            TrainTicket tt1 = new TrainTicket(RouteRepository.FindRouteByName("1"), dt, 1, "11", "pera123");
            TrainTicket tt2 = new TrainTicket(RouteRepository.FindRouteByName("1"), dt, 1, "12", "pera123");
            TrainTicket tt3 = new TrainTicket(RouteRepository.FindRouteByName("1"), dt, 1, "13", "jova123");
            tickets.Add(tt1);
            tickets.Add(tt2);
            tickets.Add(tt3);
        }

        internal static List<TrainTicket> getTicketsForUser(string username)
        {
            var result = from ticket in tickets
                         where ticket.clientUsername == username
                         orderby ticket.dateTime descending
                         select ticket;
            return result.ToList();
        }

        public static void AddNewTrainTicket(Route route, DateTime dateTime, int wagonNum, string seat, string clientUsername)
        {
            string takenSeat = (int.Parse(seat) + 11).ToString();
            TrainTicket tt = new TrainTicket(route, dateTime, wagonNum, takenSeat, clientUsername);
            tickets.Add(tt);
        }

        internal static List<TrainTicket> searchForTickets(string from, string to, DateTime? selectedDate, string username)
        {
            var result = (from ticket in getTicketsForUser(username)
                         where ticket.dateTime.Date == selectedDate.Value.Date && ticket.dateTime.Month == selectedDate.Value.Month &&
                         ticket.dateTime.Year == selectedDate.Value.Year
                          orderby ticket.dateTime descending
                         select ticket).ToList();
            if (from != "")
                result = result.Where(x => x.route.places[0].Name == from).ToList();
            if(to != "")
                result = result.Where(x => x.route.places[x.route.places.Count - 1].Name == to).ToList();
            return result;
        }

        private static object getTicketsForUser()
        {
            throw new NotImplementedException();
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
