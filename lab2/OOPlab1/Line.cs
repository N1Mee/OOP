using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Line shape data. Rendering is done by LineRenderer.
    /// </summary>
    class Line : Shape
    {
        /// <summary>
        /// X coordinate of the start point.
        /// </summary>
        public int X1 { get; }

        /// <summary>
        /// Y coordinate of the start point.
        /// </summary>
        public int Y1 { get; }

        /// <summary>
        /// X coordinate of the end point.
        /// </summary>
        public int X2 { get; }

        /// <summary>
        /// Y coordinate of the end point.
        /// </summary>
        public int Y2 { get; }

        public Line(int x1, int y1, int x2, int y2, Pen pen)
            : base(pen)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }
    }

}
