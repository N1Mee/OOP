using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Renderer interface for drawing a specific shape type.
    /// </summary>
    interface IShapeRenderer
    {
        /// <summary>
        /// Draws the given shape using provided graphics context.
        /// </summary>
        void Draw(Shape shape, Graphics g);
    }
}

