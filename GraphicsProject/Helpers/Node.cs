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
        private Dictionary<String, Vector3[]> modes = new Dictionary<String, Vector3[]>();

        public Node(Cube pCube)
        {
            cube = pCube;
            modes.Add("Unvisited",  new Vector3[] { new Vector3(1, 0, 0), new Vector3(0.5f, 0, 0) });
            modes.Add("Visited",    new Vector3[] { new Vector3(1, 1, 0), new Vector3(0.5f, 0.5f, 0) });
            modes.Add("Start",      new Vector3[] { new Vector3(0, 1, 0), new Vector3(0, 0.5f, 0) });
            modes.Add("End",        new Vector3[] { new Vector3(0, 0, 1), new Vector3(0, 0, 0.5f) });
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

        public void setMode(String pMode)
        {
            Vector3 priColor = modes[pMode][0];
            Vector3 secColor = modes[pMode][1];
            this.cube.setColor(priColor, secColor);
        }


    }
}
