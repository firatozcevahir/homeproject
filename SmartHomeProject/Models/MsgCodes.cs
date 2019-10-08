using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeProject.Helpers
{
    public class MsgCodes
    {
        public int ErrItemAlreadyExists { get; }
        public int ErrProcessingCommand { get; }
        public int ErrWrongCommandType { get; }
        public int ErrCantFindObject { get; }
        public int SuccItemAdded { get; }
        public int SuccItemDeleted { get; }

        public MsgCodes()
        {
            ErrItemAlreadyExists = 0;
            ErrProcessingCommand = 102;
            ErrWrongCommandType = 100;
            ErrCantFindObject = 101;
            SuccItemAdded = 103;
            SuccItemDeleted = 104;
        }
    }
}
