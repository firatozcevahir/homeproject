using SmartHomeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeProject.Helpers
{
    public class CommandProcessor
    {
        private readonly ProcessedCommand pcommand;
        public CommandProcessor()
        {
            pcommand = new ProcessedCommand();
        }
        public ProcessedCommand ProcessCommand(string commandtext)
        {
            if(commandtext == null)
            {
                commandtext = "";
            }

            if(commandtext.Length < 9)
            {
                commandtext += "999999999";
            }
            else
            {
                commandtext = commandtext.Substring(0, 9);
            }

            pcommand.CommandType = commandtext.Substring(0, 3);
            pcommand.Module = commandtext.Substring(3, 2);
            pcommand.Code = commandtext.Substring(5, 2);
            pcommand.Status = commandtext.Substring(7, 2);
            pcommand.ObjId = null;

            return pcommand;
        }
    }
}
