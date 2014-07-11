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
            var currentPosition = new Vector3(0, 0, -100);

            for (int i = 0; i < 50; i++)
            {
                this.Nodes.Add(new Node(new Cube(program)
                                        {
                                            Position = currentPosition
                                        }));
                currentPosition = this.GetRandomLocationFromPosition(currentPosition);
            }

            Node previousNode = this.Nodes[0];
            foreach (var node in Nodes.Skip(1))
            {
                this.Edges.Add(new Edge(program, node, previousNode));
                previousNode = node;
            }
            this.Edges.Add(new Edge(program, previousNode, this.Nodes[0]));

            //this.Edges.AddRange(new[]
            //                    {
            //                        new Edge(program, node1, node2),
            //                        new Edge(program, node2, node3),
            //                        new Edge(program, node3, node4),
            //                        new Edge(program, node4, node1),
            //                        new Edge(program, node3, node1),
            //                        new Edge(program, node4, node2),
            //                    });
        }

        private Random random;
        private Vector3 GetRandomLocationFromPosition(Vector3 sourceVector)
        {
            const int maxDistance = 30;
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
                var value = random.Next(currentValue - maxDistance, currentValue + maxDistance);
                if (Math.Abs(value - currentValue) < 5)
                    continue;
                return value;
            }
        }
    }
}
