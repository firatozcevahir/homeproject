using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartHomeProject.DataAccess;
using SmartHomeProject.Models;

namespace SmartHomeProject.Pages
{
    public class ConsoleModel : PageModel
    {
        LightDataAccess lightDataAccess;
        public ConsoleModel(SmartHomeDbContext db)
        {
            lightDataAccess = new LightDataAccess(db);
        }
        public void OnGet()
        {

        }
        public IActionResult OnPostSend(string commandtext)
        {
            return new JsonResult(lightDataAccess.ProcessLightCommand(commandtext));
        }
    }
}