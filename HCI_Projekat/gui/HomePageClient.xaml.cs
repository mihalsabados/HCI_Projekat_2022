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
using System.Diagnostics;

namespace HCI_Projekat.gui
{
	/// <summary>
	/// Interaction logic for HomePageClient.xaml
	/// </summary>
	public partial class HomePageClient : Page
	{
		public HomePageClient()
		{
			InitializeComponent();
		}

		private void Logout(object sender, RoutedEventArgs e)
		{
			MainWindow.LoggedUser = null;
			((MainWindow)App.Current.MainWindow).HomePageClient.Visibility = Visibility.Hidden;
			((MainWindow)App.Current.MainWindow).Login.Visibility = Visibility.Visible;
			this.TimetableView.Visibility = Visibility.Hidden;
		}

		private void TimetableShow(object sender, RoutedEventArgs e)
		{
			this.RouteMapView.Visibility = Visibility.Hidden;
			this.ReservationPage.Visibility = Visibility.Hidden;
			this.TimetableView.Refresh();
			this.TimetableView.Visibility = Visibility.Visible;
		}

        private void ReservationClick(object sender, RoutedEventArgs e)
        {
			this.TimetableView.Visibility = Visibility.Hidden;
			this.RouteMapView.Visibility = Visibility.Hidden;

			this.ReservationPage.Visibility = Visibility.Visible;
			this.ReservationPage.Refresh();
        }
		private void ShowRouteMap_Click(object sender, RoutedEventArgs e)
		{
			this.TimetableView.Visibility = Visibility.Hidden;
			this.ReservationPage.Visibility = Visibility.Hidden;

			this.RouteMapView.Visibility = Visibility.Visible;
			this.RouteMapView.Refresh();
		}
	}


}
