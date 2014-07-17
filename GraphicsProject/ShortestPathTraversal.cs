using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsProject.Helpers;

namespace GraphicsProject
{
    public class ShortestPathTraversal : IGraphTraverser
    {
        private readonly Graph graph;
        List<Node> worklist = new List<Node>();
        Dictionary<Node, bool> visited = new Dictionary<Node, bool>();

        private readonly IList<Node> nodes = new List<Node>();
        private Node start;
        private Node trace;
        private Node end;
        private String state;
        private String modeTemp;

        public ShortestPathTraversal(Graph graph)
        {
            this.graph = graph;
            nodes = graph.Nodes;

            // Wait until a start is chosen
            state = "Waiting";
            setStart(0);
            setEnd(nodes.Count - 1);
        }

        public void setStart(int pos)
        {
            start = nodes[pos];

            start.visited = true;
            start.distance = 0;
            start.lastNode = start;
            start.setMode("Start");

            worklist.Add(nodes[pos]);
        }

        public void setEnd(int pos)
        {
            end = nodes[pos];
            end.setMode("End");
        }

        public void reset()
        {
            // Clear these lists every time a new start is being set
            foreach (Node node in nodes)
            {
                node.visited = false;
                node.distance = 9999999;
                node.lastNode = start;
                node.setMode("Unvisited");
            }
            worklist.Clear();

            trace = start;
            modeTemp = "Start";

            setStart(0);
            setEnd(nodes.Count - 1);

            // Now it knows what to look for
            state = "Searching";
        }

        public void stepGraph()
        {
            switch (state)
            {
                // If failed, waiting, or done do nothing
                case "Waiting":
                    this.reset();
                    break;
                case "Done":
                    this.reset();
                    Console.WriteLine("Done");
                    break;

                case "Searching":
                    if (worklist.Count != 0)
                    {
                        visitNextNode();
                    }
                    else
                    {
                        // If it runs out of nodes to look at before the end is found it failed
                        state = "Returning";
                        Console.Write("Returning\n");
                        trace.setMode(modeTemp);
                        trace = end.lastNode;
                    }
                    break;

                case "Returning":
                    if (trace != start)
                    {
                        Console.Write("Stepping Back\n");
                        trace.setMode("Path");
                        trace = trace.lastNode;
                    }
                    else
                    {
                        state = "Done";
                        Console.Write("Done\n");
                    }
                    break;
            }

        }

        private void visitNextNode()
        {
            Console.Write("Visiting next node\n");
            Node nextNode = nextSortest();
            trace.setMode(modeTemp);

            trace = nextNode;
            if (nextNode != start && nextNode != end)
            {
                nextNode.setMode("Visited");
                modeTemp = "Visited";
            }
            else
            {
                modeTemp = trace.getMode();
            }

            trace.setMode("Visiting");
            nextNode.visited = true;

            foreach (Node neighbor in nextNode.getNeighbors())
            {
                if (!neighbor.visited)
                {
                    worklist.Add(neighbor);
                }

                var newDist = nextNode.distanceFrom(neighbor) + nextNode.distance;

                if (newDist < neighbor.distance)
                {
                    neighbor.distance = newDist;
                    neighbor.lastNode = nextNode;
                }
            }
            Console.Write("Visited next node\n");
        }

        private Node nextSortest()
        {
            Node shortest = worklist[0];
            Console.Write("Finding next shortest\n");
            foreach(Node node in worklist)
            {
                // Check if the node hasn't been visited
                if (!node.visited)
                {
                    node.visited = true;
                    // Check if the distance is shorter
                    if (shortest.distance > node.distance)
                        shortest = node;
                }
                    
            }

            worklist.Remove(shortest);
            Console.Write("Found next shortest\n");
            return shortest;
        }

        public IEnumerable<Node> TraversalOrder
        {
            get
            {
                while (true)
                {
                    this.stepGraph();
                    yield return nodes[0];
                }
            }
        }
    }
}
