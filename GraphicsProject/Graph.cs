using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsProject.Helpers;

namespace GraphicsProject
{
    public class Graph
    {
        public Graph()
        {
            this.Nodes = new List<Node>();
            this.Edges = new List<Edge>();
        }
        public List<Node> Nodes { get; set; }
        public List<Edge> Edges { get; set; }

        //public IEnumerable<Node> GetAdjacentNodes(Node node)
        //{
        //    return this.Edges.Where(e => e.FromNode == node || e.ToNode == node)
        //        .Select(e =>
        //                {
        //                    if (e.FromNode == node)
        //                        return e.ToNode;
        //                    return e.FromNode;
        //                });
        //}
    }
}
