using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsProject.Helpers;
using OpenTK;

namespace GraphicsProject
{
    public class BigGraph : Graph
    {
        public BigGraph(ShaderProgram program)
        {
            this.random = new Random((int)DateTime.Now.Ticks);
            var currentPosition = new Vector3(0, 0, -200);

            for (int i = 0; i < 30; i++)
            {
                this.Nodes.Add(new Node(new Cube(program)
                                        {
                                            Position = currentPosition
                                        }));
                currentPosition = this.GetRandomLocationFromPosition(currentPosition);
            }

            foreach (Node node in this.Nodes)
            {
                int NumEdges = this.random.Next(1, 3);
                for (int i = 0; i < NumEdges; i++)
                {
                    var other = GetRandomCloseNode(node);
                    this.Edges.Add(new Edge(program, node, other));
                    node.connect(other, Edges[Edges.Count - 1]);
                    other.connect(node, Edges[Edges.Count - 1]);
                }
            }
        }

        private Random random;
        private Vector3 GetRandomLocationFromPosition(Vector3 sourceVector)
        {
            const int maxDistance = 40;
            var position = new Vector3(
                this.GetRandomValue((int)sourceVector.X, maxDistance),
                this.GetRandomValue((int)sourceVector.Y, maxDistance),
                sourceVector.Z);
            return position;
        }

        private float GetRandomValue(int currentValue, int maxDistance)
        {
            while (true)
            {
                var value = random.Next(currentValue - maxDistance, currentValue + maxDistance + 5);
                if (Math.Abs(value - currentValue) < 5)
                    continue;
                return value;
            }
        }

        private Node GetRandomCloseNode(Node pNode)
        {
            Node next;
            Node tempNext;
            double shortest;
            double tempShortest;

            next = this.Nodes[random.Next(0, Nodes.Count - 1)];

            while (pNode == next)
            {
                next = this.Nodes[random.Next(0, Nodes.Count - 1)];
            }

            shortest = pNode.distanceFrom(next);
            // 5 Chances to find a closer node
            for (int j = 0; j <= 10; j++)
            {
                // Make sure the next node isn't already a neighbor and isn't the current node
                do
                {
                    tempNext = Nodes[random.Next(0, Nodes.Count)];
                    tempShortest = pNode.distanceFrom(tempNext);
                }
                while (pNode.getNeighbors().Contains(tempNext) || tempShortest == 0);

                // If it is closer than the currently selected next use that as next
                if (shortest > tempShortest)
                {
                    shortest = tempShortest;
                    next = tempNext;
                }
            }
            return next;
        }
    }
}
