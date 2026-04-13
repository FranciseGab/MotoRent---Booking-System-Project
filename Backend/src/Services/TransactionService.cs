using Microsoft.EntityFrameworkCore;
using src.db;
using src.db.models;

namespace src.services
{
    public class TransactionService
    {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        // Get all transactions (for testing or admin later)
        public async Task<List<TransactionModel>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                .Include(t => t.Customer)
                .Include(t => t.Motor)
                .ToListAsync();
        }

        // Get transactions by customer
        public async Task<List<TransactionModel>> GetTransactionsByCustomerAsync(int customerId)
        {
            return await _context.Transactions
                .Include(t => t.Motor)
                .Where(t => t.CustomerId == customerId)
                .ToListAsync();
        }

        // Get a transaction by ID
        public async Task<TransactionModel?> GetByIdAsync(int id)
        {
            return await _context.Transactions
                .Include(t => t.Customer)
                .Include(t => t.Motor)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}