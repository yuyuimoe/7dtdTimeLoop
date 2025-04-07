using HarmonyLib;

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
            Main._TimeLooper.UpdateLoopState();
        }
    }
}