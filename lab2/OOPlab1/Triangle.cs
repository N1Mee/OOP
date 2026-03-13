using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Equilateral triangle shape data. Rendering is done by TriangleRenderer.
    /// </summary>
    class Triangle : Shape
    {
        /// <summary>
        /// Side length of the triangle.
        /// </summary>
        public int Side { get; }

        /// <summary>
        /// X coordinate of the triangle center.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Y coordinate of the triangle center.
        /// </summary>
        public int Y { get; }

        public Triangle(int side, int x, int y, Pen pen)
            : base(pen)
        {
            Side = side;
            X = x;
            Y = y;
        }
    }
}
