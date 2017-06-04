using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginInterface;

namespace GrayscalePlugin
{
    public class Grayscale : IPlugin
    {
        public string GetOperationName()
        {
            return "Grayscale";
        }
    }
}
