using SmartHomeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeProject.Interfaces
{
    public interface IHelper
    {
        ProcessedCommand ProcessCommand(string commandtext);
    }
}
