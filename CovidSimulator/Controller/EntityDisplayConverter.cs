﻿using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using View;

namespace Controller
{
    public class EntityDisplayConverter
    {
        static DisplayableElement ToDisplayableElement(IPosition source, Color? forceColor)
        {
            Color? color = null;
            Image image = null;

            if (forceColor != null)
            {
                color = forceColor;
            }
            else if (source is Room)
            {
                color = Color.Red;
            }
            else if (source is Person)
            {
                color = Color.Blue;
            }
            else if (source is HeightMapNode)
            {
                HeightMapNode source_ = source as HeightMapNode;

                color = Color.Red;
                if (source_.ContactCount < HeightMapNode.MaxCountatCount * 0.25)
                {
                    color = Color.Green;
                }
                else if (source_.ContactCount < HeightMapNode.MaxCountatCount * 0.50)
                {
                    color = Color.Yellow;
                }
                else if (source_.ContactCount < HeightMapNode.MaxCountatCount * 0.75)
                {
                    color = Color.Orange;
                }
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
                        image = Image.FromFile("../../../Assets/left_16.png");
                        break;
                    case WalkingDirection.RIGHT:
                        image = Image.FromFile("../../../Assets/right_16.png");
                        break;
                    case WalkingDirection.UP:
                        image = Image.FromFile("../../../Assets/up_16.png");
                        break;
                    case WalkingDirection.DOWN:
                        image = Image.FromFile("../../../Assets/down_16.png");
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

        public static List<DisplayableElement> ToDisplayableElements(List<IPosition> source, Color? forceColor = null)
        {
            List<DisplayableElement> displayableElements = new List<DisplayableElement>();
            foreach (IPosition position in source)
            {
                displayableElements.Add(ToDisplayableElement(position, forceColor));
            }

            return displayableElements;
        }
    }
}
