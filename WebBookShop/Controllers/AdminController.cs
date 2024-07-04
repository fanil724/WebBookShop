using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBookShop.Models;

namespace WebBookShop.Controllers
{
    public class AdminController : Controller
    {
        ApplicationContext _context;

        public AdminController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult AdminPanel()
        {
            return View();
        }

        public IActionResult Book(string str)
        {
            var genre = _context.genres.ToList();
            ViewBag.Genre = genre;
            var book = _context.books.ToList();
            ViewBag.Comment = str;
            return View(book);
        }

        public IActionResult Discount(string str)
        {
            var genre = _context.genres.ToList();
            ViewBag.Genre = genre;
            var discounts = _context.discounts.ToList();
            ViewBag.Comment = str;
            return View(discounts);
        }


        public IActionResult AddBook(Book book)
        {
            string s = "";
            var bk = _context.books.FirstOrDefault(x => x.Name == book.Name);
            if (bk == null)
            {
                if (book.BookContinuation == null) { book.BookContinuation = ""; }
                _context.books.Add(book);
                _context.SaveChanges();
                s = "Книга добавлена";
            }
            else { s = "Книга уже есть"; }

            return RedirectToAction("Book", new { str = s });
        }

        public IActionResult AddDiscount(string name, string dicount)
        {
            string s = "";
            var discount = _context.discounts.FirstOrDefault(x => x.Genre == name);
            if (discount == null)
            {
                _context.discounts.Add(new Discounts() { Genre = name, Discoute = Convert.ToInt32(dicount) });
                _context.SaveChanges();
                s = "Скидка добалена!";
            }
            else
            {
                s = "Скидка на этот жанр есть!";
            }
            return RedirectToAction("Discount", new { str = s });
        }

        public IActionResult DelDiscount(string id)
        {
            string s = "";
            var dis = _context.discounts.FirstOrDefault(x => x.Id == Convert.ToInt32(id));
            if (dis == null)
            {
                s = "Скидка не найдена!";
            }
            else
            {
                _context.discounts.Remove(dis);
                _context.SaveChanges();
                s = "Скидка удалена!";
            }

            return RedirectToAction("Discount", new { str = s });
        }

        [HttpPost]
        public IActionResult BooksCount(string name, string count)
        {
            string s = "";
            var cas = _context.quantityAndSales.Include(x => x.Book).FirstOrDefault(b => b.Book.Name == name);
            if (cas == null)
            {
                _context.quantityAndSales.Add(
                       new QuantityAndSales()
                       {
                           Book = _context.books.FirstOrDefault(x => x.Name == name),
                           countBooks = Convert.ToInt32(count),
                           SalesBooks = 0
                       }
                    );
                _context.SaveChanges();
                s = "Книга добавлена в таблицу";
            }
            else
            {
                cas.countBooks += Convert.ToInt32(count);
                _context.quantityAndSales.Update(cas);
                _context.SaveChanges();
                s = "Количество книг увеличено";
            }
            return RedirectToAction("Book", new { str = s });
        }

    }
}
