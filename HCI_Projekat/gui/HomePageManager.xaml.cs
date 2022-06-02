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
		}
    }
}
