using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Room : IPosition
    {
        public Point Position { get; }
        public string Name;
        public int NbCurrentPeople;
        public int NbMaxPeople;


        public List<Person> Persons = new List<Person>();

        public Room(Point location, string name, int maxPeople)
        {
            Position = location;
            Name = name;
            NbMaxPeople = maxPeople;
            NbCurrentPeople = 0;
        }

        public void EnterRoom (Person person)
        {
            if (!Persons.Contains(person))
            {
                Persons.Add(person);
                NbCurrentPeople++;
                // TODO : Check MaxPeople & Trigger event ?
            }
        }

        public void LeaveRoom(Person person)
        {
            if (Persons.Remove(person))
                NbCurrentPeople--;
            // TODO : Trigger event if under the MaxPeople Threshold
        }
    }
}
