using Microsoft.EntityFrameworkCore;
using src.db.models;

namespace src.db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<CustomerModel> Customers => Set<CustomerModel>();
        public DbSet<MotorModel> Motors => Set<MotorModel>();
        public DbSet<TransactionModel> Transactions => Set<TransactionModel>();
    }
}