using System.Collections.Generic;
using DotNetGraph;
using DotNetGraph.Edge;
using DotNetGraph.Node;

namespace ConvertExcelToDependencyGraphApp
{
    //not yet implemented
    internal class PathsInGraphCalculator
    {
        private readonly DotGraph _projectGraph;

        public PathsInGraphCalculator(DotGraph projectGraph)
        {
            _projectGraph = projectGraph;
        }

        public string Calculate()
        {
            LinkedList<DotNode> sortedNodes = SortGraphNodes();
            Dictionary<DotNode, Dictionary<DotEdge, List<DotEdge>>> pathsOnGraph = ConstructPaths(sortedNodes);
            Dictionary<DotNode, (string PathDescription, int TotalPahtEffort)> nodesWork = new Dictionary<DotNode, (string PathDescription, int TotalPahtEffort)>();
            foreach (DotNode node in sortedNodes)
            {
                if (pathsOnGraph.ContainsKey(node))
                {
                    nodesWork.Add(node, CalculatePathsWork(node, pathsOnGraph[node]));
                }
            }

            return ConvertWorkPathToText(nodesWork);
        }

        private LinkedList<DotNode> SortGraphNodes()
        {
            LinkedList<DotNode> ll = new LinkedList<DotNode>();
            return ll;
        }

        private Dictionary<DotNode, Dictionary<DotEdge, List<DotEdge>>> ConstructPaths(LinkedList<DotNode> sortedNodes)
        {
            return null;
        }

        private (string PathDescription, int TotalPahtEffort) CalculatePathsWork(DotNode node, Dictionary<DotEdge, List<DotEdge>> dictionary)
        {
            return (string.Empty, 0);
        }

        private string ConvertWorkPathToText(Dictionary<DotNode, (string PathDescription, int TotalPahtEffort)> nodesWork)
        {
            return string.Empty;
        }
    }
}