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
            var node1 = new Node(new Cube(program)
                                 {
                                     Position = new Vector3(-10, 10, -100)
                                 });
            var node2 = new Node(new Cube(program)
                                 {
                                     Position = new Vector3(10, 10, -100)
                                 });
            var node3 = new Node(new Cube(program)
                                 {
                                     Position = new Vector3(10, -10, -100)
                                 });
            var node4 = new Node(new Cube(program)
                                 {
                                     Position = new Vector3(-10, -10, -100)
                                 });
            
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
