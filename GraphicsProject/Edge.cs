using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject
{
    public class Edge
    {
        public Edge(Node fromNode, Node toNode)
        {
            this.FromNode = fromNode;
            this.ToNode = toNode;
        }

        public Node FromNode { get; set; }
        public Node ToNode { get; set; }
    }
}
