using Model;
using System.Collections.Generic;
using System.Drawing;
using View;

namespace Controller
{
    public class EntityDisplayConverter
    {
        static DisplayableElement ToDisplayableElement(IPosition source)
        {
            Color? color = null;
            Image image = null;

            if (source is Room)
            {
                color = Color.Red;
            }
            else if (source is Person)
            {
                color = Color.Blue;
            }
            else
            {
                switch (((Node)source).walkingDirection)
                {
                    case WalkingDirection.ALL:
                        color = Color.White;
                        break;
                    case WalkingDirection.NONE:
                        color = Color.Black;
                        break;
                    case WalkingDirection.LEFT:
                        image = Image.FromFile("../../../Assets/left.png");
                        break;
                    case WalkingDirection.RIGHT:
                        image = Image.FromFile("../../../Assets/right.png");
                        break;
                    case WalkingDirection.UP:
                        image = Image.FromFile("../../../Assets/up.png");
                        break;
                    case WalkingDirection.DOWN:
                        image = Image.FromFile("../../../Assets/down.png");
                        break;
                }
            }

            if (image != null)
            {
                return new DisplayableElement(source.Position, image);
            }
            else
            {
                return new DisplayableElement(source.Position, (Color)color);
            }
        }

        public static List<DisplayableElement> ToDisplayableElements(List<IPosition> source)
        {
            List<DisplayableElement> displayableElements = new List<DisplayableElement>();
            foreach (IPosition position in source)
            {
                displayableElements.Add(ToDisplayableElement(position));
            }

            return displayableElements;
        }
    }
}
