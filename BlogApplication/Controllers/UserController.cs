using BlogApplication.Models;
using BlogApplication.Services.Users;
using BlogApplication.Suppliers;
using BlogApplication.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.Controllers
{
    public class UserController:Controller
    {
        private readonly BlogDbContext _context;
        IUserService _userService;

        public UserController(BlogDbContext context, IUserService userService)
        {
            _context = context;
            _userService= userService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.isAdmin = AdminRoleSupplier.isAdmin;
            ViewBag.userId = UserIdSupplier.id;
            return View(await _userService.GetUsersAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            string msg = "";

            List<User> users = (List<User>)await _userService.GetUsersAsync();
            if (users.Count == 0)
            {
                user.UserId = 1;
            }
            else
            {
                user.UserId = users[users.Count() - 1].UserId + 1;
            }

            if (user.UserImageFile != null && user.UserImageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await user.UserImageFile.CopyToAsync(memoryStream);
                    user.Image = memoryStream.ToArray();
                }
            }

            msg = UserValidator.ReturnErrorsCreate(users, user);
            ViewBag.Message = msg;

            if (msg == "")
            {
                UserIdSupplier.id = user.UserId;
                ViewBag.userId = user.UserId;
                await _userService.CreateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // Other actions...


        public IActionResult Login()
        {
            ViewBag.isAdmin=AdminRoleSupplier.isAdmin;
            ViewBag.userId = UserIdSupplier.id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginEntity partUser)
        {
            string msg = $"There is no user with such username and password!";
            User user = _context.Users.FirstOrDefault(e => e.Username == partUser.Username && e.Password == partUser.Password);
            if (user != null)
            {
                UserIdSupplier.id = user.UserId;
                msg = $"You logged successfully, {user.Username}!";
                ViewBag.LoginSuccess = true;

                //Check if user is admin
                if(user.UserId == _context.Users.ToList()[0].UserId)
                {
                    AdminRoleSupplier.isAdmin = true;
                    ViewBag.isAdmin = true;
                }
                else
                {
                    AdminRoleSupplier.isAdmin = false;
                    ViewBag.isAdmin = false;
                }

                return RedirectToAction("Index", "Post"); // Redirect to the Index action of the Post controller
            }
            else
            {
                UserIdSupplier.id = 0;
                msg = $"Invalid username or password!";
                ViewBag.LoginSuccess = false;
            }
            ViewBag.Message = msg;
            ViewBag.userId = UserIdSupplier.id;
            ViewBag.isAdmin = AdminRoleSupplier.isAdmin;
            return View();
        }
        public IActionResult Logout()
        {
            UserIdSupplier.id = -1;
            return View();
        }

        public async Task<IActionResult> EditAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.CurrentImage = user.Image; // Ensure 'Image' is a byte array in the User model
            return View(user);
        }


        public async Task<IActionResult> DeleteAsync(int id)
        {
       

            ViewBag.userId = UserIdSupplier.id;
            ViewBag.isAdmin = AdminRoleSupplier.isAdmin;

            var user = await _userService.GetUserByIdAsync(id);
            return View(user);

        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
       
       
                await _userService.DeleteUserByIdAsync(id);
                if (id==UserIdSupplier.id)
                {
                    //The current user
                    ViewBag.userId = -1;
                    ViewBag.isAdmin = false;
                    UserIdSupplier.id = -1;
                    AdminRoleSupplier.isAdmin = false;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }




        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int userId, User user)
        {
            var oldUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(p => p.UserId == userId);
            var users = await _context.Users.AsNoTracking().ToListAsync();

            string msg = UserValidator.ReturnErrorsEdit(users, oldUser, user.UserId);
            ViewBag.Message = msg;

            if (string.IsNullOrEmpty(msg))
            {

                user.CreatedAt = oldUser.CreatedAt; // Keep the original CreatedAt value

                if (user.UserImageFile != null && user.UserImageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await user.UserImageFile.CopyToAsync(memoryStream);
                        user.Image = memoryStream.ToArray();
                    }
                }
                else
                {
                    user.Image = oldUser.Image; // Set the image to null if no new image is provided
                }

                await _userService.EditUserAsync(user);
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }




        public async Task<IActionResult> DetailsAsync(int id)
        {
       


            ViewBag.userId = UserIdSupplier.id;
            ViewBag.isAdmin = AdminRoleSupplier.isAdmin;

            var user = await _userService.GetUserByIdAsync(id);
            return View(user);
        }
    }
}
