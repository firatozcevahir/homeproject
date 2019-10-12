using SmartHomeProject.Interfaces;
using SmartHomeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeProject.Helpers
{
    public class BaseHelper : IHelper
    {
        public MsgCodes MsgCodes;
        public BaseHelper()
        {
            MsgCodes = new MsgCodes();
        }
        public ProcessedCommand ProcessCommand(string commandtext)
        {
            string[] arrCommandText = new string[2];
            if (commandtext == null)
            {
                commandtext = "";
            }
            commandtext = commandtext.Trim().ToLower();

            if (commandtext.Contains("-d"))
            {
                arrCommandText = commandtext.Split("-d");
                arrCommandText[0] = arrCommandText[0].Trim();
                arrCommandText[1] = arrCommandText[1].Trim();
            }
            else
            {
                arrCommandText[0] = commandtext;
                arrCommandText[1] = null;
            }
            if (arrCommandText[0].Length < 7)
            {
                return new ProcessedCommand();
            }
            else if (arrCommandText[0].Length < 9)
            {
                arrCommandText[0] += "11";
                arrCommandText[0] = arrCommandText[0].Substring(0, 9);
            }

            return new ProcessedCommand()
            {
                CommandType = arrCommandText[0].Substring(0, 3),
                Module = arrCommandText[0].Substring(3, 2),
                Description = arrCommandText[1],
                Code = arrCommandText[0].Substring(5, 2),
                Status = arrCommandText[0].Substring(7, 2),
                ObjId = null
            };
        }

    }
}
