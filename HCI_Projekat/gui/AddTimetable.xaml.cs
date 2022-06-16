using HCI_Projekat.model;
using HCI_Projekat.services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace HCI_Projekat.gui
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            var selectedFrom = TimetableViewManager.SelectedFromRoute;
            var selectedTo = TimetableViewManager.SelectedToRoute;
            this.from.Content = "Od: " + selectedFrom;
            this.to.Content = "Do: " + selectedTo;
            this.dayWeek.Items.Add(createComboBoxItem("Radni dan"));
            this.dayWeek.Items.Add(createComboBoxItem("Subota"));
            this.dayWeek.Items.Add(createComboBoxItem("Nedelja"));
            initCommands();
        }
        private void initCommands()
        {
            RoutedCommand newCmdSave = new RoutedCommand();
            newCmdSave.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            this.CommandBindings.Add(new CommandBinding(newCmdSave, AddTimetable));

            RoutedCommand newCmdReset = new RoutedCommand();
            newCmdReset.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control));
            this.CommandBindings.Add(new CommandBinding(newCmdReset, Cancel));
        }

        private ComboBoxItem createComboBoxItem(string content)
        {
            ComboBoxItem c = new ComboBoxItem();
            c.Content = content;
            c.Visibility = Visibility.Visible;
            c.Foreground = (Brush)(new BrushConverter().ConvertFrom("#FF485B83"));

            return c;
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

        private void AddTimetable(object sender, RoutedEventArgs e)
        {
            var selectedFrom = TimetableViewManager.SelectedFromRoute;
            var selectedTo = TimetableViewManager.SelectedToRoute;
            List<Timetable> t = TimetableService.GetAllTimetables();

            OverlapError.Visibility = Visibility.Hidden;
            Overlap2Error.Visibility = Visibility.Hidden;

            var selectedDay = dayWeek.SelectedItem;
            if (selectedDay == null) 
                DayError.Visibility = Visibility.Visible;
            else 
                DayError.Visibility = Visibility.Hidden;
   
            var fromTime = PresetTimePickerFrom.SelectedTime;
            if (fromTime == null) {
                FromError.Content = "Unesite vreme polaska.";
                FromError.Visibility = Visibility.Visible;
            }
            else FromError.Visibility = Visibility.Hidden;

            var toTime = PresetTimePickerTo.SelectedTime;
            if (toTime == null) ToError.Visibility = Visibility.Visible;
            else ToError.Visibility = Visibility.Hidden;

            

            if (selectedDay != null && fromTime != null && toTime != null)
            {
                if (toTime <= fromTime)
                {
                    FromError.Content = "Vreme polaska mora biti manje od vremena dolaska.";
                    FromError.Visibility = Visibility.Visible;
                }
                else
                {
                    FromError.Visibility = Visibility.Hidden;

                    ComboBoxItem item = (ComboBoxItem)dayWeek.SelectedItem;
                    string selectedDayValue = item.Content.ToString();
                    if (checkIfTimetableCanCreate(RouteService.FindRouteByAttr(selectedFrom, selectedTo), selectedDayValue, fromTime.Value, toTime.Value))
                    {
                        DayOfWeekTimetable d;
                        if (selectedDayValue.Equals("Radni dan"))
                        {
                            d = DayOfWeekTimetable.WORK;
                            t.Add(new Timetable(t[t.Count - 1].Id + 1,
                                RouteService.FindRouteByAttr(selectedFrom, selectedTo),
                                fromTime.Value, toTime.Value, DayOfWeekTimetable.WORK));
                        }
                        else if (selectedDayValue.Equals("Subota"))
                        {
                            d = DayOfWeekTimetable.SATURDAY;
                            t.Add(new Timetable(t[t.Count - 1].Id + 1,
                                RouteService.FindRouteByAttr(selectedFrom, selectedTo),
                                fromTime.Value, toTime.Value, DayOfWeekTimetable.SATURDAY));
                        }
                        else
                        {
                            d = DayOfWeekTimetable.SUNDAY;
                            t.Add(new Timetable(t[t.Count - 1].Id + 1,
                                RouteService.FindRouteByAttr(selectedFrom, selectedTo),
                                fromTime.Value, toTime.Value, DayOfWeekTimetable.SUNDAY));
                        }
                        notifier.ShowSuccess("Uspešno ste dodali novi red vožnje.");
                        this.Close();
                    }
                    else
                    {
                        OverlapError.Visibility = Visibility.Visible;
                        Overlap2Error.Visibility = Visibility.Visible;
                    }
                }
            }

        }

        private bool checkIfTimetableCanCreate(Route route, string selectedDay, DateTime fromTime, DateTime toTime)
        {
            return TimetableService.checkIfTimetableCanCreate(route, selectedDay, fromTime, toTime);
        }

        private void PresetTimePicker_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            

        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
