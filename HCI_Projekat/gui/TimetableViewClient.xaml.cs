using HCI_Projekat.model;
using HCI_Projekat.services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HCI_Projekat.gui
{
    /// <summary>
    /// Interaction logic for TimetableView.xaml
    /// </summary>
    public partial class TimetableViewClient : Page
    {
        public List<DataGridTimetable> dataGridTimetables;
        public TimetableViewClient()
        {
            InitializeComponent();
            addFromAndToRoutes();   
        }

        private void addFromAndToRoutes()
        {
            foreach (Place place in RouteService.GetAllDistinctPlacesForStartRoute())
            {
                fromRoutes.Items.Add(createComboBoxItem(place.Name));
            }
            foreach (Place place in RouteService.GetAllDistinctPlacesForEndRoute())
            {
                toRoutes.Items.Add(createComboBoxItem(place.Name));
            }
        }

        private ComboBoxItem createComboBoxItem(string content)
        {
            ComboBoxItem c = new ComboBoxItem();
            c.Content = content;
            c.Visibility = Visibility.Visible;
            c.Foreground = (Brush)(new BrushConverter().ConvertFrom("#FF485B83"));
            
            return c;
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            String selectedFromRoute = "";
            String selectedToRoute = "";
            var date = LocaleDatePickerRTL.SelectedDate.Value;

            if (fromRoutes.SelectedItem != null)
            {
                ComboBoxItem item = (ComboBoxItem)fromRoutes.SelectedItem;
                selectedFromRoute = item.Content.ToString();
            }
            else
            {
                FromError.Visibility = Visibility;
                
            }
            if (toRoutes.SelectedItem != null)
            {
                ComboBoxItem item = (ComboBoxItem)toRoutes.SelectedItem;
                selectedToRoute = item.Content.ToString();
            }
            else
            {
                ToError.Visibility = Visibility;
            }
            if (!selectedFromRoute.Equals(""))
            {
                FromError.Visibility = Visibility.Hidden;
                if (!selectedToRoute.Equals("")){
                    ToError.Visibility = Visibility.Hidden;
                    titleMessage.Content = "Redovi vožnje od stanice: " + selectedFromRoute +
                        " do stanice: " + selectedToRoute;
                    Route route = checkIfRouteExists(selectedFromRoute, selectedToRoute);
                    if (route == null)
                    {
                        searchedMessage.Content = "Nema vozne linije za traženu relaciju.";
                        this.searchedMessage.Visibility = Visibility.Visible;
                        this.timetable.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        DayOfWeekTimetable selectedDay;
                        if (date.DayOfWeek == DayOfWeek.Saturday)
                        {
                            selectedDay = DayOfWeekTimetable.SATURDAY;
                        }
                        else if (date.DayOfWeek == DayOfWeek.Sunday)
                        {
                            selectedDay = DayOfWeekTimetable.SUNDAY;
                        }
                        else
                        {
                            selectedDay = DayOfWeekTimetable.WORK;
                        }
                        List<Timetable> timetables = TimetableService.FindTimetablesByRoute(route.Name, selectedDay);
                        if (timetables.Count == 0)
                        {
                            searchedMessage.Content = "Nema redova vožnje za traženu relaciju i izabrani datum.";
                            this.searchedMessage.Visibility = Visibility.Visible;
                            this.timetable.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            dataGridTimetables = changeTimetableToDataGridTimetable(timetables, date);
                            this.timetable.ItemsSource = dataGridTimetables;

                            this.timetable.Visibility = Visibility.Visible;
                            this.searchedMessage.Visibility = Visibility.Hidden;
                        }
                    } 
                }
            }
        }

        private List<DataGridTimetable> changeTimetableToDataGridTimetable(List<Timetable> timetables, DateTime selectedDate)
        {
            List<DataGridTimetable> tts = new List<DataGridTimetable>();
            foreach (Timetable t in timetables)
            {
                var s = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, t.StartDateTime.Hour, t.StartDateTime.Minute, 0);
                var e = s.Add(t.Duration);
                tts.Add(new DataGridTimetable(s, e, t.weekDay, t.Duration));
            }
            return tts;
        }

        private Route checkIfRouteExists(string selectedFromRoute, string selectedToRoute)
        {
            foreach (Route route in RouteService.GetAllRoutes())
            {
                if (route.places[0].Name.Equals(selectedFromRoute) && route.places[route.places.Count - 1].Name.Equals(selectedToRoute))
                {
                    return route;
                }
            }
            return null;
        }
    }
}
