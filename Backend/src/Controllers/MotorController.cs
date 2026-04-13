using Microsoft.AspNetCore.Mvc;
using src.dtos;
using src.services;

namespace src.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotorController : ControllerBase
    {
        private readonly MotorService _service;

        public MotorController(MotorService service)
        {
            _service = service;
        }

        // GET: api/motor
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var motors = await _service.GetAllMotorsAsync();

            var dto = motors.Select(m => new MotorResponseDto(
                m.Id,
                m.Brand,
                m.PricePerDay,
                m.IsAvailable,
                m.ImageUrl      // ✅ Added
            )).ToList();

            return Ok(dto);
        }

        // POST: api/motor/rent
        [HttpPost("rent")]
        public async Task<IActionResult> RentMotor([FromBody] RentRequestDto request)
        {
            if (request.Days <= 0)
                return BadRequest(new { message = "Days must be greater than 0." });

            var transaction = await _service.RentMotorAsync(
                request.CustomerId,
                request.MotorId,
                request.Days
            );

            if (transaction == null)
                return BadRequest(new { message = "Customer or Motor not found, or Motor unavailable." });

            var transactionDto = new TransactionResponseDto(
                transaction.Id,
                transaction.CustomerId,
                transaction.Customer?.Email ?? "Unknown",
                transaction.MotorId,
                transaction.Motor?.Brand ?? "Unknown",
                transaction.DaysRented,
                transaction.TotalAmount,
                transaction.DateRented
            );

            return Ok(transactionDto);
        }
    }
}