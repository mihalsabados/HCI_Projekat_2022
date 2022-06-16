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
using System.Windows.Shapes;
using BingMapsRESTToolkit;
using BingMapsRESTToolkit.Extensions;
using HCI_Projekat.model;
using HCI_Projekat.services;
using HCI_Projekat.database;
using Microsoft.Maps.MapControl.WPF;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace HCI_Projekat.gui
{
    /// <summary>
    /// Interaction logic for EditRoute.xaml
    /// </summary>
    public partial class EditRoute : Window, INotifyPropertyChanged
    {

        private string BingMapsKey = "oU6C1d9L3SpiIjWjcMtX~NlvJ4abp72u-diLrup_xdw~AtDP3HyzbLn6vnOjapJ7nLaqM4g-sR1TpNVBbl6c53FGjGJEVOp5jRDt96RJyCmC";
        private string SessionKey;
        private DataGridRoute selectedRoute;
        private List<Place> newInnerStations = new List<Place>();
        private model.Route currentRoute = null;
        private ComboBoxItem startPlaceItem;
        private ComboBoxItem endPlaceItem;
        private ComboBoxItem trainItem;
        private bool loadingFinished = false;


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



        private void showMapRoute()
        {
            if (checkValidationStations())
            {
                List<Place> places = new List<Place>();
                ComboBoxItem startItem = (ComboBoxItem)startStationCb.SelectedItem;
                string startPlace = startItem.Content.ToString();

                ComboBoxItem endItem = (ComboBoxItem)endStationCb.SelectedItem;
                string endPlace = endItem.Content.ToString();

                places.Add(PlaceService.FindPlaceByName(startPlace));
                foreach (Place p in newInnerStations)
                {
                    places.Add(p);
                }
                places.Add(PlaceService.FindPlaceByName(endPlace));

                model.Route newRoute = new model.Route("name", selectedRoute.Route.RouteTrain, places);
                currentRoute = newRoute;
                CalculateRouteFromPlaces(newRoute);
            }
        }


        private async void CalculateRouteFromPlaces(model.Route foundRoute)
        {
            Mapa.Children.Clear();
            LoadingBar.Visibility = Visibility.Visible;
            List<SimpleWaypoint> waypoints = new List<SimpleWaypoint>();

            foreach (Place p in foundRoute.places)
            {
                waypoints.Add(new SimpleWaypoint(p.Latitude, p.Longitude));
            }

            var travelMode = (TravelModeType)Enum.Parse(typeof(TravelModeType), (string)("Walking"));
            
            var tspOptimization = (TspOptimizationType)Enum.Parse(typeof(TspOptimizationType), (string)("StraightLineDistance"));

            try
            {
                //Calculate a route between the waypoints so we can draw the path on the map. 
                var routeRequest = new RouteRequest()
                {
                    Waypoints = waypoints,

                    //Specify that we want the route to be optimized.
                    WaypointOptimization = tspOptimization,

                    RouteOptions = new RouteOptions()
                    {
                        TravelMode = travelMode,
                        RouteAttributes = new List<RouteAttributeType>()
                         {
                             RouteAttributeType.RoutePath,
                             RouteAttributeType.ExcludeItinerary
                         }
                    },

                    //When straight line distances are used, the distance matrix API is not used, so a session key can be used.
                    BingMapsKey = (tspOptimization == TspOptimizationType.StraightLineDistance) ? SessionKey : BingMapsKey
                };

                //Only use traffic based routing when travel mode is driving.
                if (routeRequest.RouteOptions.TravelMode != TravelModeType.Driving)
                {
                    routeRequest.RouteOptions.Optimize = RouteOptimizationType.Time;
                }
                else
                {
                    routeRequest.RouteOptions.Optimize = RouteOptimizationType.TimeWithTraffic;
                }

                var r = await routeRequest.Execute();

                RenderRouteResponse(routeRequest, r);
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Access was"))
                {
                    MessageBox.Show("Došlo je do greške prilikom učitavanja rute. Probajte da otvorite prikaz ponovo,\n ili kliknite na dugme 'Odustani' kako biste osvežili prikaz");
                }
                else if (ex.Message.StartsWith("The route"))
                {
                    MessageBox.Show("Ruta je predugačka da bi se prikazala na mapi");
                }

            }

            LoadingBar.Visibility = Visibility.Collapsed;
        }

        private void RenderRouteResponse(RouteRequest routeRequest, Response response)
        {
            //Render the route on the map.
            if (response != null && response.ResourceSets != null && response.ResourceSets.Length > 0 &&
               response.ResourceSets[0].Resources != null && response.ResourceSets[0].Resources.Length > 0
               && response.ResourceSets[0].Resources[0] is BingMapsRESTToolkit.Route)
            {
                var route = response.ResourceSets[0].Resources[0] as BingMapsRESTToolkit.Route;

                var timeSpan = new TimeSpan(0, 0, (int)Math.Round(route.TravelDurationTraffic));

                var routeLine = route.RoutePath.Line.Coordinates;
                var routePath = new LocationCollection();

                for (int i = 0; i < routeLine.Length; i++)
                {
                    routePath.Add(new Microsoft.Maps.MapControl.WPF.Location(routeLine[i][0], routeLine[i][1]));
                }

                var routePolyline = new MapPolyline()
                {
                    Locations = routePath,
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 3
                };

                Mapa.Children.Add(routePolyline);

                var locs = new List<Microsoft.Maps.MapControl.WPF.Location>();

                for (var i = 0; i < currentRoute.places.Count; ++i)
                {
                    var loc = new Microsoft.Maps.MapControl.WPF.Location(currentRoute.places[i].Latitude, currentRoute.places[i].Longitude);

                    Mapa.Children.Add(new Pushpin()
                    {
                        Location = loc,
                        Content = i
                    });
                    locs.Add(loc);
                }

                Mapa.SetView(locs, new Thickness(50), 0);
            }
            else if (response != null && response.ErrorDetails != null && response.ErrorDetails.Length > 0)
            {
                throw new Exception(String.Join("", response.ErrorDetails));
            }
        }



        public ObservableCollection<Place> InnerStations
        {
            get;
            set;
        }


        private AllRoutesGridView parent;

        public EditRoute(AllRoutesGridView allRoutesGridView)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Mapa.Focus();
            this.parent = allRoutesGridView;
            Mapa.CredentialsProvider = new ApplicationIdCredentialsProvider(BingMapsKey);
            Mapa.CredentialsProvider.GetCredentials((c) =>
            {
                SessionKey = c.ApplicationId;
            });

            selectedRoute = AllRoutesGridView.SelectedRoute;
            initInnerStations();
            fillComboBoxes();
            setInitialiValuesForComboBoxes();
            loadingFinished = true;
            initCommands();
        }

        private void initCommands()
        {
            RoutedCommand newCmdAddRoute = new RoutedCommand();
            newCmdAddRoute.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Control));
            this.CommandBindings.Add(new CommandBinding(newCmdAddRoute, addRoute_Click));

            RoutedCommand newCmdSaveRoute = new RoutedCommand();
            newCmdSaveRoute.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            this.CommandBindings.Add(new CommandBinding(newCmdSaveRoute, Save_Click));

            RoutedCommand newCmdResetRoute = new RoutedCommand();
            newCmdResetRoute.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control));
            this.CommandBindings.Add(new CommandBinding(newCmdResetRoute, Reset_Click));
        }


        private void setInitialiValuesForComboBoxes()
        {
            startStationCb.SelectedItem = startPlaceItem;
            endStationCb.SelectedItem = endPlaceItem;
            trainCb.SelectedItem = trainItem;
        }


        private void fillComboBoxes()
        {
            foreach (Place place in PlaceService.GetAllPlaces())
            {
                ComboBoxItem itemStart = createComboBoxItem(place.Name);
                ComboBoxItem itemEnd = createComboBoxItem(place.Name);
                ComboBoxItem itemInner = createComboBoxItem(place.Name);

                startStationCb.Items.Add(itemStart);
                endStationCb.Items.Add(itemEnd);
                newInnerStationCb.Items.Add(itemInner);

                if (selectedRoute.Route.places[0].Name.Equals(place.Name))
                {
                    startPlaceItem = itemStart;
                }
                if (selectedRoute.Route.places[^1].Name.Equals(place.Name))
                {
                    endPlaceItem = itemEnd;
                }
            }

            foreach (Train train in TrainRepository.getAllTrains())
            {
                ComboBoxItem item = createComboBoxItem(train.Name);
                trainCb.Items.Add(item);
                if (selectedRoute.TrainName.Equals(train.Name))
                {
                    trainItem = item;
                }
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

        private void initInnerStations()
        {
            newInnerStations = new List<Place>();
            for (int i = 1; i < selectedRoute.Route.places.Count - 1; ++i)
            {
                Place curr = selectedRoute.Route.places[i];
                Place newPlace = new Place(curr.Name, curr.Latitude, curr.Longitude);
                newInnerStations.Add(newPlace);
            }
            InnerStations = new ObservableCollection<Place>(newInnerStations);
            View = CollectionViewSource.GetDefaultView(InnerStations);
        }

        private void refreshTable()
        {
            List<Place> placesData = new List<Place>();
            foreach (Place p in newInnerStations)
            {
                placesData.Add(p);
            }
            InnerStations = new ObservableCollection<Place>(placesData);
            this.OnPropertyChanged("InnerStations");
        }

        private void DeleteInnerStation_Click(object sender, RoutedEventArgs e)
        {
            StationError.Visibility = Visibility.Hidden;
            Place item = innerStationsTable.SelectedItem as Place;
            newInnerStations.RemoveAll(i => i.Name.Equals(item.Name));

            showMapRoute();
            refreshTable();
        }


        private void OnLoadMap(object sender, RoutedEventArgs e) 
        {
            ButtonAutomationPeer peer = new ButtonAutomationPeer(showMapBtn);
            IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            invokeProv.Invoke();
        }

        private void ShowMapClicked(object sender, RoutedEventArgs e)
        {
            currentRoute = selectedRoute.Route;
            CalculateRouteFromPlaces(currentRoute);
        }

        private void addRoute_Click(object sender, RoutedEventArgs e)
        {
            InnerStationError.Visibility = Visibility.Hidden;
            if (newInnerStationCb.SelectedItem == null)
            {
                InnerStationError.Content = "Izaberite međustanicu";
                InnerStationError.Visibility = Visibility;
                return;
            }
            ComboBoxItem item = (ComboBoxItem)newInnerStationCb.SelectedItem;
            string innerPlaceName = item.Content.ToString();

            ComboBoxItem startItem = (ComboBoxItem)startStationCb.SelectedItem;
            string startPlace = startItem.Content.ToString();

            ComboBoxItem endItem = (ComboBoxItem)endStationCb.SelectedItem;
            string endPlace = endItem.Content.ToString();


            if (innerPlaceName.Equals(startPlace))
            {
                InnerStationError.Content = "Ova stanica je već izabrana kao početna";
                InnerStationError.Visibility = Visibility;
                return;
            }
            if (innerPlaceName.Equals(endPlace))
            {
                InnerStationError.Content = "Ova stanica je već izabrana kao krajnja";
                InnerStationError.Visibility = Visibility;
                return;
            }
            foreach (Place p in newInnerStations)
            {
                if (p.Name.Equals(innerPlaceName))
                {
                    InnerStationError.Content = "Ova stanica se već nalazi u međustanicama";
                    InnerStationError.Visibility = Visibility;
                    return;
                }
            }
            Place newPlace = PlaceService.FindPlaceByName(innerPlaceName);
            newInnerStations.Add(newPlace);
            refreshTable();
            showMapRoute();
        }



        private bool checkValidationStations()
        {
            StationError.Visibility = Visibility.Hidden;

            if (startStationCb.SelectedItem == null)
                return false;
            if (endStationCb.SelectedItem == null)
                return false;

            ComboBoxItem startItem = (ComboBoxItem)startStationCb.SelectedItem;
            string startPlace = startItem.Content.ToString();

            ComboBoxItem endItem = (ComboBoxItem)endStationCb.SelectedItem;
            string endPlace = endItem.Content.ToString();

            if (startPlace == endPlace)
            {
                StationError.Content = "Početna i kranja stanica se moraju razlikovati";
                StationError.Visibility = Visibility.Visible;
                return false;
            }
            foreach (Place p in newInnerStations)
            {
                if (p.Name == startPlace)
                {
                    StationError.Content = "Početna stanica se mora razlikovati od međustanica";
                    StationError.Visibility = Visibility.Visible;
                    return false;
                }
                if (p.Name == endPlace)
                {
                    StationError.Content = "Krajnja stanica se mora razlikovati od međustanica";
                    StationError.Visibility = Visibility.Visible;
                    return false;
                }
            }
            return true;
        }

        private void startStationCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            startEndCbChanged();
        }

        private void endStationCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            startEndCbChanged();
        }

        private void startEndCbChanged()
        {
            if (loadingFinished)
            {
                if (checkValidationStations())
                {
                    showMapRoute();
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            if (checkValidationStations())
            {
                ComboBoxItem startItem = (ComboBoxItem)startStationCb.SelectedItem;
                string startPlace = startItem.Content.ToString();
                Place startP = PlaceService.FindPlaceByName(startPlace);

                ComboBoxItem endItem = (ComboBoxItem)endStationCb.SelectedItem;
                string endPlace = endItem.Content.ToString();
                Place endP = PlaceService.FindPlaceByName(endPlace);

                ComboBoxItem trainItem = (ComboBoxItem)trainCb.SelectedItem;
                string trainS = trainItem.Content.ToString();
                Train train = TrainRepository.FindTrainByName(trainS);

                List<Place> places = new List<Place>();
                places.Add(startP);
                newInnerStations.ForEach(s => places.Add(s));
                places.Add(endP);

    
                if (!placesDidNotChanged(places) && RouteService.CheckIfRouteWithPlacesAlreadyExist(places))
                {
                    StationError.Content = "Vozna linija sa unetim stanicama već postoji";
                    StationError.Visibility = Visibility.Visible;
                    return;
                }
                RouteService.ChangeRouteByName(selectedRoute.Route.Name, train, places);
                notifier.ShowSuccess("Vozna linija je uspešno promenjena");
                parent.refreshData();
                this.Close();
            }
        }

        private bool placesDidNotChanged(List<Place> places)
        {
            if (places.Count != selectedRoute.Route.places.Count)
                return false;
            int counter = 0;
            for (int i = 0; i < places.Count; ++i)
            {
                if (places[i].Name.Equals(selectedRoute.Route.places[i].Name))
                    counter++;
            }
            if (counter == places.Count)
                return true;
            return false;

        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            loadingFinished = false;
            setInitialiValuesForComboBoxes();
            loadingFinished = true;
            initInnerStations();
            refreshTable();
            showMapRoute();
        }

    }
}
