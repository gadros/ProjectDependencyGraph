using System.IO;
using DotNetGraph;
using DotNetGraph.Extensions;
using GraphVizConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1.GraphVizConverterTests
{
    [TestClass]
    public class BasicConversionsTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            TestCreateGraph tg = new TestCreateGraph();
            tg.CreateGraphTest();
        }

        [TestMethod]
        public void ConvertToGraphViz_Basic_Success()
        {
            ExcelRowsWithDependencies excel = CreateBasicExcel();

            ExecuteConversion(excel);
        }

        [TestMethod]
        public void ConvertToGraphViz_ParallelFlows_Success()
        {
            ExcelRowsWithDependencies excel = CreateExcelWithParallelFlows();

            ExecuteConversion(excel);
        }

        [TestMethod]
        public void ConvertToGraphViz_ParerntsDefinedLater_Success()
        {
            ExcelRowsWithDependencies excel = ExcelRowsForTests.CreateExcelWithParentsDefinedLater();

            ExecuteConversion(excel);
        }

        [TestMethod]
        public void ConvertToGraphViz_ParerntsDefinedLaterMoreAdvanced_Success()
        {
            ExcelRowsWithDependencies excel = ExcelRowsForTests.CreateExcelWithMoreAdvancedParentsDefinedLater();

            ExecuteConversion(excel);
        }

        [TestMethod]
        public void ConvertToGraphViz_NoAllParerntsDefined_ConvertedByThrows()
        {
            ExcelRowsWithDependencies excel = CreateExcelNotAllParentsDefined();

            ExecuteConversion(excel);
        }

        private static void ExecuteConversion(ExcelRowsWithDependencies excel)
        {
            ExcelRowsConverterToGraphViz convertor = new ExcelRowsConverterToGraphViz();
            DotGraph graph = convertor.Convert(excel).GeneratedGraph;
            string graphInText = graph.Compile();
            File.WriteAllText(@"C:\dev\examples\ProjectDependencyGraph\UnitTestProject1\bin\Debug\ConvertToGraphViz1.dot", graphInText);
        }

        private ExcelRowsWithDependencies CreateExcelNotAllParentsDefined()
        {
            return ExcelRowsWithDependencies.Empty;
        }

        private static ExcelRowsWithDependencies CreateBasicExcel()
        {
            ExcelRowsWithDependencies excel = new ExcelRowsWithDependencies(new ExcelRow[]
            {
                new ExcelRow(1, "1", null, 0),
                new ExcelRow(2, "2", "1", 0),
                new ExcelRow(3, "3", "1", 0),
            });
            return excel;
        }

        private static ExcelRowsWithDependencies CreateExcelWithParallelFlows()
        {
            ExcelRowsWithDependencies excel = new ExcelRowsWithDependencies(new ExcelRow[]
            {
                new ExcelRow(1, "1", null, 0),
                new ExcelRow(2, "2", "1", 0),
                new ExcelRow(3, "3", "1", 0),

                new ExcelRow(4, "4", null, 0),
                new ExcelRow(5, "5", "4", 0),
                new ExcelRow(6, "6", "4", 0),

                new ExcelRow(7, "7", "4,1", 0),
            });
            return excel;
        }
    }

    internal static class ExcelRowsForTests
    {
        internal static ExcelRowsWithDependencies CreateExcelWithParentsDefinedLater()
        {
            ExcelRowsWithDependencies excel = new ExcelRowsWithDependencies(new ExcelRow[]
            {
                new ExcelRow(1, "1", null, 0),
                new ExcelRow(2, "2", "3", 0),
                new ExcelRow(3, "3", "1", 0),
            });
            return excel;
        }

        internal static ExcelRowsWithDependencies CreateExcelWithMoreAdvancedParentsDefinedLater()
        {
            ExcelRowsWithDependencies excel = new ExcelRowsWithDependencies(new ExcelRow[]
            {
                new ExcelRow(1, "1", null, 0),
                new ExcelRow(2, "2", "3,6", 0),
                new ExcelRow(7, "7", "2", 0),
                new ExcelRow(3, "3", "1", 0),
                new ExcelRow(6, "6", null, 0),
            });
            return excel;
        }
    }
}
