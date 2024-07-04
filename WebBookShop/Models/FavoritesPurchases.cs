

namespace WebBookShop.Models
{
    public class FavoritesPurchases
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public User User { get; set; }
        public string? Status { get; set; }

    }
}
