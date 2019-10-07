using System;
using System.Collections.Generic;

namespace SmartHomeProject.Models
{
    public partial class Light
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public bool Status { get; set; }
    }
}
