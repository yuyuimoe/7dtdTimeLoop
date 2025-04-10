using System.Collections.Generic;
using TimeLoop.Managers;
using TimeLoop.Serializer;

namespace TimeLoop.Modules.ConsoleCommands
{
    public class EnableCommand : ConsoleCmdAbstract
    {
        public override string getHelp()
        {
            return @"Usage:
tl_enable <0/1>
    0 - Disables the mod.
    1 - Enables the mod.";
        }
        
        public override string[] getCommands()
        {
            return new[] { "tl_enable", "timeloop_enable", "timeloop" };
        }

        public override string getDescription()
        {
            return "[TimeLoop] Enables or disables the mod";
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
        {
            if (_params.Count == 0)
            {
                SdtdConsole.Instance.Output("[TimeLoop] Is Mod Enabled? {0}", ConfigManager.Instance.Config.Enabled);
                return;
            }
            if (_params.Count > 1)
            {
                SdtdConsole.Instance.Output("[TimeLoop] Wrong number of arguments. Excepted 0, found {0}.", _params.Count);
                return;
            }

            if (!int.TryParse(_params[0], out int newValue))
            {
                SdtdConsole.Instance.Output("[TimeLoop] Invalid parameter type. Expected integer, received {0}", _params[0].GetType());
                return;
            }

            ConfigManager.Instance.Config.Enabled = newValue >= 1;
            ConfigManager.Instance.SaveToFile();
            TimeLoopManager.Instance.UpdateLoopState();
            string newState = ConfigManager.Instance.Config.Enabled ? "Enabled" : "Disabled";
            SdtdConsole.Instance.Output("[TimeLoop] Time Looper has been {0}", newState);
        }
    }
}