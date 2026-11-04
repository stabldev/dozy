# Dozy

Dozy is a tiny Windows 11 system-tray utility that toggles sleep prevention with a single left-click. It has no main window, settings page, installer, or background service.

## Behavior

- **Left-click** the tray icon: toggle sleep prevention OFF/ON.
- **Right-click** the tray icon: `Turn ON` / `Turn OFF`, plus `Exit`.
- **OFF:** sleep is allowed and the display can turn off normally.
- **ON:** Windows will not enter automatic sleep, but the display can still turn off.

Dozy starts **OFF** every time it launches. It does not start automatically with Windows.

## Requirements

- Windows 11 x64
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) to build

## Build

```powershell
dotnet build Dozy.csproj -c Release
```

## Publish

Self-contained, single-file Windows x64 executable:

```powershell
dotnet publish Dozy.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:PublishTrimmed=false -p:DebugType=None -p:DebugSymbols=false -o publish
```

Run:

```powershell
.\publish\Dozy.exe
```

The executable is large because it includes the .NET runtime. Trimming is intentionally disabled because WinForms trimming is not reliable. The published exe is unsigned, so Windows SmartScreen may warn on first launch.

## Windows tray visibility

Windows 11 hides new tray icons by default. To keep Dozy visible, open the tray overflow and drag the Dozy icon onto the taskbar, or enable it under **Settings > Personalization > Taskbar > Other system tray icons**.

## Limitations

Dozy calls `SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED)`. It prevents **automatic idle sleep** only. It does not block:

- The user choosing Sleep from the Start menu
- Closing the lid or pressing the power button
- Group policy, critical battery actions, or other power-management overrides
- The screen saver

The display is deliberately allowed to turn off. Sleep prevention is cleared on normal exit, and Windows releases the thread's execution state automatically if the process is terminated.

Only one instance runs per user session; a second launch exits silently.

## Verify sleep prevention

Run an elevated PowerShell and check the active power requests:

```powershell
powercfg /requests
```

While Dozy is ON, `Dozy.exe` should appear under `SYSTEM`. While OFF, that request should disappear.

To verify that the display can still turn off, temporarily set a 1-minute monitor timeout and a 2-minute standby timeout in Windows power settings, turn Dozy ON, and leave the machine idle. The display should turn off at about 1 minute while the system remains awake. Restore your original timeout values afterward. Dozy never changes Windows power-plan settings itself.

## License

MIT

The tray icon artwork is based on Microsoft Fluent Emoji (MIT). See [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md).
