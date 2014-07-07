using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var renderer = new Renderer())
            {
                renderer.Run(30, 30);
            }
        }
    }
}
