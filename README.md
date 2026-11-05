Dozy
====

Dozy is a tiny Windows 11 tray utility that toggles sleep prevention. It has no
window, installer, service, or auto-start requirement.

Quick Start
-----------

* Toggle sleep prevention: Left-click the tray icon
* Open the menu: Right-click the tray icon
* Turn on or off: Turn ON / Turn OFF
* Launch at sign-in: Start with Windows
* Quit: Exit

Dozy starts OFF. While ON, it keeps the display on and prevents idle sleep and
hibernate.


Using Dozy
==========

The tray icon shows the current state:

* Sleeping face: Dozy is OFF and normal power behavior applies
* Smiling face: Dozy is ON and the PC is kept awake

The right-click menu contains:

* Turn ON / Turn OFF: Toggle sleep prevention
* Start with Windows: Enable or disable launch at sign-in
* Exit: Restore normal power behavior and quit

If Windows rejects a request, Dozy shows a notification and leaves the previous
state unchanged.


Building
========

Requirements:

* Windows 11 x64
* .NET 10 SDK: https://dotnet.microsoft.com/download/dotnet/10.0

Build a release binary:

    dotnet build -c Release


Publishing
----------

Publish a self-contained single file:

    dotnet publish Dozy.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:PublishTrimmed=false -p:DebugType=None -p:DebugSymbols=false -o publish

The result is `publish\Dozy.exe`. The binary is self-contained and unsigned,
so SmartScreen may warn on first run.


Files
=====

* License: See LICENSE (MIT)
* Third-party notices: See THIRD-PARTY-NOTICES.md
* Source: https://github.com/stabldev/dozy
