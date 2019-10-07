using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartHomeProject.DataAccess;
using SmartHomeProject.Models;

namespace SmartHomeProject.Pages
{
    public class CreateModel : PageModel
    {
        readonly LightDataAccess lightDataAccess;

        [TempData]
        public string ErrorMessage { get; set; }
        public CreateModel(SmartHomeDbContext db)
        {
            lightDataAccess = new LightDataAccess(db);
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Light Light { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var result = lightDataAccess.AddLight(Light);
            if (result > 0)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ErrorMessage = "Item with the Code : " + Light.Code + " already exists";
                return Page();
            }
        }
    }
}
