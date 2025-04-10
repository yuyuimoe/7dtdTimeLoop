using HarmonyLib;
using TimeLoop.Managers;

namespace TimeLoop.Patches
{
    [HarmonyPatch(typeof(ConnectionManager), nameof(ConnectionManager.DisconnectClient))]
    public class DisconnectPatch
    {
        static void Postfix(ConnectionManager __instance)
        {
            if (!Main.IsDedicatedServer())
                return;
            
            Log.Out("[TimeLoop] Player disconnected. Updating loop parameters.");
            TimeLoopManager.Instance.UpdateLoopState();
        }
    }
}