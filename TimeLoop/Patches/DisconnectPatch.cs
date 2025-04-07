using HarmonyLib;

namespace TimeLoop.Patches
{
    [HarmonyPatch(typeof(ConnectionManager), nameof(ConnectionManager.DisconnectClient))]
    public class DisconnectPatch
    {
        static void Postfix(ConnectionManager __instance)
        {
            Main._TimeLooper.UpdateLoopState();
        }
    }
}