using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HCI_Projekat.model
{
    public class DataGridRoute : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public Route Route { get; set; }

        private string _trainName;
        public string TrainName
        {
            get
            {
                return _trainName;
            }
            set
            {
                if (value != _trainName)
                {
                    _trainName = value;
                    OnPropertyChanged("TrainName");
                }
            }
        }


        private string _startStation;
        public string StartStation
        {
            get
            {
                return _startStation;
            }
            set
            {
                if (value != _startStation)
                {
                    _startStation = value;
                    OnPropertyChanged("StartStation");
                }
            }
        }

        private string _endStation;
        public string EndStation
        {
            get
            {
                return _endStation;
            }
            set
            {
                if (value != _endStation)
                {
                    _endStation = value;
                    OnPropertyChanged("EndStation");
                }
            }
        }


        private int _numInnerStations;
        public int NumInnerStations
        {
            get
            {
                return _numInnerStations;
            }
            set
            {
                if (value != _numInnerStations)
                {
                    _numInnerStations = value;
                    OnPropertyChanged("NumInnerStations");
                }
            }
        }

        public DataGridRoute() { }

        public DataGridRoute(Route route) 
        {
            Route = route;
            StartStation = route.places[0].Name;
            EndStation = route.places[^1].Name;
            NumInnerStations = route.places.Count - 2;
            TrainName = route.RouteTrain.Name;
        }

    }
}
