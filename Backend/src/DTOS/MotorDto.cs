namespace src.dtos
{
    // Request DTO for renting a motor
    public record RentRequestDto(int CustomerId, int MotorId, int Days);

    // Response DTO for motors
    public record MotorResponseDto(
        int Id,
        string Brand,
        decimal PricePerDay,
        bool IsAvailable,
        string ImageUrl   // ✅ Added
    );

    // Response DTO for transactions
    public record TransactionResponseDto(
        int Id,
        int CustomerId,
        string CustomerEmail,
        int MotorId,
        string MotorBrand,
        int DaysRented,
        decimal TotalAmount,
        DateTime DateRented
    );
}