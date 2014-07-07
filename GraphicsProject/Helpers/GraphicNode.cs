using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GraphicsProject.Helpers
{
    class GraphicNode
    {
        private IList<GraphicNode> neighbors = new List<GraphicNode>();
        private IList<GraphicEdge> edges = new List<GraphicEdge>();
        private Cube cube;

        public GraphicNode(Cube pCube)
        {
            cube = pCube;
        }

        public void connect(GraphicNode other, GraphicEdge connection)
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

        public IList<GraphicNode> getNeighbors()
        {
            return neighbors;
        }

        public void select()
        {
            this.cube.select();
        }
    }
}
