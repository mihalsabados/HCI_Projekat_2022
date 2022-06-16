using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.model
{
    public class Route
    {
        public string Name { get; set; }
        public List<Place> places { get; set; }
        public string FromTo { get; set; }
        public Train Train { get; set; }
        public Train RouteTrain { get; set; }
        public Route(string name, Train train, List<Place> places)
        {
            RouteTrain = train;
            Name = name;
            this.places = places;
            this.FromTo = places.ElementAt(0).Name + " - " + places.ElementAt(places.Count - 1).Name;
            this.Train = train;
        }
        public Route() { }

        public Route(List<Place> places)
        {
            this.places = places;
        }

    }

}
