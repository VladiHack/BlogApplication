using BlogApplication.Models;
using BlogApplication.Suppliers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace BlogApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.userId = UserIdSupplier.id;
            ViewBag.isAdmin = AdminRoleSupplier.isAdmin;

            BlogDbContext dbContext = new BlogDbContext();
            ViewBag.Posts = dbContext.Posts.ToList().Count;
            ViewBag.Comments = dbContext.Comments.ToList().Count;
            ViewBag.Users = dbContext.Users.ToList().Count;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Logout()
        {
            UserIdSupplier.id = -1;
            AdminRoleSupplier.isAdmin = false;
            return View();
        }

        public IActionResult Login()
        {
            ViewBag.isAdmin = AdminRoleSupplier.isAdmin;
            return View();
        }
    }
}
