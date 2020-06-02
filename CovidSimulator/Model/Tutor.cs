using System.Drawing;

namespace Model
{
    public class Tutor : Person
    {
        public Tutor(Point location, string name, Room mainRoom) : base(location, name, mainRoom)
        {
            Type = PersonTypes.TUTOR;
        }
    }
}
