namespace WebBookShop.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? FIOAvtora { get; set; }
        public string? NameOfThePublisher { get; set; }
        public int PageNumber { get; set; }
        public string Genre { get; set; }

        public string? PublicationYear { get; set; }
        public double CostPrice { get; set; }
        public double SellingPrice { get; set; }


        public string? BookContinuation { get; set; }
    }
}
