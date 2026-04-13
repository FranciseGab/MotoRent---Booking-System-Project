using src.db.models;
using src.db.seeds;

namespace src.db
{
    public static class DbInit
    {
        public static void Initialize(AppDbContext context)
        {
            // Create DB if not exists
            context.Database.EnsureCreated();

            // Seed Customers if empty
            if (!context.Customers.Any())
            {
                context.Customers.AddRange(CustomerSeed.GetCustomers());
            }

            // Seed Motors if empty
            if (!context.Motors.Any())
            {
                context.Motors.AddRange(MotorSeed.GetMotors());
            }

            context.SaveChanges();
        }
    }
}