using System;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Factory for creating Square shapes from two points.
    /// </summary>
    class SquareFactory : IShapeFactory
    {
        public string Name => "Square";

        public Shape Create(Point start, Point end, Pen pen)
        {
            int dx = Math.Abs(end.X - start.X);
            int dy = Math.Abs(end.Y - start.Y);
            int side = Math.Min(dx, dy);

            int x = start.X;
            int y = start.Y;

            return new Square(side, x, y, pen);
        }
    }
}

