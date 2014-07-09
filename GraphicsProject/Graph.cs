using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsProject.Helpers;

namespace GraphicsProject
{
    public class Graph : IDisposable
    {
        public Graph()
        {
            this.Nodes = new List<Node>();
            this.Edges = new List<Edge>();
        }
        public List<Node> Nodes { get; set; }
        public List<Edge> Edges { get; set; }

        public void Dispose()
        {
            foreach (var node in Nodes)
                node.Dispose();
            foreach (var edge in Edges)
                edge.Dispose();
        }
    }
}
