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
  <!-- Skip looping for x amount of days. 0 to disable -->
  <DaysToSkip>0</DaysToSkip>
  <!-- Limit max loops per day. 0 to unlimited loops -->
  <LoopLimit>0</LoopLimit>
  <!-- Global language for the mod. Check the i18n folder for available languages -->
  <Language>en_us</Language>
  <!-- That's it, you're done. -->
</TimeLoopConfig>
```

## Console commands

**v2.2.0 or greater required**

```
tl_enable <0/1> - Enables or Disables the Mod
  0 - Disable
  1 - Enable

tl_mode <0/1/2/3> - Changes the mode.
  0 - Always.
  1 - Whitelist
  2 - Threshold
  3 - Whitelisted Threshold

tl_auth <platform_id/player_name> <0/1> - (Un)authorizes a player.
  (Whitelist or Whitelisted Threshold only)
  platform_id/player_name - Player that will get their authorization status changed.
  0 - Unauthorize
  1 - Authorize

tl_minplayers <amount> - Changes the minimum amount of players required to pass time.
  (Threshold or Whitelisted Threshold only)
  <amount> - New amount of players
 
tl_list <all/auth/unauth> - Lists the players registed in the database
  all - Shows all players
  auth - Shows all authorized players
  unauth - Shows all unauthorized players

tl_ll <amount> - Limit the amount of loops a day can have.
    <amount> - The amount of loops a day can have. 0 to loop indefinitely.

tl_skipdays <days> - Skip the looping for N amount of days.
    <days> - The amount of days to skip looping.

tl_state - Displays if the current day will loop or not.

tl_locale <locale> - Changes the current language.
    <locale> - The locale available in i18n folder, without .json.
```

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
