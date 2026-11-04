using System.Runtime.InteropServices;

namespace Dozy;

/// <summary>
/// Keeps the current thread's execution state in sync with Dozy's on/off state.
/// SetThreadExecutionState is thread-affine, so this service is created and used
/// only on the WinForms UI thread.
/// </summary>
internal sealed class SleepPreventionService : IDisposable
{
    private const uint ES_CONTINUOUS = 0x80000000u;
    private const uint ES_SYSTEM_REQUIRED = 0x00000001u;

    public bool IsEnabled { get; private set; }

    public bool TrySetEnabled(bool enabled)
    {
        uint flags = enabled
            ? ES_CONTINUOUS | ES_SYSTEM_REQUIRED
            : ES_CONTINUOUS;

        if (SetThreadExecutionState(flags) == 0)
        {
            return false;
        }

        IsEnabled = enabled;
        return true;
    }

    public void Dispose()
    {
        if (!IsEnabled)
        {
            return;
        }

        SetThreadExecutionState(ES_CONTINUOUS);
        IsEnabled = false;
    }

    [DllImport("kernel32.dll", ExactSpelling = true)]
    private static extern uint SetThreadExecutionState(uint esFlags);
}
