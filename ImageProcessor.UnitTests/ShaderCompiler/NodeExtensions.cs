using System;
using System.Collections.Generic;
using System.Linq;
using Diggins.Jigsaw;

namespace ImageProcessor.UnitTests.ShaderCompiler
{
    static class NodeExtensions
    {
        public static Node FindByLabel(this List<Node> nodeList, string label, bool recursive = false)
        {
            if (!recursive)
                return nodeList.Find(p => p.Label == label);
            else
            {
                foreach (var node in nodeList)
                {
                    if (node.Label == label)
                        return node;

                    if (node.Nodes.Any())
                    {
                        var childNode = FindByLabel(node.Nodes, label, recursive);

                        if (childNode != null)
                            return childNode;
                    }
                }

                return null;
            }
        }

        public static Node Find(this List<Node> nodeList, Predicate<Node> predicate, bool recursive)
        {
            if (!recursive)
                return nodeList.Find(predicate);
            else
            {
                foreach (var node in nodeList)
                {
                    if (predicate(node))
                        return node;

                    if (node.Nodes.Any())
                    {
                        var childNode = Find(node.Nodes, predicate, recursive);

                        if (childNode != null)
                            return childNode;
                    }
                }

                return null;
            }
        }

        public static Node FindLeaf(this Node node)
        {
            if (node.IsLeaf)
                return node;

            foreach (var item in node.Nodes)
            {
                var result = FindLeaf(item);

                if (result != null)
                    return result;
            }

            return null;
        }

    }
}
