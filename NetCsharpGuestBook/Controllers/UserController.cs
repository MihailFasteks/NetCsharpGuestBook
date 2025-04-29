using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using NetCsharpGuestBook.Models;
using System.Security.Cryptography;
using System.Text;


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
        public async Task<IActionResult> Registration(RegisterModel reg)
        {
            if (ModelState.IsValid)
            {
                Users user = new Users();
                user.Login = reg.Login;
                
                byte[] saltbuf=new byte[16];
                
                RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);
                
                StringBuilder sb=new StringBuilder(16);

                for (int i = 0; i < 16; i++)
                {
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));
                }
                string salt = sb.ToString();

                byte[] password = Encoding.Unicode.GetBytes(salt + reg.Password);

                var md5 = MD5.Create();
                
                byte[] byteHash=md5.ComputeHash(password);
                
                StringBuilder hash=new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                {
                    hash.Append(string.Format("{0:X2}", byteHash[i]));
                }
                user.Password=hash.ToString();
                user.Salt=salt;
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login", "User");
            }
            return RedirectToAction(nameof(Index));
        }
        
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Registration([Bind("Id,Name, Password")] Users users)
        // {
        //     try
        //     {
        //         _context.Add(users);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction("Login", "User");
        //     }
        //     catch (Exception)
        //     {
        //         return RedirectToAction(nameof(Index));
        //     }
        // }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel logon)
        {
            if (ModelState.IsValid)
            {
                if(_context.Users.ToList().Count == 0)
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }
                var users = _context.Users.Where(a => a.Login == logon.Login);
                if (users.ToList().Count == 0)
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }
                var user = users.First();
                string? salt = user.Salt;

                //переводим пароль в байт-массив  
                byte[] password = Encoding.Unicode.GetBytes(salt + logon.Password);

                //создаем объект для получения средств шифрования  
                var md5 = MD5.Create();

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = md5.ComputeHash(password);

                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                if (user.Password != hash.ToString())
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }
                HttpContext.Session.SetString("Login", user.Login);
               
                return RedirectToAction("Index", "Home");
            }
            return View(logon);
        }
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public IActionResult Login(Users user)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var dbUser = _context.Users.FirstOrDefault(u => u.Name == user.Name && u.Password == user.Password);
        //
        //         if (dbUser != null) {
        //             HttpContext.Session.SetString("user", user.Name); // создание сессионной переменной
        //             return RedirectToAction("Index", "Home");
        //         }
        //         ModelState.AddModelError("", "Неверный логин или пароль");
        //
        //     }
        //     return View(user);
        // }
    }
}
