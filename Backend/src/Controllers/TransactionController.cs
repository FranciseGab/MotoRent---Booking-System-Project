using Microsoft.AspNetCore.Mvc;
using src.dtos;
using src.services;

namespace src.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _service;

        public TransactionController(TransactionService service)
        {
            _service = service;
        }

        // GET: api/transaction
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _service.GetAllTransactionsAsync();

            var dtoList = transactions.Select(t => new TransactionResponseDto(
                Id: t.Id,
                CustomerId: t.CustomerId,
                CustomerEmail: t.Customer?.Email ?? "Unknown",
                MotorId: t.MotorId,
                MotorBrand: t.Motor?.Brand ?? "Unknown",
                DaysRented: t.DaysRented,
                TotalAmount: t.TotalAmount,
                DateRented: t.DateRented
            )).ToList();

            return Ok(dtoList);
        }

        // GET: api/transaction/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transaction = await _service.GetByIdAsync(id);
            if (transaction == null) return NotFound();

            var dto = new TransactionResponseDto(
                Id: transaction.Id,
                CustomerId: transaction.CustomerId,
                CustomerEmail: transaction.Customer?.Email ?? "Unknown",
                MotorId: transaction.MotorId,
                MotorBrand: transaction.Motor?.Brand ?? "Unknown",
                DaysRented: transaction.DaysRented,
                TotalAmount: transaction.TotalAmount,
                DateRented: transaction.DateRented
            );

            return Ok(dto);
        }

        // GET: api/transaction/customer/{customerId}
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(int customerId)
        {
            var transactions = await _service.GetTransactionsByCustomerAsync(customerId);

            var dtoList = transactions.Select(t => new TransactionResponseDto(
                Id: t.Id,
                CustomerId: t.CustomerId,
                CustomerEmail: t.Customer?.Email ?? "Unknown",
                MotorId: t.MotorId,
                MotorBrand: t.Motor?.Brand ?? "Unknown",
                DaysRented: t.DaysRented,
                TotalAmount: t.TotalAmount,
                DateRented: t.DateRented
            )).ToList();

            return Ok(dtoList);
        }
    }
}