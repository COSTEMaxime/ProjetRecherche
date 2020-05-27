using System;
using System.Collections.Generic;
using System.Drawing;

namespace Model
{
    public class Person : IPosition
    {
        private Point _position;

        public Point Position
        {
            get => _position;
            set { _position = value; AsMoved = true; }
        }
        public string Name;

        public Stack<Point> Path { get; set; }
        public bool AsMoved { get; set; }
        public PersonTypes Type { get; set; }

        public Person(Point location, string name)
        {
            Position = location;
            Name = name;

            Path = new Stack<Point>();
        }

        public Room SelectDestination(List<Room> destinations)
        {
            Random rnd = new Random();
            int roomIndex = rnd.Next(destinations.Count);

            return destinations[roomIndex];
        }

        public bool WantToMove()
        {
            Random rnd = new Random();
            int waitingProbability = 6;
            return rnd.Next(10) > waitingProbability ? true : false;
        }

    }
}
