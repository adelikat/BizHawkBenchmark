// Assumes BizHawk is cloned in a folder next to this one

using System.Collections.Generic;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using BizHawkBenchmark.Noops;

public static class Program
{
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

        var emulator = new NullEmulator();

        var movieSession = new MovieSession(movieConfig, ".", new NoopDialogParent(), new NoopQuickBmpFile(), Noop, Noop);
        var bk2 = new Bk2Movie(movieSession, "test123.bk2");
        movieSession.QueueNewMovie(bk2, true, emulator.SystemId, new Dictionary<string, string>());
        movieSession.RunQueuedMovie(true, emulator);
    }

    private static void Noop()
    {
    }
}



