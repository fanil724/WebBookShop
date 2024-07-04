using Microsoft.AspNetCore.Mvc;
using WebBookShop.Models;

namespace WebBookShop.Controllers
{
    public class EditUserController : Controller
    {
        ApplicationContext _context;
        public EditUserController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index(string id)
        {
            var u = _context.users.FirstOrDefault(x => x.Id == Convert.ToInt32(id));
            if (u != null)
            {
                RegisterModel rm = new RegisterModel()
                {
                    Name = u.Name,
                    Surname = u.SurName,
                    Email = u.Mail,
                    Password = u.Passowrd,
                    ConfirmPassword = u.Passowrd,
                    Login = u.Login
                };
                return View(rm);
            }
            return Redirect("Пользователь не найден");
        }

        public IActionResult Save(RegisterModel register)
        {
            if (ModelState.IsValid)
            {
                var id = User.Claims.FirstOrDefault(x => x.Type == "USERID")?.Value;
                var u = _context.users.FirstOrDefault(x => x.Id == Convert.ToInt32(id));
                if (u != null)
                {
                    u.SurName = register.Surname;
                    u.Name = register.Name;
                    u.Passowrd = register.Password;
                    u.Login = register.Login;
                    u.Mail = register.Email;
                    _context.users.Update(u);
                    _context.SaveChanges();
                }
            }
            else
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            return RedirectToAction("Index", "Home");
        }
    }
}
