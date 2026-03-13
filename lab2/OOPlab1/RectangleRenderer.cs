using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Renderer for Rectangle shapes.
    /// </summary>
    class RectangleRenderer : IShapeRenderer
    {
        public void Draw(Shape shape, Graphics g)
        {
            Rectangle rect = (Rectangle)shape;
            g.DrawRectangle(rect.Pen, rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}

