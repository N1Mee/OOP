using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Factory for creating Rectangle shapes from two points.
    /// </summary>
    class RectangleFactory : IShapeFactory
    {
        public string Name => "Rectangle";

        public Shape Create(Point start, Point end, Pen pen)
        {
            int x = System.Math.Min(start.X, end.X);
            int y = System.Math.Min(start.Y, end.Y);
            int width = System.Math.Abs(end.X - start.X);
            int height = System.Math.Abs(end.Y - start.Y);

            return new Rectangle(width, height, x, y, pen);
        }
    }
}

