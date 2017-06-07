﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;


namespace PluginInterface
{
    public interface IPlugin
    {
        string GetOperationName();
        Bitmap GetElement(BitmapSource source);
    }
}
