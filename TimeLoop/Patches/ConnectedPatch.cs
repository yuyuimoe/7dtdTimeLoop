using HarmonyLib;

namespace TimeLoop.Patches
{
    [HarmonyPatch(typeof(AuthorizationManager), nameof(AuthorizationManager.playerAllowed))]
    public class ConnectedPatch
    {
        static void Postfix(AuthorizationManager __instance)
        {
            Main._TimeLooper.UpdateLoopState();
        }
    }
}