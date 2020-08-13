using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DebugVisualizer.DataExtraction;
using DebugVisualizer.DataExtraction.Data;
using DebugVisualizer.DataExtraction.Extractors;

namespace WindowsFormsAppDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            DataExtractor.MainDataExtractor.DataExtractors.Add(new BitmapDataExtractor());
            DataExtractor.MainDataExtractor.DataExtractors.Add(new ControlDataExtractor());
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap flag = new Bitmap(200, 100);

            Graphics flagGraphics = Graphics.FromImage(flag);
            int red = 0;
            int white = 10;
            while (white <= 100)
            {
                flagGraphics.FillRectangle(Brushes.Red, 0, red, 200, 10);
                flagGraphics.FillRectangle(Brushes.White, 0, white, 200, 10);
                red += 20;
                white += 20;
            }

            pictureBox1.Image = flag;
            
        }
    }

    public class ControlDataExtractor : GenericDataExtractor<Control>
    {
        public override void GetExtractions(Control control, IDataExtractorContext context)
        {
            context.AddExtraction(() =>
            {
                var root = control;
                while (root.Parent != null)
                {
                    root = root.Parent;
                }

                var controlLocationRelativeToRoot = root.PointToClient(control.PointToScreen(Point.Empty));
                var rootLocationRelativeToWindow = (Size) root.PointToScreen(Point.Empty) - (Size) root.Bounds.Location;

                var windowBitmap = DrawControl(root);
                var windowBitmapGraphics = Graphics.FromImage(windowBitmap);
                windowBitmapGraphics.DrawRectangle(new Pen(Brushes.Red, 3),
                    new Rectangle(controlLocationRelativeToRoot + rootLocationRelativeToWindow, control.ClientSize));

                return BitmapDataExtractor.GetPngImageDataFromBitmap(windowBitmap);
            }, new DataExtractorInfo("control-tree", "Control Tree", 1500));

            context.AddExtraction(() => BitmapDataExtractor.GetPngImageDataFromBitmap(DrawControl(control)),
                new DataExtractorInfo("control", "Control", 1000));
        }

        private static Bitmap DrawControl(Control control)
        {
            var bitmap = new Bitmap(control.Size.Width, control.Size.Height);
            bitmap.SetResolution(control.DeviceDpi, control.DeviceDpi);
            control.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size));
            return bitmap;
        }
    }

    public class BitmapDataExtractor : GenericDataExtractor<Bitmap>
    {
        public override void GetExtractions(Bitmap value, IDataExtractorContext context)
        {
            context.AddExtraction(() => GetPngImageDataFromBitmap(value),
                new DataExtractorInfo("bitmap-png", "Bitmap PNG", 1000));
        }

        public static PngImageData GetPngImageDataFromBitmap(Bitmap bitmap)
        {
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            var base64 = Convert.ToBase64String(ms.GetBuffer());
            return new PngImageData(base64);
        }
    }
}