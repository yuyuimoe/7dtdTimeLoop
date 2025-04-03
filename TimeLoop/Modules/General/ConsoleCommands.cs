
using System.Collections.Generic;
using Platform;
using TimeLoop.Enums;
using TimeLoop.Serializer;

namespace TimeLoop.Modules.General
{
    public class EnableTimeLoop : IConsoleCommand
    {
        public static XmlContentData ContentData;

        public bool IsExecuteOnClient => false;

        public int DefaultPermissionLevel => 0;

        public bool AllowedInMainMenu => false;

        public DeviceFlag AllowedDeviceTypes => DeviceFlag.StandaloneWindows;

        public DeviceFlag AllowedDeviceTypesClient => DeviceFlag.StandaloneWindows;

        public void Execute(List<string> commandParams, CommandSenderInfo senderInfo)
        {
            if (!ContentData)
            {
                Log.Warning("Data was not loaded.");
                return;
            }

            switch (commandParams[0])
            {
                case "enable":
                    ContentData.EnableTimeLooper = true;
                    Log.Out("[TimeLoop] enabled!");
                    break;
                case "disable":
                    ContentData.EnableTimeLooper = false;
                    Log.Out("[TimeLoop] disabled!");
                    break;
                case "mode":
                    switch (commandParams[1])
                    {
                        case "none":
                        case "0":
                            ContentData.Mode = EMode.Disabled;
                            Log.Out("[TimeLoop] Mod disabled!");
                            break;
                        case "whitelist":
                        case "1":
                            ContentData.Mode = EMode.Whitelist;
                            Log.Out("[TimeLoop] Whitelist Mode enabled!");
                            break;
                        case "threshold":
                        case "2":
                            ContentData.Mode = EMode.MinPlayerCount;
                            Log.Out("[TimeLoop] Threshold Mode enabled!");
                            break;
                        case "whitelisted_threshold":
                        case "3":
                            ContentData.Mode = EMode.MinWhitelistPlayerCount;
                            Log.Out("[TimeLoop] Whitelisted Threshold Mode enabled!");
                            break;
                    }
                    break;
                case "player":
                    switch (commandParams[1])
                    {
                        case "min":
                            int.TryParse(commandParams[2], out ContentData.MinPlayers);
                            Log.Out($"[TimeLoop] Min player count has been set to {commandParams[2]}!");
                            break;
                        case "auth":
                            Models.PlayerData? player = ContentData.PlayerData.Find(x => x.playerName == commandParams[2]);
                            if (player != null) player.skipTimeLoop = true;
                            Log.Out($"[TimeLoop] Player {commandParams[2]} has been authorized!");
                            break;
                        case "refuse":
                            player = ContentData.PlayerData.Find(x => x.playerName == commandParams[2]);
                            if (player != null) player.skipTimeLoop = false;
                            Log.Out($"[TimeLoop] Player {commandParams[2]} has been unauthorized!");
                            break;
                    }
                    break;
            }

            ContentData.SaveConfig();
        }

        public string[] GetCommands()
        {
            return new string[] { 
                    "timeloop",
                    "tilo"
                };
        }

        public string GetDescription()
        {
            return "Enabled funcionalities of the time loop mod";
        }

        public string GetHelp()
        {
            return "Syntax is: [timeloop|tilo] [(enable|disable)|mode|player] [(none|whitelist|threshold|whitelisted_threshold)|(min|auth|refuse)]";
        }
    }

}
