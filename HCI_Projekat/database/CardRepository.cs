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
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 5, 14, 4, 48, 0), 1, 1, 657, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 5, 12, 4, 48, 0), 1, 1, 657, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 5, 11, 4, 48, 0), 1, 1, 657, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 6, 7, 4, 48, 0), 1, 1, 167, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 6, 9, 4, 48, 0), 1, 1, 167, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 6, 8, 4, 48, 0), 1, 1, 167, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 6, 6, 4, 48, 0), 1, 1, 167, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 6, 7, 4, 48, 0), 1, 2, 167, TimetableService.FindTimetableById(1)),

            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 6, 7, 4, 48, 0), 1, 1, 167, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 7, 13, 4, 48, 0), 1, 1, 1200, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 8, 4, 4, 48, 0), 1, 1, 899, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 9, 16, 4, 48, 0), 1, 1, 324, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 10, 13, 4, 48, 0), 1, 1, 440, TimetableService.FindTimetableById(1)),
            new Card(RouteService.FindRouteByName("1"), new DateTime(2022, 11, 11, 4, 48, 0), 1, 1, 657, TimetableService.FindTimetableById(1)),
        };

        internal static List<Card> GetSoldCardForTimetable(int timetableId)
        {
            List<Card> cs = new List<Card>();
            foreach (Card card in cards)
            {
                if (card.TimetableForCard.Id == timetableId)
                    cs.Add(card);
            }
            return cs;
        }

        internal static List<Card> GetCardsForYearAndMonth(int year, int month)
        {
            List<Card> cs = new List<Card>();
            foreach (Card card in cards)
            {
                if (card.DateTimeForCard.Year == year && card.DateTimeForCard.Month == month)
                    cs.Add(card);
            }
            return cs;
        }

        internal static List<double> RevenueForTimetable(int timetableId)
        {
            List<double> revenue = new List<double>();
            
            Dictionary<string, double> dicRev = new Dictionary<string, double>();
            InitializationDict(dicRev, timetableId);

            foreach (Card card in cards)
            {
                if (card.TimetableForCard.Id == timetableId)
                {
                    if (dicRev.ContainsKey(card.DateTimeForCard.Month + " " + card.DateTimeForCard.Year))
                    {
                        dicRev[card.DateTimeForCard.Month + " " + card.DateTimeForCard.Year] += 1;
                    }
                    else
                        dicRev.Add(card.DateTimeForCard.Month + " " + card.DateTimeForCard.Year, 1);
                }
            }
            foreach (double a in dicRev.Values)
            {
                revenue.Add(a);
            }
            return revenue;
        }

        private static void InitializationDict(Dictionary<string, double> dicRev, int timetableId)
        {
            List<int> years = GetAvailableYearsWithSoldCards(timetableId);
            foreach (int year in years)
            {
                for (int i = 1; i <= 12; i++) {
                    dicRev.Add(i + " " + year, 0); 
                }
            }
        }

        internal static List<int> GetAvailableYearsWithSoldCards(int timetableId)
        {
            List<int> years = new List<int>();
            foreach (Card card in cards)
            {
                if (!years.Contains(card.DateTimeForCard.Year) && card.TimetableForCard.Id == timetableId)
                    years.Add(card.DateTimeForCard.Year);
            }
            years.Sort();
            return years;
        }

        internal static List<double> RevenueForMonths(int yearRevenue, int month)
        {
            List<double> revenue = new List<double>();
            Dictionary<int, double> dicRev = new Dictionary<int, double>();
            InitializationDict(dicRev, yearRevenue, month);
            foreach (Card card in cards)
            {
                if (card.DateTimeForCard.Year == yearRevenue && card.DateTimeForCard.Month == month)
                {
                    if (dicRev.ContainsKey(card.DateTimeForCard.Day))
                    {
                        dicRev[card.DateTimeForCard.Day] += 1;
                    }
                    else
                        dicRev.Add(card.DateTimeForCard.Day, 1);
                    
                }
            }
            foreach (double a in dicRev.Values)
            {
                revenue.Add(a);
            }
            return revenue;
        }

        private static void InitializationDict(Dictionary<int, double> dicRev, int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int i=1; i <= days; i++)
            {
                dicRev.Add(i, 0);
            }
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
