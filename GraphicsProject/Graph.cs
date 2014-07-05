using GraphicsProject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject
{
    class Graph
    {
        private readonly IList<Node> nodes = new List<Node>();
        private readonly IList<Edge> edges = new List<Edge>();
        private Node start;
        private Node trace;
        private Node end;
        private ShaderProgram program;
        private String state;
        private String modeTemp;
        private IList<Node> worklist = new List<Node>();
        private Random rand = new Random();

        public Graph(ShaderProgram shader)
        {
            // Wait until a start is chosen
            state = "Waiting";
            program = shader;
        }

        public void setStart(int pos)
        {
            start = nodes[pos]; 
            // Clear these lists every time a new start is being set
            foreach(Node node in nodes)
            {
                node.visited = false;
                node.distance = 9999999;
                node.lastNode = start;
                node.setMode("Unvisited");
            }
            worklist.Clear();

            trace = start;
            modeTemp = "Start";
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

            // Now it knows what to look for
            state = "Searching";
        }

        public Line addNode(Cube newNode)
        {
            // Create new node
            Node node = new Node(newNode);
            this.nodes.Add(node);

            // Connect the new node to a random node
            int sourceNum = (int)(rand.Next(0, nodes.Count - 1));
            return addEdge(nodes[sourceNum], node);
        }

        public Line addEdge(Node source, Node dest)
        {
            var edge = new Edge(this.program, source, dest);
            var line = edge.getLine();
            line.OnLoad();

            return line;
        }

        public Line addRandEdge()
        {
            int sourceNum;
            int destNum;

            // Make sure they aren't the same node and that they aren't connected yet
            do
            {
                sourceNum = (int)(rand.Next(0, nodes.Count - 1));
                destNum = (int)(rand.Next(0, nodes.Count - 1));
            } while (sourceNum == destNum || nodes[sourceNum].getNeighbors().Contains(nodes[destNum]));

            return addEdge(nodes[sourceNum], nodes[destNum]);
        }

        public void stepGraph()
        {
            switch(state)
            {
                // If failed, waiting, or done do nothing
                case "Waiting":
                case "Done":
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

            return shortest;
        }
    }
}
