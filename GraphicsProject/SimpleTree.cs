using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsProject.Helpers;
using OpenTK;

namespace GraphicsProject
{
    public class SimpleTree : Graph
    {
        public SimpleTree(ShaderProgram program)
        {
            var node1 = new  Node(new Cube(program)
                                  {
                                      Position = new Vector3(0, 10, -100)
                                  });
            var node2 = new  Node(new Cube(program)
                                  {
                                      Position = new Vector3(-20, 0, -100)
                                  });
            var node3 = new  Node(new Cube(program)
                                  {
                                      Position = new Vector3(0, 0, -100)
                                  });
            var node4 = new  Node(new Cube(program)
                                  {
                                      Position = new Vector3(20, 0, -100)
                                  });
            var node5 = new  Node(new Cube(program)
                                  {
                                      Position = new Vector3(-25, -10, -100)
                                  });
            var node6 = new  Node(new Cube(program)
                                  {
                                      Position = new Vector3(-20, -10, -100)
                                  });
            var node7 = new  Node(new Cube(program)
                                  {
                                      Position = new Vector3(-5, -10, -100)
                                  });
            var node8 = new  Node(new Cube(program)
                                  {
                                      Position = new Vector3(0, -10, -100)
                                  });
            var node9 = new  Node(new Cube(program)
                                  {
                                      Position = new Vector3(5, -10, -100)
                                  });
            var node10 = new  Node(new Cube(program)
                                  {
                                      Position = new Vector3(-30, -20, -100)
                                  });

            this.Nodes.AddRange(new[] { node1, node2, node3, node4, node5, node6, node7, node8, node9, node10 });
            this.Edges.AddRange(new[]
                                {
                                    new Edge(program, node1, node2),
                                    new Edge(program, node1, node3),
                                    new Edge(program, node1, node4),
                                    new Edge(program, node2, node5),
                                    new Edge(program, node2, node6),
                                    new Edge(program, node3, node7),
                                    new Edge(program, node3, node8),
                                    new Edge(program, node3, node9),
                                    new Edge(program, node5, node10)
                                });
        }
    }
}
