using System.Reflection;
using TimeLoop.Serializer;
using System.Text;
using HarmonyLib;
using TimeLoop.Helpers;
using TimeLoop.Modules;

namespace TimeLoop
{
    public class Main : IModApi
    {
        public static TimeLooper _TimeLooper { get; private set; }
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
            
            _TimeLooper = new TimeLooper(ConfigManager.Instance.Config);
        }

        private void Update()
        {
            if (!IsDedicatedServer())
                return;
            
            ConfigManager.Instance.UpdateFromFile();
            if(ConfigManager.Instance.Config.Enabled)
                _TimeLooper.CheckForTimeLoop();
        }

        private void OnPlayerRespawn(ClientInfo clientInfo, RespawnType respawnType, Vector3i spawnLocation)
        {
            if (!ConfigManager.Instance.Config.Enabled)
                return;
                
            if (respawnType != RespawnType.JoinMultiplayer)
                return;
            
            if (_TimeLooper.IsTimeFlowing)
                return;
            
            MessageHelper.SendPrivateChat("[TimeLoop] TimeLoop is active. Day will reset at midnight.", clientInfo);
        }
    }
}
