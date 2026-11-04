# Dozy

Tiny Windows 11 tray utility that toggles sleep prevention. No window, installer, service, or auto-start.

- Left-click: toggle sleep prevention.
- Right-click: `Turn ON` / `Turn OFF`, `Start with Windows`, `Exit`.
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
