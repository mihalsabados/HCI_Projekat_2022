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

using Microsoft.Maps.MapControl.WPF;



using BingMapsRESTToolkit;
using BingMapsRESTToolkit.Extensions;
using HCI_Projekat.model;
using HCI_Projekat.services;
using System.Net;
using System.IO;
using System.Diagnostics;

using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace HCI_Projekat.gui
{
    /// <summary>
    /// Interaction logic for StationsView.xaml
    /// </summary>
    public partial class StationsView : Page, INotifyPropertyChanged
    {

        private string BingMapsKey = "oU6C1d9L3SpiIjWjcMtX~NlvJ4abp72u-diLrup_xdw~AtDP3HyzbLn6vnOjapJ7nLaqM4g-sR1TpNVBbl6c53FGjGJEVOp5jRDt96RJyCmC";
        private string SessionKey;
        private Pushpin addedPushPin = null;
        private Coordinate addedCordinate = null;
        private List<Place> bindedPlaces = new List<Place>();

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

        public ObservableCollection<Place> Places
        {
            get;
            set;
        }

        public StationsView()
        {
            InitializeComponent();
            this.DataContext = this;
            initTableData();

            this.Mapa.Focus();
            Mapa.CredentialsProvider = new ApplicationIdCredentialsProvider(BingMapsKey);
            Mapa.CredentialsProvider.GetCredentials((c) =>
            {
                SessionKey = c.ApplicationId;
            });
            addExistingStationsToMap();
        }

        private void initTableData()
        {
            List<Place> places = PlaceService.GetAllPlaces();
            foreach (Place p in places)
            {
                bindedPlaces.Add(p);
            }

            Places = new ObservableCollection<Place>(bindedPlaces);
            View = CollectionViewSource.GetDefaultView(Places);
        }
        private void refreshData()
        {
            pretraziStaniceTxt.Text = "";
            bindedPlaces = new List<Place>();
            foreach (Place p in PlaceService.GetAllPlaces())
            {
                bindedPlaces.Add(p);
            }
            Places = new ObservableCollection<Place>(bindedPlaces);
            this.OnPropertyChanged("Places");
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


        private void ButtonClickDelete(object sender, RoutedEventArgs e)
        {
            Place item = stationsTable.SelectedItem as Place;

            bool cantDelete = RouteService.CheckIfSomeRouteUsePlaceWithName(item.Name);
            if (cantDelete)
            {
                notifier.ShowError("Nije moguće obrisati stanicu jer postoje vozne linije koje je koriste.");
            }
            else
            {
                bool deleted = PlaceService.DeletePlaceByName(item.Name);
                if (deleted)
                {
                    notifier.ShowSuccess("Uspešno ste obrisali stanicu.");
                    addExistingStationsToMap();
                    refreshData();
                }
                else
                {
                    notifier.ShowError("Došlo je do greške prilikom brisanja stanice. Probajte ponovo");
                }
            }
            
        }

        private void Mapa_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (addedPushPin != null)
                return;

            // Disables the default mouse double-click action.
            e.Handled = true;

            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            System.Windows.Point mousePosition = e.GetPosition(Mapa);
            //Convert the mouse coordinates to a locatoin on the map
            Microsoft.Maps.MapControl.WPF.Location pinLocation = Mapa.ViewportPointToLocation(mousePosition);

            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();
            pin.Location = pinLocation;
            pin.Content = "S";
            addedCordinate = new Coordinate(pinLocation.Latitude, pinLocation.Longitude);

            // Adds the pushpin to the map.
            Mapa.Children.Add(pin);
            addedPushPin = pin;

            processRequestClick();
        }

        private void processRequestClick()
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://" + $@"dev.virtualearth.net/REST/v1/Locations/{addedCordinate.Latitude}, {addedCordinate.Longitude}?includeEntityTypes=Address, Neighborhood, CountryRegion&includeNeighborhood=1&include=ciso2&key={BingMapsKey}");
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream());

                string temp = sr.ReadToEnd();
                List<char> charsToRemove = new List<char>() { '@', '.', '"', '{', '}', '\\' };
                foreach (char c in charsToRemove)
                {
                    temp = temp.Replace(c.ToString(), string.Empty);
                }

                if (!temp.Contains("Serbia"))
                {
                    MapError.Content = "Izaberite mesto u Srbiji";
                    MapError.Visibility = Visibility.Visible;
                    return;
                }
                if (temp.Contains("locality"))
                {
                    resetErrors();
                    string cityName = temp.Split("locality:")[1].Trim().Split(",")[0];
                    newStation.Text = cityName;
                }
                else
                {
                    MapError.Content = "Izgleda da niste izabrali naseljeno mesto. Probajte ponovo";
                    MapError.Visibility = Visibility.Visible;
                    return;
                }

            }
            catch (Exception)
            {
                return;
            }
        }


        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (addedPushPin != null)
            {
                Mapa.Children.Remove(addedPushPin);
                addedPushPin = null;
                addedCordinate = null;
                resetErrors();
                newStation.Text = "";
            }
        }


        private void addExistingStationsToMap()
        {
            Mapa.Children.Clear();
            List<Place> places = PlaceService.GetAllPlaces();
            for (int i = 0; i < places.Count; ++i)
            {
                var loc = new Microsoft.Maps.MapControl.WPF.Location(places[i].Latitude, places[i].Longitude);
                Mapa.Children.Add(new Pushpin()
                {
                    Location = loc,
                });
            }
        }

        private void resetErrors()
        {
            StationNameError.Visibility = Visibility.Hidden;
            MapError.Visibility = Visibility.Hidden;
        }

        private bool stationNameAlreadyExist(string name)
        {
            Place place = PlaceService.FindPlaceByName(name);
            return place != null; 
        }


        static private Resource[] GetResourcesFromRequest(BaseRestRequest rest_request)
        {
            var r = ServiceManager.GetResponseAsync(rest_request).GetAwaiter().GetResult();

            if (!(r != null && r.ResourceSets != null &&
                r.ResourceSets.Length > 0 &&
                r.ResourceSets[0].Resources != null &&
                r.ResourceSets[0].Resources.Length > 0))

                throw new Exception("No results found.");

            return r.ResourceSets[0].Resources;
        }

        private void Sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            resetErrors();

            string newStationText = newStation.Text.Trim();
            if (newStationText.Length == 0)
            {
                StationNameError.Content = "Unesite naziv stanice";
                StationNameError.Visibility = Visibility.Visible;
            }
            else if (stationNameAlreadyExist(newStationText))
            {
                StationNameError.Content = "Stanica se datim nazivom već postoji";
                StationNameError.Visibility = Visibility.Visible;
            }
            else if (addedPushPin == null)
                MapError.Visibility = Visibility.Visible;
            else
            {
                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://" + $@"dev.virtualearth.net/REST/v1/Locations/{addedCordinate.Latitude}, {addedCordinate.Longitude}?includeEntityTypes=Address, Neighborhood, CountryRegion&includeNeighborhood=1&include=ciso2&key={BingMapsKey}");
                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                    StreamReader sr = new StreamReader(res.GetResponseStream());

                    string temp = sr.ReadToEnd();
                    List<char> charsToRemove = new List<char>() { '@', '.', '"', '{', '}', '\\' };
                    foreach (char c in charsToRemove)
                    {
                        temp = temp.Replace(c.ToString(), string.Empty);
                    }

                    if (!temp.Contains("Serbia"))
                    {
                        MapError.Content = "Izaberite mesto u Srbiji";
                        MapError.Visibility = Visibility.Visible;
                        return;
                    }
                    if (!temp.Contains("locality"))
                    {
                        MapError.Content = "Izgleda da niste izabrali naseljeno mesto. Probajte ponovo";
                        MapError.Visibility = Visibility.Visible;
                        return;
                    }


                    Place newPlace = new Place(newStationText, addedCordinate.Latitude, addedCordinate.Longitude);
                    PlaceService.AddNewPlace(newPlace);

                    refreshData();
                    addExistingStationsToMap();
                    notifier.ShowSuccess("Uspešno ste dodali stanicu.");
                }
                catch (Exception)
                {
                    notifier.ShowSuccess("Došlo je do greške prilikom čuvanja stanice. Probajte ponovo.");
                    return;
                }
            }
        }

        private void pretraziStanice_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (pretraziStaniceTxt.Text.Trim().Length == 0)
                refreshData();
            string criteria = pretraziStaniceTxt.Text.Trim().ToLower();

            bindedPlaces = new List<Place>();
            foreach (Place p in PlaceService.GetAllPlaces())
            {
                if (p.Name.ToLower().Contains(criteria))
                {
                    bindedPlaces.Add(p);
                }
            }
            Places = new ObservableCollection<Place>(bindedPlaces);
            this.OnPropertyChanged("Places");
        }
    }
}
