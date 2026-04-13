// namespace src.dtos
// {
//     public record CustomerResponseDto(int Id, string Email);

//     public record CustomerWithTransactionsDto(
//         int Id,
//         string Email,
//         List<TransactionDto> Transactions
//     );

//     public record TransactionDto(
//         int Id,
//         int MotorId,
//         string MotorBrand,
//         int DaysRented,
//         decimal TotalAmount,
//         DateTime DateRented
//     );
//     public record LoginRequest(string Email, string Password);
// }

namespace src.dtos
{
    // Response DTO for returning customer info
    public record CustomerResponseDto(int Id, string Email);

    // Response DTO including transactions
    public record CustomerWithTransactionsDto(
        int Id,
        string Email,
        List<TransactionDto> Transactions
    );

    public record TransactionDto(
        int Id,
        int MotorId,
        string MotorBrand,
        int DaysRented,
        decimal TotalAmount,
        DateTime DateRented
    );

    // Login request DTO
    public record LoginRequest(string Email, string Password);

    // Signup request DTO
    public record SignupRequest(string Email, string Password);
}