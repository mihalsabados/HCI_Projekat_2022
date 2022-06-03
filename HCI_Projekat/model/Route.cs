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

        public Route(string name, List<Place> places)
        {
            Name = name;
            this.places = places;
            this.FromTo = places.ElementAt(0).Name + " - " + places.ElementAt(places.Count - 1).Name;
        }
        public Route() { }

    }

}
