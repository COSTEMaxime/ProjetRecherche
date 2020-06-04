using System.Drawing;

namespace Model
{
    public class HeightMapNode : IPosition
    {
        public Point Position { get; }
        public int ContactCount { get; private set; }
        static public int MaxCountatCount { get; set; } = 0;

        public HeightMapNode(Point position, int contactCount = 0)
        {
            Position = position;
            ContactCount = contactCount;

            if (contactCount > MaxCountatCount)
            {
                MaxCountatCount = contactCount;
            }
        }
    }
}
