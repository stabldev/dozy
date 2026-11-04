using System.Security;
using Microsoft.Win32;

namespace Dozy;

internal sealed class StartupRegistration
{
    private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string ValueName = "Dozy";

    public bool IsEnabled
    {
        get
        {
            try
            {
                using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RunKeyPath);
                return key?.GetValue(ValueName) is string command
                    && !string.IsNullOrWhiteSpace(command);
            }
            catch (Exception ex) when (IsRegistryAccessFailure(ex))
            {
                return false;
            }
        }
    }

    public bool TrySetEnabled(bool enabled)
    {
        try
        {
            using RegistryKey? key = Registry.CurrentUser.CreateSubKey(RunKeyPath, writable: true);
            if (key is null)
            {
                return false;
            }

            if (enabled)
            {
                key.SetValue(ValueName, GetCommand(), RegistryValueKind.String);
            }
            else
            {
                key.DeleteValue(ValueName, throwOnMissingValue: false);
            }

            return true;
        }
        catch (Exception ex) when (IsRegistryAccessFailure(ex))
        {
            return false;
        }
    }

    public void RefreshIfEnabled()
    {
        if (IsEnabled)
        {
            TrySetEnabled(true);
        }
    }

    private static string GetCommand()
    {
        string executablePath = Environment.ProcessPath ?? Application.ExecutablePath;
        return $"\"{executablePath}\"";
    }

    private static bool IsRegistryAccessFailure(Exception exception)
    {
        return exception is UnauthorizedAccessException
            or SecurityException
            or IOException;
    }
}
