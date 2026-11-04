# Dozy

Tiny Windows 11 tray utility that toggles sleep prevention. No window, installer, service, or auto-start.

- Left-click: toggle sleep prevention.
- Right-click: `Turn ON` / `Turn OFF`, `Exit`.
- Starts OFF. While ON, Windows will not idle-sleep, but the display can still turn off.

## Build

Requires Windows 11 x64 and [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

```powershell
dotnet build -c Release
```

## Publish

```powershell
dotnet publish Dozy.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:PublishTrimmed=false -p:DebugType=None -p:DebugSymbols=false -o publish
```

`publish\Dozy.exe` is self-contained and unsigned; SmartScreen may warn on first run.

## Notes

- Uses `SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED)`.
- Blocks automatic idle sleep only. Manual sleep, lid close, power button, policy, and critical battery actions still work.
- State is cleared on exit. One instance per user session.
- Windows 11 hides new tray icons by default; enable it under **Settings > Personalization > Taskbar > Other system tray icons**.
- Verify with an elevated `powercfg /requests`.
- Tray icons use Microsoft Fluent Emoji (Flat), MIT; see `THIRD-PARTY-NOTICES.md`.

## License

MIT
