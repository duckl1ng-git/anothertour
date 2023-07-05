using anothertour.Models;
using anothertour.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace anothertour.Controllers
{
    [Authorize(Roles = "manager")]
    public class ScheduleController : Controller
    {
        public ApplicationContext db { get; set; }
        private readonly UserManager<IdentityUser> _userManager;

        public ScheduleController(ApplicationContext context, UserManager<IdentityUser> userManager) 
        { 
            db = context;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var items = db.ScheduleItems.Include(i => i.SelectedTour).ToList();
            var model = new ScheduleViewModel { Items = items, Days = GetDays() };
            return View(model);
        }

        private List<DateTime> GetDays()
        {
            var days = new List<DateTime>();
            DateTime date = DateTime.Now;
            int offset = (int)date.DayOfWeek;

            if (offset == 0)
                date = date.AddDays(-6);
            else
                date = date.AddDays((offset - 1) * -1);

            for (int i = 0; i < 7 * 6; i++)
            {
                days.Add(date.Date);
                date = date.AddDays(1);
            }

            return days;
        }

        [HttpGet]
        public async Task<IActionResult> EventsList()
        {
            var events = db.ScheduleItems.Include(i => i.SelectedTour).ToList();
            var guides = await _userManager.GetUsersInRoleAsync("guide");
            foreach (var guide in guides)
            {
                ViewData[guide.Id] = guide.UserName;
            }
            return View(events);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEventFromOrder(string tourDate, int tourId, int touristsCount)
        {
            var slist = new SelectList(db.Tours.ToList(), "Id", "Name");
            ViewBag.Tours = slist;
            slist = new SelectList(await _userManager.GetUsersInRoleAsync("guide"), "Id", "UserName");
            ViewBag.Guides = slist;
            var l = tourDate.Split('.', ' ', ':');
            var dt = new DateTime(int.Parse(l[2]), int.Parse(l[1]), int.Parse(l[0]), int.Parse(l[3]), int.Parse(l[4]), int.Parse(l[5]));
            var _event = new ScheduleItem { Date_Time = dt, TourId = tourId, TouristsCount = touristsCount };
            return View("CreateEvent", _event);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEvent()
        {
            var slist = new SelectList(db.Tours.ToList(), "Id", "Name");
            ViewBag.Tours = slist;
            slist = new SelectList (await _userManager.GetUsersInRoleAsync("guide"), "Id", "UserName");
            ViewBag.Guides = slist;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(ScheduleItem model)
        {
            if (ModelState.IsValid)
            {
                db.ScheduleItems.Add(model);
                db.SaveChanges();
                return RedirectToAction("EventsList");
            }
            else
            {
                var slist = new SelectList(db.Tours.ToList(), "Id", "Name");
                ViewBag.Tours = slist;
                slist = new SelectList(await _userManager.GetUsersInRoleAsync("guide"), "Id", "UserName");
                ViewBag.Guides = slist;
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditEvent(int id)
        {
            var slist = new SelectList(db.Tours.ToList(), "Id", "Name");
            ViewBag.Tours = slist;
            slist = new SelectList(await _userManager.GetUsersInRoleAsync("guide"), "Id", "UserName");
            ViewBag.Guides = slist;
            var _event = db.ScheduleItems.Where(s => s.ScheduleItemId == id).FirstOrDefault();
            return View(_event);
        }

        [HttpPost]
        public async Task<IActionResult> EditEvent (ScheduleItem model)
        {
            if (ModelState.IsValid)
            {
                db.ScheduleItems.Update(model);
                db.SaveChanges();
                return RedirectToAction("EventsList", "Schedule");
            }
            else
            {
                var slist = new SelectList(db.Tours.ToList(), "Id", "Name");
                ViewBag.Tours = slist;
                slist = new SelectList(await _userManager.GetUsersInRoleAsync("guide"), "Id", "UserName");
                ViewBag.Guides = slist;
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var _event = db.ScheduleItems.Where(s => s.ScheduleItemId == id).FirstOrDefault();
            ViewBag.TourName = db.Tours.Where(t => t.Id == _event.TourId).FirstOrDefault().Name;
            var guide = await _userManager.FindByIdAsync(_event.GuideId);
            ViewBag.GuideName = guide.UserName;
            return View(_event);
        }

        [HttpPost]
        public IActionResult DeleteEvent(ScheduleItem model)
        {
            db.Entry<ScheduleItem>(model).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("EventsList", "Schedule");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var _event = db.ScheduleItems.Where(s => s.ScheduleItemId == id).FirstOrDefault();
            ViewBag.TourName = db.Tours.Where(t => t.Id == _event.TourId).FirstOrDefault().Name;
            var guide = await _userManager.FindByIdAsync(_event.GuideId);
            ViewBag.GuideName = guide.UserName;
            return View(_event);
        }
    }
}
