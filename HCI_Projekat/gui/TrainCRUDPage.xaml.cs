using HCI_Projekat.database;
using HCI_Projekat.model;
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
using ToastNotifications.Messages;

namespace HCI_Projekat.gui
{
    /// <summary>
    /// Interaction logic for TrainCRUDPage.xaml
    /// </summary>
    public partial class TrainCRUDPage : Page
    {
        public string selectedTrain { get; set; }

        public List<Wagon> selectedTrainWagons { get; set; }

        public TrainCRUDPage()
        {
            InitializeComponent();

            FormLayout.Visibility = Visibility.Hidden;

            createTrainChips();
        }

        private void createTrainChips()
        {
            Control label = (Control)TrainLayout.Children[0];
            TrainLayout.Children.Clear();
            TrainLayout.Children.Add(label);
            int marginLeft = 30;
            int marginTop = 5;
            int row = 0;
            foreach (var train in TrainRepository.getAllTrains())
            {
                MaterialDesignThemes.Wpf.Chip chip = new MaterialDesignThemes.Wpf.Chip();
                chip.Content = train.Name;
                chip.Margin = new Thickness(marginLeft, marginTop, 0, 0);
                chip.Click += TrainChipClick;
                chip.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF485B83");
                chip.Foreground = Brushes.White;
                chip.VerticalAlignment = VerticalAlignment.Top;
                MaterialDesignThemes.Wpf.PackIcon icon = new MaterialDesignThemes.Wpf.PackIcon();
                icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Train;
                chip.Icon = icon;
                chip.IconBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFC34E43");
                chip.IsDeletable = true;
                chip.DeleteClick += TrainChipDelete;
                Grid.SetRow(chip, 1);
                TrainLayout.Children.Add(chip);
                marginLeft += 90 + train.Name.Length * (int)(chip.FontSize / 2);
                row++;
                if (row == 3)
                {
                    marginLeft = 30;
                    marginTop += 50 + (int)chip.FontSize;
                    row = 0;
                }
            }
            MaterialDesignThemes.Wpf.Chip newChip = new MaterialDesignThemes.Wpf.Chip();
            newChip.Content = "Dodaj novi voz";
            newChip.Margin = new Thickness(marginLeft, marginTop, 0, 0);
            newChip.Click += AddNewTrain;
            newChip.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF485B83");
            newChip.Foreground = Brushes.White;
            newChip.VerticalAlignment = VerticalAlignment.Top;
            MaterialDesignThemes.Wpf.PackIcon addIcon = new MaterialDesignThemes.Wpf.PackIcon();
            addIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Add;
            newChip.Icon = addIcon;
            newChip.IconBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFC34E43");
            Grid.SetRow(newChip, 1);
            TrainLayout.Children.Add(newChip);
        }

        private void AddNewTrain(object sender, RoutedEventArgs e)
        {
            resetTrainChipStyle();
            FormLayout.Visibility = Visibility.Visible;
            trainNameTxt.Text = "";
            WagonListBox.Items.Clear();
            myCanvas.Visibility = Visibility.Hidden;
            selectedTrain = null;
            selectedTrainWagons = new List<Wagon>();
            setNumberOfPassengersTxt();
            saveBtn.Visibility = Visibility.Hidden;
            addBtn.Visibility = Visibility.Visible;
            createAddNewWagonChip();
        }

        private void TrainChipDelete(object sender, RoutedEventArgs e)
        {
            bool? Result = new CustomMessageBox("Da li ste sigurni da želite da obrišete ovaj voz ?", MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

            MaterialDesignThemes.Wpf.Chip chip = sender as MaterialDesignThemes.Wpf.Chip;

            if (Result.Value)
            {
                TrainRepository.removeTrainByName(chip.Content.ToString());
                createTrainChips();
                FormLayout.Visibility = Visibility.Hidden;
            }

        }

        private void WagonChipDelete(object sender, RoutedEventArgs e)
        {
            MaterialDesignThemes.Wpf.Chip ctrl = sender as MaterialDesignThemes.Wpf.Chip;
            int wagonIndex = int.Parse(ctrl.Content.ToString().Split(':')[0].Trim());

            bool? Result = new CustomMessageBox("Da li ste sigurni da želite da obrišete ovaj vagon ?", MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

            MaterialDesignThemes.Wpf.Chip chip = sender as MaterialDesignThemes.Wpf.Chip;

            if (Result.Value)
            {
                selectedTrainWagons.RemoveAt(wagonIndex - 1);
                addWagonChips();
                setNumberOfPassengersTxt();
            }
        }

        private void ChangeChipColorToDefault()
        {
            foreach (var item in WagonListBox.Items)
            {
                MaterialDesignThemes.Wpf.Chip chip = item as MaterialDesignThemes.Wpf.Chip;
                //if(chip.GetValue(Control.BackgroundProperty) != null)
                chip.ClearValue(Control.BackgroundProperty); 
                chip.ClearValue(Control.ForegroundProperty); 
            }

        }

        private void WagonChipClick(object sender, RoutedEventArgs e)
        {
            MaterialDesignThemes.Wpf.Chip ctrl = sender as MaterialDesignThemes.Wpf.Chip;
            string wagonName = ctrl.Content.ToString().Split(':')[1].Trim();
            ChangeChipColorToDefault();
            ctrl.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF485B83");
            ctrl.Foreground = Brushes.White;
            myCanvas.Visibility = Visibility.Visible;

            double rows;
            if (wagonName.Equals("Mali vagon"))
                rows = (int)Wagon.WagonType.SMALL;
            else if (wagonName.Equals("Srednji vagon"))
                rows = (int)Wagon.WagonType.MEDIUM;
            else
                rows = (int)Wagon.WagonType.LARGE;

            WagonRect.Height = rows * 23;
            WagonRect.Width = 100;
            WagonLayout.Width = WagonRect.Width;
            WagonLayout.Height = WagonRect.Height;
            WagonLayout.Children.Clear();

            double left = 5;
            double top = 8;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Path seat = new Path();
                    seat.Fill = Brushes.Black;
                    seat.Data = Geometry.Parse("M19 9V7C19 5.35 17.65 4 16 4H8C6.35 4 5 5.35 5 7V9C3.35 9 2 10.35 2 12V17C2 18.65 3.35 20 5 20V22H7V20H17V22H19V20C20.65 20 22 18.65 22 17V12C22 10.35 20.65 9 19 9M7 7C7 6.45 7.45 6 8 6H16C16.55 6 17 6.45 17 7V9.78C16.39 10.33 16 11.12 16 12V14H8V12C8 11.12 7.61 10.33 7 9.78V7M20 17C20 17.55 19.55 18 19 18H5C4.45 18 4 17.55 4 17V12C4 11.45 4.45 11 5 11S6 11.45 6 12V16H18V12C18 11.45 18.45 11 19 11S20 11.45 20 12V17Z");
                    seat.Height = 18;
                    seat.Width = 18;
                    seat.Stretch = Stretch.Fill;
                    WagonLayout.Children.Add(seat);
                    Canvas.SetLeft(seat, left);
                    Canvas.SetTop(seat, top);
                    if (j == 1)
                        left += seat.Width + 12;
                    else
                        left += seat.Width + 2;
                }
                top += 22;
                left = 5;
            }
        }

        private void resetTrainChipStyle()
        {
            foreach (var item in TrainLayout.Children)
            {
                if(item.GetType() == typeof(MaterialDesignThemes.Wpf.Chip))
                {
                    MaterialDesignThemes.Wpf.Chip chip = item as MaterialDesignThemes.Wpf.Chip;
                    chip.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF485B83");
                    chip.Foreground = Brushes.White;
                }

            }

        }

        private void TrainChipClick(object sender, RoutedEventArgs e)
        {
            FormLayout.Visibility = Visibility.Visible;
            MaterialDesignThemes.Wpf.Chip ctrl = sender as MaterialDesignThemes.Wpf.Chip;
            resetTrainChipStyle();
            //ctrl.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF34425F");
            ctrl.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFC34E43");
            string trainName = ctrl.Content.ToString();
            selectedTrain = trainName;
            trainNameTxt.Text = trainName;
            myCanvas.Visibility = Visibility.Hidden;
            selectedTrainWagons = TrainRepository.FindTrainByName(selectedTrain).Wagons;
            addWagonChips();
            setNumberOfPassengersTxt();
        }

        private void setNumberOfPassengersTxt()
        {
            var numOfPassengers = selectedTrainWagons.Sum(x => x.Seats.Count * x.Seats[0].Count);
            passengerNumTxt.Content = passengerNumTxt.Content.ToString().Split(":")[0] + ": " + numOfPassengers;
        }


        private void addWagonChips()
        {
            WagonListBox.Items.Clear();
            int br = 1;

            foreach (var wagon in selectedTrainWagons)
            {
                MaterialDesignThemes.Wpf.Chip chip = new MaterialDesignThemes.Wpf.Chip();
                chip.Content = br++ + ": ";
                if (wagon.wagonType == model.Wagon.WagonType.SMALL)
                    chip.Content += "Mali vagon";
                else if (wagon.wagonType == model.Wagon.WagonType.MEDIUM)
                    chip.Content += "Srednji vagon";
                else
                    chip.Content += "Veliki vagon";
                chip.HorizontalAlignment = HorizontalAlignment.Stretch;
                chip.VerticalAlignment = VerticalAlignment.Stretch;
                chip.Margin = new Thickness(0, 0, 0, 0);
                chip.IsDeletable = true;
                chip.DeleteClick += WagonChipDelete;
                chip.Click += WagonChipClick;
                MaterialDesignThemes.Wpf.PackIcon icon = new MaterialDesignThemes.Wpf.PackIcon();
                icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.TrainCarPassenger;
                chip.Icon = icon;
                chip.IconBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF485B83");

                WagonListBox.Items.Add(chip);
            }
            createAddNewWagonChip();
            
        }

        private void createAddNewWagonChip()
        {
            MaterialDesignThemes.Wpf.Chip newChip = new MaterialDesignThemes.Wpf.Chip();
            newChip.Content = "Dodaj vagon";
            newChip.HorizontalAlignment = HorizontalAlignment.Stretch;
            newChip.VerticalAlignment = VerticalAlignment.Stretch;
            newChip.Click += AddNewWagon;
            MaterialDesignThemes.Wpf.PackIcon addIcon = new MaterialDesignThemes.Wpf.PackIcon();
            addIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Add;
            newChip.Icon = addIcon;
            newChip.IconBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF485B83");
            WagonListBox.Items.Add(newChip);

        }


        private void AddNewWagon(object sender, RoutedEventArgs e)
        {
            bool? result = new AddNewWagonMessageBox().ShowDialog();

            if (result.Value)
            {
                selectedTrainWagons.Add(new Wagon(AddNewWagonMessageBox.WagonTypeSelected));
                addWagonChips();
                setNumberOfPassengersTxt();
            }
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            AddError.Visibility = Visibility.Hidden;
            if (trainNameTxt.Text == "")
            {
                AddError.Visibility = Visibility.Visible;
                AddError.Content = "Morate uneti naziv voza.";
                return;
            }
            if (selectedTrainWagons.Count == 0)
            {
                AddError.Visibility = Visibility.Visible;
                AddError.Content = "Voz mora imati barem jedan vagon.";
                return;
            }
            TrainRepository.updateTrain(selectedTrain, trainNameTxt.Text, selectedTrainWagons);
            Registration.notifier.ShowSuccess("Uspešno sačuvane izmene.");
            createTrainChips();
            addBtn.Visibility = Visibility.Hidden;
            saveBtn.Visibility = Visibility.Visible;
        }

        private void AddNewTrainEvent(object sender, RoutedEventArgs e)
        {
            AddError.Visibility = Visibility.Hidden;
            if (trainNameTxt.Text == "")
            {
                AddError.Visibility = Visibility.Visible;
                AddError.Content = "Morate uneti naziv voza.";
                return;
            }
            if (selectedTrainWagons.Count == 0)
            {
                AddError.Visibility = Visibility.Visible;
                AddError.Content = "Voz mora imati barem jedan vagon.";
                return;
            }
            Train trainExists = TrainRepository.FindTrainByName(trainNameTxt.Text);
            if (trainExists != null)
            {
                AddError.Visibility = Visibility.Visible;
                AddError.Content = "Voz sa istim nazivom već postoji.";
                return;
            }
            TrainRepository.addNewTrain(trainNameTxt.Text, selectedTrainWagons);
            Registration.notifier.ShowSuccess("Uspešno dodat novi voz.");
            createTrainChips();
            addBtn.Visibility = Visibility.Hidden;
            saveBtn.Visibility = Visibility.Visible;
        }
    }
}
