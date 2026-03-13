using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Base abstract shape that stores only geometric data.
    /// Rendering is delegated to separate renderer classes.
    /// </summary>
    abstract class Shape
    {
        /// <summary>
        /// Pen used to draw the shape.
        /// </summary>
        public Pen Pen { get; }

        protected Shape(Pen pen)
        {
            Pen = pen;
        }
    }
}
