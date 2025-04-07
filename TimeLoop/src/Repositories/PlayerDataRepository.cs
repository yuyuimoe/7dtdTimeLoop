using System.Collections.Generic;
using System.Linq;
using TimeLoop.Serializer;

namespace TimeLoop.Repositories
{
    public class PlayerDataRepository
    {
        private readonly XmlContentData _contentData;

        public PlayerDataRepository(XmlContentData contentData)
        {
            _contentData = contentData;
        }

        public Models.PlayerData? GetPlayerDataNameOrId(string nameOrId)
        {
            if (string.IsNullOrEmpty(nameOrId))
                return null;
            
            return _contentData.PlayerData?.Find(data =>
            {
                if (data == null)
                    return false;

                return data.playerName.Equals(nameOrId) || data.id.Equals(nameOrId);
            });
        }
        
        public Models.PlayerData? GetPlayerData(ClientInfo clientInfo)
        {
            return _contentData.PlayerData?.Find(data =>
            {
                if (data == null)
                    return false;

                if (data.id == clientInfo.CrossplatformId?.CombinedString)
                    return true;
                
                return clientInfo.PlatformId.CombinedString == data.id;
            });
        }
        
        public bool IsAuthPlayerOnline()
        {
            List<ClientInfo> clients = GetConnectedClients();
            return clients.Any(IsClientAuthorized);
        }

        public bool IsMinPlayerThreshold()
        {
            List<ClientInfo> clients = GetConnectedClients();
            return clients.Count >= this._contentData.MinPlayers;
        }

        public bool IsMinAuthPlayerThreshold()
        {
            List<ClientInfo> clients = GetConnectedClients();
            int authorizedClientCount = clients.Count(IsClientAuthorized);
            return authorizedClientCount >= this._contentData.MinPlayers;
        }

        public List<Models.PlayerData> GetAllUsers()
        {
            List<Models.PlayerData>? list = XmlContentData.Instance.PlayerData?.FindAll(data => data.playerName.Count() > 1);
            return list ?? new List<Models.PlayerData>();
        }

        public List<Models.PlayerData> GetAllAuthorizedUsers(bool unauthorizedInstead = false)
        {
            List<Models.PlayerData>? list = 
                XmlContentData
                .Instance
                .PlayerData?
                .FindAll(data => data.playerName.Count() > 1 && data.skipTimeLoop == !unauthorizedInstead);
            return list ?? new List<Models.PlayerData>();
        }
        
        private List<ClientInfo> GetConnectedClients()
        {
            ConnectionManager.Instance.LateUpdate();
            if (ConnectionManager.Instance.Clients != null && ConnectionManager.Instance.Clients.Count > 0)
            {
                return ConnectionManager.Instance.Clients.List.Where(x => 
                    x is { loginDone: true, disconnecting: false } &&
                    (x.CrossplatformId != null || x.PlatformId != null)).ToList();
            }
            return new List<ClientInfo>();
        }

        private bool IsClientAuthorized(ClientInfo cInfo)
        {
            var plyData = this.GetPlayerData(cInfo);
            if (plyData != null) 
                return plyData.skipTimeLoop;
            
            Log.Error($"Player data could not be found for player {cInfo.playerName}.");
            return false;

        }
    }
}