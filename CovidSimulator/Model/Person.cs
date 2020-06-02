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
        public Room MainRoom { get; set; }
        public Room CurrentRoom { get; set; }

        private Random rnd;

        public Person(Point location, string name, Room mainRoom = null)
        {
            Position = location;
            Name = name;
            MainRoom = mainRoom;

            Path = new Stack<Point>();

            rnd = new Random(Guid.NewGuid().GetHashCode());
        }

        public Room SelectDestination(List<Room> destinations)
        {
            if (CurrentRoom != MainRoom)
                return MainRoom;

            destinations.Remove(CurrentRoom);
            int roomIndex = rnd.Next(destinations.Count);

            return destinations[roomIndex];
        }

        public void EnterRoom(Room currentRoom)
        {
            CurrentRoom = currentRoom;
        }

        public void LeaveRoom()
        {
            CurrentRoom = null;
        }

        public bool WantToMove()
        {
            int waitingProbability;

            if (CurrentRoom == MainRoom)
                waitingProbability = 998;
            else
                waitingProbability = 849;

            return rnd.Next(1000) > waitingProbability ? true : false;
        }

    }
}
