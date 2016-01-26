using System.Collections.Generic;
using Diggins.Jigsaw;

namespace ImageProcessor.UnitTests.ShaderCompiler
{
    static class NodeExtensions
    {
        public static Node FindByLabel(this List<Node> nodeList, string label)
        {
            return nodeList.Find(p => p.Label == label);
        }
    }
}
