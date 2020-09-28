using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2dGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int size = 128;
        WriteableBitmap bms;
        DispatcherTimer timer;

        Circle circle;
        Circle circle1;


        double offsetX = 0.5f;
        double offsetY = 0.5f;
        double scale = 1f;


        double startMousePosX;
        double startMousePosY;


        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += InitObjects;
        }
        private void InitObjects(object sender, RoutedEventArgs e)
        {
            circle = new Circle(0, -0.3, 0.2, new byte[] { 255, 0, 0 });
            circle1 = new Circle(0, 0.3, 0.2, new byte[] { 0, 0, 255 });

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1 / 25);
            timer.Tick += PrintPixels;
            bms = new WriteableBitmap(size, size, 1, 1, PixelFormats.Rgb24, null);
            img_Picture.Source = bms;
            timer.Start();


            img_Picture.MouseDown += Click;
            img_Picture.MouseMove += Move;
            img_Picture.MouseWheel += Scale;
        }

        #region Mouse
        private void Scale(object sender, MouseWheelEventArgs e)
        {
            scale += e.Delta / 120 / (1 / scale) / 10;
        }
        private void Move(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {

                double mx = e.GetPosition(img_Picture).X / size - 0.5;
                double my = e.GetPosition(img_Picture).Y / size - 0.5;

                offsetX += (mx - startMousePosX) / 5;
                offsetY += (my - startMousePosY) / 5;

                startMousePosX = mx;
                startMousePosY = my;
            }
        }
        private void Click(object sender, MouseButtonEventArgs e)
        {
            startMousePosX = e.GetPosition(img_Picture).X / size - 0.5;
            startMousePosY = e.GetPosition(img_Picture).Y / size - 0.5;
        }
        #endregion
       
        private void PrintPixels(object sender, EventArgs e)
        {
            byte[] color;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    color = Funk(j, i);
                    Int32Rect rect = new Int32Rect(j, i, 1, 1);
                    bms.WritePixels(rect, color, 3, 0);
                }

            }

        }

         private SdfResult SdfCircle(double x, double y, Circle circle)
        {
            double Ox = circle.Ox;
            double Oy = circle.Oy;
            double d = Math.Sqrt(Math.Pow(x - Ox, 2) + Math.Pow(y - Oy, 2));

            SdfResult res = new SdfResult(d-circle.Radius, circle);
            return res;
        }

        private byte[] Funk(int jInt, int iInt)
        {
            Random rand = new Random();
            byte[] color = new byte[3];
            color[0] = 0;
            color[1] = 0;
            color[2] = 0;

            byte direction = 8;
            byte steps = 4;

            double angel = 2 * Math.PI / direction * (rand.NextDouble() + 0.5);
            for (int i = 0; i < direction; i++)
            {
                double x = ((double)jInt / size - offsetX) / scale;
                double y = ((double)iInt / size - offsetY) / scale;

                double opacity = 1;
                bool firstTime = true;
                for (int j = 0; j < steps; j++)
                {
                    SdfResult res1 = SdfCircle(x, y, circle);
                    SdfResult res2 = SdfCircle(x, y, circle1);
                    SdfResult res = res1.Distance < res2.Distance ? res1 : res2;

                    //res = SdfCircle(x, y, circle);
                    
                    double d = res.Distance;
                    double r = res.Circle.Radius;

                    // if (firstTime){
                    //     opacity = d <= r? 1 : 0;
                    //     if(opacity == 0){

                    //         double s = r * 4;
                    //         double A = s - r;
                    //         double B = d - r;
                    //         double C = (A - B) / A;

                    //         opacity = C < 0? 0 : C;
                    //     }
                    //     firstTime = false;
                    // }

                    if (res.Distance <= 0.1)
                    {
                        color[0] += (byte)((res.Circle.Color[0] / direction));
                        color[1] += (byte)((res.Circle.Color[1] / direction));
                        color[2] += (byte)((res.Circle.Color[2] / direction));
                        break;
                    }

                    double randNumber = rand.NextDouble() + 0.5;

                    x += res.Distance * Math.Cos(angel * randNumber);
                    y += res.Distance * Math.Sin(angel * randNumber);
                }
                angel += 2 * Math.PI / direction;
            }

            return color;
        }
    }
}

