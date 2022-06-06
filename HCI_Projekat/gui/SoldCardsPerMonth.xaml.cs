using HCI_Projekat.services;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
    /// Interaction logic for SoldCardsPerMonth.xaml
    /// </summary>
    public partial class SoldCardsPerMonth : Page
    {
        public SoldCardsPerMonth()
        {
            InitializeComponent();
            AddYears();
            loadBarChart();
        }

        private void AddYears()
        {
            List<int> years = CardService.GetAvailableYearsWithSoldCards();
            if (years.Count == 0)
                searchedYears.Visibility = Visibility.Visible;
            else
            {
                searchedYears.Visibility = Visibility.Hidden;
                foreach (int year in years)
                    yearsComboBox.Items.Add(createComboBoxItem(year));
                int maxYear = years.Max();
                int i = 0;
                foreach (int a in years)
                {
                    if (maxYear == a)
                        break;
                    i++;
                }
                yearsComboBox.SelectedIndex = i;
            }
             
        }

        private ComboBoxItem createComboBoxItem(int content)
        {
            ComboBoxItem c = new ComboBoxItem();
            c.Content = content + "";
            c.Visibility = Visibility.Visible;
            c.Foreground = (Brush)(new BrushConverter().ConvertFrom("#FF485B83"));
           
            return c;
        }

        private void loadBarChart()
        {
            columnChart.Series.Clear();
            columnChart.AxisX.Clear();
            columnChart.AxisY.Clear();

            ComboBoxItem item = (ComboBoxItem)yearsComboBox.SelectedItem;
            int yearRevenue = Int32.Parse(item.Content.ToString());
            List<double> revenue = CardService.RevenueForMonths(yearRevenue);

            label1.Content = "Mesečni prihodi od prodatih karata u " + yearRevenue + ".";
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

            }) ;

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

        private void yearsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadBarChart();
        }
    }
}
