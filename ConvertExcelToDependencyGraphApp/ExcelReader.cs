using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using GraphVizConverter;
using OfficeOpenXml;
using ExcelRow = GraphVizConverter.ExcelRow;

namespace ConvertExcelToDependencyGraphApp
{
    internal class ExcelReader
    {
        private const string c_excelFileName = @"C:\dev\examples\ProjectDependencyGraph\UnitTestProject1\bin\Debug\In Cloud - Project plan.xlsx";

        private readonly string _file;
        private readonly StructureOfExcelFile _structureOfExcelFile;

        public ExcelReader(StructureOfExcelFile excelFileDescriptor, string excelFileName = c_excelFileName)
        {
            _file = excelFileName;
            _structureOfExcelFile = excelFileDescriptor;
        }

        public (ExcelRowsWithDependencies LoadedExcel, string LoadingErrors) Load()
        {
            try
            {
                FileInfo existingFile = new FileInfo(_file);
                List<ExcelRow> rows;
                StringBuilder loadErrors = null;
                using (ExcelPackage package = new ExcelPackage(existingFile))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    ExcelWorksheet worksheet = package.Workbook.Worksheets[_structureOfExcelFile.WorkBreakDownSheetName];

                    if (worksheet == null)
                    {
                        return (null, $"{existingFile.Name} doesn't have worksheet '{_structureOfExcelFile.WorkBreakDownSheetName}'. cannot continue.");
                    }

                    int rowCount = worksheet.Dimension.End.Row;
                    rows = new List<ExcelRow>(rowCount);
                    for (int i = (int)_structureOfExcelFile.FirstRowWithData; i <= rowCount; i++)
                    {
                        string rowKey = worksheet.Cells[i, _structureOfExcelFile.RowKeyColumn].Value?.ToString();
                        if (string.IsNullOrWhiteSpace(rowKey))
                        {
                            continue;
                        }

                        int rowId = int.Parse(rowKey);
                        string description = worksheet.Cells[i, 4].Value?.ToString();
                        if (string.IsNullOrWhiteSpace(description))
                        {
                            description = worksheet.Cells[i, 3].Value?.ToString();
                            if (string.IsNullOrWhiteSpace(description))
                                description = worksheet.Cells[i, 2].Value?.ToString();
                        }

                        if (string.IsNullOrWhiteSpace(description))
                        {
                            if (loadErrors == null)
                                loadErrors = new StringBuilder();
                            loadErrors.AppendLine($"couldn't load row {rowId} because couldn't set the description");
                            continue;
                        }
                        string parentRows = worksheet.Cells[i, _structureOfExcelFile.ParentRowsColumn].Value?.ToString();

                        string effort = worksheet.Cells[i, _structureOfExcelFile.EffortEstimationColumn].Value?.ToString();
                        int effortEstimate = string.IsNullOrWhiteSpace(effort) ? 0 : int.Parse(effort);
                        rows.Add(new ExcelRow(rowId, description, parentRows, effortEstimate));
                    }
                }
                return (new ExcelRowsWithDependencies(rows), loadErrors?.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (null, null);
            }
        }
    }
}