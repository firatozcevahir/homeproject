using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeProject.Models
{
    public class ProcessedCommand
    {
        public string CommandType { get; set; }
        public string Module { get; set; }
        public string Code { get; set; }
#nullable enable
        public string? Status { get; set; }
        public int? ObjId { get; set; }

    }
}
