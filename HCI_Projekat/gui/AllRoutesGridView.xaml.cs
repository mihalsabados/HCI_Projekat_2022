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
        }
        
        private void initTableData()
        {
            List<Route> routes = RouteService.GetAllRoutes();
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
    }
}
