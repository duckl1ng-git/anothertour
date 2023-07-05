using anothertour.Models;
using Microsoft.AspNetCore.Mvc;
using ApplicationContext = anothertour.Models.ApplicationContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace anothertour.Controllers
{
    [Authorize(Roles = "manager")]
    public class TourController : Controller
    {
        IWebHostEnvironment env;
        ApplicationContext _db;
        const int ImageWidth = 500;
        const int ImageHeight = 500;
        private const Int64 MaxFileSize = 1L * 1024L * 1024L * 1024L; // 1GB

        public TourController(ApplicationContext db, IWebHostEnvironment environment)
        { 
            _db = db;
            env = environment;
        }

        public IActionResult Index()
        {
            var tours = _db.Tours.ToList();
            return View(tours);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create (Tour tour, IFormFile cover, List<IFormFile> additional_images, IFormFile video)
        {
            _db.Tours.Add(tour);
            _db.SaveChanges();

            var added_tour = _db.Tours.OrderBy(t => t.Id).Last();
            ResetMedia(ref added_tour, ref cover, ref additional_images, ref video);
            ImageWorker(ref added_tour, cover, true);
            foreach (var item in additional_images)
            {
                ImageWorker(ref added_tour, item, false);
            }

            VideoWorker(ref added_tour, video);

            _db.Tours.Update(added_tour);
            _db.SaveChanges();

            return RedirectToAction("Index", "Tour");
        }

        [HttpGet]
        public IActionResult Delete(int id) 
        {
            var tour = _db.Tours.Where(t => t.Id == id).First();
            return View(tour);
        }

        [HttpPost]
        public IActionResult Delete(Tour tour)
        {
            if (Directory.Exists($"{env.ContentRootPath}/wwwroot/images/{tour.Id}"))
                Directory.Delete($"{env.ContentRootPath}/wwwroot/images/{tour.Id}", true);
            if (Directory.Exists($"{env.ContentRootPath}/wwwroot/video/{tour.Id}"))
                Directory.Delete($"{env.ContentRootPath}/wwwroot/video/{tour.Id}", true);
            _db.Entry<Tour>(tour).State = EntityState.Deleted;
            _db.SaveChanges();
            return RedirectToAction("Index", "Tour");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var tour = _db.Tours.Where(t => t.Id == id).First();
            return View(tour);
        }

        [HttpPost]
        [RequestSizeLimit(MaxFileSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
        public IActionResult Edit(Tour tour, IFormFile cover, List<IFormFile> additional_images, IFormFile video)
        {
            ResetMedia(ref tour, ref cover, ref additional_images, ref video);
            ImageWorker(ref tour, cover, true);
            if (additional_images.Count != 0)
            {
                foreach (var item in additional_images)
                {
                    ImageWorker(ref tour, item, false);
                }
            }
            VideoWorker(ref tour, video);

            _db.Tours.Update(tour);
            _db.SaveChanges();
            return RedirectToAction("Index", "Tour");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var tour = _db.Tours.Where(t => t.Id == id).First();
            var reviews = _db.Reviews.Where(r => r.TourId == id).ToList();
            var clients = _db.Clients.ToList();
            foreach(var review in reviews)
            {
                var client = clients.Where(c => c.Id == review.ClientId).First();
                ViewData[review.ClientId] = client.FirstName + " " + client.LastName;
            }
            ViewBag.Reviews = reviews;
            
            return View(tour);
        }

        private void ImageWorker(ref Tour tour, IFormFile upload, bool is_main)
        {
            if (upload != null)
            {
                string fileName = Path.GetFileName(upload.FileName);
                var extFile = fileName.Substring(fileName.Length - 3);
                fileName = is_main? "main." + extFile: Path.GetFileName(upload.FileName);
                if (extFile.Contains("png") || extFile.Contains("jpg") || extFile.Contains("jpeg"))
                {
                    using (var image = Image.Load(upload.OpenReadStream()))
                    {
                        //image.Mutate(i => i.Resize(ImageWidth, ImageHeight));
                        var rootPath = env.ContentRootPath;
                        string dir = $"/wwwroot/images/{tour.Id}/";
                        Directory.CreateDirectory(rootPath + dir);
                        var path = rootPath + dir + fileName;
                        image.Save(path);
                        if (is_main) 
                        {
                            tour.MainImage = fileName;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(tour.AdditionalImages)) 
                            { 
                                tour.AdditionalImages = ""; 
                            }
                            tour.AdditionalImages += fileName + "|";
                        }
                    }
                }
            }
        }

        private void VideoWorker (ref Tour tour, IFormFile upload)
        {
            if (upload != null)
            {
                string fileName = Path.GetFileName(upload.FileName);
                var extFile = fileName.Substring(fileName.Length - 3);
                if (extFile.Contains("mp4") || extFile.Contains("mov"))
                {
                    var rootPath = env.ContentRootPath;
                    string dir = $"/wwwroot/video/{tour.Id}/";
                    Directory.CreateDirectory(rootPath + dir);
                    var path = rootPath + dir + fileName;
                    FileInfo file = new FileInfo(path);
                    var mode = FileMode.Create;
                    if (file.Exists)
                        mode = FileMode.Truncate;
                       
                    using (FileStream fs = new FileStream(path, mode))
                    {
                        upload.CopyTo(fs);
                    }
                    tour.Video = fileName;
                }
            }
        }

        private void ResetMedia(ref Tour tour, ref IFormFile cover, ref List<IFormFile> additional_images, ref IFormFile video)
        {
            if (cover != null)
            {
                if (tour.MainImage != null)
                    System.IO.File.Delete($"{env.ContentRootPath}/wwwroot/images/{tour.Id}/{tour.MainImage}");
                tour.MainImage = "";
            }
            if (additional_images.Count != 0)
            {
                if (tour.AdditionalImages != null)
                {
                    foreach (var image in tour.AdditionalImages.Split('|', StringSplitOptions.RemoveEmptyEntries))
                    {
                        System.IO.File.Delete($"{env.ContentRootPath}/wwwroot/images/{tour.Id}/{image}");
                    }
                }
                tour.AdditionalImages = "";
            }
            if (video != null)
            {
                if (tour.Video != null)
                    System.IO.File.Delete($"{env.ContentRootPath}/wwwroot/video/{tour.Id}/{tour.Video}");
                tour.Video = "";
            }
        }
    }
}
