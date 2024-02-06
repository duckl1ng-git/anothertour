using anothertour.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace anothertour.Controllers
{
    [Authorize(Roles = "manager")]
    public class OrderController : Controller
    {
        ApplicationContext db;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(ApplicationContext context, UserManager<IdentityUser> userManager)
        {
            db = context;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Create(int tourId)
        {
            var order = new Order { TourId = tourId, Status = "Новый", Date = DateTime.Now, TotalPrice = 1, TourDate = DateTime.Now.Date };
            ViewBag.TourName = db.Tours.Where(t => t.Id == tourId).First().Name;
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var client = db.Clients.Where(c => c.Id == user.Id).First();
                order.ClientId = client.Id;
                order.Email = user.Email;
                order.FirstName = client.FirstName; 
                order.LastName = client.LastName;
                order.PhoneNumber = client.PhoneNumber;
            }
            return View(order);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create (Order model)
        {
            if (ModelState.IsValid)
            {
                if (model.ClientId != null)
                {
                    var client = db.Clients.Where(c => c.Id == model.ClientId).First();
                    client.OrdersNumber++;
                }
                model.TotalPrice = model.TouristsCount * (db.Tours.Where(t => t.Id == model.TourId).First().TicketPrice);
                db.Add(model);
                db.SaveChanges();

                var ems = new EmailService();

                var managers = await _userManager.GetUsersInRoleAsync("manager");
                foreach (var manager in managers)
                {
                    await ems.SendEmailAsync(manager.Email, "Новая заявка", 
                        $"Новая заявка!<br />" +
                        $"Экскурсия: {db.Tours.Where(t => t.Id == model.TourId).First().Name}<br />" +
                        $"Дата: {model.TourDate}<br />" +
                        $"Количество участников: {model.TouristsCount}<br />" +
                        $"Статус: {model.Status}<br />" +
                        $"Подробнее по ссылке: {Url.Action("Details", "Order", new { id = model.Id }, protocol: HttpContext.Request.Scheme)}");
                }

                await ems.SendEmailAsync(model.Email, "Ваша заявка принята", 
                    $"Ваша заявка принята!<br />" +
                    $"Наш менеджер обязательно свяжется с Вами для уточнения деталей.<br />" +
                    $"Экскурсия: {db.Tours.Where(t => t.Id == model.TourId).First().Name}<br />" +
                    $"Дата: {model.TourDate}<br />" +
                    $"Количество участников: {model.TouristsCount}<br />" +
                    $"Статус: {model.Status}<br />");

                return View(viewName: "CreateOrderConfirmation");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CreateFromEvent(int tourId, int scheduleItemId)
        {
            var scheduleItem = db.ScheduleItems.Where(x => x.ScheduleItemId == scheduleItemId).First();
            var order = new Order { TourId = tourId, Status = "Новый", Date = DateTime.Now, TotalPrice = 1, TourDate = scheduleItem.Date_Time, ScheduleItemId = scheduleItemId };
            ViewBag.TourName = db.Tours.Where(t => t.Id == tourId).First().Name;
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var client = db.Clients.Where(c => c.Id == user.Id).First();
                order.ClientId = client.Id;
                order.Email = user.Email;
                order.FirstName = client.FirstName;
                order.LastName = client.LastName;
                order.PhoneNumber = client.PhoneNumber;
            }
            return View(model: order);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateFromEvent(Order model)
        {
            if (ModelState.IsValid)
            {
                if (model.ClientId != null)
                {
                    var client = db.Clients.Where(c => c.Id == model.ClientId).First();
                    client.OrdersNumber++;
                }
                model.TotalPrice = model.TouristsCount * (db.Tours.Where(t => t.Id == model.TourId).First().TicketPrice);
                db.Add(model);
                db.SaveChanges();

                var scheduleItem = db.ScheduleItems.Where(x => x.ScheduleItemId == model.ScheduleItemId).First();
                scheduleItem.TouristsCount += model.TouristsCount;
                db.Update(scheduleItem);
                db.SaveChanges();

                var ems = new EmailService();

                var managers = await _userManager.GetUsersInRoleAsync("manager");
                foreach (var manager in managers)
                {
                    await ems.SendEmailAsync(manager.Email, "Новая заявка",
                        $"Новая заявка!<br />" +
                        $"Экскурсия: {db.Tours.Where(t => t.Id == model.TourId).First().Name}<br />" +
                        $"Дата: {model.TourDate}<br />" +
                        $"Количество участников: {model.TouristsCount}<br />" +
                        $"Статус: {model.Status}<br />" +
                        $"Подробнее по ссылке: {Url.Action("Details", "Order", new { id = model.Id }, protocol: HttpContext.Request.Scheme)}");
                }

                await ems.SendEmailAsync(model.Email, "Ваша заявка принята",
                    $"Ваша заявка принята!<br />" +
                    $"Наш менеджер обязательно свяжется с Вами для уточнения деталей.<br />" +
                    $"Экскурсия: {db.Tours.Where(t => t.Id == model.TourId).First().Name}<br />" +
                    $"Дата: {model.TourDate}<br />" +
                    $"Количество участников: {model.TouristsCount}<br />" +
                    $"Статус: {model.Status}<br />");

                return View(viewName: "CreateOrderConfirmation");
            }
            else
            {
                return View(model: model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManualCreate()
        {
            var slist = new SelectList(db.Tours.ToList(), "Id", "Name");
            ViewBag.Tours = slist;
            slist = new SelectList(await _userManager.GetUsersInRoleAsync("guide"), "Id", "UserName");
            ViewBag.Guides = slist;
            var order = new Order { TourDate = DateTime.Now };
            return View(order);
        }

        [HttpPost]
        public IActionResult ManualCreate(Order model)
        {
            if (ModelState.IsValid)
            {
                model.TotalPrice = model.TouristsCount * (db.Tours.Where(t => t.Id == model.TourId).First().TicketPrice);
                db.Orders.Add(model);
                db.SaveChanges();
                return RedirectToAction(actionName: "Index", controllerName: "Order");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var order = db.Orders.Where(o => o.Id == id).First();
            ViewBag.TourName = db.Tours.Where(t => t.Id == order.TourId).First().Name;
            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var slist = new SelectList(db.Tours.ToList(), "Id", "Name");
            ViewBag.Tours = slist;
            var order = db.Orders.Where(o => o.Id == id).First();
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Order model)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Update(model);
                db.SaveChanges();
                return RedirectToAction(actionName: "Index", controllerName: "Order");
            }
            else
            {
                var slist = new SelectList(db.Tours.ToList(), "Id", "Name");
                ViewBag.Tours = slist;
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var order = db.Orders.Where(o => o.Id == id).First();
            var tour = db.Tours.Where(t => t.Id == order.TourId).FirstOrDefault();
            if (tour != null)
                ViewBag.TourName = tour.Name;
            return View(order);
        }

        [HttpPost]
        public IActionResult Delete(Order model)
        {
            db.Orders.Remove(model);
            db.SaveChanges();
            return RedirectToAction(actionName: "Index", controllerName: "Order");
        }

        public IActionResult Index()
        {
            var orders = db.Orders.ToList();
            var tours = db.Tours.ToList();
            foreach (var item in tours)
            {
                ViewData[item.Id.ToString()] = item.Name;
            }
            return View(orders);
        }
    }
}