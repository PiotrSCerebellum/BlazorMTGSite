namespace MTG.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Collection { get; set; }
    }
}
