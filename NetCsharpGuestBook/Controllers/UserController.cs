using Microsoft.AspNetCore.Mvc;
using NetCsharpGuestBook.Models;


namespace NetCsharpGuestBook.Controllers
{
    public class UserController : Controller
    {
        private readonly BookContext _context;

        public UserController(BookContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Registration()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration([Bind("Id,Name, Password")] Users users)
        {
            try
            {
                _context.Add(users);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "User");
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Users user)
        {
            if (ModelState.IsValid)
            {
                var dbUser = _context.Users.FirstOrDefault(u => u.Name == user.Name && u.Password == user.Password);

                if (dbUser != null) {
                    HttpContext.Session.SetString("user", user.Name); // создание сессионной переменной
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Неверный логин или пароль");

            }
            return View(user);
        }
    }
}
