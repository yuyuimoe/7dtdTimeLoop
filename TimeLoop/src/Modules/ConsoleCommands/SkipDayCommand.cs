using System.Collections.Generic;
using TimeLoop.Managers;

namespace TimeLoop.Modules.ConsoleCommands
{
    public class SkipDayCommand : ConsoleCmdAbstract
    {
        public override string getHelp()
        {
            return @"Usage:
tl_skipdays <days>
    <days> The amount of days to skip looping.";
        }

        public override string[] getCommands()
        {
            return new[] { "tk_skipdays", "timeloop_skipdays" };
        }

        public override string getDescription()
        {
            return "[TimeLoop] Skip the looping for N amount of days.";
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
        {
            if (_params.Count == 0)
            {
                if (ConfigManager.Instance.Config.DaysToSkip == 0)
                {
                    SdtdConsole.Instance.Output("[TimeLoop] No days will skip the loop.");
                    return;
                }

                SdtdConsole.Instance.Output(
                    $"[TimeLoop] The following {ConfigManager.Instance.Config.DaysToSkip} days will skip the loop");
                return;
            }

            if (_params.Count > 1)
            {
                SdtdConsole.Instance.Output("[TimeLoop] Wrong number of arguments. Excepted 1, found {0}.",
                    _params.Count);
                return;
            }

            if (!int.TryParse(_params[0], out int days))
            {
                SdtdConsole.Instance.Output("[TimeLoop] Invalid parameter type. Expected integer, received {0}",
                    _params[0].GetType());
                return;
            }

            ConfigManager.Instance.Config.DaysToSkip = days;
            ConfigManager.Instance.SaveToFile();
            if (days == 0)
            {
                SdtdConsole.Instance.Output("[TimeLoop] No days will skip the loop.");
                return;
            }

            SdtdConsole.Instance.Output($"[TimeLoop] The following {days} days will skip the loop.");
        }
    }
}