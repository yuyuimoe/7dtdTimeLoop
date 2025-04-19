using System.Collections.Generic;
using TimeLoop.Managers;

namespace TimeLoop.Modules.ConsoleCommands {
    public class LoopLimitCommand : ConsoleCmdAbstract {
        public override string getHelp() {
            return LocaleManager.Instance.LocalizeWithPrefix("cmd_looplimit_help");
        }

        public override string[] getCommands() {
            return new[] { "tl_ll", "tl_looplimit", "timeloop_looplimit" };
        }

        public override string getDescription() {
            return LocaleManager.Instance.LocalizeWithPrefix("cmd_looplimit_desc");
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo) {
            if (_params.Count == 0) {
                var loopLimit = ConfigManager.Instance.Config.LoopLimit > 0
                    ? ConfigManager.Instance.Config.LoopLimit.ToString()
                    : LocaleManager.Instance.Localize("infinite");
                SdtdConsole.Instance.Output(
                    LocaleManager.Instance.LocalizeWithPrefix("cmd_minplayers_state", loopLimit));
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

            ConfigManager.Instance.Config.LoopLimit = newValue;
            ConfigManager.Instance.SaveToFile();
            TimeLoopManager.Instance.UpdateLoopState();
            SdtdConsole.Instance.Output(LocaleManager.Instance.LocalizeWithPrefix("cmd_looplimit_return", newValue));
        }
    }
}