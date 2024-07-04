namespace WebBookShop.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Login { get; set; }
        public string? Passowrd { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public string? Mail { get; set; }
        public string? Status { get; set; }


       /* public bool IsNull()
        {
            if (null == Login) return false;
            if (null == Passowrd) return false;
            if (null == Name) return false;
            if (null == SurName) return false;
            if (null == Mail) return false;

            return true;
        }*/
    }

}
