using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Square shape data. Rendering is done by SquareRenderer.
    /// </summary>
    class Square : Shape
    {
        /// <summary>
        /// Side length of the square.
        /// </summary>
        public int Side { get; }

        /// <summary>
        /// X coordinate of the top-left corner.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Y coordinate of the top-left corner.
        /// </summary>
        public int Y { get; }

        public Square(int side, int x, int y, Pen pen)
            : base(pen)
        {
            Side = side;
            X = x;
            Y = y;
        }
    }
}
