using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Rectangle shape data. Rendering is done by RectangleRenderer.
    /// </summary>
    class Rectangle : Shape
    {
        /// <summary>
        /// Width of the rectangle.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Height of the rectangle.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// X coordinate of the top-left corner.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Y coordinate of the top-left corner.
        /// </summary>
        public int Y { get; }

        public Rectangle(int width, int height, int x, int y, Pen pen)
            : base(pen)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }
    }
}
