using HCI_Projekat.model;
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
        public List<CardDataGrid> cards;
        public bool check;
        public SoldCardsPerMonth()
        {
            InitializeComponent();
            List<int> years = AddYears();
            if (years.Count != 0)
                AddMonths();
            loadBarChart();
        }

        private List<int> AddYears()
        {
            List<int> years = CardService.GetAvailableYearsWithSoldCards();
            if (years.Count == 0)
            {
                searchedYears.Visibility = Visibility.Visible;
                yearsComboBox.Visibility = Visibility.Hidden;
                monthComboBox.Visibility = Visibility.Hidden;
                label1.Visibility = Visibility.Hidden;
                label.Visibility = Visibility.Hidden;
                columnChart.Visibility = Visibility.Hidden;
                review.Visibility = Visibility.Hidden;
            }
            else
            {
                
                searchedYears.Visibility = Visibility.Hidden;
                yearsComboBox.Visibility = Visibility.Visible;
                monthComboBox.Visibility = Visibility.Visible;
                label1.Visibility = Visibility.Visible;
                label.Visibility = Visibility.Visible;
                columnChart.Visibility = Visibility.Visible;
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
            return years;
        }
        private void AddMonths()
        {
            List<string> months = new List<string> { "Jan", "Feb", "Mart", "Apr", "Maj", "Jun", "Jul", "Avg", "Sept", "Okt", "Nov", "Dec" };
            
                foreach (string month in months)
                    monthComboBox.Items.Add(createComboBoxItem(month));
                DateTime dateTime = DateTime.Now;
                int i = 1;
                for (i = 1; i <= 12; i++)
                {
                    if (dateTime.Month == i) 
                        break;
                }
                monthComboBox.SelectedIndex = i-1;
          
        }

        private ComboBoxItem createComboBoxItem(int content)
        {
            ComboBoxItem c = new ComboBoxItem();
            c.Content = content + "";
            c.Visibility = Visibility.Visible;
            c.Foreground = (Brush)(new BrushConverter().ConvertFrom("#FF485B83"));
           
            return c;
        }
        private ComboBoxItem createComboBoxItem(string content)
        {
            ComboBoxItem c = new ComboBoxItem();
            c.Content = content;
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
            int month = monthComboBox.SelectedIndex + 1;
            ComboBoxItem item2 = (ComboBoxItem)monthComboBox.SelectedItem;
            string monthName = item2.Content.ToString();

            List<double> revenue = CardService.RevenueForMonths(yearRevenue, month);
            check = checkIfNoSoldCards(revenue);
            if (check)
            {
                label.Content = "Nema prodatih karti za " + monthName + ", " + yearRevenue + ".";
                
                review.Visibility = Visibility.Hidden;
                columnChart.Visibility = Visibility.Hidden;
                label1.Visibility = Visibility.Hidden;
            }
            else
            {
                label1.Content = "Grafički prikaz broja prodatih karti za " + monthName + ", " + yearRevenue + ".";
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
                    Title = "Broj prodatih karti po danu",
                    Values = chartValues,
                    Configuration = new CartesianMapper<double>()
                    .Y(point => point)
                    .Stroke(point => (point == chartValues.Max()) ? (Brush)(new BrushConverter().ConvertFrom("#FFECA77D")) : (point == chartValues.Min()) ? (Brush)(new BrushConverter().ConvertFrom("#FFC34E43")) : (Brush)(new BrushConverter().ConvertFrom("#FF485B83")))
                    .Fill(point => (point == chartValues.Max()) ?  (Brush)(new BrushConverter().ConvertFrom("#FFECA77D")) : (point == chartValues.Min()) ? (Brush)(new BrushConverter().ConvertFrom("#FFC34E43")) : (Brush)(new BrushConverter().ConvertFrom("#FF485B83"))),
               }
            };


                seriesCollection.Add(new LineSeries()
                {
                    Title = "Broj prodatih karti po danu",
                    Values = chartValues,
                    DataLabels = false,
                    Fill = Brushes.Transparent,
                    PointGeometrySize = 0,
                    IsEnabled = false,
                    Configuration = new CartesianMapper<double>()
                       .Y(point => point)
                });

                List<string> labelForChart = new List<string>();
                int days = DateTime.DaysInMonth(yearRevenue, month);
                for (int i = 1; i <= days; i++)
                {
                    labelForChart.Add(i + "." + month + "." + yearRevenue);
                }
                columnChart.AxisX.Add(new Axis
                {
                    Title = "Dan",
                    Labels = labelForChart,
                    Foreground = (Brush)(new BrushConverter().ConvertFrom("#FF485B83")),
                    FontSize = 14,
                    Separator = new LiveCharts.Wpf.Separator { Step = 10 }

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
                monthComboBox.Visibility = Visibility.Visible;
                columnChart.Visibility = Visibility.Visible;
                label1.Visibility = Visibility.Visible;
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

        private void yearsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (monthComboBox.SelectedIndex != -1)
            {
                loadBarChart();
                loadDataGrid();
            }
        }
        private void monthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadBarChart();
            loadDataGrid();
        }

        private void loadDataGrid()
        {
            if (!check)
            {
                ComboBoxItem item = (ComboBoxItem)yearsComboBox.SelectedItem;
                int year = Int32.Parse(item.Content.ToString());
                int month = monthComboBox.SelectedIndex + 1;
                cards = CardService.GetCardForYearAndMonth(year, month);
                this.review.ItemsSource = cards;
                this.review.Visibility = Visibility.Visible;
                ComboBoxItem item2 = (ComboBoxItem)monthComboBox.SelectedItem;
                string monthName = item2.Content.ToString();
                label.Content = "Tabelarni prikaz broja prodatih karti za " + monthName + ", " + year + ".";
                label.HorizontalAlignment = HorizontalAlignment.Center;
            }

        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string str = "soldTicketsMonth";
            HelpProvider.ShowHelp(str, this);
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                this.Focusable = true;
                this.Focus();
                RoutedCommand newCmdFilter = new RoutedCommand();
                newCmdFilter.InputGestures.Add(new KeyGesture(Key.F1));
                this.CommandBindings.Add(new CommandBinding(newCmdFilter, CommandBinding_Executed));
            }
        }
    }
}
