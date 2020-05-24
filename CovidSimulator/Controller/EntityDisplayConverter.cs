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
        static DisplayableElement ToDisplayableElement(IPosition source)
        {
            Color color = Color.White;
            if (source is Room)
            {
                color = Color.Red;
            } else if (source is Person)
            {
                color = Color.Blue;
            }

            return new DisplayableElement(source.Position, color);
        }

        public static List<DisplayableElement> ToDisplayableElements(List<Node> source)
        {
            List<DisplayableElement> displayableElements = new List<DisplayableElement>();
            foreach (Node entity in source)
            {
                displayableElements.Add(ToDisplayableElement(entity));
            }

            return displayableElements;
        }
    }
}
