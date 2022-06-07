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

        internal static List<double> RevenueForMonths(int yearRevenue)
        {
            return CardRepository.RevenueForMonths(yearRevenue);
        }

        internal static List<double> RevenueForTimetable(int timetableId)
        {
            return CardRepository.RevenueForTimetable(timetableId);
        }
    }
}
