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
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace HCI_Projekat.gui
{
    /// <summary>
    /// Interaction logic for TimetableView.xaml
    /// </summary>
    public partial class TimetableViewManager : Page
    {
        public static List<DataGridTimetable> dataGridTimetables;
        public static DataGridTimetable SelectedTimetable { get; set; }
        public static String SelectedFromRoute { get; set; }
        public static String SelectedToRoute { get; set; }
        public static DateTime SelectedDate { get; set; }
        public TimetableViewManager()
        {

            InitializeComponent();
            addFromAndToRoutes();

        }

        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5)); ; ;

            cfg.Dispatcher = Application.Current.Dispatcher;
        });


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
            SelectedFromRoute = "";
            SelectedToRoute = "";
            var date = LocaleDatePickerRTL.SelectedDate.Value;
            SelectedDate = date;
            if (fromRoutes.SelectedItem != null)
            {
                ComboBoxItem item = (ComboBoxItem)fromRoutes.SelectedItem;
                SelectedFromRoute = item.Content.ToString();
            }
            else
            {
                FromError.Visibility = Visibility;

            }
            if (toRoutes.SelectedItem != null)
            {
                ComboBoxItem item = (ComboBoxItem)toRoutes.SelectedItem;
                SelectedToRoute = item.Content.ToString();
            }
            else
            {
                ToError.Visibility = Visibility;
            }
            if (!SelectedFromRoute.Equals(""))
            {
                FromError.Visibility = Visibility.Hidden;
                if (!SelectedToRoute.Equals(""))
                {
                    ToError.Visibility = Visibility.Hidden;
                    titleMessage.Content = "Redovi vožnje od stanice: " + SelectedFromRoute +
                        " do stanice: " + SelectedToRoute;
                    Route route = checkIfRouteExists(SelectedFromRoute, SelectedToRoute);
                    if (route == null)
                    {
                        searchedMessage.Content = "Nema vozne linije za traženu relaciju.";
                        this.searchedMessage.Visibility = Visibility.Visible;
                        this.timetable.Visibility = Visibility.Hidden;
                        this.addTimetable.Visibility = Visibility.Hidden;
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
                            this.addTimetable.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            dataGridTimetables = changeTimetableToDataGridTimetable(timetables, date);
                            this.timetable.ItemsSource = dataGridTimetables;
                            this.addTimetable.Visibility = Visibility.Visible;
                            this.timetable.Visibility = Visibility.Visible;
                            this.searchedMessage.Visibility = Visibility.Hidden;
                        }
                    }
                }
            }
        }

        public static List<DataGridTimetable> changeTimetableToDataGridTimetable(List<Timetable> timetables, DateTime selectedDate)
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

        private void ButtonClickEdit(object sender, RoutedEventArgs e)
        {
            SelectedTimetable = timetable.SelectedItem as DataGridTimetable;
            EditTimetable a = new EditTimetable();
            a.ShowDialog();
            Search(sender, e);
        }


        private void ButtonClickDelete(object sender, RoutedEventArgs e)
        { 
            DataGridTimetable item = timetable.SelectedItem as DataGridTimetable;
            bool deleted = TimetableService.DeleteTimetable(RouteService.FindRouteByAttr(SelectedFromRoute, SelectedToRoute), item.StartDateTime, item.EndDateTime, item.weekDay);
            
            Search(sender, e);
            notifier.ShowSuccess("Uspešno ste obrisali red vožnje.");
        }

        private void ButtonClickAdd(object sender, RoutedEventArgs e)
        {
            Window1 a = new Window1();
            a.ShowDialog();
            Search(sender, e);
        }

    }
}
