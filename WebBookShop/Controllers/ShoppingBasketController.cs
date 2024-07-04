using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebBookShop.Models;


namespace WebBookShop.Controllers
{
    [Authorize]
    public class ShoppingBasketController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext _context;

        public ShoppingBasketController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult Bascket(string status, string str, string bks)
        {
            ViewBag.Error = str;
            if (bks != null) { ViewBag.Book = JsonSerializer.Deserialize<List<Book>>(bks); }

            List<Book> books = new List<Book>();
            var id = User.Claims.FirstOrDefault(x => x.Type == "USERID")?.Value;
            Console.WriteLine(id);
            books = _context.favoritesPurchases.Include(x => x.Book).Where(x => x.User.Id == Convert.ToInt32(id) && x.Status == status).Select(x => x.Book).ToList();
            ViewBag.Status = status;
            return View(books);
        }


        public IActionResult AddFavorite(string idbook, string iduser)
        {
            string s = "";
            var bas = _context.favoritesPurchases.Include(x => x.Book).FirstOrDefault(x => x.User.Id == Convert.ToInt32(iduser) && x.Book.Id == Convert.ToInt32(idbook));
            if (bas != null)
            {
                bas.Status = "favorit";
                _context.SaveChanges();
                s = $"{bas.Book.Name} добавлена в избранное";
            }
            else { s = $"файл не найден"; }
            return RedirectToAction("Bascket", new { status = "korzina", str = s });
        }


        public IActionResult AddBasket(string idbook, string iduser)
        {
            string s = "";
            var bas = _context.favoritesPurchases.Include(x => x.Book).FirstOrDefault(x => x.User.Id == Convert.ToInt32(iduser) && x.Book.Id == Convert.ToInt32(idbook));
            if (bas != null)
            {
                bas.Status = "korzina";
                _context.SaveChanges();
                s = $"{bas.Book.Name} добавлена в корзину";
            }
            else { s = $"файл не найден"; }
            return RedirectToAction("Bascket", new { status = "favorit", str = s });
        }

        public IActionResult DelFavorite(string idbook, string iduser)
        {
            string s = "";
            var bas = _context.favoritesPurchases.Include(x => x.Book).FirstOrDefault(x => x.User.Id == Convert.ToInt32(iduser) && x.Book.Id == Convert.ToInt32(idbook));
            if (bas != null)
            {
                _context.favoritesPurchases.Remove(bas);
                _context.SaveChanges();
                s = $"{bas.Book.Name} книга удалена";
            }
            else { s = $"файл не найден"; }
            return RedirectToAction("Bascket", new { status = "favorit", str = s });
        }


        public IActionResult DelBasket(string idbook, string iduser)
        {
            string s = "";
            var bas = _context.favoritesPurchases.Include(x => x.Book).FirstOrDefault(x => x.User.Id == Convert.ToInt32(iduser) && x.Book.Id == Convert.ToInt32(idbook));
            if (bas != null)
            {
                _context.favoritesPurchases.Remove(bas);
                _context.SaveChanges();
                s = $"{bas.Book.Name} книга удалена";
            }
            else { s = $"файл не найден"; }
            return RedirectToAction("Bascket", new { status = "korzina", str = s });
        }

        public IActionResult Buy(string iduser)
        {
            List<Book> bk = new List<Book>();

            string s = "";
            List<FavoritesPurchases> FP = _context.favoritesPurchases.Include(b => b.Book).Where(x => x.User.Id == Convert.ToInt32(iduser) && x.Status == "korzina").ToList();
            if (FP.Count>0)
            {
                List<QuantityAndSales> q = _context.quantityAndSales.Where(x => x.countBooks == 0).Include(c => c.Book).ToList();
                foreach (var item in q)
                {
                    foreach (var t in FP)
                    {
                        if (item.Book.Name == t.Book.Name)
                        {
                            bk.Add(item.Book);
                        }
                    }
                }
                if (bk.Count > 0)
                {
                    s = "Данных книг нет в наличии:";
                    string strJson = JsonSerializer.Serialize(bk);
                    return RedirectToAction("Bascket", new { status = "korzina", str = s, bks = strJson });
                }
                s = "Спасибо за покупку!";
                _context.RemoveRange(FP);
                _context.SaveChanges();
            }
            else { s = $"Корзина пуста"; }
            return RedirectToAction("Bascket", new { status = "korzina", str = s });
        }
    }
}
