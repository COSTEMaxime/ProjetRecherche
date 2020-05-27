using System.Drawing;

namespace Model
{
    public class Student : Person
    {
        public Student(Point location, string name) : base(location, name)
        {
            Type = PersonTypes.STUDENT;
        }
    }
}
