using System;
using System.Drawing;

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
            // can't walk on a Node if our current direction is the opposite of the target walkingDirection
            if ((dest.walkingDirection == WalkingDirection.UP && dest.Position.Y > Position.Y) ||
                (dest.walkingDirection == WalkingDirection.DOWN && dest.Position.Y < Position.Y) ||
                (dest.walkingDirection == WalkingDirection.LEFT && dest.Position.X > Position.X) ||
                (dest.walkingDirection == WalkingDirection.RIGHT && dest.Position.X < Position.X))
            {
                return false;
            }


            // can't walk backwards
            // eg: if walkingDirection == UP
            // we can go up, left or right
            switch (walkingDirection)
            {
                case WalkingDirection.NONE:
                    return false;
                case WalkingDirection.ALL:
                    return true;
                case WalkingDirection.UP:
                    return !(dest.Position.Y > this.Position.Y);
                case WalkingDirection.DOWN:
                    return !(dest.Position.Y < this.Position.Y);
                case WalkingDirection.LEFT:
                    return !(dest.Position.X > this.Position.X);
                case WalkingDirection.RIGHT:
                    return !(dest.Position.X < this.Position.X);
                default:
                    throw new NotImplementedException("Walking Direction not implemented : " + dest.walkingDirection);
            }
        }

        public void Reset()
        {
            G = H = F = 0;
            parent = null;
        }
    }
}
