using System;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Factory for creating Ellipse shapes from two points.
    /// </summary>
    class EllipseFactory : IShapeFactory
    {
        public string Name => "Ellipse";

        public Shape Create(Point start, Point end, Pen pen)
        {
            int width = Math.Abs(end.X - start.X);
            int height = Math.Abs(end.Y - start.Y);

            int centerX = (start.X + end.X) / 2;
            int centerY = (start.Y + end.Y) / 2;

            return new Ellipse(width, height, centerX, centerY, pen);
        }
    }
}

