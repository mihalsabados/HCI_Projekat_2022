using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.model
{
    public class Train
    {

        public string Name { get; set; }

        public List<Wagon> Wagons { get; set; }

        public Train()
        {
            this.Wagons = new List<Wagon>();
        }

        public Train(string name, List<Wagon> wagons)
        {
            this.Name = name;
            this.Wagons = wagons;
        }

    }
}
