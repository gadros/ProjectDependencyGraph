using GraphVizConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1.GraphVizConverterTests
{
    [TestClass]
    public class ExcelRowsWithDependenciesTests
    {
        [TestMethod]
        public void Sort_Basic_SortedCorrectly()
        {
            ExcelRowsWithDependencies excelRows = ExcelRowsForTests.CreateExcelWithParentsDefinedLater();
            int i = 0;
            int[] expectedOrder = {1, 3, 2};
            foreach (ExcelRow row in excelRows)
            {
                Assert.AreEqual(expectedOrder[i], row.Key);
                i++;
            }
        }

        [TestMethod]
        public void Sort_ParentsCombination_SortedCorrectly()
        {
            /*
                new ExcelRow(1, "1", null),
                new ExcelRow(2, "2", "3,6"),
                new ExcelRow(7, "7", "2"),
                new ExcelRow(3, "3", "1"),
                new ExcelRow(6, "6", null),
             */
            ExcelRowsWithDependencies excelRows = ExcelRowsForTests.CreateExcelWithMoreAdvancedParentsDefinedLater();
            int i = 0;
            int[] expectedOrder = {1, 3, 6, 2, 7};
            foreach (ExcelRow row in excelRows)
            {
                Assert.AreEqual(expectedOrder[i], row.Key);
                i++;
            }
        }
    }
}