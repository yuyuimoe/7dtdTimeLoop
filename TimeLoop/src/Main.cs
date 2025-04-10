using System.Reflection;
using HarmonyLib;
using TimeLoop.Helpers;
using TimeLoop.Managers;

namespace TimeLoop
{
    public class Main : IModApi
    {
        public const string ConfigFilePath = "Mods/TimeLoop/TimeLooper.xml";

        public static bool IsDedicatedServer() => GameManager.Instance && GameManager.IsDedicatedServer;
        
        public void InitMod(Mod _modInstance)
        {
            Log.Out("[TimeLoop] Initializing ...");
            ModEvents.GameAwake.RegisterHandler(Awake);
            ModEvents.GameUpdate.RegisterHandler(Update);
            ModEvents.PlayerSpawnedInWorld.RegisterHandler(OnPlayerRespawn);
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }

        private void Awake()
        {
            if (!IsDedicatedServer())
                return;

            ConfigManager.Instantiate();
            TimeLoopManager.Instantiate();
        }

        private void Update()
        {
            if (!IsDedicatedServer())
                return;
            
            ConfigManager.Instance.UpdateFromFile();
            if(ConfigManager.Instance.Config.Enabled)
                TimeLoopManager.Instance.CheckForTimeLoop();
        }

        private void OnPlayerRespawn(ClientInfo clientInfo, RespawnType respawnType, Vector3i spawnLocation)
        {
            if (!ConfigManager.Instance.Config.Enabled)
                return;
                
            if (respawnType != RespawnType.JoinMultiplayer)
                return;
            
            if (TimeLoopManager.Instance.IsTimeFlowing)
                return;
            
            MessageHelper.SendPrivateChat("[TimeLoop] TimeLoop is active. Day will reset at midnight.", clientInfo);
        }
    }
}
