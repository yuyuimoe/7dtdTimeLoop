#if XML_SERIALIZATION
using ContentData = TimeLoop.Functions.Serializer.XmlContentData;
#else
using ContentData = TimeLoop.Functions.JsonContentData;
#endif
using System.Linq;
using System.Text;
using System.Collections.Generic;
using TimeLoop.Functions.Message;
using TimeLoop.Functions.Player;
using TimeLoop.Modules.General;
using TimeLoop.Modules.TimeLoop;
using UnityEngine;

namespace TimeLoop
{
    public class Main : IModApi
    {
        private TimeLooper _TimeLooper;
        private ContentData _ContentData;

        private static bool IsDedicatedServer() => GameManager.Instance && GameManager.IsDedicatedServer;
        
        public void InitMod(Mod _modInstance)
        {
            Log.Out("[TimeLoop] Initializing ...");
            ModEvents.GameAwake.RegisterHandler(Awake);
            ModEvents.GameUpdate.RegisterHandler(Update);
            ModEvents.PlayerLogin.RegisterHandler(PlayerLogin);
            //SdtdConsole.Instance.RegisterCommands();
        }

        private void Awake()
        {
            if (!IsDedicatedServer())
                return;
            
            if (_ContentData)
                return;

            _ContentData = ContentData.DeserializeInstance();
            EnableTimeLoop.ContentData = _ContentData;
            
            _TimeLooper = new TimeLooper(_ContentData);
        }

        private void Update()
        {
            if (!IsDedicatedServer())
                return;
            
            _ContentData.CheckForUpdate();
            if(_ContentData.EnableTimeLooper)
                _TimeLooper.Update();
        }

        private bool PlayerLogin(ClientInfo cInfo, string message, StringBuilder stringBuild)
        {
            if (!IsDedicatedServer())
                return false;
            
            if (cInfo.PlatformId == null)
                return false;

            var playerData = (new PlayerDataRepository(cInfo, _ContentData)).GetPlayerData();

            if (playerData == null)
            {
                var plyData = new Functions.Serializer.PlayerData(cInfo);
                _ContentData.PlayerData.Add(plyData);
                _ContentData.SaveConfig();
                Log.Out($"[TimeLoop] Player added to config. {plyData.ID}");
            }
            
            if (_ContentData.EnableTimeLooper) 
                Message.SendPrivateChat($"Time loop is active. Therefore the time will reset every 24 hours until the precondition is met.", cInfo);
            
            return true;
        }
    }
}
