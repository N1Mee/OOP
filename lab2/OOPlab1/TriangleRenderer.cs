using System;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Renderer for Triangle shapes.
    /// </summary>
    class TriangleRenderer : IShapeRenderer
    {
        public void Draw(Shape shape, Graphics g)
        {
            Triangle triangle = (Triangle)shape;

            float side = triangle.Side;
            float height = (float)(Math.Sqrt(3) / 2 * side);

            PointF top = new PointF(triangle.X, triangle.Y - 2 / 3f * height);
            PointF left = new PointF(triangle.X - side / 2, triangle.Y + 1 / 3f * height);
            PointF right = new PointF(triangle.X + side / 2, triangle.Y + 1 / 3f * height);

            PointF[] points = { top, left, right };
            g.DrawPolygon(triangle.Pen, points);
        }
    }
}

