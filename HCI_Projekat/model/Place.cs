namespace HCI_Projekat.model
{
    public class Place
    {
        public string Name { get; set; }
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