using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GraphicsProject.Helpers
{
    public class Node : IDisposable
    {
        private IList<Node> neighbors = new List<Node>();
        private IList<Edge> edges = new List<Edge>();
        private Cube cube;
        private Dictionary<String, Vector3[]> modes = new Dictionary<String, Vector3[]>();
        private String mode;

        public bool visited { get; set; }
        public Node lastNode { get; set; }
        public Double distance { get; set; }

        public Node(Cube pCube)
        {
            cube = pCube;
            visited = false;
            distance = 999999999;

            modes.Add("Unvisited", new Vector3[] { new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.25f, 0.25f, 0.25f) });
            modes.Add("Visited",    new Vector3[] { new Vector3(1, 0, 0), new Vector3(0.5f, 0, 0) });
            modes.Add("Visiting", new Vector3[] { new Vector3(1, 1, 1), new Vector3(0.25f, 0.25f, 0.25f) });
            modes.Add("Path",       new Vector3[] { new Vector3(1, 1, 0), new Vector3(0.5f, 0.5f, 0) });
            modes.Add("Start",      new Vector3[] { new Vector3(0, 1, 0), new Vector3(0, 0.5f, 0) });
            modes.Add("End",        new Vector3[] { new Vector3(0, 0, 1), new Vector3(0, 0, 0.5f) });
            //this.setMode("Unvisited");
        }

        public String getMode()
        {
            return mode;
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

        public void ResetColoring()
        {
            this.cube.ResetColoring();
        }

        public void select()
        {
            this.cube.select();
            //var pos = this.getPostion();
            //var otherPos = other.getPostion();

            //Double difX = (Double)Math.Abs(otherPos.X - pos.X);
            //Double difY = (Double)Math.Abs(otherPos.Y - pos.Y);
            //Double difZ = (Double)Math.Abs(otherPos.Z - pos.Z);

            //return Math.Sqrt(difX * difX + difY * difY + difZ * difZ);
        }

        public Cube getCube()
        {
            return this.cube;
        }

        public void Dispose()
        {
            this.cube.Dispose();
        }
    }
}
