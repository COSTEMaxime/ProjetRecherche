using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Person : IPosition
    {
        
        public Point Position { get; set; }
        public string Name;


        public Person(Point location, string name)
        {
            Position = location;
            Name = name;
        }
    }
}
