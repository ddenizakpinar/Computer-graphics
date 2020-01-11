using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PainterForms
{
    public partial class Form1 : Form
    {
        Graphics g;
        List<Point> pointList;
        bool free, line, rectangle, triangle, pollygon, circle, ellipse, arch, fill = false;
        Bitmap bitmap = new Bitmap(800, 500);
        //  
        Color paintColor;



        public Form1()
        {
            InitializeComponent();
            // g = panel.CreateGraphics();
            g = Graphics.FromImage(bitmap);
            pointList = new List<Point>();
            label6.Visible = false;
            label7.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            radioButton1.Visible = false;
            radioButton2.Visible = false;
            radioButton3.Visible = false;
            groupBox1.Visible = false;

        }

        private void letThemOff()
        {
            free = false;
            line = false;
            triangle = false;
            rectangle = false;
            pollygon = false;
            circle = false;
            ellipse = false;
            arch = false;
        }

        private void putPixel(Point point, int intense)
        {

            Brush aBrush = (Brush)Brushes.Black;
            g.FillRectangle(aBrush, point.X, point.Y, intense, intense);
            //    this.Refresh();
            // 2 pixelin büyüklüğü (default pixel size = 1)
        }

        private void showPoint(MouseEventArgs e)
        {
            label1.Text = "X:" + e.X + " Y:" + e.Y;
        }

        private void changeColor()
        {

        }

        private void drawLine(Point a, Point b)
        {
            //putPixel(a);
            //putPixel(b);
            //label2.Text = "Line";
            label3.Text = "A : X:" + a.X + " Y:" + a.Y;
            label4.Text = "B : X:" + b.X + " Y:" + b.Y;
            int dx = b.X - a.X;
            int dy = b.Y - a.Y;

            int pixelSayisi = Math.Abs(dx) > Math.Abs(dy) ? Math.Abs(dx) : Math.Abs(dy);

            var xFark = (float)dx / (float)pixelSayisi;
            var yFark = (float)dy / (float)pixelSayisi;

            float x = (float)a.X;
            float y = (float)a.Y;

            while (pixelSayisi > 0)
            {
                Point point = new Point((int)(x + 0.5f), (int)(y + 0.5f));
                putPixel(point, 1);
                x += xFark;
                y += yFark;
                pixelSayisi--;
            }

        }

        private void drawTriangle()
        {
            label2.Text = "Triangle";
            Point c = pointList[pointList.Count - 1];
            Point b = pointList[pointList.Count - 2];
            Point a = pointList[pointList.Count - 3];
            drawLine(a, b);
            drawLine(b, c);
            drawLine(c, a);
        }
        private void filledDrawTriangle()
        {

            label2.Text = "Triangle";
            Point c = pointList[pointList.Count - 1];
            Point b = pointList[pointList.Count - 2];
            Point a = pointList[pointList.Count - 3];


            drawLine(a, b);
            drawLine(b, c);
            drawLine(c, a);


        }

        private void drawRectangle()
        {
            label2.Text = "Rectangle";
            Point a = pointList[pointList.Count - 1];
            Point b = pointList[pointList.Count - 2];
            Point c = new Point(a.X, b.Y);
            Point d = new Point(b.X, a.Y);

            drawLine(a, d);
            drawLine(d, b);
            drawLine(b, c);
            drawLine(c, a);
        }

        private void drawPollygon()
        {
            label2.Text = "Pollygon";
            for (int i = 0; i < pointList.Count; i++)
            {
                if (i + 1 < pointList.Count)
                    drawLine(pointList[i], pointList[i + 1]);

            }
            drawLine(pointList[pointList.Count - 1], pointList[0]);
        }

        private void drawCircle(int xc, int yc, int x, int y)
        {
            label2.Text = "Circle";
            putPixel(new Point(xc + x, yc + y), 2);
            putPixel(new Point(xc - x, yc + y), 2);
            putPixel(new Point(xc + x, yc - y), 2);
            putPixel(new Point(xc - x, yc - y), 2);
            putPixel(new Point(xc + y, yc + x), 2);
            putPixel(new Point(xc - y, yc + x), 2);
            putPixel(new Point(xc + y, yc - x), 2);
            putPixel(new Point(xc - y, yc - x), 2);


        }

        private void circleBres()
        {
            // Merkez noktası
            var pointU = pointList[pointList.Count - 1];
            // İkinci nokta
            var pointM = pointList[pointList.Count - 2];
            var xc = pointM.X;
            var yc = pointM.Y;
            // İki nokta arası uzaklık R
            var r = (int)Math.Sqrt(Math.Pow((pointU.X - pointM.X), 2) + Math.Pow((pointU.Y - pointM.Y), 2));
            //  var r = 30;
            int x = 0, y = r;
            int d = 3 - 2 * r;
            drawCircle(xc, yc, x, y);
            while (y >= x)
            {
                x++;
                if (d > 0)
                {
                    y--;
                    d = d + 4 * (x - y) + 10;
                }
                else
                    d = d + 4 * x + 6;
                drawCircle(xc, yc, x, y);
            }
        }

        private void filledCircleBres()
        {
            // Merkez noktası
            var pointU = pointList[pointList.Count - 1];
            // İkinci nokta
            var pointM = pointList[pointList.Count - 2];
            // İki nokta arası uzaklık R
            var r = (int)Math.Sqrt(Math.Pow((pointU.X - pointM.X), 2) + Math.Pow((pointU.Y - pointM.Y), 2));
            while (r > 0)
            {
                r--;
                var xc = pointM.X;
                var yc = pointM.Y;

                //  var r = 30;
                int x = 0, y = r;
                int d = 3 - 2 * r;
                drawCircle(xc, yc, x, y);
                while (y >= x)
                {
                    x++;
                    if (d > 0)
                    {
                        y--;
                        d = d + 4 * (x - y) + 10;
                    }
                    else
                        d = d + 4 * x + 6;
                    drawCircle(xc, yc, x, y);
                }
            }
        }


        private void drawEllipse()
        {

            var p1 = pointList[pointList.Count - 3];
            var p2 = pointList[pointList.Count - 2];
            var p3 = pointList[pointList.Count - 1];

            var xc = p1.X;
            var yc = p1.Y;
            // Merkezin 2. tıklanan noktaya olan yatay uzaklığı
            var rx = (int)Math.Sqrt(Math.Pow((p1.X - p2.X), 2) + Math.Pow((p1.Y - p2.Y), 2));
            // Merkezin 3. tıklanan noktaya olan dikey uzaklığı
            var ry = (int)Math.Sqrt(Math.Pow((p1.X - p3.X), 2) + Math.Pow((p1.Y - p3.Y), 2));

            int x, y, p;
            x = 0;
            y = ry;
            p = (int)(Math.Pow(ry, 2) - (Math.Pow(rx, 2) * ry) + (Math.Pow(rx, 2) / 4));
            while ((2 * x * ry * ry) < (2 * y * rx * rx))
            {
                putPixel(new Point(xc + x, yc - y), 2);
                putPixel(new Point(xc - x, yc + y), 2);
                putPixel(new Point(xc + x, yc + y), 2);
                putPixel(new Point(xc - x, yc - y), 2);

                if (p < 0)
                {
                    x = x + 1;
                    p = p + (2 * ry * ry * x) + (ry * ry);
                }
                else
                {
                    x = x + 1;
                    y = y - 1;
                    p = p + (2 * ry * ry * x + ry * ry) - (2 * rx * rx * y);
                }
            }
            p = (int)(((float)x + 0.5) * ((float)x + 0.5) * ry * ry + (y - 1) * (y - 1) * rx * rx - rx * rx * ry * ry);

            while (y >= 0)
            {
                putPixel(new Point(xc + x, yc - y), 2);
                putPixel(new Point(xc - x, yc + y), 2);
                putPixel(new Point(xc + x, yc + y), 2);
                putPixel(new Point(xc - x, yc - y), 2);

                if (p > 0)
                {
                    y = y - 1;
                    p = p - (2 * rx * rx * y) + (rx * rx);

                }
                else
                {
                    y = y - 1;
                    x = x + 1;
                    p = p + (2 * ry * ry * x) - (2 * rx * rx * y) - (rx * rx);
                }
            }

        }




        private void drawArch(int start_angle, int end_angle)
        {

            start_angle = int.Parse(textBox1.Text);
            end_angle = int.Parse(textBox2.Text);
            var p1 = pointList[pointList.Count - 1];
            double r = 10;
            double angle = (int)(((start_angle <= end_angle) ? start_angle : end_angle) * (Math.PI / 180));
            double range = (((end_angle > start_angle) ? end_angle : start_angle) * (Math.PI / 180));

            var x = (r * Math.Cos(angle));
            var y = (r * Math.Sin(angle));

            do
            {


                putPixel(new Point(p1.X + (int)x, p1.Y - (int)y), 3);

                angle += 0.001;

                x = (r * Math.Cos(angle));
                y = (r * Math.Sin(angle));
            } while (angle <= range);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }



        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {



            showPoint(e);
            Point lastPoint = new Point(e.X, e.Y);
            putPixel(lastPoint, 4);
            pointList.Add(lastPoint);
            // e.Graphics.DrawImage(bitmap, 0, 0);


        }


        private void DrawButton_Click(object sender, EventArgs e)
        {

            if (pointList.Count > 0)
            {
                if (line)
                    drawLine(pointList[pointList.Count - 2], pointList[pointList.Count - 1]);

                if (triangle)
                    //drawTriangle();
                    filledDrawTriangle();

                if (rectangle)
                    drawRectangle();

                if (pollygon)
                    drawPollygon();
                if (circle)
                    // Bresenham’s Çember çizme algoritması
                    if (fill)
                        filledCircleBres();
                    else
                        circleBres();

                if (ellipse)
                    // Midpoint elips algoritması
                    drawEllipse();
                if (arch)
                    drawArch(50, 250);
                this.Refresh();
            }





            try
            {
                pointList.Clear();
            }
            catch (Exception)
            {

            }






        }

        private void LineButton_Click(object sender, EventArgs e)
        {
            letThemOff();
            ellipseInfoOff();
            line = true;
            label5.Text = "Çizgi çizmek için 2 nokta seçin.";
        }

        private void CleanButton_Click_1(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            this.Refresh();
            bitmap = new Bitmap(800, 500);


            g = Graphics.FromImage(bitmap);

        }
        private void FillCheck_CheckedChanged(object sender, EventArgs e)
        {
            fill = fillCheck.Checked;
        }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
            if (free)
            {


                // g.DrawPolygon(Pens.Black, points.ToArray());
                //    drawPollygon();
                // label8.Text = "Boyuyorum"+ pointList[pointList.Count - 1].X.ToString()+ " "+
                // pointList[pointList.Count - 1].Y.ToString();
                int pp1 = pointList[pointList.Count - 1].X;
                int pp2 = pointList[pointList.Count - 1].Y;

                try
                {
                    FloodFill(bitmap, pp1 + 5, pp2 + 5, paintColor);

                    e.Graphics.DrawImage(bitmap, 0, 0);
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }

        void FloodFill(Bitmap bitmap, int x, int y, Color color)
        {
            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int[] bits = new int[data.Stride / 4 * data.Height];
            Marshal.Copy(data.Scan0, bits, 0, bits.Length);

            LinkedList<Point> check = new LinkedList<Point>();
            int floodTo = color.ToArgb();
            int floodFrom = bits[x + y * data.Stride / 4];
            bits[x + y * data.Stride / 4] = floodTo;

            if (floodFrom != floodTo)
            {
                check.AddLast(new Point(x, y));
                while (check.Count > 0)
                {
                    Point cur = check.First.Value;
                    check.RemoveFirst();

                    foreach (Point off in new Point[] {
                new Point(0, -1), new Point(0, 1),
                new Point(-1, 0), new Point(1, 0)})
                    {
                        Point next = new Point(cur.X + off.X, cur.Y + off.Y);
                        if (next.X >= 0 && next.Y >= 0 &&
                            next.X < data.Width &&
                            next.Y < data.Height)
                        {
                            if (bits[next.X + next.Y * data.Stride / 4] == floodFrom)
                            {
                                check.AddLast(next);
                                bits[next.X + next.Y * data.Stride / 4] = floodTo;
                            }
                        }
                    }
                }
            }

            Marshal.Copy(bits, 0, data.Scan0, bits.Length);
            bitmap.UnlockBits(data);
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            paintColor = Color.Red;
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            paintColor = Color.Green;
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            paintColor = Color.Blue;
        }

        private void FreeButton_Click(object sender, EventArgs e)
        {
            radioButton1.Visible = true;
            radioButton2.Visible = true;
            radioButton3.Visible = true;
            groupBox1.Visible = true;
            letThemOff();
            ellipseInfoOff();
            free = true;
            label5.Text = @"Boyamak istediğiniz alanı 
ve rengini seçin.";
            //   Refresh();

        }

        private void TriangleButton_Click(object sender, EventArgs e)
        {
            letThemOff();
            ellipseInfoOff();
            triangle = true;
            label5.Text = "Üçgen çizmek için 3 nokta seçin.";

        }

        private void RectangleButton_Click(object sender, EventArgs e)
        {
            letThemOff();
            ellipseInfoOff();
            rectangle = true;
            label5.Text = "Dörtgen çizmek için 2 nokta seçin.";

        }

        private void PollygonButton_Click(object sender, EventArgs e)
        {
            letThemOff();
            ellipseInfoOff();
            pollygon = true;
            label5.Text = "Poligon çizmek için istediğiniz sayıda nokta seçin.";
        }

        private void CircleButton_Click(object sender, EventArgs e)
        {
            letThemOff();
            ellipseInfoOff();
            circle = true;
            label5.Text = @"Çember çizmek için 2 nokta seçin. 
1. nokta merkez 2. nokta yarı çap";
        }

        private void EllipseButton_Click(object sender, EventArgs e)
        {
            letThemOff();
            ellipseInfoOff();
            ellipse = true;
            label5.Text = @"Elips çizmek için 3 nokta seçin. 
1. nokta merkez 
2.nokta merkezin X uzaklığı 
3.nokta merkezin Y uzaklığı";
        }

        private void ArchButton_Click(object sender, EventArgs e)
        {
            letThemOff();
            arch = true;
            label5.Text = @"Yay çizmek için bir nokta seçin ve başlangıç bitiş açısı girin.";
            label6.Visible = true;
            label7.Visible = true;
            textBox1.Visible = true;
            textBox2.Visible = true;
        }
        private void ellipseInfoOff()
        {
            label6.Visible = false;
            label7.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
        }
    }
}
