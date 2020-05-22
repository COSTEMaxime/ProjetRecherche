using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View;

namespace Controller
{
    public class EntityDisplayConverter
    {
        static DisplayableElement ToDisplayableElement(Entity source)
        {
            Color color = Color.White;
            if (source is Room)
            {
                color = Color.Red;
            } else if (source is MovableEntity)
            {
                color = Color.Blue;
            }

            return new DisplayableElement(source.position, color);
        }

        public static List<DisplayableElement> ToDisplayableElements(List<Entity> source)
        {
            List<DisplayableElement> displayableElements = new List<DisplayableElement>();
            foreach (Entity entity in source)
            {
                displayableElements.Add(ToDisplayableElement(entity));
            }

            return displayableElements;
        }
    }
}
