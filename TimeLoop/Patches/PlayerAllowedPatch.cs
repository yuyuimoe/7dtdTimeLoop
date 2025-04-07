using HarmonyLib;
using TimeLoop.Repository;
using TimeLoop.Serializer;

namespace TimeLoop.Patches
{
    [HarmonyPatch(typeof(AuthorizationManager), nameof(AuthorizationManager.playerAllowed))]
    public class ConnectedPatch
    {
        static void Postfix(ClientInfo _clientInfo, AuthorizationManager __instance)
        {
            if (!Main.IsDedicatedServer())
                return;

            if (_clientInfo.PlatformId == null)
                return;
            
            Log.Out("[TimeLoop] Player logged in. Updating loop parameters.");
            Main._TimeLooper.UpdateLoopState();
            
            var playerData = (new PlayerDataRepository(XmlContentData.Instance)).GetPlayerData(_clientInfo);

            if (playerData != null) return;
            var plyData = new Models.PlayerData(_clientInfo);
            XmlContentData.Instance.PlayerData.Add(plyData);
            XmlContentData.Instance.SaveConfig();
            Log.Out($"[TimeLoop] Player added to config. {plyData.id}");
        }
    }
}