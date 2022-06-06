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
using Route = HCI_Projekat.model.Route;


namespace HCI_Projekat.gui
{
    /// <summary>
    /// Interaction logic for RouteMapView.xaml
    /// </summary>
    public partial class RouteMapView : Page
    {
        private string BingMapsKey = "oU6C1d9L3SpiIjWjcMtX~NlvJ4abp72u-diLrup_xdw~AtDP3HyzbLn6vnOjapJ7nLaqM4g-sR1TpNVBbl6c53FGjGJEVOp5jRDt96RJyCmC";
        private string SessionKey;

        public static String SelectedFromRoute { get; set; }
        public static String SelectedToRoute { get; set; }

        private Route currentRoute = null;

        public RouteMapView()
        {
            InitializeComponent();
            this.Mapa.Focus();


            Mapa.CredentialsProvider = new ApplicationIdCredentialsProvider(BingMapsKey);
            Mapa.CredentialsProvider.GetCredentials((c) =>
            {
                SessionKey = c.ApplicationId;
            });

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



        private async void CalculateRouteFromPlaces(Route foundRoute)
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
                MessageBox.Show("Error: " + ex.Message);
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


        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SelectedFromRoute = "";
            SelectedToRoute = "";
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
                    Route route = checkIfRouteExists(SelectedFromRoute, SelectedToRoute);
                    if (route == null)
                    {
                        searchedMessage.Content = "Trenuno ne postoji direktna vozna linija: " + SelectedFromRoute + " - " + SelectedToRoute;
                        this.searchedMessage.Visibility = Visibility.Visible;
                        this.Mapa.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        searchedMessage.Content = "Vozna linija: " + SelectedFromRoute + " - " + SelectedToRoute;
                        this.searchedMessage.Visibility = Visibility.Visible;
                        this.Mapa.Visibility = Visibility.Visible;
                        currentRoute = route;
                        CalculateRouteFromPlaces(route);
                    }
                }
            }
        }
    }
}
