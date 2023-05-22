using System.IO;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;

namespace BizHawkBenchmark.Noops
{
    internal class NoopQuickBmpFile : IQuickBmpFile
    {
        public void Copy(IVideoProvider src, IVideoProvider dst)
        {
        }

        public bool Load(IVideoProvider v, Stream s)
        {
            return true;
        }

        public bool LoadAuto(Stream s, out IVideoProvider vp)
        {
            vp = new NullVideo();
            return true;
        }

        public void Save(IVideoProvider v, Stream s, int w, int h)
        {
        }
    }
}

