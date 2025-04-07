using System.Reflection;
using TimeLoop.Serializer;
using System.Text;
using HarmonyLib;
using TimeLoop.Helpers;
using TimeLoop.Modules.TimeLoop;
using TimeLoop.Repository;

namespace TimeLoop
{
    public class Main : IModApi
    {
        public static TimeLooper _TimeLooper { get; private set; }
        private XmlContentData _ContentData;

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
            
            if (_ContentData)
                return;

            _ContentData = XmlContentData.DeserializeInstance();
            _TimeLooper = new TimeLooper(_ContentData);
        }

        private void Update()
        {
            if (!IsDedicatedServer())
                return;
            
            _ContentData.CheckForUpdate();
            if(_ContentData.EnableTimeLooper)
                _TimeLooper.CheckForTimeLoop();
        }

        private void OnPlayerRespawn(ClientInfo clientInfo, RespawnType respawnType, Vector3i spawnLocation)
        {
            if (!_ContentData.EnableTimeLooper)
                return;
                
            if (respawnType != RespawnType.JoinMultiplayer)
                return;
            
            if (_TimeLooper.IsTimeFlowing)
                return;
            
            MessageHelper.SendPrivateChat("[TimeLoop] TimeLoop is active. Day will reset at midnight.", clientInfo);
        }
    }
}
