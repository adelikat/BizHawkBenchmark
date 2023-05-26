using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using BizHawkBenchmark.Noops;

namespace BizHawkBenchmark
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.Method)]
    [RankColumn]
    [SimpleJob(RuntimeMoniker.Net48)]
    [SimpleJob(RuntimeMoniker.Net70)]
    public class MovieRunner
    {
        private static readonly string[] NesButtons = { "P1 Up", "P1 Down", "P1 Left", "P1 Right", "P1 Select", "P1 Start", "P1 A", "P1 B" };

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
        private readonly SimpleController _unpressedController;
        private readonly SimpleController _pressedController;

        private readonly IMovieSession _movieSessionForSaving;

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
            _unpressedController = new SimpleController(definition);
            _pressedController = new SimpleController(definition);

            // Is all this really necessary?
           
            foreach (var btn in definition.BoolButtons)
            {
               _pressedController[btn] = true;
            }
            
            var adapter = new AutoFireStickyXorAdapter { Source = _unpressedController };
            _movieMovieSession.MovieIn = adapter;

            _movieSessionForSaving = new MovieSession(MovieConfig, ".", new NoopDialogParent(), new NoopQuickBmpFile(), Noop, Noop);
            var bk2ForSaving = new Bk2Movie(_movieSessionForSaving, "saving123.bk2");
            _movieMovieSession.QueueNewMovie(bk2ForSaving, true, _emulator.SystemId, new Dictionary<string, string>());
            _movieMovieSession.RunQueuedMovie(true, _emulator);
            for (int i = 0; i < 100_000; i++)
            {
                _movieMovieSession.HandleFrameBefore();
                _movieMovieSession.HandleFrameAfter();
            }

            _movieMovieSession.Movie.Save();
        }

        //[GlobalSetup]
        //public void Blah()
        //{
        //}

        [Benchmark]
        public void Save100k()
        {
            _movieMovieSession.Movie.Save();
        }

        [Benchmark]
        public void Load100k()
        {
            _movieMovieSession.Movie.Load();
        }

        [Benchmark]
        public void RecordEmptyFrame()
        {
            _movieMovieSession.MovieIn.Source = _unpressedController;
            _movieMovieSession.HandleFrameBefore();
            _emulator.FrameAdvance(_unpressedController, true);
            _movieMovieSession.HandleFrameAfter();
        }

        [Benchmark]
        public void RecordNonEmptyFrame()
        {
            _movieMovieSession.MovieIn.Source = _pressedController;
            _movieMovieSession.HandleFrameBefore();
            _emulator.FrameAdvance(_pressedController, true);
            _movieMovieSession.HandleFrameAfter();
        }

        [Benchmark]
        public void RecordEmptyFrame100K()
        {
            _movieMovieSession.MovieIn.Source = _unpressedController;

            for (int i = 0; i < 100_000; i++)
            {
                _movieMovieSession.HandleFrameBefore();
                _emulator.FrameAdvance(_unpressedController, true);
                _movieMovieSession.HandleFrameAfter();
            }
        }

        [Benchmark]
        public void RecordNonEmptyFrame100K()
        {
            _movieMovieSession.MovieIn.Source = _pressedController;

            for (int i = 0; i < 100_000; i++)
            {
                _movieMovieSession.HandleFrameBefore();
                _emulator.FrameAdvance(_pressedController, true);
                _movieMovieSession.HandleFrameAfter();
            }
        }
    }
}
