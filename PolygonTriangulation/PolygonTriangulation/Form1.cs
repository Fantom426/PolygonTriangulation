using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Object;


namespace PolygonTriangulation
{
    public class Triangle
    {
        public Point first = new Point(0,0);
        public Point second = new Point(0, 0);
        public Point third = new Point(0, 0);
    }

    public partial class Form1 : Form
    {
        Bitmap bitmap;
        Point firstClick = Point.Empty;
        Point firstPolygonPoint = Point.Empty;
        List<Point> Polygon = new List<Point>();

        List<Point> Vertex = new List<Point>();

        List<Triangle> Triangles = new List<Triangle>();

        Stack<Point> St = new Stack<Point>();

        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bitmap;
        }

        private double vector_mult(double x1, double y1, double x2, double y2)
        {
            return x1 * y2 - x2 * y1;
        }

        public bool Cross_Lines(Point p1, Point p2, Point p3, Point p4)
        {

            double v1 = vector_mult(p4.X - p3.X, p4.Y - p3.Y, p1.X - p3.X, p1.Y - p3.Y);
            double v2 = vector_mult(p4.X - p3.X, p4.Y - p3.Y, p2.X - p3.X, p2.Y - p3.Y);
            double v3 = vector_mult(p2.X - p1.X, p2.Y - p1.Y, p3.X - p1.X, p3.Y - p1.Y);
            double v4 = vector_mult(p2.X - p1.X, p2.Y - p1.Y, p4.X - p1.X, p4.Y - p1.Y);

            bool res = ((v1 * v2) < 0) && ((v3 * v4) < 0);
            return res;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Bitmap im = (Bitmap)pictureBox1.Image;
            Pen blackPen = new Pen(Color.Black, 3);

            if (firstClick == Point.Empty)
            {
                firstClick = e.Location;
                firstPolygonPoint = e.Location;
                Polygon.Clear();
                Polygon.Add(e.Location);
                //Line.Add(firstClick);
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                using (Graphics g = Graphics.FromImage(im))
                    g.DrawLine(blackPen, firstPolygonPoint, firstClick);
                pictureBox1.Image = im;
                firstPolygonPoint = Point.Empty;

                pictureBox1.Enabled = false;
                firstClick = Point.Empty;
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                for (int i = 0; i < Polygon.Count; i++)
                   if (Cross_Lines(firstClick, e.Location, Polygon[i], Polygon[(i+1) % Polygon.Count]))
                    {
                        pictureBox1.Enabled = false;
                        MessageBox.Show("Это не монотонный многоугольник!!!");
                        return;
                    }

                if (e.Location == Polygon[0])
                {
                    using (Graphics g = Graphics.FromImage(im))
                        g.DrawLine(blackPen, firstClick, e.Location);
                    //firstClick = Point.Empty;
                    //firstPolygonPoint = Point.Empty;
                    pictureBox1.Image = im;
                    return;
                }

                using (Graphics g = Graphics.FromImage(im))
                    g.DrawLine(blackPen, firstClick, e.Location);
                firstClick = e.Location;
                Polygon.Add(firstClick);
            }

            pictureBox1.Image = im;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap im = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(im))
                g.Clear(Color.White);
            pictureBox1.Enabled = true;

            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                firstClick = Point.Empty;
               
            }
            Polygon.Clear();
            pictureBox1.Image = im;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Vertex_Sort(Polygon);
            
        }

        public void Triangulation(List<Point> Polygon)
        {
            St = new Stack<Point>();
            Triangles = new List<Triangle>();
            St.Push(Polygon[0]);
            St.Push(Polygon[1]);
            /*for (int i = 2; i < Polygon.Count; i++)
            {

            }
            */
        }

        
        public void Vertex_Sort(List<Point> Pol)
        {
            int min = int.MaxValue;
            int k = 0;
            int i = 0;
            Point p = new Point();
            while (i < Pol.Count)
            {
                for (int j = i; j < Pol.Count; j++)
                {
                    if (Pol[j].X < min)
                    {
                        min = Pol[j].X;
                        p = Pol[j];
                        k = j;
                    }
                    
                }
                Pol[k] = Pol[i];
                Pol[i] = p;
                i++;
                min = int.MaxValue;
            }
        }
    }
}
