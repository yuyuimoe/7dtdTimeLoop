using System.Collections.Generic;
using TimeLoop.Managers;

namespace TimeLoop.Modules.ConsoleCommands {
    public class EnableCommand : ConsoleCmdAbstract {
        public override string getHelp() {
            return LocaleManager.Instance.Localize("cmd_enable_help");
        }

        public override string[] getCommands() {
            return new[] { "tl_enable", "timeloop_enable", "timeloop" };
        }

        public override string getDescription() {
            return LocaleManager.Instance.Localize("cmd_enable_desc");
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo) {
            if (_params.Count == 0) {
                SdtdConsole.Instance.Output(LocaleManager.Instance.LocalizeWithPrefix("cmd_enable_state",
                    ConfigManager.Instance.Config.Enabled));
                return;
            }

            if (_params.Count > 1) {
                SdtdConsole.Instance.Output(
                    LocaleManager.Instance.LocalizeWithPrefix("cmd_invalid_param_count", 1, _params.Count));
                return;
            }

            if (!int.TryParse(_params[0], out var newValue)) {
                SdtdConsole.Instance.Output(LocaleManager.Instance.LocalizeWithPrefix("cmd_invalid_param_type",
                    typeof(int), _params[0].GetType()));
                return;
            }

            ConfigManager.Instance.Config.Enabled = newValue >= 1;
            ConfigManager.Instance.SaveToFile();
            TimeLoopManager.Instance.UpdateLoopState();
            var newState = ConfigManager.Instance.Config.Enabled
                ? LocaleManager.Instance.Localize("enabled")
                : LocaleManager.Instance.Localize("disabled");
            SdtdConsole.Instance.Output(LocaleManager.Instance.LocalizeWithPrefix("cmd_enable_return", newState));
        }
    }
}