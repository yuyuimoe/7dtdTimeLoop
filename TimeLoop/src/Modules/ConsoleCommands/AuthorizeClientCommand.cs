﻿using System.Collections.Generic;
using TimeLoop.Managers;
using TimeLoop.Repositories;

namespace TimeLoop.Modules.ConsoleCommands
{
    public class AuthorizeClientCommand : ConsoleCmdAbstract
    {
        public override string GetHelp()
        {
            return @"Usage:
(whitelist mode)
tl_auth <player_name/platform_id> <0/1> - Authorizes a player to leave the time loop.
    <player_name/platform_id> - Player name or Platform ID of the player to authorize.
    <0/1> - 0 to unauthorize, 1 to authorize
                    ";
        }
        public override string[] getCommands()
        {
            return new[] { "tl_auth", "tl_authorize", "timeloop_auth", "timeloop_authorize" };
        }

        public override string getDescription()
        {
            return "[TimeLoop] Authorizes a player to leave the time loop.";
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
        {
            if (_params.Count != 2)
            {
                SdtdConsole.Instance.Output("[TimeLoop] Invalid number of parameters. Expected 2, received {0}", _params.Count);
                return;
            }

            if (!int.TryParse(_params[1], out int newValue))
            {
                SdtdConsole.Instance.Output("[TimeLoop] Invalid type for argument 2. Expected integer, received {0}", _params[1].GetType());
                return;
            }

            var playerDataRepository = new PlayerRepository();
            Models.PlayerModel? playerData = playerDataRepository.GetPlayerDataNameOrId(_params[0]);
            if (playerData == null)
            {
                SdtdConsole.Instance.Output("[TimeLoop] Client {0} could not be found in the database", _params[0]);
                return;
            }

            playerData.skipTimeLoop = newValue >= 1;
            ConfigManager.Instance.SaveToFile();
            TimeLoopManager.Instance.UpdateLoopState();
            string newState = playerData.skipTimeLoop ? "Authorized" : "Unauthorized";
            SdtdConsole.Instance.Output("[TimeLoop] {0} client {1} to skip the time loop", newState, playerData.playerName);
        }
    }
}