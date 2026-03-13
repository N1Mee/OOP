using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Circle shape data. Rendering is done by CircleRenderer.
    /// </summary>
    class Circle : Shape
    {
        /// <summary>
        /// Radius of the circle.
        /// </summary>
        public int Radius { get; }

        /// <summary>
        /// X coordinate of the circle center.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Y coordinate of the circle center.
        /// </summary>
        public int Y { get; }

        public Circle(int radius, int x, int y, Pen pen)
            : base(pen)
        {
            Radius = radius;
            X = x;
            Y = y;
        }
    }
}
