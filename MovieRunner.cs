using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using BizHawkBenchmark.Noops;

namespace BizHawkBenchmark
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class MovieRunner
    {
        private static readonly string[] NesButtons = { "Up", "Down", "Left", "Right", "Select", "Start", "A", "B" };

        private static readonly MovieConfig MovieConfig = new MovieConfig
        {
            MovieEndAction = MovieEndAction.Finish,
            EnableBackupMovies = false,
            MoviesOnDisk = false,
            MovieCompressionLevel = 2,
            VBAStyleMovieLoadState = false,
            DefaultTasStateManagerSettings = new ZwinderStateManagerSettings()
        };

        private readonly IMovieSession _movieMovieSession;
        private readonly IEmulator _emulator;
        private readonly IController _controller;

        private static void Noop()
        {
        }

        public MovieRunner()
        {
            var definition = new BenchmarkControllerDefinition(NesButtons);
            _emulator = new BenchmarkEmulator(definition);
            _movieMovieSession = new MovieSession(MovieConfig, ".", new NoopDialogParent(), new NoopQuickBmpFile(), Noop, Noop);
            var bk2 = new Bk2Movie(_movieMovieSession, "test123.bk2");
            _movieMovieSession.QueueNewMovie(bk2, true, _emulator.SystemId, new Dictionary<string, string>());
            _movieMovieSession.RunQueuedMovie(true, _emulator);
            _controller = new Controller(definition);
            var adapter = new AutoFireStickyXorAdapter { Source = _controller };
            _movieMovieSession.MovieIn = adapter;
        }

        [Benchmark]
        public void RecordFrames()
        {
            for (var i = 0; i < 1; i++)
            {
                _movieMovieSession.HandleFrameBefore();
                _emulator.FrameAdvance(_controller, true);
                _movieMovieSession.HandleFrameAfter();
            }
        }
    }
}
