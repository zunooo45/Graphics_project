using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GraphicsProject.Helpers
{
    public class Edge
    {
        private Node start;
        private Node end;
        private Line line;

        public Edge(ShaderProgram program, Node pStart, Node pEnd)
        {
            start = pStart;
            end = pEnd;

            start.connect(end, this);
            end.connect(start, this);

            line = new Line(program, start.getPostion(), end.getPostion());
            line.OnLoad();
        }

        public Line getLine()
        {
            return this.line;
        }
    }
}
