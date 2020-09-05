
using System;
using System.Collections.Generic;

namespace ConvertExcelToDependencyGraphApp
{
    internal class StructureOfExcelFile
    {
        internal bool Valid { get; }
        internal string FirstError { get; }

        private StructureOfExcelFile()
        {
            
        }

        public StructureOfExcelFile(string workBreakDownSheetName, byte rowKeyColumn, byte effortEstimationColumn, uint firstRowWithData,
            string descriptionColumnsFailbackColumns, byte parentRowsColumn)
        {
            if (string.IsNullOrEmpty(workBreakDownSheetName))
            {
                Valid = false;
                if (rowKeyColumn == 0 && effortEstimationColumn == 0 && firstRowWithData == 0 &&
                    string.IsNullOrEmpty(descriptionColumnsFailbackColumns))
                {
                    FirstError = "not Excel structure is was given!";
                    return;
                }
                FirstError = "not name given for work break down Excel sheet";
                return;
            }

            WorkBreakDownSheetName = workBreakDownSheetName;

            if (rowKeyColumn == 0)
            {
                Valid = false;
                FirstError = "not column number was given for the row key column";
                return;
            }

            RowKeyColumn = rowKeyColumn;

            if (effortEstimationColumn == 0)
            {
                Valid = false;
                FirstError = "not column number was given for the column containing the efforts estimations";
                return;
            }

            EffortEstimationColumn = effortEstimationColumn;

            if (firstRowWithData == 0)
            {
                Valid = false;
                FirstError = "not row number was given to state from where the data begins";
                return;
            }

            FirstRowWithData = firstRowWithData;
            string potentialDescriptionCols = descriptionColumnsFailbackColumns;
            string[] splitColumnIds = potentialDescriptionCols.Split(new[] {','}, System.StringSplitOptions.RemoveEmptyEntries);
            if (splitColumnIds != null)
            {
                int i = 0;
                byte[] descCols = new byte[splitColumnIds.Length];
                foreach (string column in splitColumnIds)
                {
                    if (byte.TryParse(column, out byte colNumber))
                    {
                        descCols[i] = colNumber;
                        i++;
                    }
                }

                //we'll set the value only if the input was meaningful with no errors
                if (i == 0 || i != splitColumnIds.Length)
                {
                    Valid = false;
                    FirstError =
                        "no column id(s) was given to take the work description from (you can provide multiple column numbers separated by comma and the application will try each until a value is found";
                    return;
                }

                DescriptionColumnsFailBackColumns = descCols;
            }


            if (parentRowsColumn == 0)
            {
                Valid = false;
                FirstError =
                    "not column number was given for parent rows. the application assumes hierarchical work order. the parent rows column is expected to contain integer values each representing another row with work that precedeces this work";
                return;
            }

            ParentRowsColumn = parentRowsColumn;
            try
            {
                var ht = new HashSet<uint>() {ParentRowsColumn, FirstRowWithData, RowKeyColumn, EffortEstimationColumn};
                for (int i = 0; i < DescriptionColumnsFailBackColumns.Length; i++)
                {
                    ht.Add(DescriptionColumnsFailBackColumns[i]);
                }
            }
            catch
            {
                FirstError = "some column values were given more than once. your setup is wrong";
                Valid = false;
            }

            Valid = true;
        }

        internal string WorkBreakDownSheetName{ get;}
        internal byte RowKeyColumn{ get;}
        internal byte EffortEstimationColumn{ get;}
        internal uint FirstRowWithData { get;}
        internal byte ParentRowsColumn{ get;}
        internal byte[] DescriptionColumnsFailBackColumns { get; }
    }
}