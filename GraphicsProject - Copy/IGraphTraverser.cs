using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsProject.Helpers;

namespace GraphicsProject
{
    public interface IGraphTraverser
    {
        IEnumerable<Node> TraversalOrder { get; }
    }
}
