using Microsoft.EntityFrameworkCore;
using src.db;
using src.db.models;

namespace src.services
{
    public class MotorService
    {
        private readonly AppDbContext _context;

        public MotorService(AppDbContext context)
        {
            _context = context;
        }

        // Return all motors
        public async Task<List<MotorModel>> GetAllMotorsAsync()
        {
            return await _context.Motors.ToListAsync();
        }

        // Get motor by id
        public async Task<MotorModel?> GetMotorByIdAsync(int id)
        {
            return await _context.Motors.FirstOrDefaultAsync(m => m.Id == id);
        }

        // Rent a motor
        public async Task<TransactionModel?> RentMotorAsync(int customerId, int motorId, int days)
        {
            if (days <= 0) return null;

            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) return null;

            var motor = await _context.Motors.FindAsync(motorId);
            if (motor == null || !motor.IsAvailable) return null;

            var total = motor.PricePerDay * days;

            var transaction = new TransactionModel
            {
                CustomerId = customerId,
                MotorId = motorId,
                DaysRented = days,
                TotalAmount = total,
                DateRented = DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);
            motor.IsAvailable = false;

            await _context.SaveChangesAsync();

            // Attach for DTO mapping
            transaction.Customer = customer;
            transaction.Motor = motor;

            return transaction;
        }
    }
}