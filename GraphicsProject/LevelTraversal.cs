using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsProject.Helpers;

namespace GraphicsProject
{
    public class LevelTraversal : IGraphTraverser
    {
        private readonly Graph graph;
        Queue<Node> worklist = new Queue<Node>();
        Dictionary<Node, bool> visited = new Dictionary<Node, bool>();

        public LevelTraversal(Graph graph)
        {
            this.graph = graph;
            var nodes = graph.Nodes;

            visited.Add(nodes[0], false);
            worklist.Enqueue(nodes[0]);
        }

        public IEnumerable<Node> TraversalOrder
        {
            get
            {
                while (true)
                {
                    if (worklist.Count != 0)
                    {
                        var nextNode = worklist.Dequeue();

                        foreach (var neighbor in nextNode.getNeighbors())
                        {
                            if (!visited.ContainsKey(neighbor))
                            {
                                visited.Add(neighbor, false);
                                worklist.Enqueue(neighbor);
                            }
                        }

                        yield return nextNode;
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
        }
    }
}
