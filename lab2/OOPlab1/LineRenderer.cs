using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Renderer for Line shapes.
    /// </summary>
    class LineRenderer : IShapeRenderer
    {
        public void Draw(Shape shape, Graphics g)
        {
            Line line = (Line)shape;
            g.DrawLine(line.Pen, line.X1, line.Y1, line.X2, line.Y2);
        }
    }
}

