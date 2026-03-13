using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Collection of shapes that can be rendered using renderer registry.
    /// </summary>
    class ShapeList
    {
        private readonly List<Shape> list = new List<Shape>();

        /// <summary>
        /// Adds a new shape to the list.
        /// </summary>
        public void Add(Shape shape)
        {
            list.Add(shape);
        }

        /// <summary>
        /// Draws all shapes using the ShapeRendererRegistry.
        /// </summary>
        public void Draw(Graphics g)
        {
            foreach (var item in list)
            {
                ShapeRendererRegistry.DrawShape(item, g);
            }
        }
    }
}
