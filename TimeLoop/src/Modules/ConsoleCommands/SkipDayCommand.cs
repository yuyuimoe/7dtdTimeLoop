using System.Collections.Generic;
using TimeLoop.Managers;

namespace TimeLoop.Modules.ConsoleCommands {
    public class SkipDayCommand : ConsoleCmdAbstract {
        public override string getHelp() {
            return LocaleManager.Instance.Localize("cmd_skipdays_help");
        }

        public override string[] getCommands() {
            return new[] { "tk_skipdays", "timeloop_skipdays" };
        }

        public override string getDescription() {
            return LocaleManager.Instance.LocalizeWithPrefix("cmd_skipdays_desc");
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo) {
            if (_params.Count == 0) {
                if (ConfigManager.Instance.Config.DaysToSkip == 0) {
                    SdtdConsole.Instance.Output(
                        LocaleManager.Instance.LocalizeWithPrefix("cmd_skipdays_return_disabled"));
                    return;
                }

                SdtdConsole.Instance.Output(LocaleManager.Instance.LocalizeWithPrefix("cmd_skipdays_return_enabled",
                    ConfigManager.Instance.Config.DaysToSkip));
                return;
            }

            if (_params.Count > 1) {
                SdtdConsole.Instance.Output(
                    LocaleManager.Instance.LocalizeWithPrefix("cmd_invalid_param_count", 1, _params.Count));
                return;
            }

            if (!int.TryParse(_params[0], out var days)) {
                SdtdConsole.Instance.Output(LocaleManager.Instance.LocalizeWithPrefix("cmd_invalid_param_type",
                    typeof(int), _params[0].GetType()));
                return;
            }

            ConfigManager.Instance.Config.DaysToSkip = days;
            ConfigManager.Instance.SaveToFile();
            if (days == 0) {
                LocaleManager.Instance.LocalizeWithPrefix("cmd_skipdays_return_disabled");
                return;
            }

            SdtdConsole.Instance.Output(LocaleManager.Instance.LocalizeWithPrefix("cmd_skipdays_return_enabled", days));
        }
    }
}