using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginInterface;

namespace WatermarkPlugin
{
    public class Watermark : IPlugin
    {
        public string GetOperationName()
        {
            return ("Watermark");
        }
    }
}
