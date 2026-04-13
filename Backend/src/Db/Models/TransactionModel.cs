using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.db.models
{
    public class TransactionModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public CustomerModel? Customer { get; set; }

        [Required]
        public int MotorId { get; set; }

        [ForeignKey("MotorId")]
        public MotorModel? Motor { get; set; }

        public int DaysRented { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime DateRented { get; set; } = DateTime.UtcNow;
    }
}