using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Renderer for Circle shapes.
    /// </summary>
    class CircleRenderer : IShapeRenderer
    {
        public void Draw(Shape shape, Graphics g)
        {
            Circle circle = (Circle)shape;
            float diameter = 2 * circle.Radius;
            float x = circle.X - circle.Radius;
            float y = circle.Y - circle.Radius;
            g.DrawEllipse(circle.Pen, x, y, diameter, diameter);
        }
    }
}

