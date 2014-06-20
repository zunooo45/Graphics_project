using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GraphicsProject
{
    public class ColorCube : Cube
    {
        readonly Vector3 color = new Vector3(1, 1, 1);

        public ColorCube(Vector3 color)
        {
            this.color = color;
        }

        public override Vector3[] GetColorData()
        {
            return new[]
                   {
                       this.color,
                       this.color,
                       this.color,
                       this.color,
                       this.color,
                       this.color,
                       this.color,
                       this.color
                   };
        }
    }
}
