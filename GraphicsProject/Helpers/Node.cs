using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GraphicsProject.Helpers
{
    class Node
    {
        private IList<Node> neighbors = new List<Node>();
        private IList<Edge> edges = new List<Edge>();
        private Cube cube;

        public Node(Cube pCube)
        {
            cube = pCube;
        }

        public void connect(Node other, Edge connection)
        {
            if (!neighbors.Contains(other))
            {
                neighbors.Add(other);
                edges.Add(connection);
            }
        }

        public Vector3 getPostion()
        {
            return cube.getPosition(); 
        }

        public IList<Node> getNeighbors()
        {
            return neighbors;
        }

        public void select()
        {
            this.cube.select();
        }
    }
}
