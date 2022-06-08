using HCI_Projekat.model;
using HCI_Projekat.services;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for SoldCardsPerRoute.xaml
    /// </summary>
    public partial class SoldCardsPerRoute : Page
    {
        public List<CardDataGrid> cards;
        public bool check;
        public SoldCardsPerRoute()
        {
            InitializeComponent();
            addFromAndToRoutes();
            initCommands();
        }

        private void initCommands()
        {
            RoutedCommand newCmdSearch = new RoutedCommand();
            newCmdSearch.InputGestures.Add(new KeyGesture(Key.Enter));
            this.CommandBindings.Add(new CommandBinding(newCmdSearch, Search));
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

        private void Search(object sender, RoutedEventArgs e)
        {
            String selectedFromRoute = "";
            String selectedToRoute = "";
            String selectedTimetable = timetableComboBox.Text;
            List<Timetable> timetables = new List<Timetable>();
            bool noDataForRoute = false;
            bool noTimetableData = false;

            timtableError.Visibility = Visibility.Hidden;

            if (fromRoutes.SelectedItem != null)
            {
                ComboBoxItem item = (ComboBoxItem)fromRoutes.SelectedItem;
                selectedFromRoute = item.Content.ToString();
            }
            else
            {
                FromError.Visibility = Visibility;
                columnChart.Visibility = Visibility.Hidden;
                label1.Visibility = Visibility.Hidden;
                label.Visibility = Visibility.Hidden;
                review.Visibility = Visibility.Hidden;
            }
            if (toRoutes.SelectedItem != null)
            {
                ComboBoxItem item = (ComboBoxItem)toRoutes.SelectedItem;
                selectedToRoute = item.Content.ToString();
            }
            else
            {
                ToError.Visibility = Visibility;
                columnChart.Visibility = Visibility.Hidden;
                label1.Visibility = Visibility.Hidden;
                label.Visibility = Visibility.Hidden;
                review.Visibility = Visibility.Hidden;
            }
            if (!selectedFromRoute.Equals(""))
            {
                FromError.Visibility = Visibility.Hidden;
                if (!selectedToRoute.Equals(""))
                {
                    ToError.Visibility = Visibility.Hidden;
                    
                    Route route = checkIfRouteExists(selectedFromRoute, selectedToRoute);
                    if (route == null)
                    {
                        noDataForRoute = true;
                        searchedMessage.Content = "Nema vozne linije za traženu relaciju.";
                        searchButton.Content = "Pretraži";
                        this.searchedMessage.Visibility = Visibility.Visible;
                        this.timetableComboBox.Visibility = Visibility.Hidden;
                        selectedTimetable = "";
                        columnChart.Visibility = Visibility.Hidden;
                        label1.Visibility = Visibility.Hidden;
                        label.Visibility = Visibility.Hidden;
                        review.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        
                        timetables = TimetableService.FindTimetablesByIdRouteName(route.Name);
                        if (timetables.Count == 0)
                        {
                            noTimetableData = true;
                            searchedMessage.Content = "Nema redova vožnje za traženu relaciju.";
                            selectedTimetable = "";
                            searchButton.Content = "Pretraži";
                            this.searchedMessage.Visibility = Visibility.Visible;
                            this.timetableComboBox.Visibility = Visibility.Hidden;
                            columnChart.Visibility = Visibility.Hidden;
                            label1.Visibility = Visibility.Hidden;
                            label.Visibility = Visibility.Hidden;
                            review.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            timetableComboBox.Items.Clear();
                            foreach (Timetable tt in timetables)
                            {
                                String fromT = tt.StartDateTime.Hour + ":" + tt.StartDateTime.Minute;
                                String toT = tt.EndDateTime.Hour + ":" + tt.EndDateTime.Minute;
                                String weekDay = "";
                                if (tt.weekDay == DayOfWeekTimetable.WORK)
                                    weekDay = "Radni dan";
                                else if (tt.weekDay == DayOfWeekTimetable.SATURDAY)
                                    weekDay = "Subota";
                                else weekDay = "Nedelja";
                                timetableComboBox.Items.Add(createComboBoxItem(tt.Id + " ( " + fromT + "-" + toT + " ) *" + weekDay));
                            }
                            this.timetableComboBox.Visibility = Visibility.Visible;
                            this.searchedMessage.Visibility = Visibility.Hidden;
                            
                        }
                    }

                }

            }
            if (!selectedFromRoute.Equals("") && !selectedToRoute.Equals(""))
            {
                if (!noDataForRoute && !noTimetableData)
                {
                    if (selectedTimetable.Equals("") && !searchButton.Content.Equals("Pretraži"))
                    {
                        timtableError.Visibility = Visibility.Visible;
                        columnChart.Visibility = Visibility.Hidden;
                        label1.Visibility = Visibility.Hidden;
                        label.Visibility = Visibility.Hidden;
                        review.Visibility = Visibility.Hidden;
                    }
                    else if (!selectedTimetable.Equals("") && !searchButton.Content.Equals("Pretraži"))

                    {
                        int i = 0;
                        foreach (Timetable tt in timetables)
                        {
                            String fromT = tt.StartDateTime.Hour + ":" + tt.StartDateTime.Minute;
                            String toT = tt.EndDateTime.Hour + ":" + tt.EndDateTime.Minute;
                            String weekDay = "";
                            if (tt.weekDay == DayOfWeekTimetable.WORK)
                                weekDay = "Radni dan";
                            else if (tt.weekDay == DayOfWeekTimetable.SATURDAY)
                                weekDay = "Subota";
                            else weekDay = "Nedelja";
                            string ttName = tt.Id + " ( " + fromT + "-" + toT + " ) *" + weekDay;

                            if (ttName.Equals(selectedTimetable))
                                break;
                            i++;
                        }
                        timetableComboBox.SelectedIndex = i;
                        string[] tokens = selectedTimetable.Split('(');
                        int timetableId = Int32.Parse(tokens[0].Trim());
                        timtableError.Visibility = Visibility.Hidden;
                        columnChart.Visibility = Visibility.Visible;
                        label1.Visibility = Visibility.Visible;
                        label.Visibility = Visibility.Visible;
                        review.Visibility = Visibility.Visible;
                        loadLineChart(timetableId);
                        loadDataGrid(timetableId);
                    }
                    else
                    {
                        timtableError.Visibility = Visibility.Hidden;
                        columnChart.Visibility = Visibility.Hidden;
                        label1.Visibility = Visibility.Hidden;
                        label.Visibility = Visibility.Hidden;
                        review.Visibility = Visibility.Hidden;
                    }
                    searchButton.Content = "Generiši pregled";
                }
            }
        }

        private void loadLineChart(int timetableId)
        {
            columnChart.Series.Clear();
            columnChart.AxisX.Clear();
            columnChart.AxisY.Clear();

            List<double> revenue = CardService.RevenueForTimetable(timetableId);

            check = checkIfNoSoldCards(revenue);
            if (check)
            {
                label.Content = "Nema prodatih karti za izabrani red vožnje.";
                timtableError.Visibility = Visibility.Hidden;
                columnChart.Visibility = Visibility.Hidden;
                label1.Visibility = Visibility.Hidden;
                label.Visibility = Visibility.Visible;
                review.Visibility = Visibility.Hidden;
                searchedMessage.Visibility = Visibility.Hidden;
            }
            else
            {

                label1.Content = "Grafički prikaz broja prodatih karti za izabrani red vožnje";
                label1.HorizontalAlignment = HorizontalAlignment.Center;

                var chartValues = new ChartValues<double>();
                foreach (var monthly in revenue)
                {
                    chartValues.Add(monthly);
                }


                var seriesCollection = new SeriesCollection
            {
               new ColumnSeries
                {
                    Title = "Broj prodatih karti po mesecu",
                    Values = chartValues,
                    Configuration = new CartesianMapper<double>()
                    .Y(point => point)
                    .Stroke(point => (point == chartValues.Max()) ? (Brush)(new BrushConverter().ConvertFrom("#FFECA77D")) : (point == chartValues.Min()) ? (Brush)(new BrushConverter().ConvertFrom("#FFC34E43")) : (Brush)(new BrushConverter().ConvertFrom("#FF485B83")))
                    .Fill(point => (point == chartValues.Max()) ?  (Brush)(new BrushConverter().ConvertFrom("#FFECA77D")) : (point == chartValues.Min()) ? (Brush)(new BrushConverter().ConvertFrom("#FFC34E43")) : (Brush)(new BrushConverter().ConvertFrom("#FF485B83"))),
               }
            };


                seriesCollection.Add(new LineSeries()
                {
                    Title = "Broj prodatih karti po mesecu",
                    Values = chartValues,
                    DataLabels = false,
                    Fill = Brushes.Transparent,
                    PointGeometrySize = 0,
                    IsEnabled = false,
                    Configuration = new CartesianMapper<double>()
                       .Y(point => point)
                });
                List<int> yearsForTt = CardService.GetAvailableYearsWithSoldCards(timetableId);
                List<string> labelForChart = new List<string>();

                foreach (int year in yearsForTt)
                {
                    labelForChart.Add("Jan, " + year + ".");
                    labelForChart.Add("Feb, " + year + ".");
                    labelForChart.Add("Mart, " + year + ".");
                    labelForChart.Add("Apr, " + year + ".");
                    labelForChart.Add("Maj, " + year + ".");
                    labelForChart.Add("Jun, " + year + ".");
                    labelForChart.Add("Jul, " + year + ".");
                    labelForChart.Add("Avg, " + year + ".");
                    labelForChart.Add("Sept, " + year + ".");
                    labelForChart.Add("Okt, " + year + ".");
                    labelForChart.Add("Nov, " + year + ".");
                    labelForChart.Add("Dec, " + year + ".");
                }

                columnChart.AxisX.Add(new Axis
                {
                    Title = "Mesec",
                    Labels = labelForChart,
                    Foreground = (Brush)(new BrushConverter().ConvertFrom("#FF485B83")),
                    FontSize = 14

                });

                Func<double, string> labFormat = value => string.Format("{0:0}", value);

                columnChart.AxisY.Add(new Axis
                {
                    Title = "Broj prodatih karti",
                    LabelFormatter = labFormat,
                    Foreground = (Brush)new BrushConverter().ConvertFrom("#FF485B83"),
                    FontSize = 14

                });

                columnChart.Series = seriesCollection;
            }
        }

        private void loadDataGrid(int timetableId)
        {
            if (!check)
            {
                cards = CardService.GetSoldCardForTimetable(timetableId);
                this.review.ItemsSource = cards;
                this.review.Visibility = Visibility.Visible;
                label.Content = "Tabelarni prikaz broja prodatih karti za izabrani red vožnje";
                label.HorizontalAlignment = HorizontalAlignment.Center;
            }

        }

        private bool checkIfNoSoldCards(List<double> revenue)
        {
            foreach (double r in revenue)
            {
                if (r != 0) return false;
            }
            return true;
        }

    }
}
