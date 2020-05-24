using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Node : IPosition
    {
        public Point Position { get; set; }
        public float G { get; set; }
        public float H { get; set; }
        public float F { get; set; }

        public Node parent { get; set; }
        public WalkingDirection walkingDirection { get; set; }

        public Node(Point position, WalkingDirection walkingDirection)
        {
            this.Position = position;
            this.walkingDirection = walkingDirection;

            G = H = F = 0;
            parent = null;
        }

        public bool CanWalk(Node dest)
        {
            switch (dest.walkingDirection)
            {
                case WalkingDirection.NONE:
                    return false;
                case WalkingDirection.ALL:
                    return true;
                case WalkingDirection.DOWN:
                    return dest.Position.Y > this.Position.Y;
                case WalkingDirection.UP:
                    return dest.Position.Y < this.Position.Y;
                case WalkingDirection.LEFT:
                    return dest.Position.X > this.Position.X;
                case WalkingDirection.RIGHT:
                    return dest.Position.X < this.Position.X;
                default:
                    throw new NotImplementedException("Walking Direction not implemented : " + dest.walkingDirection);
            }
        }
    }
}
