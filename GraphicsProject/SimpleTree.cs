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
            var cube1 = new Cube(program);
            cube1.OnLoad();
            cube1.SetPosition(new Vector3(0, 10, -100));

            var cube2 = new Cube(program);
            cube2.OnLoad();
            cube2.SetPosition(new Vector3(-20, 0, -100));

            var cube3 = new Cube(program);
            cube3.OnLoad();
            cube3.SetPosition(new Vector3(0, 0, -100));

            var cube4 = new Cube(program);
            cube4.OnLoad();
            cube4.SetPosition(new Vector3(20, 0, -100));

            var cube5 = new Cube(program);
            cube5.OnLoad();
            cube5.SetPosition(new Vector3(-25, -10, -100));

            var cube6 = new Cube(program);
            cube6.OnLoad();
            cube6.SetPosition(new Vector3(-20, -10, -100));

            var cube7 = new Cube(program);
            cube7.OnLoad();
            cube7.SetPosition(new Vector3(-5, -10, -100));

            var cube8 = new Cube(program);
            cube8.OnLoad();
            cube8.SetPosition(new Vector3(0, -10, -100));

            var cube9 = new Cube(program);
            cube9.OnLoad();
            cube9.SetPosition(new Vector3(5, -10, -100));

            var cube10 = new Cube(program);
            cube10.OnLoad();
            cube10.SetPosition(new Vector3(-30, -20, -100));

            var node1 = new  Node(cube1);
            var node2 = new Node(cube2);
            var node3 = new Node(cube3);
            var node4 = new Node(cube4);
            var node5 = new Node(cube5);
            var node6 = new Node(cube6);
            var node7 = new Node(cube7);
            var node8 = new Node(cube8);
            var node9 = new Node(cube9);
            var node10 = new Node(cube10);

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
