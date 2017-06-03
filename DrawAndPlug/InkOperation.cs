using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Ink;
using System.Windows.Controls;



namespace DrawAndPlug
{
    class InkOperation
    {
        public static BitmapFrame CreateBitmapSource(InkCanvas inkCanvas)
        {
            int margin = (int)inkCanvas.Margin.Left;
            int width = (int)inkCanvas.ActualWidth - margin;
            int height = (int)inkCanvas.ActualHeight - margin;

            RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Default);
            rtb.Render(inkCanvas);

            BitmapFrame frame = BitmapFrame.Create(rtb);

            return frame;
        }


        public static BitmapFrame CreateBitmapFromInk(StrokeCollection ink, BitmapSource background)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(background, new Rect(0, 0, background.Width, background.Height));

                foreach (var item in ink)
                {
                    item.Draw(drawingContext);
                }
                drawingContext.Close();
                var bitmap = new RenderTargetBitmap((int)background.Width, (int)background.Height, background.DpiX, background.DpiY, PixelFormats.Pbgra32);
                bitmap.Render(drawingVisual);
                return BitmapFrame.Create(bitmap);
            }
        }
    }
}
