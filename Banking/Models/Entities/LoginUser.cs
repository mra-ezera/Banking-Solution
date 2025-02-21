namespace Banking.Models.Entities
{
    public class LoginUser
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
    }
}
