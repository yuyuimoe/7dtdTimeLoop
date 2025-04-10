using HarmonyLib;
using TimeLoop.Repositories;
using TimeLoop.Serializer;

namespace TimeLoop.Patches
{
    [HarmonyPatch(typeof(AuthorizationManager), nameof(AuthorizationManager.playerAllowed))]
    public class PlayerAllowedPatch
    {
        static void Postfix(ClientInfo _clientInfo, AuthorizationManager __instance)
        {
            if (!Main.IsDedicatedServer())
                return;

            if (_clientInfo.PlatformId == null)
                return;
            
            Log.Out("[TimeLoop] Player logged in. Updating loop parameters.");
            Main._TimeLooper.UpdateLoopState();
            
            var playerData = new PlayerRepository().GetPlayerData(_clientInfo);

            if (playerData != null) 
                return;
            
            var plyData = new Models.PlayerModel(_clientInfo);
            ConfigManager.Instance.Config.Players.Add(plyData);
            ConfigManager.Instance.SaveToFile();
            Log.Out($"[TimeLoop] Player added to config. {plyData.id}");
        }
    }
}