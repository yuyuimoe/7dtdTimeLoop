using System.Collections.Generic;
using TimeLoop.Managers;

namespace TimeLoop.Modules.ConsoleCommands {
    public class MinPlayersCommand : ConsoleCmdAbstract {
        public override string GetHelp() {
            return LocaleManager.Instance.Localize("cmd_minplayers_help");
        }

        public override string[] getCommands() {
            return new[] { "tl_min", "tl_minplayers", "timeloop_minplayers" };
        }

        public override string getDescription() {
            return LocaleManager.Instance.LocalizeWithPrefix("cmd_minplayers_desc");
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo) {
            if (_params.Count == 0) {
                SdtdConsole.Instance.Output(LocaleManager.Instance.LocalizeWithPrefix("cmd_minplayers_state",
                    ConfigManager.Instance.Config.MinPlayers));
                return;
            }

            if (_params.Count != 1) {
                SdtdConsole.Instance.Output(
                    LocaleManager.Instance.LocalizeWithPrefix("cmd_invalid_param_count", 1, _params.Count));
                return;
            }

            if (!int.TryParse(_params[0], out var newValue)) {
                SdtdConsole.Instance.Output(LocaleManager.Instance.LocalizeWithPrefix("cmd_invalid_param_type",
                    typeof(int), _params[0].GetType()));
                return;
            }

            ConfigManager.Instance.Config.MinPlayers = newValue;
            ConfigManager.Instance.SaveToFile();
            TimeLoopManager.Instance.UpdateLoopState();
            SdtdConsole.Instance.Output(LocaleManager.Instance.LocalizeWithPrefix("cmd_minplayers_return", newValue));
        }
    }
}