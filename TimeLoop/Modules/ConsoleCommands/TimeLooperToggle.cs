using System.Collections.Generic;
using TimeLoop.Serializer;

namespace TimeLoop.Modules.ConsoleCommands
{
    public class TimeLooperToggle : ConsoleCmdAbstract
    {
        public override string getHelp()
        {
            return @"Usage:
tl_toggle
    Toggles the TimeLooper Mod";
        }
        
        public override string[] getCommands()
        {
            return new[] { "tl_toggle", "timeloop_toggle", "timeloop" };
        }

        public override string getDescription()
        {
            return "[TimeLoop] Toggles the Mod on or off";
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
        {
            if (_params.Count > 1)
            {
                SdtdConsole.Instance.Output("[TimeLoop] Wrong number of arguments. Excepted 0, found {0}.", _params.Count);
                return;
            }

            XmlContentData.Instance.EnableTimeLooper = !XmlContentData.Instance.EnableTimeLooper;
            XmlContentData.Instance.SaveConfig();
            string newState = XmlContentData.Instance.EnableTimeLooper ? "Enabled" : "Disabled";
            SdtdConsole.Instance.Output("[TimeLoop] Time Looper has been {0}", newState);
        }
    }
}