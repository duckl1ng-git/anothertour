using anothertour.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace anothertour.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        public ApplicationContext db;
        public readonly UserManager<IdentityUser> _userManager;

        public ClientController (ApplicationContext context, UserManager<IdentityUser> userManager)
        {
            db = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Profile(string id)
        {
            var client = db.Clients.Where(c => c.Id == id).FirstOrDefault();
            if (client == null)
                return RedirectToAction("CreateProfile", new {id = id});
            return View(client);
        }

        [HttpGet]
        public IActionResult EditProfile(string id)
        {
            var client = db.Clients.Where(c => c.Id == id).FirstOrDefault();
            if (client == null)
                return NotFound();
            return View(client);
        }

        [HttpPost]
        public IActionResult EditProfile (Client model)
        {
            if (ModelState.IsValid)
            {
                db.Update(model);
                db.SaveChanges();
                return RedirectToAction("Profile", new { id = model.Id });
            }
            else
            { 
                return View(model); 
            }
        }

        [HttpGet]
        public IActionResult CreateProfile(string id)
        {
            var client = new Client { Id = id, CurrentDiscount = 0, OrdersNumber = 0, ReviewsNumber = 0, TotalOrdersCost = 0 };
            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile(Client model)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(model);
                await db.SaveChangesAsync();
                return RedirectToAction("Profile", new {id = model.Id});
            }
            return View(model);
        }
    }
}