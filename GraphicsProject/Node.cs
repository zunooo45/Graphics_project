using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject
{
    public class Node
    {
        public Node()
        {
            
        }

        public Node(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Node(float x, float y, float z, string label)
            : this(x, y, z)
        {
            this.Label = label;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public string Label { get; set; }
        public Action<bool> OnVisited { get; set; }

        public void Visit()
        {
            var handler = this.OnVisited;
            if (handler != null)
                handler(true);
        }
    }
}
