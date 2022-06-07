﻿using System;
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
    }
}
