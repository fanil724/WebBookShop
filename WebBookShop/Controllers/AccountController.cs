
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebBookShop.Models;

namespace WebBookShop.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext db;
        public AccountController(ApplicationContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User? user = db.users.FirstOrDefault(u => u.Login == model.Login && u.Passowrd == model.Password);
                if (user != null)
                {
                    await Authenticate(user.Login, user.Id.ToString(), user.Status);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User? user = db.users.FirstOrDefault(u => u.Mail == model.Email);
                if (user == null)
                {
                    user = new User
                    {
                        Mail = model.Email,
                        Passowrd = model.Password,
                        Login = model.Login,
                        Name = model.Name,
                        SurName = model.Surname,
                        Status = "user"
                    };
                    db.users.Add(user);
                    await db.SaveChangesAsync();
                    Console.WriteLine(user.Id);
                    await Authenticate(user.Login, user.Id.ToString(), user.Status);
                    return RedirectToAction("Index", "Home");
                }                
            }
            else
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            return View(model);
        }

        private async Task Authenticate(string userName, string idUser, string status)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim("USERID", idUser),
                new Claim("STATUS", status)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
