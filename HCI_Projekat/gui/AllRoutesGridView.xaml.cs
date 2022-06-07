using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using HCI_Projekat.model;
using HCI_Projekat.services;
using HCI_Projekat.database;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;


namespace HCI_Projekat.gui
{
    /// <summary>
    /// Interaction logic for AllRoutesGridView.xaml
    /// </summary>
    public partial class AllRoutesGridView : Page, INotifyPropertyChanged
    {

        public static DataGridRoute SelectedRoute { get; set; }
        private List<Route> filteredRoutes = new List<Route>();


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        private ICollectionView _View;
        public ICollectionView View
        {
            get
            {
                return _View;
            }
            set
            {
                _View = value;
                OnPropertyChanged("View");
            }
        }

        public ObservableCollection<DataGridRoute> Routes
        {
            get;
            set;
        }


        public AllRoutesGridView()
        {
            InitializeComponent();
            this.DataContext = this;
            initTableData();
            initComboboxes();
        }

        private void initComboboxes()
        {
            foreach (Place place in PlaceService.GetAllPlaces())
            {
                ComboBoxItem itemStart = createComboBoxItem(place.Name);
                ComboBoxItem itemEnd = createComboBoxItem(place.Name);
                ComboBoxItem itemInner = createComboBoxItem(place.Name);

                startStationCb.Items.Add(itemStart);
                endStationCb.Items.Add(itemEnd);
                innerStationCb.Items.Add(itemInner);
            }

            foreach (Train train in TrainRepository.getAllTrains())
            {
                ComboBoxItem item = createComboBoxItem(train.Name);
                trainStationCb.Items.Add(item);
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


        private void initTableData()
        {
            List<Route> routes = RouteService.GetAllRoutes();
            filteredRoutes = routes;
            List<DataGridRoute> drList = new List<DataGridRoute>();
            foreach (Route route in routes)
            {
                drList.Add(new DataGridRoute(route));
            }
            Routes = new ObservableCollection<DataGridRoute>(drList);
            View = CollectionViewSource.GetDefaultView(Routes);
        }

        public void refreshData()
        {
            List<Route> routes = RouteService.GetAllRoutes();
            filteredRoutes = routes;
            List<DataGridRoute> drList = new List<DataGridRoute>();
            foreach (Route route in routes)
            {
                drList.Add(new DataGridRoute(route));
            }
            Routes = new ObservableCollection<DataGridRoute>(drList);
            this.OnPropertyChanged("Routes");
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


        private void AddRoute_Click(object sender, RoutedEventArgs e)
        {
            SelectedRoute = routesTable.SelectedItem as DataGridRoute;
            AddRoute addRoute = new AddRoute(this);
            addRoute.ShowDialog();
        }

        private void EditRoute_Click(object sender, RoutedEventArgs e)
        {
            SelectedRoute = routesTable.SelectedItem as DataGridRoute;
            EditRoute editRoute = new EditRoute(this);
            editRoute.ShowDialog();
        }

        private void DeleteRoute_Click(object sender, RoutedEventArgs e)
        {
            DataGridRoute item = routesTable.SelectedItem as DataGridRoute;
            bool cantDelete = false; // ovde dodati posle ako ima rezervacija da ne moze da brise
            if (cantDelete)
            {
                notifier.ShowError("Nije moguće obrisati voznu liniju jer trenutno postoje rezervisane karte za nju.");
            }
            else
            {
                bool deleted = RouteService.DeleteRouteByName(item.Route.Name);
                if (deleted)
                {
                    notifier.ShowSuccess("Uspešno ste obrisali voznu liniju.");
                    refreshData();
                }
                else
                {
                    notifier.ShowError("Došlo je do greške prilikom brisanja vozne linije. Probajte ponovo");
                }
            }
        }

        private void resetFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            startStationCb.SelectedItem = null;
            endStationCb.SelectedItem = null;
            trainStationCb.SelectedItem = null;
            innerStationCb.SelectedItem = null;
            refreshData();
        }

        private void filterBtn_Click(object sender, RoutedEventArgs e)
        {
            filteredRoutes = RouteService.GetAllRoutes();
            filterByStart();
            filterByEnd();
            filterByInner();
            filterByTrain();

            List<DataGridRoute> drList = new List<DataGridRoute>();
            foreach (Route route in filteredRoutes)
            {
                drList.Add(new DataGridRoute(route));
            }
            Routes = new ObservableCollection<DataGridRoute>(drList);
            this.OnPropertyChanged("Routes");
        }

        private void filterByStart()
        {
            if (startStationCb.SelectedItem != null)
            {
                ComboBoxItem startItem = (ComboBoxItem)startStationCb.SelectedItem;
                string startPlace = startItem.Content.ToString();
                filteredRoutes = filteredRoutes.FindAll(f => f.places[0].Name.Equals(startPlace));
            }
        }
        private void filterByEnd()
        {
            if (endStationCb.SelectedItem != null)
            {
                ComboBoxItem endItem = (ComboBoxItem)endStationCb.SelectedItem;
                string endPlace = endItem.Content.ToString();
                filteredRoutes = filteredRoutes.FindAll(f => f.places[^1].Name.Equals(endPlace));
            }
        }
        private void filterByTrain()
        {
            if (trainStationCb.SelectedItem != null)
            {
                ComboBoxItem trainItem = (ComboBoxItem)trainStationCb.SelectedItem;
                string train = trainItem.Content.ToString();
                filteredRoutes = filteredRoutes.FindAll(f => f.RouteTrain.Name.Equals(train));
            }
        }

        private void filterByInner()
        {
            if (innerStationCb.SelectedItem != null)
            {
                ComboBoxItem innerItem = (ComboBoxItem)innerStationCb.SelectedItem;
                string innerPlace = innerItem.Content.ToString();
                List<Route> newRoutes = new List<Route>();
                foreach (Route r in filteredRoutes)
                {
                    for (int i = 1; i < r.places.Count - 1; ++i)
                    {
                        if (r.places[i].Name.Equals(innerPlace))
                        {
                            newRoutes.Add(r);
                            break;
                        }
                    }
                }
                filteredRoutes = newRoutes;
            }
        }

    }
}
