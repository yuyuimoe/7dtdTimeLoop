using System.Collections.Generic;
using System.Linq;
using TimeLoop.Managers;

namespace TimeLoop.Repositories
{
    public class PlayerRepository
    {
        public Models.PlayerModel? GetPlayerDataNameOrId(string nameOrId)
        {
            if (string.IsNullOrEmpty(nameOrId))
                return null;
            
            return ConfigManager.Instance.Config.Players.Find(data =>
            {
                if (data == null)
                    return false;

                return data.playerName.Equals(nameOrId) || data.id.Equals(nameOrId);
            });
        }
        
        public Models.PlayerModel? GetPlayerData(ClientInfo clientInfo)
        {
            return ConfigManager.Instance.Config.Players.Find(data =>
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
            return clients.Count >= ConfigManager.Instance.Config.MinPlayers;
        }

        public bool IsMinAuthPlayerThreshold()
        {
            List<ClientInfo> clients = GetConnectedClients();
            int authorizedClientCount = clients.Count(IsClientAuthorized);
            return authorizedClientCount >= ConfigManager.Instance.Config.MinPlayers;
        }

        public List<Models.PlayerModel> GetAllUsers()
        {
            return ConfigManager.Instance.Config.Players.FindAll(data => data.playerName.Count() > 1);
        }

        public List<Models.PlayerModel> GetAllAuthorizedUsers(bool unauthorizedInstead = false)
        {
            return ConfigManager
                .Instance
                .Config
                .Players
                .FindAll(data => data.playerName.Count() > 1 && data.skipTimeLoop == !unauthorizedInstead);
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