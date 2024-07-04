using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebBookShop.Models;

namespace WebBookShop.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

       
        public IActionResult Mag()
        {          
            List<Book> books = new List<Book>();
            books = _context.books.ToList();
            return View(books);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Mag(string search)
        {
            List<Book> books = new List<Book>();
            if (search == null)
            {
                books = _context.books.ToList();
                return View(books);
            }
            books = _context.books.Where(x => x.Name == search).ToList();
            return View(books);
        }

        public IActionResult AddBasket(string idbook, string iduser)
        {
            var bas = _context.favoritesPurchases.Include(x => x.Book).FirstOrDefault(x => x.User.Id == Convert.ToInt32(iduser)
            && x.Book.Id == Convert.ToInt32(idbook) && x.Status == "korzina");
            if (bas == null)
            {
                var b = _context.books.First(x => x.Id == Convert.ToInt32(idbook));
                _context.favoritesPurchases.Add(new FavoritesPurchases()
                {
                    Book = b,
                    User = _context.users.First(x => x.Id == Convert.ToInt32(iduser)),
                    Status = "korzina"
                });
                _context.SaveChanges();
                ViewBag.TextComment = $"{b.Name} добавлена в корзину";
            }
            else
            {
                ViewBag.TextComment = $"{bas.Book.Name} уже есть в корзине";
            }
            return Redirect("Mag");
        }


        public IActionResult AddFavorite(string idbook, string iduser)
        {
            var bas = _context.favoritesPurchases.Include(x => x.Book).FirstOrDefault(x => x.User.Id == Convert.ToInt32(iduser)
            && x.Book.Id == Convert.ToInt32(idbook) && x.Status == "favorit");
            if (bas == null)
            {
                var b = _context.books.First(x => x.Id == Convert.ToInt32(idbook));
                _context.favoritesPurchases.Add(new FavoritesPurchases()
                {
                    Book = b,
                    User = _context.users.First(x => x.Id == Convert.ToInt32(iduser)),
                    Status = "favorit"
                });
                _context.SaveChanges();
                ViewBag.TextComment = $"{b.Name} добавлена в избранное";
            }
            else { ViewBag.TextComment = $"{bas.Book.Name} уже есть в избранном"; }

            return Redirect("Mag");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
