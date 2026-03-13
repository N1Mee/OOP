using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Factory for creating Line shapes from two points.
    /// </summary>
    class LineFactory : IShapeFactory
    {
        public string Name => "Line";

        public Shape Create(Point start, Point end, Pen pen)
        {
            return new Line(start.X, start.Y, end.X, end.Y, pen);
        }
    }
}

