using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class StatsAggregator
    {
        private List<Tuple<string, Point>> people;
        private IDictionary<Room, int> rooms;


        public StatsAggregator()
        {
            people = new List<Tuple<string, Point>>();
            rooms = new Dictionary<Room, int>();
        }

        public void AddPeopleTooClose(Person person, Person otherPerson)
        {
            people.Add(new Tuple<string, Point>(person.Name, person.Position));
            people.Add(new Tuple<string, Point>(otherPerson.Name, otherPerson.Position));
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

            var temp = new Dictionary<string, int>();
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
            int maxPrint = 5;
            int count = 0;

            foreach (var element in temp.OrderBy(x => x.Value).Reverse())
            {
                if (++count > maxPrint) { break; }

                Console.WriteLine("{0}:\t{1}", element.Key, element.Value);
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("The following rooms where overcrowded :");
            foreach (var element in rooms)
            {
                Console.WriteLine("{0} was overcrowded for {1} ticks (Authorized : {2}, max = {3}", element.Key.Name, element.Value, element.Key.NbMaxPeople, element.Key.AllTimeMax) ;
            }
        }
    }
}
