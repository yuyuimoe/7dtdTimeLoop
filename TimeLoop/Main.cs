using TimeLoop.Serializer;
using System.Text;
using TimeLoop.Helpers;
using TimeLoop.Modules.TimeLoop;
using TimeLoop.Repository;

namespace TimeLoop
{
    public class Main : IModApi
    {
        public static TimeLooper _TimeLooper { get; private set; }
        private XmlContentData _ContentData;

        private static bool IsDedicatedServer() => GameManager.Instance && GameManager.IsDedicatedServer;
        
        public void InitMod(Mod _modInstance)
        {
            Log.Out("[TimeLoop] Initializing ...");
            ModEvents.GameAwake.RegisterHandler(Awake);
            ModEvents.GameUpdate.RegisterHandler(Update);
            ModEvents.PlayerLogin.RegisterHandler(PlayerLogin);
            ModEvents.PlayerDisconnected.RegisterHandler(PlayerDisconnect);
            ModEvents.PlayerSpawnedInWorld.RegisterHandler(OnPlayerRespawn);
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

        private void PlayerDisconnect(ClientInfo clientInfo, bool isShutdown)
        {
            if (!IsDedicatedServer())
                return;
            Log.Out("[TimeLoop] Player disconnected. Updating loop parameters.");
            _TimeLooper.UpdateLoopState();
        }

        private void OnPlayerRespawn(ClientInfo clientInfo, RespawnType respawnType, Vector3i spawnLocation)
        {
            if (!_ContentData.EnableTimeLooper)
                return;
                
            if (respawnType != RespawnType.JoinMultiplayer)
                return;
            
            if (!_TimeLooper.IsLooping)
                return;
            
            MessageHelper.SendPrivateChat("[TimeLoop] TimeLoop is active. Day will reset at midnight.", clientInfo);
        }
        
        private bool PlayerLogin(ClientInfo cInfo, string message, StringBuilder stringBuild)
        {
            if (!IsDedicatedServer())
                return false;
            
            if (cInfo.PlatformId == null)
                return false;
            
            Log.Out("[TimeLoop] Player logged in. Updating loop parameters.");
            _TimeLooper.UpdateLoopState();
            
            var playerData = (new PlayerDataRepository(_ContentData)).GetPlayerData(cInfo);

            if (playerData == null)
            {
                var plyData = new Models.PlayerData(cInfo);
                _ContentData.PlayerData.Add(plyData);
                _ContentData.SaveConfig();
                Log.Out($"[TimeLoop] Player added to config. {plyData.id}");
            }
            
            return true;
        }
    }
}
