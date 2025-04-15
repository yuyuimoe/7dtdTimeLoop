using System.Collections.Generic;
using TimeLoop.Managers;

namespace TimeLoop.Modules.ConsoleCommands
{
    public class LoopLimitCommand : ConsoleCmdAbstract
    {
        public override string getHelp()
        {
            return @"Usage:
tl_looplimit <amount>
    <amount> - The amount of loops a day can have. 0 to loop indefinitely.";
        }
        
        public override string[] getCommands()
        {
            return new string[] { "tl_ll", "tl_looplimit", "timeloop_looplimit" };
        }

        public override string getDescription()
        {
            return "[TimeLoop] Limit the amount of loops a day can have.";
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
        {
            if (_params.Count == 0)
            {
                string loopLimit = ConfigManager.Instance.Config.LoopLimit > 0 ? ConfigManager.Instance.Config.LoopLimit.ToString() : "Infinite";
                SdtdConsole.Instance.Output("[TimeLoop] Current loop limit is {0}", loopLimit);
                return;
            }

            if (_params.Count > 1)
            {
                SdtdConsole.Instance.Output("[TimeLoop] Wrong number of arguments. Excepted 1, found {0}.", _params.Count);
                return;
            }

            if (!int.TryParse(_params[0], out int newValue))
            {
                SdtdConsole.Instance.Output("[TimeLoop] Invalid parameter type. Expected integer, received {0}", _params[0].GetType());
                return;
            }
            
            ConfigManager.Instance.Config.LoopLimit = newValue;
            ConfigManager.Instance.SaveToFile();
            TimeLoopManager.Instance.UpdateLoopState();
            SdtdConsole.Instance.Output("[TimeLoop] Loop limit set to {0}", newValue);
        }
    }
}