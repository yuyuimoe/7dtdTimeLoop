using System.Collections.Generic;
using TimeLoop.Managers;
using TimeLoop.Repositories;

namespace TimeLoop.Modules.ConsoleCommands {
    public class AuthorizeClientCommand : ConsoleCmdAbstract {
        public override string GetHelp() {
            return LocaleManager.Instance.Localize("cmd_authorize_help");
        }

        public override string[] getCommands() {
            return new[] { "tl_auth", "tl_authorize", "timeloop_auth", "timeloop_authorize" };
        }

        public override string getDescription() {
            return LocaleManager.Instance.LocalizeWithPrefix("cmd_authorize_desc");
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo) {
            if (_params.Count != 2) {
                SdtdConsole.Instance.Output(
                    LocaleManager.Instance.LocalizeWithPrefix("cmd_invalid_param_count", 2, _params.Count));
                return;
            }

            if (!int.TryParse(_params[1], out var newValue)) {
                SdtdConsole.Instance.Output(LocaleManager.Instance.LocalizeWithPrefix("cmd_invalid_param_type", 2,
                    typeof(int), _params[1].GetType()));
                return;
            }

            var playerDataRepository = new PlayerRepository();
            var playerData = playerDataRepository.GetPlayerDataNameOrId(_params[0]);
            if (playerData == null) {
                SdtdConsole.Instance.Output(
                    LocaleManager.Instance.LocalizeWithPrefix("cmd_player_not_found", _params[0]));
                return;
            }

            playerData.skipTimeLoop = newValue >= 1;
            ConfigManager.Instance.SaveToFile();
            TimeLoopManager.Instance.UpdateLoopState();
            var newState = playerData.skipTimeLoop
                ? LocaleManager.Instance.Localize("authorized")
                : LocaleManager.Instance.Localize("unauthorized");
            SdtdConsole.Instance.Output(
                LocaleManager.Instance.LocalizeWithPrefix("cmd_authorized_return", newState, playerData.playerName));
        }
    }
}