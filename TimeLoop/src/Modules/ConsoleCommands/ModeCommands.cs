using System;
using System.Collections.Generic;
using TimeLoop.Enums;
using TimeLoop.Managers;

namespace TimeLoop.Modules.ConsoleCommands {
    public class ModeCommands : ConsoleCmdAbstract {
        public override string getHelp() {
            return LocaleManager.Instance.Localize("cmd_mode_help");
        }

        public override string[] getCommands() {
            return new[] { "tl_mode", "timeloop_mode" };
        }

        public override string getDescription() {
            return LocaleManager.Instance.Localize("cmd_mode_desc");
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo) {
            if (_params.Count == 0) {
                SdtdConsole.Instance.Output(LocaleManager.Instance.LocalizeWithPrefix(
                    "cmd_mode_state",
                    LocaleManager.Instance.Localize(ConfigManager.Instance.Config.Mode.ToString().ToLower())));
                return;
            }

            if (_params.Count != 1) {
                SdtdConsole.Instance.Output(
                    LocaleManager.Instance.LocalizeWithPrefix("cmd_invalid_param_count", 1, _params.Count));
                return;
            }

            if (!int.TryParse(_params[0], out var mode)) {
                SdtdConsole.Instance.Output(LocaleManager.Instance.LocalizeWithPrefix("cmd_invalid_param_type", 1,
                    typeof(int),
                    _params[0].GetType()));
                return;
            }

            if (!Enum.TryParse<EMode>(mode.ToString(), out var newMode)) {
                SdtdConsole.Instance.Output(LocaleManager.Instance.LocalizeWithPrefix("cmd_mode_invalid_mode"));
                return;
            }

            ConfigManager.Instance.Config.Mode = newMode;
            ConfigManager.Instance.SaveToFile();
            TimeLoopManager.Instance.UpdateLoopState();
            SdtdConsole.Instance.Output(LocaleManager.Instance.LocalizeWithPrefix("cmd_mode_return",
                LocaleManager.Instance.Localize(ConfigManager.Instance.Config.Mode.ToString().ToLower())));
        }
    }
}