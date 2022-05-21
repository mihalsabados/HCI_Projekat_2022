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
using HCI_Projekat.model;
using HCI_Projekat.services;

namespace HCI_Projekat
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
            
        }


        private void ResetState()
        {
            LoginError.Visibility = Visibility.Hidden;
            usernameTxt.Text = "";
            passwordTxt.Password = "";
        }

        private void LoginClicked(object sender, RoutedEventArgs e)
        {
            var username = usernameTxt.Text;
            var password = passwordTxt.Password;
            User user = UserService.Login(username, password);

            if (user != null)
            {
                this.Visibility = Visibility.Hidden;
                if (user.UserType == UserType.MANAGER)
                    ((MainWindow)App.Current.MainWindow).HomePageManager.Visibility = Visibility.Visible;
                else
                    ((MainWindow)App.Current.MainWindow).HomePageClient.Visibility = Visibility.Visible;
            } else
            {
                LoginError.Visibility = Visibility.Visible;
            }
        }


		private void ButtonRegister_OnClick(object sender, RoutedEventArgs e)
		{
           ((MainWindow)App.Current.MainWindow).Login.Visibility = Visibility.Hidden;
            ResetState();
           ((MainWindow)App.Current.MainWindow).Registration.Visibility = Visibility.Visible;
        }
	}
}
