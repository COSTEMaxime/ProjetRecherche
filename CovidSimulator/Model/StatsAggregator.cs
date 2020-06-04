using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Model
{
    public class StatsAggregator
    {
        private List<Tuple<Person, Point>> people;
        private IDictionary<Room, int> rooms;


        public StatsAggregator()
        {
            people = new List<Tuple<Person, Point>>();
            rooms = new Dictionary<Room, int>();
        }

        public void AddPeopleTooClose(Person person, Person otherPerson)
        {
            people.Add(new Tuple<Person, Point>(person, person.Position));
            people.Add(new Tuple<Person, Point>(otherPerson, otherPerson.Position));
        }

        public void AddOvercrowdedRoom(Room room)
        {
            if (rooms.ContainsKey(room))
            {
                rooms[room]++;
            }
            else
            {
                rooms.Add(room, 1);
            }
        }

        public void consoleDisplay()
        {
            Console.WriteLine("Contacts count : {0}", people.Count());
            Console.WriteLine("People with high number of contacts : ");

            var temp = new Dictionary<Person, int>();
            foreach (var contact in people)
            {
                if (temp.ContainsKey(contact.Item1))
                {
                    temp[contact.Item1]++;
                }
                else
                {
                    temp.Add(contact.Item1, 1);
                }
            }

            // print 5 entry max
            int maxPrint = 100;
            int count = 0;

            foreach (var element in temp.OrderBy(x => x.Value).Reverse())
            {
                if (++count > maxPrint) { break; }

                //Console.WriteLine("{0}:\t{1}", element.Key.Name, element.Value);
                Console.WriteLine("{0}", element.Value); // To put raw data in stats
            }

            int infectedCount = 0;
            foreach (var element in temp)
            {
                if (element.Key.AsVirus) { infectedCount++; }
            }

            Console.WriteLine("Total person with at least one contact: {0}", count);

            Console.WriteLine();
            Console.WriteLine("Number of people infected at the end of the simulation : {0}", infectedCount);
            Console.WriteLine();

            Console.WriteLine("The following rooms where overcrowded :");
            foreach (var element in rooms)
            {
                Console.WriteLine("{0} was overcrowded for {1} ticks (Authorized : {2}, max : {3})", element.Key.Name, element.Value, element.Key.NbMaxPeople, element.Key.AllTimeMax);
            }
        }

        public IDictionary<Point, int> GetPositions()
        {
            IDictionary<Point, int> temp = new Dictionary<Point, int>();
            foreach (var pos in people)
            {
                if (temp.ContainsKey(pos.Item2))
                {
                    temp[pos.Item2]++;
                }
                else
                {
                    temp.Add(pos.Item2, 1);
                }
            }

            return temp;
        }
    }
}
