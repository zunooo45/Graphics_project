using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GraphicsProject.Helpers
{
    class GraphicEdge
    {
        private GraphicNode start;
        private GraphicNode end;
        private Line line;

        public GraphicEdge(ShaderProgram program, GraphicNode pStart, GraphicNode pEnd)
        {
            start = pStart;
            end = pEnd;

            start.connect(end, this);
            start.connect(start, this);

            line = new Line(program, start.getPostion(), end.getPostion());
        }

        public Line getLine()
        {
            return this.line;
        }
    }
}
