using System;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Factory for creating Circle shapes from two points.
    /// </summary>
    class CircleFactory : IShapeFactory
    {
        public string Name => "Circle";

        public Shape Create(Point start, Point end, Pen pen)
        {
            int dx = end.X - start.X;
            int dy = end.Y - start.Y;
            int radius = (int)(Math.Sqrt(dx * dx + dy * dy));

            return new Circle(radius, start.X, start.Y, pen);
        }
    }
}

