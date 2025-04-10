using System;
using System.Collections.Generic;
using TimeLoop.Repositories;
using TimeLoop.Serializer;
using UniLinq;

namespace TimeLoop.Modules.ConsoleCommands
{
    public class ListCommand : ConsoleCmdAbstract
    {
        public override string GetHelp()
        {
            return @"Usage:
tl_list <all/auth/unauth>:
    all - Lists all users in database
    auth - Lists all authorized users
    unauth - Lists all unauthorized users";
        }
        
        public override string[] getCommands()
        {
            return new [] { "tl_list", "timeloop_list" };
        }

        public override string getDescription()
        {
            return "Lists all users in database";
        }

        private string FormatPlayerList(List<Models.PlayerModel> plyList)
        {
            if (plyList.Count == 0)
                return "[TimeLoop] No users in database";
            
            string formattedPlyList = string.Join(
                Environment.NewLine,
                plyList.Select(ply => $"Player: {ply.playerName}, Platform ID: {ply.id}, Authorized? {ply.skipTimeLoop}"));
            return "[TimeLoop]" + formattedPlyList + Environment.NewLine + "Total: " + plyList.Count;
        }
        
        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
        {
            if (_params.Count == 0 || _params[0].ToLower() == "all")
            {
                var playerDataRepository = new PlayerRepository();
                string plyList = FormatPlayerList(playerDataRepository.GetAllUsers());
                SdtdConsole.Instance.Output(plyList);
                return;
            }

            if (_params.Count != 1)
            {
                SdtdConsole.Instance.Output("[TimeLoop] Invalid number of arguments. Expected 1, received {0}.", _params.Count);
                return;
            }
            
            string arg = _params[0].ToLower();
            if (!(arg.Contains("auth") || arg.Contains("unauth")))
            {
                SdtdConsole.Instance.Output("[TimeLoop] Invalid arguments. Expected 'auth', 'unauth' or 'all', received {0}.", _params[0]);
                return;
            }
            
            var plyDataRepo = new PlayerRepository();
            bool unauthorizedInstead = arg.Equals("unauth");
            SdtdConsole.Instance.Output(FormatPlayerList(plyDataRepo.GetAllAuthorizedUsers(unauthorizedInstead)));
        }
    }
}