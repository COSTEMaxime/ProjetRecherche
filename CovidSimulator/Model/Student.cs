using System.Drawing;

namespace Model
{
    public class Student : Person
    {
        public Student(Point location, string name, Room mainRoom) : base(location, name, mainRoom)
        {
            Type = PersonTypes.STUDENT;
        }
    }
}
