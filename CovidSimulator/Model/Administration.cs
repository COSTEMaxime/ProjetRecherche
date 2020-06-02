using System.Drawing;

namespace Model
{
    public class Administration : Person
    {
        public Administration(Point location, string name, Room mainRoom) : base(location, name, mainRoom)
        {
            Type = PersonTypes.ADMINISTRATION;
        }
    }
}
