using System.Drawing;

namespace View
{
    public class DisplayableElement
    {
        public static int SIZE = 32;

        public Point position { get; private set; }
        public Color? color { get; private set; } = null;
        public Image image { get; private set; } = null;

        public DisplayableElement(Point position, Color color)
        {
            this.position = position;
            this.color = color;
        }

        public DisplayableElement(Point position, Image image)
        {
            this.position = position;
            this.image = image;
        }
    }
}
