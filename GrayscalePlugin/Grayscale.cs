using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginInterface;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;

namespace GrayscalePlugin
{
    public class Grayscale : IPlugin
    {
        public string GetOperationName()
        {
            return ("Grayscale");
        }

        public Bitmap GetElement(BitmapSource source)
        {
            Bitmap c = BitmapFromSource(source);
            Bitmap d = new Bitmap(c.Width, c.Height);

            for (int i = 0; i < c.Width; i++)
            {
                for (int x = 0; x < c.Height; x++)
                {
                    Color oc = c.GetPixel(i, x);
                    int grayScale = (int)((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
                    Color nc = Color.FromArgb(oc.A, grayScale, grayScale, grayScale);
                    d.SetPixel(i, x, nc);
                }
            }

            return d;
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
