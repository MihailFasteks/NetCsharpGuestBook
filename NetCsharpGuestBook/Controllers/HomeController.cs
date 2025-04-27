using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCsharpGuestBook.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NetCsharpGuestBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookContext _context;

        public HomeController(BookContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users.Include(u => u.Messages).ToList();
            return View(users);
        }

        public IActionResult AddMessage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMessage(string Text)
        {
            try
            {
                
                var username = HttpContext.Session.GetString("user");
                Console.WriteLine("Username from session: " + username); 

                if (string.IsNullOrEmpty(username))
                {
                    Console.WriteLine("User is not logged in, redirecting to login page.");
                    return RedirectToAction("Login", "User");
                }

                var dbUser = _context.Users.FirstOrDefault(u => u.Name == username);
                Console.WriteLine("Found user: " + (dbUser != null ? dbUser.Name : "Not found")); 

                if (dbUser == null)
                {
                    Console.WriteLine("User not found, redirecting to login page.");
                    return RedirectToAction("Login", "User");
                }

                var message = new Messages
                {
                    Text = Text,
                    Date = DateTime.Now,
                    UserId = dbUser.Id
                };

                Console.WriteLine($"Adding message: {Text} for user: {dbUser.Name}"); 

                _context.Add(message);

               
                Console.WriteLine("Saving changes to database...");
                await _context.SaveChangesAsync();

                Console.WriteLine("Message added successfully.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Error while adding message: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Index", "Home"); 
        }
    }
}
