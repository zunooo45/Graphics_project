using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsProject.Helpers;
using OpenTK;

namespace GraphicsProject
{
    public class SimpleGraph : Graph
    {
        public SimpleGraph(ShaderProgram program)
        {
            var cube1 = new Cube(program);
            cube1.OnLoad();
            cube1.SetPosition(new Vector3(-10, 10, -100));

            var cube2 = new Cube(program);
            cube2.OnLoad();
            cube2.SetPosition(new Vector3(10, 10, -100));

            var cube3 = new Cube(program);
            cube3.OnLoad();
            cube3.SetPosition(new Vector3(10, -10, -100));

            var cube4 = new Cube(program);
            cube4.OnLoad();
            cube4.SetPosition(new Vector3(-10, -10, -100));

            var node1 = new Node(cube1);
            var node2 = new Node(cube2);
            var node3 = new Node(cube3);
            var node4 = new Node(cube4);

            this.Nodes.AddRange(new[] {node1, node2, node3, node4});
            this.Edges.AddRange(new[]
                                {
                                    new Edge(program, node1, node2),
                                    new Edge(program, node2, node3),
                                    new Edge(program, node3, node4),
                                    new Edge(program, node4, node1),
                                    new Edge(program, node3, node1),
                                    new Edge(program, node4, node2),
                                });
        }
    }
}
