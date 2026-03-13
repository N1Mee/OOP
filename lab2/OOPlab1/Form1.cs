using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace OOPlab1
{
    public partial class Form1: Form
    {
        /// <summary>
        /// List of all shapes drawn on the canvas.
        /// </summary>
        private readonly ShapeList shapes = new ShapeList();

        /// <summary>
        /// Starting point of the current mouse drag operation.
        /// </summary>
        private Point? mouseDownPoint;

        /// <summary>
        /// Currently selected factory for creating shapes.
        /// </summary>
        private IShapeFactory currentFactory;

        /// <summary>
        /// Pen used for drawing new shapes.
        /// </summary>
        private Pen currentPen = new Pen(Color.Red, 3);

        public Form1()
        {
            InitializeComponent();

            InitializeShapeFactories();

            // Subscribe to mouse events on the drawing panel.
            panel1.MouseDown += Panel1_MouseDown;
            panel1.MouseUp += Panel1_MouseUp;
            panel1.MouseMove += Panel1_MouseMove;
        }

        /// <summary>
        /// Initializes available shape factories and binds them to the combo box.
        /// </summary>
        private void InitializeShapeFactories()
        {
            if (shapeComboBox != null)
            {
                shapeComboBox.DataSource = ShapeFactoryRegistry.Factories.ToList();
                shapeComboBox.DisplayMember = "Name";

                if (ShapeFactoryRegistry.Factories.Count > 0)
                {
                    currentFactory = ShapeFactoryRegistry.Factories[0];
                }

                shapeComboBox.SelectedIndexChanged += ShapeComboBox_SelectedIndexChanged;
            }
        }

        /// <summary>
        /// Handles change of selected shape type in combo box.
        /// </summary>
        private void ShapeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentFactory = shapeComboBox.SelectedItem as IShapeFactory;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Form itself does not draw shapes.
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Draw all shapes on the panel.
            shapes.Draw(e.Graphics);
        }

        /// <summary>
        /// Starts shape creation on mouse down.
        /// </summary>
        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownPoint = e.Location;
        }

        /// <summary>
        /// Finalizes shape creation on mouse up and adds it to the list.
        /// </summary>
        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDownPoint.HasValue && currentFactory != null)
            {
                Shape shape = currentFactory.Create(mouseDownPoint.Value, e.Location, currentPen);
                shapes.Add(shape);
                mouseDownPoint = null;
                panel1.Invalidate();
            }
        }

        /// <summary>
        /// Handles mouse move events. Can be used for preview if needed.
        /// </summary>
        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            // Preview could be implemented here if required.
        }
    }
}
