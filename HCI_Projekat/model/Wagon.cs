using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.model
{
    public class Wagon
    {
        public List<List<int>> Seats { get; set; }

        public WagonType wagonType { get; set; }

        public enum WagonType { SMALL = 8, MEDIUM = 10, LARGE = 12 }

        public Wagon(WagonType wagonType)
        {
            this.Seats = new List<List<int>>();
            for(int i=0; i< (int)wagonType; i++)
            {
                this.Seats.Add(new List<int>() { 0, 0, 0, 0 });
            }
            this.wagonType = wagonType;
        }
    }
}
