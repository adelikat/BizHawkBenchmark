// Assumes BizHawk is cloned in a folder next to this one

using System.Collections.Generic;
using BizHawk.Client.Common;
using BizHawkBenchmark.Noops;

public static class Program
{
    private static readonly string[] NesButtons = { "Up", "Down", "Left", "Right", "Select", "Start", "A", "B" };

    public static void Main(string[] args)
    {
        var movieConfig = new MovieConfig
        {
            MovieEndAction = MovieEndAction.Finish,
            EnableBackupMovies = false,
            MoviesOnDisk = false,
            MovieCompressionLevel = 2,
            VBAStyleMovieLoadState = false,
            DefaultTasStateManagerSettings = new ZwinderStateManagerSettings()
        };

        var definition = new BenchmarkControllerDefinition(NesButtons);
        var emulator = new BenchmarkEmulator(definition);

        var movieSession = new MovieSession(movieConfig, ".", new NoopDialogParent(), new NoopQuickBmpFile(), Noop, Noop);
        var bk2 = new Bk2Movie(movieSession, "test123.bk2");
        movieSession.QueueNewMovie(bk2, true, emulator.SystemId, new Dictionary<string, string>());
        movieSession.RunQueuedMovie(true, emulator);
        var adapter = new AutoFireStickyXorAdapter { Source = new Controller(definition) };
        movieSession.MovieIn = adapter;
        movieSession.HandleFrameBefore();
        movieSession.HandleFrameAfter();
        var test = movieSession.Movie.GetLogEntries();
    }

    private static void Noop()
    {
    }
}



