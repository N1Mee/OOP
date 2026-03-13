using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Renderer for Square shapes.
    /// </summary>
    class SquareRenderer : IShapeRenderer
    {
        public void Draw(Shape shape, Graphics g)
        {
            Square square = (Square)shape;
            g.DrawRectangle(square.Pen, square.X, square.Y, square.Side, square.Side);
        }
    }
}

