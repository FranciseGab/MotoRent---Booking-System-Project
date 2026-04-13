using System.ComponentModel.DataAnnotations;

namespace src.db.models
{
    public class CustomerModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public List<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
    }
}