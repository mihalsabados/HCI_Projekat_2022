using HCI_Projekat.database;
using HCI_Projekat.model;
using HCI_Projekat.services;
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
    /// Interaction logic for MyReservedTicketsPage.xaml
    /// </summary>
    public partial class MyReservedTicketsPage : Page
    {
        public List<DataGridReservedTickets> reservedTicketsDataGrid;

        public MyReservedTicketsPage()
        {
            InitializeComponent();
        }

        private void fillDataGridWithData(List<TrainTicket> tickets)
        {
            reservedTicketsDataGrid = new List<DataGridReservedTickets>();
            foreach (var ticket in tickets)
            {
                DataGridReservedTickets resTick = new DataGridReservedTickets();
                resTick.FromToRoute = ticket.route.FromTo;
                resTick.StartTime = ticket.dateTime;
                resTick.Seat = ticket.seat;
                resTick.WagonNum = ticket.wagonNum;
                reservedTicketsDataGrid.Add(resTick);
            }
            ticketsDataGrid.ItemsSource = reservedTicketsDataGrid;
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            var selectedFromItem = fromRoutes.SelectedItem;
            var selectedToItem = toRoutes.SelectedItem;
            string from = "";
            string to = "";
            if (selectedFromItem != null)
                from = ((ComboBoxItem)selectedFromItem).Content.ToString();
            if (selectedToItem != null)
                to = ((ComboBoxItem)selectedToItem).Content.ToString();
            List <TrainTicket> tickets = TicketRepository.searchForTickets(from, to, LocaleDatePickerRTL.SelectedDate, MainWindow.LoggedUser.Username);
            
            
            if (tickets.Count != 0)
            {
                SearchPanel.Visibility = Visibility.Visible;
                ticketsDataGrid.Visibility = Visibility.Visible;
                fillDataGridWithData(tickets);
            }
            else
            {
                titleMessage.Visibility = Visibility.Visible;
                ticketsDataGrid.Visibility = Visibility.Hidden;
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            titleMessage.Visibility = Visibility.Hidden;
            if (Visibility == Visibility.Visible && MainWindow.LoggedUser != null)
            {
                List<TrainTicket> tickets = TicketRepository.getTicketsForUser(MainWindow.LoggedUser.Username);
                if (tickets.Count != 0)
                {
                    SearchPanel.Visibility = Visibility.Visible;
                    ticketsDataGrid.Visibility = Visibility.Visible;
                    fillDataGridWithData(tickets);
                    addFromAndToRoutes(tickets);
                }
                else
                {
                    titleMessage.Visibility = Visibility.Visible;
                    ticketsDataGrid.Visibility = Visibility.Hidden;
                    SearchPanel.Visibility = Visibility.Hidden;
                }

                this.Focusable = true;
                this.Focus();
                RoutedCommand newCmdFilter = new RoutedCommand();
                newCmdFilter.InputGestures.Add(new KeyGesture(Key.F1));
                this.CommandBindings.Add(new CommandBinding(newCmdFilter, CommandBinding_Executed));
            }
        }

        private void addFromAndToRoutes(List<TrainTicket> tickets)
        {
            fromRoutes.Items.Clear();
            toRoutes.Items.Clear();
            List<ComboBoxItem> fromRouteitems = new List<ComboBoxItem>();
            List<ComboBoxItem> toRouteitems = new List<ComboBoxItem>();
            var routeForClient = from ticket in tickets
                                 select ticket.route;

            foreach (Route route in routeForClient.ToList())
            {
                if (fromRouteitems.Find(x => x.Content.Equals(route.places[0].Name)) == null)
                {
                    fromRouteitems.Add(createComboBoxItem(route.places[0].Name));
                    fromRoutes.Items.Add(createComboBoxItem(route.places[0].Name));
                }
                if (toRouteitems.Find(x => x.Content.Equals(route.places[route.places.Count - 1].Name)) == null)
                {
                    toRouteitems.Add(createComboBoxItem(route.places[route.places.Count - 1].Name));
                    toRoutes.Items.Add(createComboBoxItem(route.places[route.places.Count - 1].Name));
                }
            }
        }

        private ComboBoxItem createComboBoxItem(string content)
        {
            ComboBoxItem c = new ComboBoxItem();
            c.Content = content;
            c.Visibility = Visibility.Visible;
            c.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF485B83");

            return c;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string str = "myReservations";
            HelpProvider.ShowHelp(str, this);
        }

    }
}
