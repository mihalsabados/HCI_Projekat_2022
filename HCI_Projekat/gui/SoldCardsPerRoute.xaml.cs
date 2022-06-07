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
        public SoldCardsPerRoute()
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
            String selectedTimetable = "";

            timtableError.Visibility = Visibility.Hidden;

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
                if (!selectedToRoute.Equals(""))
                {
                    ToError.Visibility = Visibility.Hidden;
                    
                    Route route = checkIfRouteExists(selectedFromRoute, selectedToRoute);
                    if (route == null)
                    {
                        searchedMessage.Content = "Nema vozne linije za traženu relaciju.";
                        this.searchedMessage.Visibility = Visibility.Visible;
                        this.timetable.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        
                        List<Timetable> timetables = TimetableService.FindTimetablesByIdRouteName(route.Name);
                        if (timetables.Count == 0)
                        {
                            searchedMessage.Content = "Nema redova vožnje za traženu relaciju.";
                            this.searchedMessage.Visibility = Visibility.Visible;
                            this.timetable.Visibility = Visibility.Hidden;

                        }
                        else
                        {
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
                                timetable.Items.Add(createComboBoxItem(tt.Id + " ( " + fromT + "-" + toT + " ) *" + weekDay));
                            }
                            this.timetable.Visibility = Visibility.Visible;
                            this.searchedMessage.Visibility = Visibility.Hidden;
                            
                        }
                    }

                }

            }
            if (!selectedFromRoute.Equals("") && !selectedToRoute.Equals(""))
            {
                if (timetable.SelectedItem == null)
                {
                    timtableError.Visibility = Visibility.Visible;
                }
                else
                {
                    ComboBoxItem item = (ComboBoxItem)timetable.SelectedItem;
                    selectedTimetable = item.Content.ToString();
                    string[] tokens = selectedTimetable.Split('(');
                    int timetableId = Int32.Parse(tokens[0].Trim());
                    loadLineChart(timetableId);
                }
            }
        }

        private void loadLineChart(int timetableId)
        {
            columnChart.Series.Clear();
            columnChart.AxisX.Clear();
            columnChart.AxisY.Clear();

            List<double> revenue = CardService.RevenueForTimetable(timetableId);

            label1.Content = "Mesečni prihodi od prodatih karata za izabrani red vožnje.";
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
                    Title = "Prihodi po mesecu",
                    Values = chartValues,
                    Configuration = new CartesianMapper<double>()
                    .Y(point => point)
                    .Stroke(point => (point == chartValues.Max()) ? (Brush)(new BrushConverter().ConvertFrom("#FFECA77D")) : (point == chartValues.Min()) ? (Brush)(new BrushConverter().ConvertFrom("#FFC34E43")) : (Brush)(new BrushConverter().ConvertFrom("#FF485B83")))
                    .Fill(point => (point == chartValues.Max()) ?  (Brush)(new BrushConverter().ConvertFrom("#FFECA77D")) : (point == chartValues.Min()) ? (Brush)(new BrushConverter().ConvertFrom("#FFC34E43")) : (Brush)(new BrushConverter().ConvertFrom("#FF485B83"))),
               }
            };


            seriesCollection.Add(new LineSeries()
            {
                Title = "Prihodi po mesecu",
                Values = chartValues,
                DataLabels = false,
                Fill = Brushes.Transparent,
                PointGeometrySize = 0,
                IsEnabled = false,
                Configuration = new CartesianMapper<double>()
                   .Y(point => point)
            });


            columnChart.AxisX.Add(new Axis
            {
                Title = "Mesec",
                Labels = new[] { "Jan", "Feb", "Mart", "April", "Maj", "Jun", "Jul", "Avg", "Sept", "Okt", "Nov", "Dec" },
                Foreground = (Brush)(new BrushConverter().ConvertFrom("#FF485B83")),
                FontSize = 14,
                Separator = new LiveCharts.Wpf.Separator { Step = 1 }

            });

            Func<double, string> labFormat = value => value.ToString("C");

            columnChart.AxisY.Add(new Axis
            {
                Title = "Prihodi (RSD)",
                LabelFormatter = labFormat,
                Foreground = (Brush)new BrushConverter().ConvertFrom("#FF485B83"),
                FontSize = 14

            });

            columnChart.Series = seriesCollection;

        }
    }
}
