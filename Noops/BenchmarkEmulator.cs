using BizHawk.Emulation.Common;

namespace BizHawkBenchmark.Noops
{
    internal class BenchmarkEmulator : IEmulator
    {
        public BenchmarkEmulator(ControllerDefinition controllerDefinition)
        {
            ControllerDefinition = controllerDefinition;
            ServiceProvider = new BasicServiceProvider(this);
        }

        public void Dispose()
        {
        }

        public IEmulatorServiceProvider ServiceProvider { get; }
        public ControllerDefinition ControllerDefinition { get; }
        public bool FrameAdvance(IController controller, bool render, bool renderSound = true)
        {
            Frame++;
            return true;
        }

        public int Frame { get; private set; }
        public string SystemId => "Benchmark";
        public bool DeterministicEmulation => true;
        public void ResetCounters()
        {
            Frame = 0;
        }
    }
}
