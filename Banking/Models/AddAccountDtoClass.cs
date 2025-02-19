using Banking.Models.Entities;

namespace Banking.Models
{
    public class AddAccountDto
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public decimal Balance { get; set; }
    }
}
