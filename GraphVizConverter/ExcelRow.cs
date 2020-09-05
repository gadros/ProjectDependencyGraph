using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphVizConverter
{
    public struct ExcelRow : IEquatable<ExcelRow>, IComparer<ExcelRow>
    {
        public int Key { get; }
        public string RowDescription { get; }
        private readonly string _potentialParentRows;
        internal int EffortEstimate { get; }
        internal int[] ParentRows { get; private set; }

        public ExcelRow(int rowKey, string rowRowDescription, string parentRows, int effortEstimate)
        {
            Key = rowKey;
            RowDescription = rowRowDescription;
            if (string.IsNullOrWhiteSpace(RowDescription))
                RowDescription = "no description"; // when not set the created DotNode will cause graph compilation to crash. preferred to set it here over logic there
            _potentialParentRows = parentRows;
            ParentRows = new int[0];
            EffortEstimate = effortEstimate;
        }

        internal static ExcelRow CreateByKey(int rowKey)
        {
            return new ExcelRow(rowKey, null,null, 0);
        }

        public bool Equals(ExcelRow other)
        {
            return Key == other.Key;
        }

        public int Compare(ExcelRow x, ExcelRow y)
        {
            if (!x.ParentRows.Any())
                x.Parse();
            if (!y.ParentRows.Any())
                y.Parse();

            if(!x.ParentRows.Any() && !y.ParentRows.Any())
                return x.Key-y.Key;

            if(!x.ParentRows.Any())
                x.ParentRows = new []{0};
            else
                y.ParentRows = new []{0};
            
            if (y.ParentRows.Any() && y.ParentRows.Contains(x.Key))
            {
                return -1;
            }

            if (x.ParentRows.Any() && x.ParentRows.Contains(y.Key))
            {
                return 1;
            }

            return x.Key-y.Key;
        }

        public override bool Equals(object obj)
        {
            return obj is ExcelRow other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Key;
        }

        public static bool operator ==(ExcelRow left, ExcelRow right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ExcelRow left, ExcelRow right)
        {
            return !left.Equals(right);
        }

        internal void Parse()
        {
            if (_potentialParentRows != null && !string.IsNullOrWhiteSpace(_potentialParentRows))
            {
                string[] parents = _potentialParentRows.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                if (parents.Any())
                {
                    var parentRows = new int[parents.Length];
                    for (int i = 0; i < parents.Length; i++)
                    {
                        if (!int.TryParse(parents[i], out parentRows[i]))
                            throw new ArgumentException(
                                $"failed parsing. for : {Key}, {RowDescription} couldn't parse parent rows: {_potentialParentRows}. expected a list of integer values separated by commas");
                    }

                    ParentRows = parentRows;
                }
            }
        }

        public override string ToString()
        {
            return $"Key={Key}, Description={RowDescription}";
        }
    }
}