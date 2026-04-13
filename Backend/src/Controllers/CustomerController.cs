// using Microsoft.AspNetCore.Mvc;
// using src.db.models;
// using src.services;
// using src.dtos;

// namespace src.controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class CustomerController : ControllerBase
//     {
//         private readonly CustomerService _service;

//         public CustomerController(CustomerService service)
//         {
//             _service = service;
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetAll()
//         {
//             var customers = await _service.GetAllCustomersAsync();

//             // Map to DTO
//             var dto = customers.Select(c => new CustomerResponseDto(c.Id, c.Email)).ToList();

//             return Ok(dto);
//         }

//         [HttpPost("login")]
//         public async Task<IActionResult> Login([FromBody] LoginRequest request)
//         {
//             var customer = await _service.LoginAsync(request.Email, request.Password);
//             if (customer == null)
//             {
//                 return Unauthorized(new { message = "Invalid email or password" });
//             }

//             var dto = new CustomerResponseDto(customer.Id, customer.Email);
//             return Ok(dto);
//         }

//         [HttpGet("{id}")]
//         public async Task<IActionResult> GetById(int id)
//         {
//             var customer = await _service.GetByIdAsync(id);
//             if (customer == null) return NotFound();

//             // Map transactions to DTOs
//             var transactionDtos = customer.Transactions.Select(t => new TransactionDto(
//                 t.Id,
//                 t.MotorId,
//                 t.Motor?.Brand ?? "Unknown",
//                 t.DaysRented,
//                 t.TotalAmount,
//                 t.DateRented
//             )).ToList();

//             var dto = new CustomerWithTransactionsDto(customer.Id, customer.Email, transactionDtos);

//             return Ok(dto);
//         }
//     }
// }

using Microsoft.AspNetCore.Mvc;
using src.db.models;
using src.services;
using src.dtos;

namespace src.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _service;

        public CustomerController(CustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _service.GetAllCustomersAsync();
            var dto = customers.Select(c => new CustomerResponseDto(c.Id, c.Email)).ToList();
            return Ok(dto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var customer = await _service.LoginAsync(request.Email, request.Password);
            if (customer == null)
                return Unauthorized(new { message = "Invalid email or password" });

            var dto = new CustomerResponseDto(customer.Id, customer.Email);
            return Ok(dto);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            // Check if email already exists
            var exists = await _service.EmailExistsAsync(request.Email);
            if (exists)
                return BadRequest(new { message = "Email is already registered." });

            var customer = await _service.SignupAsync(request.Email, request.Password);
            var dto = new CustomerResponseDto(customer.Id, customer.Email);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _service.GetByIdAsync(id);
            if (customer == null) return NotFound();

            var transactionDtos = customer.Transactions.Select(t => new TransactionDto(
                t.Id,
                t.MotorId,
                t.Motor?.Brand ?? "Unknown",
                t.DaysRented,
                t.TotalAmount,
                t.DateRented
            )).ToList();

            var dto = new CustomerWithTransactionsDto(customer.Id, customer.Email, transactionDtos);
            return Ok(dto);
        }
    }
}