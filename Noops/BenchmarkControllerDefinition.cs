using BizHawk.Emulation.Common;

namespace BizHawkBenchmark.Noops
{
    internal class BenchmarkControllerDefinition : ControllerDefinition
    {
        public BenchmarkControllerDefinition(string[] boolButtons) : base("Benchmark Controller")
        {
            BoolButtons = boolButtons;
        }

        public BenchmarkControllerDefinition(ControllerDefinition copyFrom, string? withName = null)
            : base(copyFrom, withName)
        {
        }
    }
}
