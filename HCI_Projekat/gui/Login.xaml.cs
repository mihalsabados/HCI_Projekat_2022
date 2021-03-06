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


        public void ResetState()
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
                if (user.UserType == UserType.MANAGER)
                {
                    ((MainWindow)App.Current.MainWindow).Login.Visibility = Visibility.Hidden;
                    ResetState();
                    ((MainWindow)App.Current.MainWindow).HomePageManager.Visibility = Visibility.Visible;
                }
                else
                {
                    ((MainWindow)App.Current.MainWindow).Login.Visibility = Visibility.Hidden;
                    ResetState();
                    ((MainWindow)App.Current.MainWindow).HomePageClient.Visibility = Visibility.Visible;
                }
                MainWindow.LoggedUser = user;
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

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string str = "index";
            HelpProvider.ShowHelp(str, this);
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                this.Focusable = true;
                this.Focus();
                RoutedCommand newCmdFilter = new RoutedCommand();
                newCmdFilter.InputGestures.Add(new KeyGesture(Key.F1));
                this.CommandBindings.Add(new CommandBinding(newCmdFilter, CommandBinding_Executed));
            }
        }
    }
}
