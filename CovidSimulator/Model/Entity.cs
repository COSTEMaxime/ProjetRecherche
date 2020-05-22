using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public abstract class Entity
    {
        public Point position { get; set; }
        public float G { get; set; }
        public float H { get; set; }
        public float F { get; set; }

        public Entity parent { get; set; }
        public WalkingDirection walkingDirection { get; set; }

        public Entity(Point position, WalkingDirection walkingDirection)
        {
            this.position = position;
            this.walkingDirection = walkingDirection;

            G = H = F = 0;
            parent = null;
        }

        public bool CanWalk(Entity dest)
        {
            switch (dest.walkingDirection)
            {
                case WalkingDirection.NONE:
                    return false;
                case WalkingDirection.ALL:
                    return true;
                case WalkingDirection.DOWN:
                    return dest.position.Y > this.position.Y;
                case WalkingDirection.UP:
                    return dest.position.Y < this.position.Y;
                case WalkingDirection.LEFT:
                    return dest.position.X > this.position.X;
                case WalkingDirection.RIGHT:
                    return dest.position.X < this.position.X;
                default:
                    throw new NotImplementedException("Walking Direction not implemented : " + dest.walkingDirection);
            }
        }
    }
}
