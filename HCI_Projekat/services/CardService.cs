using HCI_Projekat.database;
using HCI_Projekat.model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HCI_Projekat.services
{
    public class CardService
    {
        public static List<Card> GetCards()
        {
            return CardRepository.GetCards();
        }

        internal static List<int> GetAvailableYearsWithSoldCards()
        {
            return CardRepository.GetAvailableYearsWithSoldCards();
        }

        internal static List<double> RevenueForMonths(int yearRevenue, int month)
        {
            return CardRepository.RevenueForMonths(yearRevenue, month);
        }

        internal static List<double> RevenueForTimetable(int timetableId)
        {
            return CardRepository.RevenueForTimetable(timetableId);
        }

        internal static List<CardDataGrid> GetCardForYearAndMonth(int year, int month)
        {
            List<Card> cards = CardRepository.GetCardsForYearAndMonth(year, month);
            List<CardDataGrid> data = new List<CardDataGrid>();
            foreach (Card card in cards)
                data.Add(new CardDataGrid(card.RouteForCard.FromTo, card.DateTimeForCard, card.WagonNumber, card.SeatNumber, card.Price, card.ClientName));
            return data;
        }

        internal static List<int> GetAvailableYearsWithSoldCards(int timetableId)
        {
            return CardRepository.GetAvailableYearsWithSoldCards(timetableId);
        }

        internal static List<CardDataGrid> GetSoldCardForTimetable(int timetableId)
        {
            List<Card> cards = CardRepository.GetSoldCardForTimetable(timetableId);
            List<CardDataGrid> data = new List<CardDataGrid>();
            foreach (Card card in cards)
                data.Add(new CardDataGrid(card.RouteForCard.FromTo, card.DateTimeForCard, card.WagonNumber, card.SeatNumber, card.Price, card.ClientName));
            return data;
        }
    }
}
