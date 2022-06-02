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
using HCI_Projekat.database;
using HCI_Projekat.services;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using System.Threading;

namespace HCI_Projekat.gui
{
	/// <summary>
	/// Interaction logic for Registration.xaml
	/// </summary>
	public partial class Registration : Page
	{
		public Registration()
		{
			InitializeComponent();
		}
		Notifier notifier = new Notifier(cfg =>
		{
			cfg.PositionProvider = new WindowPositionProvider(
				parentWindow: Application.Current.MainWindow,
				corner: Corner.TopRight,
				offsetX: 10,
				offsetY: 10);

			cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
				notificationLifetime: TimeSpan.FromSeconds(2),
				maximumNotificationCount: MaximumNotificationCount.FromCount(5)); ; ;

			cfg.Dispatcher = Application.Current.Dispatcher;
		});

		private void Button_Click_Login(object sender, RoutedEventArgs e)
		{	
			((MainWindow)App.Current.MainWindow).Registration.Visibility = Visibility.Hidden;
			((MainWindow)App.Current.MainWindow).Login.Visibility = Visibility.Visible;
		}


		private void ResetState()
        {
			FirstNameError.Visibility = Visibility.Hidden;
			LastNameError.Visibility = Visibility.Hidden;
			UsernameError.Visibility = Visibility.Hidden;
			PasswordError.Visibility = Visibility.Hidden;
			ConfrimPasswordError.Visibility = Visibility.Hidden;
		}

		private void Button_Click_Register(object sender, RoutedEventArgs e)
		{
			var firstName = firstNameTxt.Text;
			var lastName = lastNameTxt.Text;
			var username = usernameTxt.Text;
			var password = passwordTxt.Password;
			var confirmPassword = confirmPasswordTxt.Password;

			ResetState();

			if (firstName.Length == 0)
				FirstNameError.Visibility = Visibility.Visible;
			else if (lastName.Length == 0)
				LastNameError.Visibility = Visibility.Visible;
			else if (username.Length == 0)
				UsernameError.Visibility = Visibility.Visible;
			else if (password.Length == 0)
				PasswordError.Visibility = Visibility.Visible;
			else if (confirmPassword.Length == 0)
				ConfrimPasswordError.Visibility = Visibility.Visible;
			else
            {
				User user = UserService.findUserByUsername(username);
				if (user != null)
                {
					UsernameError.Visibility = Visibility.Visible;
					UsernameError.Content = "Korisnik sa unetim korisničkim imenom već postoji.";
                }
				else
                {
					if (password != confirmPassword)
                    {
						ConfrimPasswordError.Visibility = Visibility.Visible;
						ConfrimPasswordError.Content = "Lozinka za proveru nije ista.";
                    } else
                    {
						User newUser = UserService.RegisterNewUser(firstName, lastName, username, password);
						notifier.ShowSuccess("Uspešno ste se registrovali.");

						((MainWindow)App.Current.MainWindow).Registration.Visibility = Visibility.Hidden;
						((MainWindow)App.Current.MainWindow).Login.Visibility = Visibility.Visible;
						
					}
                }
			}

		}
	}
}
