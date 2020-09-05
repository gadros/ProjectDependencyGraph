using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DotNetGraph;
using DotNetGraph.Edge;
using DotNetGraph.Node;

namespace GraphVizConverter
{
    public class ExcelRowsConverterToGraphViz
    {
        public (DotGraph GeneratedGraph, string ConversionErrors) Convert(ExcelRowsWithDependencies excelRowsWithDependencies)
        {
            var directedGraph = new DotGraph("DependentExcelRows", true);
            Dictionary<ExcelRow, DotNode> rowsInGraph = new Dictionary<ExcelRow, DotNode>();
            StringBuilder conversionFailures = null;

            foreach (ExcelRow row in excelRowsWithDependencies)
            {
                DotNode nodeOfCurrentRow = CreateNode(row);
                directedGraph.Elements.Add(nodeOfCurrentRow);
                rowsInGraph.Add(row, nodeOfCurrentRow);
            }

            foreach (ExcelRow row in excelRowsWithDependencies)
            {
                (Dictionary<ExcelRow, DotNode> Parents, string Failures) getParentsResult = GetAllParents(row, rowsInGraph);
                Dictionary<ExcelRow, DotNode> parentEntries = getParentsResult.Parents;
                if (getParentsResult.Failures != null)
                {
                    if (conversionFailures == null)
                    {
                        conversionFailures = new StringBuilder();
                    }

                    conversionFailures.AppendLine(getParentsResult.Failures);
                }

                var node = directedGraph.Elements.FirstOrDefault(elm =>
                    typeof(DotNode) == elm.GetType() && ((DotNode) elm).Label.Text == row.Key.ToString());
                if (node == null)
                {
                    throw new Exception("couldn't find node");
                }

                var nodeOfCurrentRow = node as DotNode;
                foreach (var parentNode in parentEntries.Values)
                {
                    DotEdge parentEdge = CreateEdgeFromParentToCurrentRow(parentNode, nodeOfCurrentRow, row.EffortEstimate);
                    directedGraph.Elements.Add(parentEdge);
                }
            }

            string errors = conversionFailures?.ToString();
            return (directedGraph, errors);
        }

        private static (Dictionary<ExcelRow, DotNode> Parents, string Failures) GetAllParents(ExcelRow row, Dictionary<ExcelRow, DotNode> rowsInGraph)
        {
            Dictionary<ExcelRow, DotNode> parentEntries = new Dictionary<ExcelRow, DotNode>();
            StringBuilder missingParents = null;
            foreach (int parentKey in row.ParentRows)
            {
                var key = ExcelRow.CreateByKey(parentKey);
                if (!rowsInGraph.ContainsKey(key))
                {
                    if (missingParents == null)
                        missingParents = new StringBuilder();
                    missingParents.AppendLine($"couldn't find parent {parentKey} for row {row}");

                    continue;
                }

                parentEntries.Add(ExcelRow.CreateByKey(parentKey), rowsInGraph[key]);
            }

            string errors = missingParents?.ToString();
            return (parentEntries, errors);
        }

        private DotNode CreateNode(ExcelRow row)
        {
            return new DotNode(row.RowDescription)
            {
                Shape = DotNodeShape.Ellipse,
                Label = row.Key.ToString(),
                FillColor = Color.Coral,
                FontColor = Color.Black,
                Style = DotNodeStyle.Solid,
                Width = 0.5f,
                Height = 0.5f,
                PenWidth = 0.5f,
            };
        }

        private DotEdge CreateEdgeFromParentToCurrentRow(DotNode parentNode, DotNode nodeOfCurrentRow, int effortEstimate)
        {
            return new DotEdge(parentNode, nodeOfCurrentRow)
            {
                ArrowHead = DotEdgeArrowType.Vee,
                ArrowTail = DotEdgeArrowType.None,
                //Color = Color.Red,
                FontColor = Color.Black,
                Label = effortEstimate > 0 ? effortEstimate.ToString() : $"{parentNode.Identifier}->{nodeOfCurrentRow.Identifier}",
                Style = DotEdgeStyle.Solid,
                PenWidth = 0.5f,
            };
        }
    }
}