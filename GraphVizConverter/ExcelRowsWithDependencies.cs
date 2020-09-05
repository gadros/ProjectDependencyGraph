using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphVizConverter
{
    public class ExcelRowsWithDependencies: IEnumerable<ExcelRow>
    {
        private readonly SortedSet<ExcelRow> _rows;

        public static readonly ExcelRowsWithDependencies Empty = new ExcelRowsWithDependencies(new List<ExcelRow>());

        public ExcelRowsWithDependencies(IEnumerable<ExcelRow> rows)
        {
            HashSet<ExcelRow> parsedRows = ParseRows(rows);
            _rows = Sort(parsedRows);
            parsedRows = new HashSet<ExcelRow>(_rows);
            _rows = Sort(parsedRows);
        }

        private HashSet<ExcelRow> ParseRows(IEnumerable<ExcelRow> rows)
        {
            if (rows == null)
            {
                throw new ArgumentNullException("no input");
            }

            var parsedRows = new HashSet<ExcelRow>();

            foreach (ExcelRow row in rows)
            {
                row.Parse();
                if (parsedRows.Contains(row))
                {
                    throw new ArgumentException($"Excel row {row} already exists");
                }
                parsedRows.Add(row);
            }

            return parsedRows;
        }

        private SortedSet<ExcelRow> Sort(HashSet<ExcelRow> rows)
        {
            var dummyToStart = new ExcelRow(0, "dummy", null, 0);
            var sorted = new SortedSet<ExcelRow>(dummyToStart);

            foreach (ExcelRow row in rows)
            {
                sorted.Add(row);
            }

            sorted.Remove(dummyToStart);
            return sorted;
        }

        public IEnumerator<ExcelRow> GetEnumerator()
        {
            return _rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}