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
    /// Interaction logic for EditTimetable.xaml
    /// </summary>
    public partial class EditTimetable : Window
    {
        public EditTimetable()
        {
            InitializeComponent();
            var selectedFrom = TimetableViewManager.SelectedFromRoute;
            var selectedTo = TimetableViewManager.SelectedToRoute;
            this.from.Content = "Od: " + selectedFrom;
            this.to.Content = "Do: " + selectedTo;
            if (TimetableViewManager.SelectedTimetable.weekDay.Equals(DayOfWeekTimetable.WORK))
            {
                this.forTimetable.Content = "* za radni dan";
            }
            else if (TimetableViewManager.SelectedTimetable.weekDay.Equals(DayOfWeekTimetable.SATURDAY))
            {
                this.forTimetable.Content = "* za subotu";
            }
            else {
                this.forTimetable.Content = "* za nedelju";
            }
            this.PresetTimePickerFrom.SelectedTime = TimetableViewManager.SelectedTimetable.StartDateTime;
            this.PresetTimePickerTo.SelectedTime = TimetableViewManager.SelectedTimetable.EndDateTime;

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

        private void Edit(object sender, RoutedEventArgs e)
        {
            var selectedFrom = TimetableViewManager.SelectedFromRoute;
            var selectedTo = TimetableViewManager.SelectedToRoute;
            List<Timetable> t = TimetableService.GetAllTimetables();

            OverlapError.Visibility = Visibility.Hidden;
            Overlap2Error.Visibility = Visibility.Hidden;

            var fromTime = PresetTimePickerFrom.SelectedTime;
            if (fromTime == null)
            {
                FromError.Content = "Unesite vreme polaska.";
                FromError.Visibility = Visibility.Visible;
            }
            else FromError.Visibility = Visibility.Hidden;

            var toTime = PresetTimePickerTo.SelectedTime;
            if (toTime == null) ToError.Visibility = Visibility.Visible;
            else ToError.Visibility = Visibility.Hidden;



            if (fromTime != null && toTime != null)
            {
                if (toTime <= fromTime)
                {
                    FromError.Content = "Vreme polaska mora biti manje od vremena dolaska.";
                    FromError.Visibility = Visibility.Visible;
                }
                else
                {
                    FromError.Visibility = Visibility.Hidden;

                    if (checkIfTimetableCanEdit(RouteService.FindRouteByAttr(selectedFrom, selectedTo), 
                        TimetableViewManager.SelectedTimetable.weekDay, fromTime.Value, toTime.Value))
                    {
                        TimetableService.Edit(TimetableViewManager.SelectedTimetable, 
                               RouteService.FindRouteByAttr(selectedFrom, selectedTo),
                               fromTime.Value, toTime.Value, TimetableViewManager.SelectedTimetable.weekDay);

                        notifier.ShowSuccess("Uspešno ste izmenili izabrani red vožnje.");
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

        private bool checkIfTimetableCanEdit(Route route, DayOfWeekTimetable selectedDay, DateTime fromTime, DateTime toTime)
        {
            return TimetableService.checkIfTimetableCanEdit(route, selectedDay, fromTime, toTime, TimetableViewManager.SelectedTimetable);
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
