using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartHomeProject.DataAccess;
using SmartHomeProject.Models;

namespace SmartHome.Pages
{
    public class IndexModel : PageModel
    {
        LightDataAccess lightDataAccess;
        public IndexModel(SmartHomeDbContext db)
        {
            lightDataAccess = new LightDataAccess(db);
        }

        public IEnumerable<Light> Lights { get; set; }
        public void OnGet()
        {
            Lights = lightDataAccess.GetAllLights();
        }

        public JsonResult OnGetList()
        {
            Lights = lightDataAccess.GetAllLights();
            return new JsonResult(Lights);
        }
        
        public IActionResult OnPostSend(string commandtext)
        {
            return new JsonResult(lightDataAccess.ProcessLightCommand(commandtext));
        }
    }
}
