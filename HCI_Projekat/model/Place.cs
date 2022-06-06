using System.ComponentModel;

namespace HCI_Projekat.model
{
    public class Place : INotifyPropertyChanged, IEditableObject
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


        #region IEditableObject

        private Place backupCopy;
        private bool inEdit;

        public void BeginEdit()
        {
            if (inEdit) return;
            inEdit = true;
            backupCopy = this.MemberwiseClone() as Place;
        }

        public void CancelEdit()
        {
            if (!inEdit) return;
            inEdit = false;
            this.Name = backupCopy.Name;
        }

        public void EndEdit()
        {
            if (!inEdit) return;
            inEdit = false;
            backupCopy = null;
        }

        #endregion


        private string _name;

        public string Name 
        { 
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public Place() { }

        public Place(string name, double latitude, double longitude)
        {
            Name = name;
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}