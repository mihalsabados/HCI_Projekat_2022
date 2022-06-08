using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HCI_Projekat.model;
using HCI_Projekat.database;
using HCI_Projekat.services;
using Microsoft.Maps.MapControl.WPF;
using BingMapsRESTToolkit;
using BingMapsRESTToolkit.Extensions;


namespace HCI_Projekat.gui
{
    /// <summary>
    /// Interaction logic for ShowOneRouteOnMap.xaml
    /// </summary>
    public partial class ShowOneRouteOnMap : Window
    {

        private string BingMapsKey = "oU6C1d9L3SpiIjWjcMtX~NlvJ4abp72u-diLrup_xdw~AtDP3HyzbLn6vnOjapJ7nLaqM4g-sR1TpNVBbl6c53FGjGJEVOp5jRDt96RJyCmC";
        private string SessionKey;


        private model.Route currentRoute;

        public ShowOneRouteOnMap(model.Route route)
        {
            InitializeComponent();
            this.currentRoute = route;
            this.Mapa.Focus();
            Mapa.CredentialsProvider = new ApplicationIdCredentialsProvider(BingMapsKey);
            Mapa.CredentialsProvider.GetCredentials((c) =>
            {
                SessionKey = c.ApplicationId;
            });
        }

        private void OnLoadMap(object sender, RoutedEventArgs e)
        {
            ButtonAutomationPeer peer = new ButtonAutomationPeer(showMapBtn);
            IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            invokeProv.Invoke();
        }
        private void ShowMapClicked(object sender, RoutedEventArgs e)
        {
            CalculateRouteFromPlaces(currentRoute);
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
                    MessageBox.Show("Došlo je do greške prilikom učitavanja rute. Probajte da otvorite prikaz ponovo,\n ili kliknite na dugme 'Osveži prikaz' u gornjem levom uglu.");
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

        private void refreshMapViewBtn_Click(object sender, RoutedEventArgs e)
        {
            CalculateRouteFromPlaces(currentRoute);
        }
    }

}
