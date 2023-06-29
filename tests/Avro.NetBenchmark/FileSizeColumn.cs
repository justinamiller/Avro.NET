using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNETBenchmark
{
    public class FileSizeColumn : IColumn
    {
        public string Id => nameof(FileSizeColumn);

        public string ColumnName => "FileSize";

        public string Legend => "Allocated memory on disk after all records are serialized (1KB = 1024B)";

        public UnitType UnitType => UnitType.Size;

        public bool AlwaysShow => true;

        public ColumnCategory Category => ColumnCategory.Metric;

        public int PriorityInCategory => 0;

        public bool IsNumeric => true;

        public bool IsAvailable(Summary summary) => true;

        public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;

        public string GetValue(Summary summary, BenchmarkCase benchmarkCase) => GetValue(summary, benchmarkCase, SummaryStyle.Default);

        public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
        {
            var benchmarkName = benchmarkCase.Descriptor.WorkloadMethod.Name.ToLower();

            var filename = $"disk-size.{benchmarkName}.txt";
            var path = $"{System.AppDomain.CurrentDomain.BaseDirectory}{filename}";
            return File.Exists(path) ? File.ReadAllText(path) : $"file not found";
        }

        public override string ToString() => ColumnName;
    }
}
