using HCI_Projekat.model;
using HCI_Projekat.services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HCI_Projekat.database
{
    public static class CardRepository
    {
        private static List<Card> cards = new List<Card>()
        {
            new Card(RouteService.FindRouteByName("1"), new DateTime(2021, 6, 13, 4, 48, 0), 1, 1, 400, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2021, 5, 13, 4, 48, 0), 1, 1, 350, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2021, 5, 12, 4, 48, 0), 1, 1, 429, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 12, 13, 4, 48, 0), 1, 1, 700, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 4, 13, 4, 48, 0), 1, 1, 657, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 1, 13, 4, 48, 0), 1, 1, 657, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 2, 13, 4, 48, 0), 1, 1, 657, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 3, 13, 4, 48, 0), 1, 1, 657, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 4, 11, 4, 48, 0), 1, 1, 657, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 5, 13, 4, 48, 0), 1, 1, 657, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 6, 7, 4, 48, 0), 1, 1, 167, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 7, 13, 4, 48, 0), 1, 1, 1200, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 8, 4, 4, 48, 0), 1, 1, 899, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 9, 16, 4, 48, 0), 1, 1, 324, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 10, 13, 4, 48, 0), 1, 1, 440, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 11, 11, 4, 48, 0), 1, 1, 657, TimetableService.FindTimetableById(1)),
        };

        internal static List<double> RevenueForTimetable(int timetableId)
        {
            List<double> revenue = new List<double>();
            Dictionary<string, double> dicRev = new Dictionary<string, double>();
          
            foreach (Card card in cards)
            {
                if (card.TimetableForCard.Id == timetableId)
                {
                    if (dicRev.ContainsKey(card.DateTimeForCard.Month + " " + card.DateTimeForCard.Year))
                    {
                        dicRev[card.DateTimeForCard.Month + " " + card.DateTimeForCard.Year] += card.Price;
                    }
                    else
                        dicRev.Add(card.DateTimeForCard.Month + " " + card.DateTimeForCard.Year, card.Price);
                }
            }
            foreach (double a in dicRev.Values)
            {
                revenue.Add(a);
            }
            return revenue;
        }

        internal static List<double> RevenueForMonths(int yearRevenue)
        {
            List<double> revenue = new List<double>();
            Dictionary<int, double> dicRev = new Dictionary<int, double>();
            InitializationDict(dicRev);
            foreach (Card card in cards)
            {
                if (card.DateTimeForCard.Year == yearRevenue)
                {
                    if (dicRev.ContainsKey(card.DateTimeForCard.Month))
                    {
                        dicRev[card.DateTimeForCard.Month] += card.Price;
                    }
                    else
                        dicRev.Add(card.DateTimeForCard.Month, card.Price);
                    
                }
            }
            foreach (double a in dicRev.Values)
            {
                revenue.Add(a);
            }
            return revenue;
        }

        private static void InitializationDict(Dictionary<int, double> dicRev)
        {
            dicRev.Add(1, 0);
            dicRev.Add(2, 0);
            dicRev.Add(3, 0);
            dicRev.Add(4, 0);
            dicRev.Add(5, 0);
            dicRev.Add(6, 0);
            dicRev.Add(7, 0);
            dicRev.Add(8, 0);
            dicRev.Add(9, 0);
            dicRev.Add(10, 0);
            dicRev.Add(11, 0);
            dicRev.Add(12, 0);
        }

        internal static List<int> GetAvailableYearsWithSoldCards()
        {
            List<int> years = new List<int>();
            foreach (Card card in cards)
            {
                if (!years.Contains(card.DateTimeForCard.Year))
                    years.Add(card.DateTimeForCard.Year);
            }
            return years;
        }

        public static List<Card> GetCards()
        {
            return cards;
        }
    }
}
