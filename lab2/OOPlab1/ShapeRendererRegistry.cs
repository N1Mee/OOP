using System;
using System.Collections.Generic;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Registry that maps shape types to renderers.
    /// </summary>
    static class ShapeRendererRegistry
    {
        private static readonly Dictionary<Type, IShapeRenderer> renderers =
            new Dictionary<Type, IShapeRenderer>();

        static ShapeRendererRegistry()
        {
            // Registration of built-in renderers.
            Register<Line>(new LineRenderer());
            Register<Rectangle>(new RectangleRenderer());
            Register<Square>(new SquareRenderer());
            Register<Circle>(new CircleRenderer());
            Register<Ellipse>(new EllipseRenderer());
            Register<Triangle>(new TriangleRenderer());
        }

        /// <summary>
        /// Registers renderer for a given shape type.
        /// </summary>
        public static void Register<TShape>(IShapeRenderer renderer) where TShape : Shape
        {
            renderers[typeof(TShape)] = renderer;
        }

        /// <summary>
        /// Draws a single shape using appropriate renderer.
        /// </summary>
        public static void DrawShape(Shape shape, Graphics g)
        {
            if (shape == null)
            {
                return;
            }

            Type type = shape.GetType();
            if (renderers.TryGetValue(type, out IShapeRenderer renderer))
            {
                renderer.Draw(shape, g);
            }
        }
    }
}

