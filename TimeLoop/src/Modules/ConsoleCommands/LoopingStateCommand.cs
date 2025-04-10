using System.Collections.Generic;
using TimeLoop.Managers;

namespace TimeLoop.Modules.ConsoleCommands
{
    public class LoopingStateCommand : ConsoleCmdAbstract
    {
        public override string getHelp()
        {
            return @"Usage:
tl_state
    Displays if the current day will loop or not.";
        }

        public override string[] getCommands()
        {
            return new[] { "tl_state", "timeloop_state" };
        }

        public override string getDescription()
        {
            return "[TimeLoop] Displays if the current day will loop or not.";
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
        {
            string isOrNot = TimeLoopManager.Instance.IsTimeFlowing ? "IS NOT" : "IS";
            SdtdConsole.Instance.Output("[TimeLoop] Current day {0} looping.", isOrNot);
        }
    }
}