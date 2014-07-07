using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject
{
    public class SimpleGraph : Graph
    {
        public SimpleGraph()
        {
            var node1 = new Node(-10, 10, -100, "Node 1");
            var node2 = new Node(10, 10, -100, "Node 2");
            var node3 = new Node(10, -10, -100, "Node 3");
            var node4 = new Node(-10, -10, -100, "Node 4");

            this.Nodes.AddRange(new[] {node1, node2, node3, node4});
            this.Edges.AddRange(new[]
                                {
                                    new Edge(node1, node2),
                                    new Edge(node2, node3),
                                    new Edge(node3, node4),
                                    new Edge(node4, node1),
                                    new Edge(node3, node1),
                                    new Edge(node4, node2),
                                });
        }
    }
}
