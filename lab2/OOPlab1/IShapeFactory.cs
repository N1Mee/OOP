using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Factory interface for creating shapes based on two points.
    /// </summary>
    interface IShapeFactory
    {
        /// <summary>
        /// Display name for UI.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Creates a shape from two points and a pen.
        /// </summary>
        Shape Create(Point start, Point end, Pen pen);
    }
}

