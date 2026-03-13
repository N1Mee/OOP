using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Ellipse shape data. Rendering is done by EllipseRenderer.
    /// </summary>
    class Ellipse : Shape
    {
        /// <summary>
        /// Width of the ellipse.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Height of the ellipse.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// X coordinate of the ellipse center.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Y coordinate of the ellipse center.
        /// </summary>
        public int Y { get; }

        public Ellipse(int width, int height, int x, int y, Pen pen)
            : base(pen)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }
    }

}
