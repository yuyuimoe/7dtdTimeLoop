using System;
using System.Collections.Generic;
using TimeLoop.Enums;
using TimeLoop.Serializer;

namespace TimeLoop.Modules.ConsoleCommands
{
    public class TimeLooperChangeMode : ConsoleCmdAbstract
    {
        public override string getHelp()
        {
            return @"Usage:
tl_mode <0/1/2/3>
    0 - Disables the mod.
    1 - Change to whitelist mode
    2 - Change to threshold mode
    3 - Change to whitelisted threshold mode";
        }
        
        public override string[] getCommands()
        {
            return new[] { "tl_mode", "timeloop_mode" };
        }

        public override string getDescription()
        {
            return "[TimeLoop] Changes the mode of the timelooper";
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
        {
            if (_params.Count != 1)
            {
                SdtdConsole.Instance.Output("[TimeLoop] Wrong number of parameters. Expected 1, found {0}", _params.Count);
                return;
            }

            if (!int.TryParse(_params[0], out int mode))
            {
                SdtdConsole.Instance.Output("[TimeLoop] Invalid parameter type. Expected integer, received {0}", _params[0].GetType().ToString());
                return;
            }

            if (!Enum.TryParse<EMode>(mode.ToString(), out var newMode))
            {
                SdtdConsole.Instance.Output("[TimeLoop] Invalid mode specified.");
                return;
            }
            
            XmlContentData.Instance.Mode = newMode;
            XmlContentData.Instance.SaveConfig();
            Main._TimeLooper.UpdateLoopState();
            SdtdConsole.Instance.Output("[TimeLoop] Mode changed to {0}.", XmlContentData.Instance.Mode);
        }
    }
}