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
using System.Windows.Shapes;

namespace HCI_Projekat.gui
{
    /// <summary>
    /// Interaction logic for AddNewWagonMessageBox.xaml
    /// </summary>
    public partial class AddNewWagonMessageBox : Window
    {
        public static model.Wagon.WagonType WagonTypeSelected { get; set; }

        public AddNewWagonMessageBox()
        {
            InitializeComponent();
            WagonTypeSelected = model.Wagon.WagonType.LARGE;
        }

        private void addWagonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void cancelAdding(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void resetStyleToButtons()
        {
            largeBtn.Background =(SolidColorBrush)new BrushConverter().ConvertFrom("#FF485B83");
            mediumBtn.Background =(SolidColorBrush)new BrushConverter().ConvertFrom("#FF485B83");
            smallBtn.Background =(SolidColorBrush)new BrushConverter().ConvertFrom("#FF485B83");
        }

        private void largeWagonClicked(object sender, RoutedEventArgs e)
        {
            resetStyleToButtons();
            largeBtn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFC34E43");
            WagonTypeSelected = model.Wagon.WagonType.LARGE;
        }

        private void mediumWagonClicked(object sender, RoutedEventArgs e)
        {
            resetStyleToButtons();
            mediumBtn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFC34E43");
            WagonTypeSelected = model.Wagon.WagonType.MEDIUM;
        }

        private void smallWagonClicked(object sender, RoutedEventArgs e)
        {
            resetStyleToButtons();
            smallBtn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFC34E43");
            WagonTypeSelected = model.Wagon.WagonType.SMALL;
        }
    }
}

