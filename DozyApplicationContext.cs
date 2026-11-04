namespace Dozy;

internal sealed class DozyApplicationContext : ApplicationContext
{
    private const string OffToolTip = "Dozy: Off";
    private const string OnToolTip = "Dozy: On";

    private readonly SleepPreventionService _sleep = new();
    private readonly StartupRegistration _startup = new();
    private readonly TrayIconFactory _icons = new();
    private readonly ContextMenuStrip _menu;
    private readonly ToolStripMenuItem _toggleItem;
    private readonly ToolStripMenuItem _startWithWindowsItem;
    private readonly NotifyIcon _notifyIcon;
    private bool _disposed;

    public DozyApplicationContext()
    {
        _toggleItem = new ToolStripMenuItem("Turn ON", null, (_, _) => Toggle());
        _startWithWindowsItem = new ToolStripMenuItem("Start with Windows", null, (_, _) => ToggleStartWithWindows());

        _menu = new ContextMenuStrip
        {
            ShowImageMargin = false,
            ShowCheckMargin = true
        };
        _menu.Items.Add(_toggleItem);
        _menu.Items.Add(_startWithWindowsItem);
        _menu.Items.Add(new ToolStripSeparator());
        _menu.Items.Add(new ToolStripMenuItem("Exit", null, (_, _) => ExitApplication()));
        _menu.Opening += (_, _) => ApplyStartupState();

        _notifyIcon = new NotifyIcon
        {
            ContextMenuStrip = _menu,
            Icon = _icons.Off,
            Text = OffToolTip,
            Visible = true
        };
        _notifyIcon.MouseClick += OnMouseClick;

        _startup.RefreshIfEnabled();
        ApplyState();
        ApplyStartupState();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Cleanup();
        }

        base.Dispose(disposing);
    }

    private void OnMouseClick(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            Toggle();
        }
    }

    private void Toggle()
    {
        bool enable = !_sleep.IsEnabled;

        if (!_sleep.TrySetEnabled(enable))
        {
            ShowFailure(enable);
            return;
        }

        ApplyState();
    }

    private void ApplyState()
    {
        bool enabled = _sleep.IsEnabled;

        _notifyIcon.Icon = enabled ? _icons.On : _icons.Off;
        _notifyIcon.Text = enabled ? OnToolTip : OffToolTip;
        _toggleItem.Text = enabled ? "Turn OFF" : "Turn ON";
    }

    private void ToggleStartWithWindows()
    {
        bool enable = !_startup.IsEnabled;

        if (!_startup.TrySetEnabled(enable))
        {
            ShowBalloon(enable
                ? "Could not enable Start with Windows."
                : "Could not disable Start with Windows.");
            return;
        }

        ApplyStartupState();
    }

    private void ApplyStartupState()
    {
        _startWithWindowsItem.Checked = _startup.IsEnabled;
    }

    private void ShowFailure(bool enable)
    {
        ShowBalloon(enable
            ? "Windows did not allow keep-awake."
            : "Windows did not allow normal power behavior to be restored.");
    }

    private void ShowBalloon(string message)
    {
        _notifyIcon.BalloonTipTitle = "Dozy";
        _notifyIcon.BalloonTipText = message;
        _notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
        _notifyIcon.ShowBalloonTip(4000);
    }

    private void ExitApplication()
    {
        _notifyIcon.Visible = false;
        _sleep.Dispose();
        ExitThread();
    }

    private void Cleanup()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _notifyIcon.MouseClick -= OnMouseClick;
        _notifyIcon.Visible = false;
        _sleep.Dispose();
        _notifyIcon.Dispose();
        _menu.Dispose();
        _icons.Dispose();
    }
}
