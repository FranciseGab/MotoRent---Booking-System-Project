using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using src.db;
using src.db.models;
using src.services;
using Xunit;

namespace src.Tests
{
    public class CustomerServiceTests
    {
        private async Task<AppDbContext> GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB for each test
                .Options;

            var context = new AppDbContext(options);

            // Seed some data
            context.Customers.AddRange(
                new CustomerModel { Id = 1, Email = "test1@test.com", Password = "pass1" },
                new CustomerModel { Id = 2, Email = "test2@test.com", Password = "pass2" }
            );
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task GetAllCustomersAsync_ShouldReturnAllCustomers()
        {
            var context = await GetInMemoryDb();
            var service = new CustomerService(context);

            var customers = await service.GetAllCustomersAsync();

            customers.Should().HaveCount(2);
            customers.Should().Contain(c => c.Email == "test1@test.com");
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnCustomer_WhenEmailPasswordMatch()
        {
            var context = await GetInMemoryDb();
            var service = new CustomerService(context);

            var customer = await service.LoginAsync("test1@test.com", "pass1");

            customer.Should().NotBeNull();
            customer!.Email.Should().Be("test1@test.com");
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenEmailPasswordDoNotMatch()
        {
            var context = await GetInMemoryDb();
            var service = new CustomerService(context);

            var customer = await service.LoginAsync("wrong@test.com", "pass");

            customer.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCustomer_WhenExists()
        {
            var context = await GetInMemoryDb();
            var service = new CustomerService(context);

            var customer = await service.GetByIdAsync(1);

            customer.Should().NotBeNull();
            customer!.Email.Should().Be("test1@test.com");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            var context = await GetInMemoryDb();
            var service = new CustomerService(context);

            var customer = await service.GetByIdAsync(999);

            customer.Should().BeNull();
        }
    }
}