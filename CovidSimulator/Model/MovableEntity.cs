using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MovableEntity : Entity
    {
        public MovableEntity(Point location, WalkingDirection walkingDirection) : base(location, walkingDirection)
        {
            throw new NotImplementedException();
        }
    }
}
