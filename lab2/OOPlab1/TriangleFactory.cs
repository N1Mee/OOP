using System;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Factory for creating Triangle shapes from two points.
    /// </summary>
    class TriangleFactory : IShapeFactory
    {
        public string Name => "Triangle";

        public Shape Create(Point start, Point end, Pen pen)
        {
            int dx = Math.Abs(end.X - start.X);
            int dy = Math.Abs(end.Y - start.Y);
            int side = Math.Min(dx, dy);

            int centerX = (start.X + end.X) / 2;
            int centerY = (start.Y + end.Y) / 2;

            return new Triangle(side, centerX, centerY, pen);
        }
    }
}

