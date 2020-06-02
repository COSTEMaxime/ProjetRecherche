using System.Collections.Generic;
using System.Drawing;

namespace Model
{
    public class Room : IPosition
    {
        public Point Position { get; }

        public string Name;
        public int NbCurrentPeople;
        public int NbMaxPeople;
        public int AllTimeMax { get; private set; }

        public List<PersonTypes> Allowed;

        public List<Person> Persons = new List<Person>();

        public Room(Point location, string name, int maxPeople, List<PersonTypes> allowed)
        {
            Position = location;
            Name = name;
            NbMaxPeople = maxPeople;
            NbCurrentPeople = 0;
            Allowed = allowed;
        }

        public void EnterRoom(Person person)
        {
            if (!Persons.Contains(person))
            {
                Persons.Add(person);
                NbCurrentPeople++;
                person.EnterRoom(this);
                if (NbCurrentPeople > AllTimeMax)
                {
                    AllTimeMax = NbCurrentPeople;
                }
            }
        }

        public void LeaveRoom(Person person)
        {
            if (Persons.Remove(person))
            {
                person.LeaveRoom();
                NbCurrentPeople--;
            }
        }
    }
}
