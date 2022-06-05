using HCI_Projekat.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.database
{
    public static class TrainRepository
    {

        private static List<Train> trains = new List<Train>();

        static TrainRepository()
        {
            Wagon wagon1 = new Wagon(Wagon.WagonType.SMALL);
            Wagon wagon2 = new Wagon(Wagon.WagonType.SMALL);
            Wagon wagon3 = new Wagon(Wagon.WagonType.MEDIUM);
            Wagon wagon4 = new Wagon(Wagon.WagonType.MEDIUM);
            Wagon wagon5 = new Wagon(Wagon.WagonType.LARGE);
            Wagon wagon6 = new Wagon(Wagon.WagonType.LARGE);

            Train train1 = new Train("741", new List<Wagon>() { wagon1,wagon3});
            Train train2 = new Train("2401", new List<Wagon>() { wagon2,wagon4});
            Train train3 = new Train("743", new List<Wagon>() { wagon5,wagon6});

            trains.Add(train1);
            trains.Add(train2);
            trains.Add(train3);
            trains.Add(train3);
            trains.Add(train3);
            trains.Add(train3);
            trains.Add(train3);
            trains.Add(train3);
            trains.Add(train3);
            trains.Add(train3);
            trains.Add(train3);
            trains.Add(train3);
            trains.Add(train3);
            trains.Add(train3);
        }


        public static Train FindTrainByName(string name)
        {
            var train = trains.Find(x => x.Name.Equals(name));
            return train;
        }

        public static List<Train> getAllTrains()
        {
            return trains;
        }

        public static Train addNewTrain(string name, List<Wagon> wagons)
        {
            Train train = new Train(name, wagons);
            trains.Add(train);
            return train;
        }

        internal static void removeTrainByName(string trainName)
        {
            trains.RemoveAll(x => x.Name.Equals(trainName));
        }

        internal static void updateTrain(string oldTrainName, string newTrainName, List<Wagon> selectedTrainWagons)
        {
            Train train = trains.Find(x => x.Name == oldTrainName);
            train.Name = newTrainName;
            train.Wagons = selectedTrainWagons;
        }
    }
}
