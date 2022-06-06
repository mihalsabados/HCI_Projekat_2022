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
using HCI_Projekat.model;
using HCI_Projekat.services;
using Microsoft.Maps.MapControl.WPF;

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

        public ObservableCollection<Place> InnerStations
        {
            get;
            set;
        }


        public EditRoute()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Mapa.Focus();
            Mapa.CredentialsProvider = new ApplicationIdCredentialsProvider(BingMapsKey);
            Mapa.CredentialsProvider.GetCredentials((c) =>
            {
                SessionKey = c.ApplicationId;
            });

            selectedRoute = AllRoutesGridView.SelectedRoute;
            initInnerStations();
        }

        private void initInnerStations()
        {
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
            Place item = innerStationsTable.SelectedItem as Place;
            newInnerStations.RemoveAll(i => i.Name.Equals(item.Name));

            // toDo, promena na mapi

            refreshTable();
        }

        private void addRoute_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
