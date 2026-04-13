using System.ComponentModel.DataAnnotations;

namespace src.db.models
{
    public class MotorModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public decimal PricePerDay { get; set; }

        public bool IsAvailable { get; set; } = true;

        // New property for image
        public string ImageUrl { get; set; } = string.Empty;
    }
}