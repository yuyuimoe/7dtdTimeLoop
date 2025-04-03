# TimeLooper
A mod that allows you to loop time while certain players are not in the server.

## Configuration
The configuration file gets generated when you launch the mod for the first time.

Here's an example of the configuration file:
```xml
<?xml version="1.0" encoding="utf-8"?>
<TimeLoopSettings xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <!-- Enables the mod -->
  <EnableTimeLooper>true</EnableTimeLooper>
  <!-- 
  "whitelist" is Whitelist mode, which means only players with `SkipTimeLoop=true` will move time
  "threshold" is MinPlayers mode, which means time only moves when x amount of players are online
  "whitelisted_threshold" is the combination of the two above: time only moves when x amount of whitelisted players are online
  -->
  <mode>whitelist</mode>
  <!-- List of known players -->
  <KnownPlayers>
    <!-- In Whitelist mode, change `SkipTimeLoop` to "true" to whitelist a player -->
    <PlayerData ID="Local_Yui" PlayerName="Yui" SkipTimeLoop="false" />
  </KnownPlayers>
  <!-- In threshold mode, change the value below to reduce or increase the amount of minimum players -->
  <MinPlayers>5</MinPlayers>
</TimeLoopSettings>
<!-- That's it, you're done. -->
```

## Building
You need:
- Windows 10 or greater
- Any C#/.NET IDE
- .NET Framework 4.8 SDK
- 7 Days to Die installation

### Get the DLLs
1. Create a folder called `Depts` inside the TimeLoop project
2. Copy the following DLLs located in `<7DaysToDieRoot>/7DaysToDie_Data/Managed/`:
- 0Harmony.dll
- Assembly-CSharp.dll
- LogLibrary.dll
- UnityEngine.dll
- UnityEngine.AnimationModule.dll
- UnityEngine.CoreModule.dll
- UnityEngine.JSONSerializeModule.dll
- UnityEngine.PhysicsModule.dll
- UnityEngine.ProfilerModule.dll
- UnityEngine.UIModule.dll
- UnityEngine.VirtualTexturingModule.dll
- UnityEngine.VRModule.dll
- UnityEngine.WindModule.dll
- UnityEngine.XRModule.dll
- Utf8Json.dll
- XblPCSandbox.dll
- zxing.unity.dll

### Change build folder
1. Open `TimeLoop.proj` on a text editor
2. Change `BaseOutputPath` and `OutputPath` to your desired path
