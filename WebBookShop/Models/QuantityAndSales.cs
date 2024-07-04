using System.ComponentModel.DataAnnotations.Schema;

namespace WebBookShop.Models
{
    public class QuantityAndSales
    {
        public int Id { get; set; }
        public int BooksID { get; set; }
        [ForeignKey("BooksID")]
        public Book Book { get; set; }
        public int countBooks { get; set; }
        public int SalesBooks { get; set; }
    }
}
