using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginInterface;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;

namespace WatermarkPlugin
{
    public class Watermark : IPlugin
    {
        public string GetOperationName()
        {
            return ("Watermark");
        }

        public Bitmap GetElement(BitmapSource source)
        {
            Bitmap bm = BitmapFromSource(source);
            Image image = Image.FromFile(@"C: \Users\Krzys\Desktop\watermark.png");

            using (Graphics grfx = Graphics.FromImage(bm))
            {
                grfx.DrawImage(image, 40, 40, 100, 100);

            }

            return bm;
        }



        private Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }
    }
}
