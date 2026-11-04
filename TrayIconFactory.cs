namespace Dozy;

internal sealed class TrayIconFactory : IDisposable
{
    private const string OffResourceName = "Dozy.Assets.dozy-off.ico";
    private const string OnResourceName = "Dozy.Assets.dozy-on.ico";

    private readonly Stream _offStream;
    private readonly Stream _onStream;
    private bool _disposed;

    public TrayIconFactory()
    {
        Size iconSize = SystemInformation.SmallIconSize;

        _offStream = OpenResource(OffResourceName);
        try
        {
            Off = new Icon(_offStream, iconSize);
            _onStream = OpenResource(OnResourceName);

            try
            {
                On = new Icon(_onStream, iconSize);
            }
            catch
            {
                Off.Dispose();
                _onStream.Dispose();
                throw;
            }
        }
        catch
        {
            _offStream.Dispose();
            throw;
        }
    }

    public Icon Off { get; }

    public Icon On { get; }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        Off.Dispose();
        On.Dispose();
        _offStream.Dispose();
        _onStream.Dispose();
    }

    private static Stream OpenResource(string name)
    {
        Stream? stream = typeof(TrayIconFactory).Assembly.GetManifestResourceStream(name);

        if (stream is null)
        {
            throw new InvalidOperationException($"Embedded icon resource '{name}' was not found.");
        }

        return stream;
    }
}
