#if XML_SERIALIZATION
using ContentData = TimeLoop.Functions.Serializer.XmlContentData;
#else
using ContentData = TimeLoop.Functions.JsonContentData;
#endif
using System.Collections.Generic;
using System.Linq;
using TimeLoop.Functions.Message;
using TimeLoop.Functions.Player;


namespace TimeLoop.Modules.TimeLoop
{
    public class TimeLooper
    {
        ContentData contentData;
        double unscaledTimeStamp;

        public TimeLooper(ContentData contentData)
        {
            this.contentData = contentData;
        }

        public void Update()
        {
            switch (this.contentData.mode)
            {
                case ContentData.Mode.WHITELIST:
                    if(CheckIfAuthPlayerOnline()) return;
                    break;
                case ContentData.Mode.MIN_PLAYER_COUNT:
                    if(CheckIfMinPlayerCountReached()) return;
                    break;
                case ContentData.Mode.MIN_WHITELIST_PLAYER_COUNT:
                    if (CheckIfMinAuthPlayerCountReached()) return;
                    break;
                default:
                    return;
            }

            if (unscaledTimeStamp != UnityEngine.Time.unscaledTimeAsDouble)
            {
                ulong worldTime = GameManager.Instance.World.GetWorldTime();
                ulong dayTime = worldTime % 24000;
                if (dayTime == 0)
                {
                    Log.Out("[TimeLoop] Time Reset.");
                    Message.SendGlobalChat($"Resetting day. Please wait for authorized personnel or enough players to stop the time loop.");
                    int previousDay = GameUtils.WorldTimeToDays(worldTime) - 1;
                    GameManager.Instance.World.SetTime(GameUtils.DaysToWorldTime(previousDay) + 2);
                }

                unscaledTimeStamp = UnityEngine.Time.unscaledTimeAsDouble;
            }
        }

        private bool CheckIfAuthPlayerOnline()
        {
            List<ClientInfo> clients = GetConnectedClients();
            for (int i = 0; i < clients.Count; i++)
            {
                if (IsClientAuthorized(clients[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckIfMinPlayerCountReached()
        {
            List<ClientInfo> clients = GetConnectedClients();
            return clients.Count >= this.contentData.MinPlayers;
        }

        private bool CheckIfMinAuthPlayerCountReached()
        {
            int authorizedClientCount = 0;

            List<ClientInfo> clients = GetConnectedClients();
            for (int i = 0; i < clients.Count; i++)
            {
                if (IsClientAuthorized(clients[i]))
                {
                    authorizedClientCount++;
                }
            }
            return authorizedClientCount >= this.contentData.MinPlayers;
        }

        private List<ClientInfo> GetConnectedClients()
        {
            if (ConnectionManager.Instance.Clients != null && ConnectionManager.Instance.Clients.Count > 0)
            {
                return ConnectionManager.Instance.Clients.List.Where(x => 
                x != null &&
                x.loginDone &&
                (x.CrossplatformId != null ||
                x.PlatformId != null)).ToList();
            }
            else
            {
                return new List<ClientInfo>();
            }
        }

        private bool IsClientAuthorized(ClientInfo cInfo)
        {
            var plyData = (new PlayerDataRepository(cInfo, contentData)).GetPlayerData();
            if (plyData != null) 
                return plyData.SkipTimeLoop;
            
            Log.Error($"Player data could not be found for player {cInfo.playerName}.");
            return false;

        }

        public static implicit operator bool(TimeLooper instance)
        {
            return instance != null;
        }
    }
}
