// using Microsoft.EntityFrameworkCore;
// using src.db;
// using src.db.models;

// namespace src.services
// {
//     public class CustomerService
//     {
//         private readonly AppDbContext _context;

//         public CustomerService(AppDbContext context)
//         {
//             _context = context;
//         }

//         // Get all customers (for testing or admin later)
//         public async Task<List<CustomerModel>> GetAllCustomersAsync()
//         {
//             return await _context.Customers.ToListAsync();
//         }

//         // Login: return customer if email/password match
//         public async Task<CustomerModel?> LoginAsync(string email, string password)
//         {
//             return await _context.Customers
//                 .FirstOrDefaultAsync(c => c.Email == email && c.Password == password);
//         }

//         // Get customer by ID
//         public async Task<CustomerModel?> GetByIdAsync(int id)
//         {
//             return await _context.Customers
//                 .Include(c => c.Transactions)
//                 .ThenInclude(t => t.Motor)
//                 .FirstOrDefaultAsync(c => c.Id == id);
//         }
//     }
// }

using Microsoft.EntityFrameworkCore;
using src.db;
using src.db.models;

namespace src.services
{
    public class CustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        // Get all customers
        public async Task<List<CustomerModel>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        // Login: return customer if email/password match
        public async Task<CustomerModel?> LoginAsync(string email, string password)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email && c.Password == password);
        }

        // Check if email already exists
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Customers.AnyAsync(c => c.Email == email);
        }

        // Signup: create and save a new customer
        public async Task<CustomerModel> SignupAsync(string email, string password)
        {
            var customer = new CustomerModel
            {
                Email = email,
                Password = password
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        // Get customer by ID
        public async Task<CustomerModel?> GetByIdAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.Transactions)
                .ThenInclude(t => t.Motor)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}