namespace Dozy;

internal static class Program
{
    private const string MutexName = @"Local\Dozy.SingleInstance";

    [STAThread]
    private static void Main()
    {
        using var mutex = new Mutex(initiallyOwned: true, MutexName, out bool createdNew);

        if (!createdNew)
        {
            return;
        }

        try
        {
            ApplicationConfiguration.Initialize();

            using var context = new DozyApplicationContext();
            Application.Run(context);
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }
}
