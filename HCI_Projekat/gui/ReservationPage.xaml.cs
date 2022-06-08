using HCI_Projekat.database;
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
    /// Interaction logic for Reservation.xaml
    /// </summary>
    public partial class ReservationPage : Page
    {
        public List<DataGridTimetable> dataGridTimetables;
        public List<DataGridAlternativeRoute> dataGridAlternativeRoutes;
        public Timetable selectedTimetable;
        public DateTime selectedDate;
        public List<Timetable> currentTimetables;
        public List<string> curTakenSeats;
        public List<Tuple<Route, Route>> alternativeRoutes;
        public Tuple<Route, Route> selectedAlternativeRoutes;
        public Route currentRoute;
        public Path movingSeat;
        public double defaultHeight;
        public double defaultWidth;

        public ReservationPage()
        {
            InitializeComponent();
            addFromAndToRoutes();
            timetable.Visibility = Visibility.Hidden;
            timetablePanel.Visibility = Visibility.Hidden;
            formReservation.Visibility = Visibility.Hidden;
        }

        public Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(2),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5)); ; ;

            cfg.Dispatcher = Application.Current.Dispatcher;
        });

        private void addFromAndToRoutes()
        {
            List<ComboBoxItem> fromRouteitems = new List<ComboBoxItem>();
            List<ComboBoxItem> toRouteitems = new List<ComboBoxItem>();
            foreach (Route route in RouteService.GetAllRoutes())
            {
                if(fromRouteitems.Find(x=>x.Content.Equals(route.places[0].Name)) == null)
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

        private void Search(object sender, RoutedEventArgs e)
        {
            alternativeNextBtn.IsEnabled = false;
            reserveBtn.IsEnabled = false;
            formReservation.Visibility = Visibility.Hidden;
            timetablePanel.Visibility = Visibility.Hidden;
            alternativeRoutesPanel.Visibility = Visibility.Hidden;
            personNumTxt.Text = 1.ToString();
            WagonComboBox.Items.Clear();

            string selectedFromRoute = "";
            string selectedToRoute = "";
            var date = LocaleDatePickerRTL.SelectedDate.Value;
            selectedDate = date;

            if (fromRoutes.SelectedItem != null)
            {
                ComboBoxItem item = (ComboBoxItem)fromRoutes.SelectedItem;
                selectedFromRoute = item.Content.ToString();
            }
            else
                FromError.Visibility = Visibility;

            if (toRoutes.SelectedItem != null)
            {
                ComboBoxItem item = (ComboBoxItem)toRoutes.SelectedItem;
                selectedToRoute = item.Content.ToString();
            }
            else
                ToError.Visibility = Visibility;


            if (!selectedFromRoute.Equals(""))
            {
                FromError.Visibility = Visibility.Hidden;
                if (!selectedToRoute.Equals(""))
                {
                    ToError.Visibility = Visibility.Hidden;
                    titleMessage.Content = "Redovi vožnje od stanice: " + selectedFromRoute +
                        " do stanice: " + selectedToRoute;
                    currentRoute = RouteService.checkIfRouteExists(selectedFromRoute, selectedToRoute);
                    if (currentRoute == null)
                    {
                        AlterantiveRouteTitleMessage.Visibility = Visibility.Hidden;
                        alternativeRoutes = RouteService.checkIfAlternateRouteExists(selectedFromRoute, selectedToRoute);
                        if(alternativeRoutes == null || alternativeRoutes.Count == 0)
                        {
                            searchedMessage.Content = "Nema vozne linije za traženu relaciju.";
                            this.searchedMessage.Visibility = Visibility.Visible;
                            this.timetable.Visibility = Visibility.Hidden;
                            formReservation.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            AlterantiveRouteTitleMessage.Content = "Postoje samo linije sa presedanjem za traženu relaciju";
                            AlterantiveRouteTitleMessage.Visibility = Visibility.Visible;
                            alternativeRoutesPanel.Visibility = Visibility.Visible;
                            setDataGridAlternativeRoutes(alternativeRoutes);
                            alternativeRouteDataGrid.ItemsSource = dataGridAlternativeRoutes;
                        }
                    }
                    else
                    {
                        CreateTimetableDataGrid();
                    }
                }
            }
        }

        private void CreateTimetableDataGrid()
        {
            if(currentRoute != null)
                titleMessage.Content = "Redovi vožnje od stanice: " + currentRoute.places[0].Name +
                            " do stanice: " + currentRoute.places[currentRoute.places.Count - 1].Name;
            var date = LocaleDatePickerRTL.SelectedDate.Value;
            DayOfWeekTimetable selectedDay;
            if (date.DayOfWeek == DayOfWeek.Saturday)
                selectedDay = DayOfWeekTimetable.SATURDAY;
            else if (date.DayOfWeek == DayOfWeek.Sunday)
                selectedDay = DayOfWeekTimetable.SUNDAY;
            else
                selectedDay = DayOfWeekTimetable.WORK;

            currentTimetables = TimetableService.FindTimetablesByRoute(currentRoute.Name, selectedDay);
            if (currentTimetables.Count == 0)
            {
                searchedMessage.Content = "Nema redova vožnje za traženu relaciju i izabrani datum.";
                this.searchedMessage.Visibility = Visibility.Visible;
                this.timetable.Visibility = Visibility.Hidden;
                timetablePanel.Visibility = Visibility.Hidden;
            }
            else
            {
                dataGridTimetables = changeTimetableToDataGridTimetable(currentTimetables, date);
                this.timetable.ItemsSource = dataGridTimetables;
                this.timetable.Visibility = Visibility.Visible;
                timetablePanel.Visibility = Visibility.Visible;
                this.searchedMessage.Visibility = Visibility.Hidden;
            }
        }

        private void setDataGridAlternativeRoutes(List<Tuple<Route, Route>> alternativeRoutes)
        {
            dataGridAlternativeRoutes = new List<DataGridAlternativeRoute>();
            foreach (var route in alternativeRoutes)
            {
                var dfar = new DataGridAlternativeRoute();
                dfar.StartingPlace = route.Item1.places[0].Name;
                dfar.MiddlePlace = route.Item2.places[0].Name;
                dfar.DestinationPlace = route.Item2.places[route.Item2.places.Count - 1].Name;
                dataGridAlternativeRoutes.Add(dfar);
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

        private void addWagonChips(List<Wagon> wagons)
        {
            WagonComboBox.Items.Clear();
            WagonComboBox.SelectionChanged += WagonChanged;
            int br = 1;

            foreach (var wagon in wagons)
            {
                string cmboxItem = br++ + ": ";
                if (wagon.wagonType == model.Wagon.WagonType.SMALL)
                    cmboxItem += "Mali vagon";
                else if (wagon.wagonType == model.Wagon.WagonType.MEDIUM)
                    cmboxItem += "Srednji vagon";
                else
                    cmboxItem += "Veliki vagon";
                MaterialDesignThemes.Wpf.PackIcon icon = new MaterialDesignThemes.Wpf.PackIcon();
                icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.TrainCarPassenger;
                //chip.Icon = icon;
                //.IconBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF485B83");
                WagonComboBox.Items.Add(cmboxItem);
            }
            WagonComboBox.SelectedIndex = 0;
        }


        private void WagonChanged(object sender, RoutedEventArgs e)
        {
            if(WagonComboBox.Items.Count > 0 && WagonComboBox.SelectedItem != null)
                generateWagonSeats();
        }

        private void generateWagonSeats()
        {
            string item = WagonComboBox.SelectedItem.ToString();
            int wagonNum = WagonComboBox.SelectedIndex;
            string wagonName = item.Split(':')[1].Trim();
            myCanvas.Visibility = Visibility.Visible;

            List<TrainTicket> tickets = TicketRepository.GetTicketsForDateTimeAndWagon(currentRoute, selectedDate, wagonNum);

            double rows;
            if (wagonName.Equals("Mali vagon"))
                rows = (int)Wagon.WagonType.SMALL;
            else if (wagonName.Equals("Srednji vagon"))
                rows = (int)Wagon.WagonType.MEDIUM;
            else
                rows = (int)Wagon.WagonType.LARGE;

            WagonRect.Height = rows * 27;
            WagonRect.Width = 120;
            WagonLayout.Width = WagonRect.Width;
            WagonLayout.Height = WagonRect.Height;
            WagonLayout.Children.Clear();

            double left = 5;
            double top = 8;

            int personNum = int.Parse(personNumTxt.Text);
            curTakenSeats = new List<string>();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Path seat = new Path();
                    seat.Fill = Brushes.Gray;
                    seat.Data = Geometry.Parse("M5 9.15V7C5 5.9 5.9 5 7 5H17C18.1 5 19 5.9 19 7V9.16C17.84 9.57 17 10.67 17 11.97V14H7V11.96C7 10.67 6.16 9.56 5 9.15M20 10C18.9 10 18 10.9 18 12V15H6V12C6 10.9 5.11 10 4 10S2 10.9 2 12V17C2 18.1 2.9 19 4 19V21H6V19H18V21H20V19C21.1 19 22 18.1 22 17V12C22 10.9 21.1 10 20 10Z");
                    seat.AllowDrop = true;
                    seat.Drop += WagonLayout_Drop;
                    if (tickets.Find(x => x.seat == ((i + 1).ToString() + (j + 1).ToString())) != null)
                    {
                        seat.Fill = Brushes.Black;
                        seat.AllowDrop = false;
                    }
                    else if (personNum > 0)
                    {
                        Path pickSeat = new Path();
                        pickSeat.Data = seat.Data;
                        pickSeat.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFC34E43");
                        curTakenSeats.Add(i + "" + j);
                        pickSeat.Height = 20;
                        pickSeat.Width = 20;
                        pickSeat.Stretch = Stretch.Fill;
                        pickSeat.MouseMove += seatMouseMove;
                        pickSeat.Name = "path_"+i + "" + j;
                        Canvas.SetLeft(pickSeat, left);
                        Canvas.SetTop(pickSeat, top);
                        Canvas.SetZIndex(pickSeat, 100);
                        //seat.DragOver += WagonLayout_DragOver;
                        WagonLayout.Children.Add(pickSeat);
                        personNum--;
                    }
                    seat.Height = 20;
                    seat.Width = 20;
                    seat.Stretch = Stretch.Fill;
                    seat.Name = "path_" + i + "" + j;
                    WagonLayout.Children.Add(seat);
                    Canvas.SetLeft(seat, left);
                    Canvas.SetTop(seat, top);
                    if (j == 1)
                        left += seat.Width + 22;
                    else
                        left += seat.Width + 3;
                }
                top += 26;
                left = 5;
            }
        }

        private void seatMouseMove(object sender, MouseEventArgs e)
        {
            Path path = sender as Path;
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                movingSeat = path;
                DragDrop.DoDragDrop(path, path, DragDropEffects.Move);
            }
        }

        private void WagonLayout_Drop(object sender, DragEventArgs e)
        {
            Path path = sender as Path;

            Canvas.SetLeft(movingSeat, Canvas.GetLeft(path));
            Canvas.SetTop(movingSeat, Canvas.GetTop(path));

            int seatIdx = curTakenSeats.FindIndex(x => x == movingSeat.Name.Split("_")[1]);

            curTakenSeats[seatIdx] = path.Name.Split("_")[1];
            movingSeat.Name = path.Name;

        }

        private void WagonLayout_DragOver(object sender, DragEventArgs e)
        {
            //Point dropPosition = e.GetPosition(movingSeat);

            //Canvas.SetLeft(movingSeat, dropPosition.X);
            //Canvas.SetTop(movingSeat, dropPosition.Y);
        }

        private void Reserve(object sender, RoutedEventArgs e)
        {
            
            bool? Result = new CustomMessageBox("Da li ste sigurni da želite da rezervišete ?", MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();
            if (Result.Value)
            {
                foreach (var takenSeat in curTakenSeats)
                {
                    TicketRepository.AddNewTrainTicket(currentRoute, selectedDate, WagonComboBox.SelectedIndex, takenSeat, MainWindow.LoggedUser.Username);
                }
                Registration.notifier.ShowSuccess("Uspešno obavljena rezervacija.");


                formReservation.Visibility = Visibility.Hidden;
                fromRoutes.SelectedIndex = -1;
                toRoutes.SelectedIndex = -1;
                WagonComboBox.Items.Clear();

                if (selectedAlternativeRoutes != null && selectedAlternativeRoutes.Item2.Name != currentRoute.Name)
                {
                    currentRoute = selectedAlternativeRoutes.Item2;
                    timetablePanel.Visibility = Visibility.Visible;
                    reserveBtn.IsEnabled = false;
                    CreateTimetableDataGrid();
                }

            }


        }

        private void timetable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(timetable.SelectedIndex >= 0)
            {
                selectedTimetable = currentTimetables.Find(x => {
                    var selected = ((DataGridTimetable)timetable.SelectedItem).StartDateTime;
                    if (x.StartDateTime.Hour == selected.Hour && x.StartDateTime.Minute == selected.Minute)
                        return true;
                    return false;
                });
                selectedDate = selectedDate.AddHours(selectedTimetable.StartDateTime.Hour);
                selectedDate = selectedDate.AddMinutes(selectedTimetable.StartDateTime.Minute);
                reserveBtn.IsEnabled = true;

            }
        }

        private void GoToReservationForm(object sender, RoutedEventArgs e)
        {
            if(selectedTimetable != null)
            {
                timetablePanel.Visibility = Visibility.Hidden;
                formReservation.Visibility = Visibility.Visible;
                addWagonChips(currentRoute.Train.Wagons);
            }
        }


        private void personNumTxt_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (int.Parse(personNumTxt.Text) < 1)
                personNumTxt.Text = 1.ToString();
            else if(int.Parse(personNumTxt.Text) > 10)
                personNumTxt.Text = 10.ToString();

            if(WagonComboBox != null && WagonComboBox.SelectedItem != null)
                generateWagonSeats();
        }

        private void CancelReservationClick(object sender, RoutedEventArgs e)
        {
            WagonComboBox.Items.Clear();
            formReservation.Visibility = Visibility.Hidden;
            timetablePanel.Visibility = Visibility.Visible;
        }

        private void myCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var st = new ScaleTransform();
            double newHeight = e.NewSize.Height;
            double newWidth = e.NewSize.Width;
            if (defaultHeight == 0)
            {
                defaultHeight = newHeight;
                defaultWidth = newWidth;
            }
            double scaleY = newHeight / defaultHeight;
            double scaleX = scaleY;
            if (scaleY < 1)
                scaleY = 1;
            if (scaleX < 1)
                scaleX = 1;
            st.ScaleY = scaleY;
            st.ScaleX = scaleX;
            WagonLayout.LayoutTransform = st;
            WagonRect.LayoutTransform = st;
        }

        private void alternativeRouteView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(alternativeRouteDataGrid.SelectedItem != null)
            {
                selectedAlternativeRoutes = alternativeRoutes.Find(x=> {
                    if (x.Item1.places[0].Name == ((DataGridAlternativeRoute)alternativeRouteDataGrid.SelectedItem).StartingPlace &&
                        x.Item1.places[x.Item1.places.Count - 1].Name == ((DataGridAlternativeRoute)alternativeRouteDataGrid.SelectedItem).MiddlePlace)
                        return true;
                    return false;
                });
                alternativeNextBtn.IsEnabled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAlternativeRoutes == null)
                return;

            alternativeRoutesPanel.Visibility = Visibility.Hidden;
            timetablePanel.Visibility = Visibility.Visible;
            currentRoute = selectedAlternativeRoutes.Item1;
            CreateTimetableDataGrid();
            
        }
    }
}
