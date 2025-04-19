using System;
using System.Collections.Generic;
using TimeLoop.Managers;
using TimeLoop.Models;
using TimeLoop.Repositories;
using UniLinq;

namespace TimeLoop.Modules.ConsoleCommands {
    public class ListCommand : ConsoleCmdAbstract {
        public override string GetHelp() {
            return LocaleManager.Instance.Localize("cmd_list_help");
        }

        public override string[] getCommands() {
            return new[] { "tl_list", "timeloop_list" };
        }

        public override string getDescription() {
            return LocaleManager.Instance.LocalizeWithPrefix("cmd_list_desc");
        }

        private string FormatPlayerList(List<PlayerModel> plyList) {
            if (plyList.Count == 0)
                return LocaleManager.Instance.Localize("cmd_list_no_users");

            var formattedPlyList = string.Join(
                Environment.NewLine,
                plyList.Select(
                    ply => LocaleManager.Instance.Localize("cmd_list_format", ply.playerName, ply.id,
                        ply.skipTimeLoop)));
            return LocaleManager.Instance.LocalizeWithPrefix("cmd_list_return", formattedPlyList + Environment.NewLine,
                plyList.Count);
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo) {
            if (_params.Count == 0 || _params[0].ToLower() == "all") {
                var playerDataRepository = new PlayerRepository();
                var plyList = FormatPlayerList(playerDataRepository.GetAllUsers());
                SdtdConsole.Instance.Output(plyList);
                return;
            }

            if (_params.Count != 1) {
                SdtdConsole.Instance.Output(
                    LocaleManager.Instance.LocalizeWithPrefix("cmd_invalid_param_count", 1, _params.Count));
                return;
            }

            var arg = _params[0].ToLower();
            if (!(arg.Contains("auth") || arg.Contains("unauth"))) {
                SdtdConsole.Instance.Output(LocaleManager.Instance.LocalizeWithPrefix("cmd_invalid_param",
                    "auth, unauth, or all", _params[0]));
                return;
            }

            var plyDataRepo = new PlayerRepository();
            var unauthorizedInstead = arg.Equals("unauth");
            SdtdConsole.Instance.Output(FormatPlayerList(plyDataRepo.GetAllAuthorizedUsers(unauthorizedInstead)));
        }
    }
}