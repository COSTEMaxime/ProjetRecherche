using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class EntityLoader
    {

        string filePath = "TextFile1.txt";

        public List<List<Node>> grid { get; private set; }
        public List<Person> movableEntities { get; private set; }
        public List<Room> rooms { get; private set; }

        public void LoadFromFile(string path)
        {
            // Load from file

            string[] FileContent = File.ReadAllLines(filePath);

            List<List<Node>> tempGrid = new List<List<Node>>();
            List<Person> tempMoveable = new List<Person>();
            List<Room> tempRooms = new List<Room>();

            int lineNb = 0;
            foreach (string line in FileContent)
            {
                // Generate Walls & Air
                if (line[0] != '/')
                {
                    List<Node> tempList = new List<Node>();

                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == '#')
                            tempList.Add(new Node(new Point(i, lineNb), WalkingDirection.NONE));
                        else
                            tempList.Add(new Node(new Point(i, lineNb), (WalkingDirection) int.Parse(line[i].ToString())));
                    }
                    tempGrid.Add(tempList);
                }
                else
                {

                    if (line[1] == 'R')
                    {
                        string[] trimed = line.Substring(2).Split(',');
                        tempRooms.Add(new Room(new Point(int.Parse(trimed[1]), int.Parse(trimed[0])), trimed[2], int.Parse(trimed[3])));

                    }
                    else
                    {
                        string[] trimed = line.Substring(2).Split(',');
                        tempMoveable.Add(PersonFactory.CreatePerson((PersonTypes) int.Parse(line[1].ToString()), new Point(int.Parse(trimed[1]), int.Parse(trimed[0])), trimed[2]));
                    }
                }
                lineNb++;
            }
            grid = tempGrid;
            movableEntities = tempMoveable;
            rooms = tempRooms;
        }
    }
}
