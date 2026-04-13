using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using src.db;
using src.db.models;
using src.services;
using Xunit;

namespace src.Tests.Services
{
    public class TransactionServiceTests
    {
        private async Task<AppDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB per test
                .Options;

            var context = new AppDbContext(options);

            if (!await context.Customers.AnyAsync())
            {
                var customers = new List<CustomerModel>
                {
                    new CustomerModel { Id = 1, Email = "cust1@test.com", Password = "pass1" },
                    new CustomerModel { Id = 2, Email = "cust2@test.com", Password = "pass2" }
                };
                var motors = new List<MotorModel>
                {
                    new MotorModel { Id = 1, Brand = "Yamaha", PricePerDay = 100 },
                    new MotorModel { Id = 2, Brand = "Honda", PricePerDay = 150 }
                };

                context.Customers.AddRange(customers);
                context.Motors.AddRange(motors);

                context.Transactions.AddRange(new List<TransactionModel>
                {
                    new TransactionModel
                    {
                        Id = 1,
                        CustomerId = 1,
                        Customer = customers[0],
                        MotorId = 1,
                        Motor = motors[0],
                        DaysRented = 2,
                        TotalAmount = 200,
                        DateRented = DateTime.UtcNow
                    },
                    new TransactionModel
                    {
                        Id = 2,
                        CustomerId = 2,
                        Customer = customers[1],
                        MotorId = 2,
                        Motor = motors[1],
                        DaysRented = 3,
                        TotalAmount = 450,
                        DateRented = DateTime.UtcNow
                    }
                });

                await context.SaveChangesAsync();
            }

            return context;
        }

        [Fact]
        public async Task GetAllTransactionsAsync_ShouldReturnAllTransactions()
        {
            var context = await GetInMemoryDbContext();
            var service = new TransactionService(context);

            var result = await service.GetAllTransactionsAsync();

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ShouldReturnTransaction()
        {
            var context = await GetInMemoryDbContext();
            var service = new TransactionService(context);

            var result = await service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.CustomerId.Should().Be(1);
            result.MotorId.Should().Be(1);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
        {
            var context = await GetInMemoryDbContext();
            var service = new TransactionService(context);

            var result = await service.GetByIdAsync(999);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetTransactionsByCustomerAsync_ShouldReturnCorrectTransactions()
        {
            var context = await GetInMemoryDbContext();
            var service = new TransactionService(context);

            var result = await service.GetTransactionsByCustomerAsync(1);

            result.Should().HaveCount(1);
            result[0].CustomerId.Should().Be(1);
        }
    }
}