using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	/// Interaction logic for HomePageManager.xaml
	/// </summary>
	public partial class HomePageManager : Page
	{
		public HomePageManager()
		{
			InitializeComponent();
		}

		private void Logout(object sender, RoutedEventArgs e)
        {
			MainWindow.LoggedUser = null;
			((MainWindow)App.Current.MainWindow).HomePageManager.Visibility = Visibility.Hidden;
			((MainWindow)App.Current.MainWindow).Login.Visibility = Visibility.Visible;
			this.TimetableView.Visibility = Visibility.Hidden;
			((MainWindow)App.Current.MainWindow).Width = 800;

		}

		private void TimetableShow(object sender, RoutedEventArgs e)
		{
			((MainWindow)App.Current.MainWindow).Width = 1000;
			this.RouteMapView.Visibility = Visibility.Hidden;
			this.StationsView.Visibility = Visibility.Hidden;
			this.TrainCRUD.Visibility = Visibility.Hidden;
			this.SoldCardsPerMonthPage.Visibility = Visibility.Hidden;
			this.SoldCardsPerRoutePage.Visibility = Visibility.Hidden;
			this.RoutesViewPage.Visibility = Visibility.Hidden;
			this.TimetableView.Visibility = Visibility.Visible;
			this.TimetableView.Refresh();
		}

        private void OpenTrainCrudView(object sender, RoutedEventArgs e)
        {
			((MainWindow)App.Current.MainWindow).Width = 800;
			this.RouteMapView.Visibility = Visibility.Hidden;
			this.StationsView.Visibility = Visibility.Hidden;
			this.TimetableView.Visibility = Visibility.Hidden;
			this.SoldCardsPerMonthPage.Visibility = Visibility.Hidden;
			this.SoldCardsPerRoutePage.Visibility = Visibility.Hidden;
			this.RoutesViewPage.Visibility = Visibility.Hidden;

			this.TrainCRUD.Visibility = Visibility.Visible;
			this.TrainCRUD.Refresh();
		}

        private void SoldCardsPerMonth(object sender, RoutedEventArgs e)
        {
			((MainWindow)App.Current.MainWindow).Width = 1100;
			this.RouteMapView.Visibility = Visibility.Hidden;
			this.StationsView.Visibility = Visibility.Hidden;
			this.TrainCRUD.Visibility = Visibility.Hidden;
			this.TimetableView.Visibility = Visibility.Hidden;
			this.SoldCardsPerRoutePage.Visibility = Visibility.Hidden;
			this.RoutesViewPage.Visibility = Visibility.Hidden;
			SoldCardsPerMonthPage.Visibility = Visibility.Visible;
			this.SoldCardsPerMonthPage.Refresh();
		}

		private void SoldCardsPerRoute(object sender, RoutedEventArgs e)
		{
			((MainWindow)App.Current.MainWindow).Width = 1100;
			this.RouteMapView.Visibility = Visibility.Hidden;
			this.TrainCRUD.Visibility = Visibility.Hidden;
			this.StationsView.Visibility = Visibility.Hidden;
			this.TimetableView.Visibility = Visibility.Hidden;
			this.SoldCardsPerMonthPage.Visibility = Visibility.Hidden;
			this.RoutesViewPage.Visibility = Visibility.Hidden;

			this.SoldCardsPerRoutePage.Visibility = Visibility.Visible;
			this.SoldCardsPerRoutePage.Refresh();
		}

		private void ShowRouteMap_Click(object sender, RoutedEventArgs e)
        {
			((MainWindow)App.Current.MainWindow).Width = 800;
			this.TrainCRUD.Visibility = Visibility.Hidden;
			this.StationsView.Visibility = Visibility.Hidden;
			this.TimetableView.Visibility = Visibility.Hidden;
			this.SoldCardsPerMonthPage.Visibility = Visibility.Hidden;
			this.SoldCardsPerRoutePage.Visibility = Visibility.Hidden;
			this.RoutesViewPage.Visibility = Visibility.Hidden;

			this.RouteMapView.Visibility = Visibility.Visible;
			this.RouteMapView.Refresh();
		}

		private void Stations_Click(object sender, RoutedEventArgs e)
        {
			((MainWindow)App.Current.MainWindow).Width = 800;
			this.TrainCRUD.Visibility = Visibility.Hidden;
			this.TimetableView.Visibility = Visibility.Hidden;
			this.RouteMapView.Visibility = Visibility.Hidden;
			this.SoldCardsPerMonthPage.Visibility = Visibility.Hidden;
			this.SoldCardsPerRoutePage.Visibility = Visibility.Hidden;
			this.RoutesViewPage.Visibility = Visibility.Hidden;

			this.StationsView.Visibility = Visibility.Visible;
			this.StationsView.Refresh();
		}

        private void Routes_Click(object sender, RoutedEventArgs e)
        {
			((MainWindow)App.Current.MainWindow).Width = 800;
			this.TrainCRUD.Visibility = Visibility.Hidden;
			this.TimetableView.Visibility = Visibility.Hidden;
			this.RouteMapView.Visibility = Visibility.Hidden;
			this.SoldCardsPerMonthPage.Visibility = Visibility.Hidden;
			this.StationsView.Visibility = Visibility.Hidden;
			this.SoldCardsPerRoutePage.Visibility = Visibility.Hidden;
			this.RoutesViewPage.Visibility = Visibility.Visible;
			this.RoutesViewPage.Refresh();
		}

        private void dokumentacijaClick(object sender, RoutedEventArgs e)
        {
			var helpPath = "timetableManager";
			if (this.TrainCRUD.Visibility == Visibility.Visible)
				helpPath = "trains";
			else if (this.RouteMapView.Visibility == Visibility.Visible)
				helpPath = "mreznaLinija";
			else if (this.SoldCardsPerMonthPage.Visibility == Visibility.Visible)
				helpPath = "soldTicketsMonth";
			else if (this.SoldCardsPerRoutePage.Visibility == Visibility.Visible)
				helpPath = "soldTicketsRoute";
			else if (this.StationsView.Visibility == Visibility.Visible)
				helpPath = "stations";
			else if (this.RoutesViewPage.Visibility == Visibility.Visible)
				helpPath = "routeMap";
			else if (this.TimetableView.Visibility == Visibility.Visible)
				helpPath = "timetableManager";

			HelpProvider.ShowHelp(helpPath, this);
		}

		private void demoClick(object sender, RoutedEventArgs e)
		{
			var helpPath = "demoTimetableManager";
			if (this.TrainCRUD.Visibility == Visibility.Visible)
				helpPath = "demoTrains";
			else if (this.RouteMapView.Visibility == Visibility.Visible)
				helpPath = "demoMreznaLinija";
			else if (this.SoldCardsPerMonthPage.Visibility == Visibility.Visible)
				helpPath = "demoSoldTicketsMonth";
			else if (this.SoldCardsPerRoutePage.Visibility == Visibility.Visible)
				helpPath = "demoSoldTicketsRoute";
			else if (this.StationsView.Visibility == Visibility.Visible)
				helpPath = "demoStations";
			else if (this.RoutesViewPage.Visibility == Visibility.Visible)
				helpPath = "demoRouteMap";
			else if (this.TimetableView.Visibility == Visibility.Visible)
				helpPath = "demoTimetableManager";

			HelpProvider.ShowHelp(helpPath, this);
		}
	}
}
