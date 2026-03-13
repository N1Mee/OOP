using System.Collections.Generic;

namespace OOPlab1
{
    /// <summary>
    /// Registry that holds all available shape factories.
    /// </summary>
    static class ShapeFactoryRegistry
    {
        private static readonly List<IShapeFactory> factories = new List<IShapeFactory>();

        static ShapeFactoryRegistry()
        {
            // Register built-in factories.
            factories.Add(new LineFactory());
            factories.Add(new RectangleFactory());
            factories.Add(new SquareFactory());
            factories.Add(new CircleFactory());
            factories.Add(new EllipseFactory());
            factories.Add(new TriangleFactory());
        }

        /// <summary>
        /// Gets all registered factories.
        /// </summary>
        public static IReadOnlyList<IShapeFactory> Factories => factories;
    }
}

