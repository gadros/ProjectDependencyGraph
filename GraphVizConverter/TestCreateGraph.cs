using System;
using System.Drawing;
using System.IO;
using DotNetGraph;
using DotNetGraph.Edge;
using DotNetGraph.Extensions;
using DotNetGraph.Node;
using DotNetGraph.SubGraph;

namespace GraphVizConverter
{
    //can be ignored. it's a class that demonstrates the creation of a simple graph. used to get a hang of it
    public class TestCreateGraph
    {
        public void CreateGraphTest()
        {
            var graph = new DotGraph("MyGraph");

            var directedGraph = new DotGraph("MyDirectedGraph", true);

            //******************************************

            var myNode1 = new DotNode("Node 1")
            {
                Shape = DotNodeShape.Ellipse,
                Label = "Node 1",
                FillColor = Color.Coral,
                FontColor = Color.Black,
                Style = DotNodeStyle.Solid,
                Width = 0.5f,
                Height = 0.5f,
                PenWidth = 0.5f
            };

            // Add the node to the graph
            graph.Elements.Add(myNode1);

            var myNode2 = new DotNode("Node 2")
            {
                Shape = DotNodeShape.Oval,
                Label = "Node 2",
                FillColor = Color.Coral,
                FontColor = Color.Black,
                Style = DotNodeStyle.Solid,
                Width = 0.5f,
                Height = 0.5f,
                PenWidth = 0.5f
            };

            // Add the node to the graph
            graph.Elements.Add(myNode2);

            //******************************************

            // Create an edge with identifiers
            //var myEdge = new DotEdge("myNode1", "myNode2");

            // Create an edge with nodes and attributes
            var myEdge = new DotEdge(myNode1, myNode2)
            {
                ArrowHead = DotEdgeArrowType.Vee,
                ArrowTail = DotEdgeArrowType.Dot,
                //Color = Color.Red,
                FontColor = Color.Black,
                //Label = "1->2",
                Style = DotEdgeStyle.Solid,
                PenWidth = 0.5f,
            };

            // Add the edge to the graph
            graph.Elements.Add(myEdge);


            //******************************************

            // Subgraph identifier need to start with "cluster" to be identified as a cluster
            //var mySubGraph = new DotSubGraph("cluster_0");

            // Create a subgraph with attributes (only used for cluster)
            var subGraph = new DotSubGraph("cluster_0")
            {
                //Color = Color.Red,
                //Style = DotSubGraphStyle.Dashed,
                Label = "My subgraph!"
            };

            // Add node, edge, subgraph
            //subGraph.Elements.Add(myNode);
            subGraph.Elements.Add(myEdge);
            //subGraph.Elements.Add(mySubGraph2);

            // Add subgraph to main graph
            graph.Elements.Add(subGraph);

            //******************************************

            // Non indented version
            //var dot = graph.Compile();
            // Indented version
            var dot = graph.Compile(true);

            // Save it to a file
            File.WriteAllText("myFile.dot", dot);
        }
    }
}
