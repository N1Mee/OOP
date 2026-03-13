using System;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Renderer for Ellipse shapes.
    /// </summary>
    class EllipseRenderer : IShapeRenderer
    {
        public void Draw(Shape shape, Graphics g)
        {
            Ellipse ellipse = (Ellipse)shape;

            float x = ellipse.X - ellipse.Width / 2f;
            float y = ellipse.Y - ellipse.Height / 2f;

            g.DrawEllipse(ellipse.Pen, x, y, ellipse.Width, ellipse.Height);
        }
    }
}

