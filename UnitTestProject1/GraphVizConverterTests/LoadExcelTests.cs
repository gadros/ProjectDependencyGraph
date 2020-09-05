using System.Linq;
using ConvertExcelToDependencyGraphApp;
using GraphVizConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1.GraphVizConverterTests
{
    [TestClass]
    public class LoadExcelTests
    {
        [TestMethod]
        public void Load_Basic_Success()
        {
            StructureOfExcelFile testFileStructure = new StructureOfExcelFile("Work Items", 1, 7, 2, "4,3,2", 6);
            var er = new ExcelReader(testFileStructure, @".\LoadMe1.xlsx");
            ExcelRowsWithDependencies rows = er.Load().LoadedExcel;
            Assert.IsNotNull(rows);
            Assert.IsTrue(rows.Any());
        }
    }

    [TestClass]
    public class ExcelRowTests
    {
        [TestMethod]
        public void Create_WhenDescriptionIsEmpty_DescriptionIsAdded()
        {
            var emptyDesc = new ExcelRow(0, string.Empty, null, 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(emptyDesc.RowDescription));

            emptyDesc = new ExcelRow(0, null, null, 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(emptyDesc.RowDescription));
        }
    }
}