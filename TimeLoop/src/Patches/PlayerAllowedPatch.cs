using HarmonyLib;
using TimeLoop.Managers;
using TimeLoop.Models;
using TimeLoop.Repositories;

namespace TimeLoop.Patches {
    [HarmonyPatch(typeof(AuthorizationManager), nameof(AuthorizationManager.playerAllowed))]
    public class PlayerAllowedPatch {
        private static void Postfix(ClientInfo _clientInfo, AuthorizationManager __instance) {
            if (!Main.IsDedicatedServer())
                return;

            if (_clientInfo.PlatformId == null)
                return;

            Log.Out("[TimeLoop] Player logged in. Updating loop parameters.");
            TimeLoopManager.Instance.UpdateLoopState();

            var playerData = new PlayerRepository().GetPlayerData(_clientInfo);

            if (playerData != null)
                return;

            var plyData = new PlayerModel(_clientInfo);
            ConfigManager.Instance.Config.Players.Add(plyData);
            ConfigManager.Instance.SaveToFile();
            //TODO: i18n has new arguments
            Log.Out($"[TimeLoop] Player added to config. {plyData.id}");
        }
    }
}