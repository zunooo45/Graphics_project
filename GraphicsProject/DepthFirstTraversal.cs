using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsProject.Helpers;

namespace GraphicsProject
{
    public class DepthFirstTraversal : IGraphTraverser
    {
        private readonly Graph graph;
        private readonly Node startNode;
        private IList<Node> visitedNodes;

        public DepthFirstTraversal(Graph graph, Node startNode)
        {
            this.graph = graph;
            this.startNode = startNode;
            this.visitedNodes = new List<Node>();
        }

        private DepthFirstTraversal(Graph graph, Node startNode, ref IList<Node> visitedNodes)
            : this(graph, startNode)
        {
            this.visitedNodes = visitedNodes ?? new List<Node>();
        }

        public IEnumerable<Node> TraversalOrder
        {
            get
            {
                yield return this.startNode;
                this.visitedNodes.Add(this.startNode);
                foreach (var node in this.startNode.getNeighbors())
                {
                    if (!this.visitedNodes.Contains(node))
                    {
                        var traversal = new DepthFirstTraversal(this.graph, node, ref this.visitedNodes);
                        foreach (var subNode in traversal.TraversalOrder)
                            yield return subNode;
                    }
                }
            }
        }
    }
}
