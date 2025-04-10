# TimeLoop
A mod that allows you to loop time while certain players are not in the server.

## Configuration
The configuration file gets generated when you launch the mod for the first time.

Here's an example of the configuration file:
```xml
<?xml version="1.0" encoding="utf-8"?>
<TimeLoopConfig xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <!-- Enables the mod -->
  <Enabled>true</Enabled>
  <!--
  "always" is Always mode, which means this day will loop no matter what.
  "whitelist" is Whitelist mode, which means only players with `Whitelisted="true"` will move time
  "threshold" is MinPlayers mode, which means time only moves when x amount of players are online
  "whitelisted_threshold" is the combination of the two above: time only moves when x amount of whitelisted players are online
  -->
  <Mode>whitelist</Mode>
  <!-- List of players -->
  <Players>
    <!-- In Whitelist mode, change `Whitelisted` to "true" to whitelist a player -->
    <PlayerModel ID="Steam_76561198061215936" Name="Yui" Whitelisted="false" />
  </Players>
  <!-- In threshold mode, change the value below to set the amount of minimum players -->
  <MinPlayers>1</MinPlayers>
  <!-- That's it, you're done. -->
</TimeLoopConfig>
```

## Console commands
**v2.2.0 or greater required**
- tl_enable <0/1> - Enables or Disables the Mod<br>
  0 - Disable<br>
  1 - Enable
- tl_mode <0/1/2/3> - Changes the mode.<br>
  0 - Always.<br>
  1 - Whitelist<br>
  2 - Threshold<br>
  3 - Whitelisted Threshold
- tl_auth <platform_id/player_name> <0/1> - (Un)authorizes a player.<br>
  (Whitelist or Whitelisted Threshold only)<br>
  platform_id/player_name - Player that will get their authorization status changed.<br>
  0 - Unauthorize<br>
  1 - Authorize
- tl_minplayers <x> Changes the minimum amount of players required to pass time.<br>
  (Threshold or Whitelisted Threshold only)<br>
  x - New amount of players
- tl_list <all/auth/unauth> - Lists the players registed in the database<br>
  all - Shows all players<br>
  auth - Shows all authorized players<br>
  unauth - Shows all unauthorized players

## Building
You need:
- Windows 10 or greater
- Any C#/.NET IDE
- .NET Framework 4.8 SDK
- 7 Days to Die installation

### Get the DLLs
Check the README on the `depts` folder.

### Change build folder
1. Open `TimeLoop.proj` on a text editor
2. Change `BaseOutputPath` and `OutputPath` to your desired path
