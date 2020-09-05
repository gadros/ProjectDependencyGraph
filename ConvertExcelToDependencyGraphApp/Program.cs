using System;
using System.IO;
using DotNetGraph;
using DotNetGraph.Extensions;
using GraphVizConverter;

namespace ConvertExcelToDependencyGraphApp
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInfo excelFile = GetExcelFileName(args);
            if (excelFile == null)
                return;
            (StructureOfExcelFile se, string settingsError) readSettingsResult = GetStructureOfExcelFile();
            if (readSettingsResult.se == null)
            {
                ShowHelpOnStructure(readSettingsResult.settingsError);
                return;
            }

            var er = new ExcelReader(readSettingsResult.se, excelFile.FullName);
            (ExcelRowsWithDependencies LoadedExcel, string LoadingErrors) excelLoadResult = er.Load();
            if (excelLoadResult.LoadedExcel == null)
            {
                LogError(excelLoadResult.LoadingErrors);
                return;
            }

            (DotGraph GeneratedGraph, string ConversionErrors) conversionResult = ConvertToGraph(excelLoadResult.LoadedExcel);

            string graphText = conversionResult.GeneratedGraph?.Compile();
            if (!string.IsNullOrWhiteSpace(graphText))
            {
                WriteGraphToDotFile(excelFile, graphText);
            }

            if (conversionResult.GeneratedGraph != null)
            {
                string pathsInfo = GenerateCriticalPathAndPathsInfo(conversionResult.GeneratedGraph);
            }

            if (excelLoadResult.LoadingErrors != null)
            {
                LogError(excelLoadResult.LoadingErrors);
            }

            if (conversionResult.ConversionErrors != null)
            {
                LogError(conversionResult.ConversionErrors);
            }
        }

        private static (StructureOfExcelFile structure, string settingsError) GetStructureOfExcelFile()
        {
            var excelStruct = new StructureOfExcelFile(
                Properties.Settings.Default.WorkBreakDownSheet,
                Properties.Settings.Default.RowKeyColumn,
                Properties.Settings.Default.EstimatedffortColumn,
                Properties.Settings.Default.FirstDataRow,
                Properties.Settings.Default.DescriptionColumnFailback,
                Properties.Settings.Default.ParentRowsColumn);

            if (excelStruct.Valid)
                return (excelStruct, string.Empty);

            return (null, excelStruct.FirstError);
        }

        private static void ShowHelpOnStructure(string settingError)
        {
            string helpOnSettings =
                "The application relies on valid settings that describe the Excel structure. these values should be set application's configuration file in the userSettings element (you need to configure these in  the .exe.config file)";
            LogError($"{helpOnSettings}\nfirst error found: {settingError}");
        }

        private static string GenerateCriticalPathAndPathsInfo(DotGraph projectGraph)
        {
            PathsInGraphCalculator pgc = new PathsInGraphCalculator(projectGraph);
            return pgc.Calculate();
        }

        private static void WriteGraphToDotFile(FileInfo excelFile, string graphText)
        {
            string nameNoExtension = excelFile.Name.Substring(0, excelFile.Name.Length - excelFile.Extension.Length);
            string folderName = excelFile.DirectoryName;
            if (string.IsNullOrWhiteSpace(folderName))
                folderName = @".\";
            string targetFile = Path.Combine(folderName, nameNoExtension) + ".dot";
            File.WriteAllText(targetFile, graphText);
            Console.WriteLine($"graph written to file {targetFile}");
        }

        private static FileInfo GetExcelFileName(string[] args)
        {
            if (args.Length == 0)
            {
                LogError($"FAILURE! must provide the path to an Excel file in order to create a dependency graph.\nProgram existed");
                return null;
            }

            var fi = new FileInfo(args[0]);
            if (!fi.Exists)
            {
                LogError($"FAILURE! couldn't find file {args[0]}.\nProgram existed");
                return null;
            }

            if (!fi.Extension.Contains("xls"))
            {
                LogError($"FAILURE! {fi.Name} is suspected to not be an Excel file.\nProgram existed");
                return null;
            }

            return fi;
        }

        private static (DotGraph GeneratedGraph, string ConversionErrors) ConvertToGraph(ExcelRowsWithDependencies excel)
        {
            ExcelRowsConverterToGraphViz convertor = new ExcelRowsConverterToGraphViz();
            return convertor.Convert(excel);
        }

        private static void LogError(string errorMessge)
        {
            Console.WriteLine(errorMessge);
        }
    }
}