namespace Banking.Models.Entities
{
    public class LoginUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
