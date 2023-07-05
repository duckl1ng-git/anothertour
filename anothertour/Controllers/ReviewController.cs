using anothertour.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace anothertour.Controllers
{
    public class ReviewController : Controller
    {
        ApplicationContext db;
        UserManager<IdentityUser> userManager;

        public ReviewController(ApplicationContext context, UserManager<IdentityUser> manager)
        {
            db = context;
            userManager = manager;
        }

        [HttpGet]
        [Authorize(Roles = "manager")]
        public IActionResult Index()
        {
            var reviews = db.Reviews.ToList();
            var clients = db.Clients.ToList();
            foreach (var client in clients)
            {
                ViewData[client.Id] = client.FirstName + " " + client.LastName;
            }
            return View(reviews);
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Create(int tourId)
        {
            var user = await userManager.FindByEmailAsync(User.Identity.Name);
            var review = new Review { TourId = tourId, ClientId = user.Id, Date = DateTime.Now };
            ViewBag.TourName = db.Tours.Where(t => t.Id == tourId).First().Name;
            return View(review);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public IActionResult Create(Review model)
        {
            if (ModelState.IsValid)
            {
                var client = db.Clients.Where(c => c.Id == model.ClientId).FirstOrDefault();
                client.ReviewsNumber++;
                db.Update(client);
                db.Reviews.Add(model);
                db.SaveChanges();
                return View("SuccessfulCreateReview");
            }
            else
            { 
                return View(model); 
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var review = db.Reviews.Where(r => r.Id == id).FirstOrDefault();
            return View(review);
        }

        [HttpPost]
        public IActionResult Delete(Review model)
        {
            db.Reviews.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
