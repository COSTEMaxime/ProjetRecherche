using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Model
{
    public class EntityLoader
    {
        public List<List<Node>> grid { get; private set; }
        public List<Person> movableEntities { get; private set; }
        public List<Room> rooms { get; private set; }

        public void LoadFromFile(string path)
        {
            // Load from file

            string[] FileContent = File.ReadAllLines(path);

            List<List<Node>> tempGrid = new List<List<Node>>();
            List<Person> tempMoveable = new List<Person>();
            List<Room> tempRooms = new List<Room>();

            int lineNb = 0;
            foreach (string line in FileContent)
            {
                // Generate Walls & Air
                if (line[0] != '/')
                {
                    tempGrid.Add(CreateNodeRow(line, lineNb));
                }
                else
                {
                    // Create Rooms
                    if (line[1] == 'R')
                    {
                        string[] trimed = line.Substring(2).Split(',');
                        List<PersonTypes> allowed = new List<PersonTypes>();

                        foreach (string idPersonType in trimed[4].Split(';'))
                        {
                            allowed.Add((PersonTypes)int.Parse(idPersonType));
                        }

                        tempRooms.Add(new Room(new Point(int.Parse(trimed[0]), int.Parse(trimed[1])), trimed[2], int.Parse(trimed[3]), allowed));
                    }
                    else // Create Persons
                    {
                        string[] trimed = line.Substring(2).Split(',');
                        
                        Room mainRoom = null;

                        if (trimed.Length == 4)
                            mainRoom = tempRooms.Find(r => r.Name == trimed[3]);

                        // Creation of multiple persons
                        for (int i = 0; i < int.Parse(trimed[2]); i++)
                        {
                            string tempName = ((PersonTypes)int.Parse(line[1].ToString())).ToString() + (tempMoveable.Count + 1);

                            tempMoveable.Add(GeneratePerson(line[1].ToString(), trimed[0], trimed[1], tempName, mainRoom));
                        }
                    }
                }
                lineNb++;
            }
            grid = tempGrid;
            movableEntities = tempMoveable;
            rooms = tempRooms;
        }

        public List<Node> CreateNodeRow(string row, int rowNb)
        {
            List<Node> newRow = new List<Node>();

            for (int i = 0; i < row.Length; i++)
            {
                if (row[i] == '#')
                    newRow.Add(new Node(new Point(i, rowNb), WalkingDirection.NONE));
                else
                    newRow.Add(new Node(new Point(i, rowNb), (WalkingDirection)int.Parse(row[i].ToString())));
            }
            return newRow;
        }

        public Person GeneratePerson(string personType, string posX, string posY, string name, Room mainRoom)
        {
            Person newPerson = PersonFactory.CreatePerson((PersonTypes)int.Parse(personType), new Point(int.Parse(posX), int.Parse(posY)), name, mainRoom);
            if(mainRoom != null && newPerson.Position == mainRoom.Position)
                mainRoom.EnterRoom(newPerson);
            return newPerson;
        }
    }
}
