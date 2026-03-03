using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOPlab1
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ShapeList list = new ShapeList();
            list.Add(new Square(50, 10, 20, new Pen(Color.Red, 3)));
            list.Add(new Triangle(50, 100, 200, new Pen(Color.Black, 3)));
            list.Add(new Circle(50, 200, 100, new Pen(Color.Blue, 3)));
            list.Add(new Ellipse(150, 100, 400, 400, new Pen(Color.Pink, 3)));
            list.Add(new Rectangle(50, 10, 600, 350, new Pen(Color.Green, 3)));
            list.Add(new Line(700, 400, 700, 200, new Pen(Color.Gray, 3)));
            list.Draw(e.Graphics);
        }
    }
}
