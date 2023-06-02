using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BizHawk.Client.EmuHawk;

namespace BizHawkBenchmark;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net70)]
public class SortedSetBenchmark
{
    private readonly SortedSet<Cell> _selectedItems = new(new SortCell());

    [IterationSetup]
    public void Setup()
    {
        for (int column = 0; column < 20; column++)
        {
            var rollColumn = new RollColumn
            {
                Text = column.ToString(),
                Name = column.ToString(),
                Type = ColumnType.Text
            };
            for (int row = 0; row < 20_000; row++)
            {
                _selectedItems.Add(new Cell
                {
                    Column = rollColumn,
                    RowIndex = row
                });
            }
        }
    }

    [Benchmark]
    public void RemoveWhere10K()
    {
        if (_selectedItems.Count != 20_000 * 20) throw new InvalidOperationException();
        _selectedItems.RemoveWhere(cell => cell.RowIndex >= 10_000);
    }
}
