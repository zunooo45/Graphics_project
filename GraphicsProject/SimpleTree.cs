using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject
{
    public class SimpleTree : Graph
    {
        public SimpleTree()
        {
            var node1 = new Node(0, 10, -100, "Node 1");
            var node2 = new Node(-20, 0, -100, "Node 2");
            var node3 = new Node(0, 0, -100, "Node 3");
            var node4 = new Node(20, 0, -100, "Node 4");
            var node5 = new Node(-25, -10, -100, "Node 5");
            var node6 = new Node(-20, -10, -100, "Node 6");
            var node7 = new Node(-5, -10, -100, "Node 7");
            var node8 = new Node(0, -10, -100, "Node 8");
            var node9 = new Node(5, -10, -100, "Node 9");
            var node10 = new Node(-30, -20, -100, "Node 10");

            this.Nodes.AddRange(new[] { node1, node2, node3, node4, node5, node6, node7, node8, node9, node10 });
            this.Edges.AddRange(new[]
                                {
                                    new Edge(node1, node2),
                                    new Edge(node1, node3),
                                    new Edge(node1, node4),
                                    new Edge(node2, node5),
                                    new Edge(node2, node6),
                                    new Edge(node3, node7),
                                    new Edge(node3, node8),
                                    new Edge(node3, node9),
                                    new Edge(node5, node10)
                                });
        }
    }
}
