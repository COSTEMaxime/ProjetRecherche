using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View
{
    public class DisplayableElement
    {
        public static int SIZE = 32;

        public Point position { get; private set; }
        public Color color { get; private set; }

        public DisplayableElement(Point position, Color color)
        {
            this.position = position;
            this.color = color;
        }
    }
}
