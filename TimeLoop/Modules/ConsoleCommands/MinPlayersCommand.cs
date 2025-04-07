using System.Collections.Generic;
using TimeLoop.Serializer;

namespace TimeLoop.Modules.ConsoleCommands
{
    public class MinPlayersCommand : ConsoleCmdAbstract
    {
        public override string GetHelp()
        {
            return @"Usage:
(threshold mode)
tl_minplayers <x> 
    <x> - Number of minimum players for time to flow normally";
        }
        
        public override string[] getCommands()
        {
            return new[] { "tl_min", "tl_minplayers", "timeloop_minplayers" };
        }

        public override string getDescription()
        {
            return "[TimeLoop] (In Threshold Mode) Changes the minimum players requirement for time to flow normally";
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
        {
            if (_params.Count == 0)
            {
                SdtdConsole.Instance.Output("[TimeLoop] Minimum required players: {0}", XmlContentData.Instance.MinPlayers);
                return;
            }
            if (_params.Count != 1)
            {
                SdtdConsole.Instance.Output("[TimeLoop] Invalid number of arguments. Expected 1, received {0}.", _params.Count);
                return;
            }

            if (!int.TryParse(_params[0], out int newValue))
            {
                SdtdConsole.Instance.Output("[TimeLoop] Invalid parameter type. Expected integer, received {0}", _params[0].GetType());
                return;
            }
            
            XmlContentData.Instance.MinPlayers = newValue;
            XmlContentData.Instance.SaveConfig();
            Main._TimeLooper.UpdateLoopState();
            SdtdConsole.Instance.Output("[TimeLoop] Minimum player requirements changed to {0}", newValue);
        }
    }
}