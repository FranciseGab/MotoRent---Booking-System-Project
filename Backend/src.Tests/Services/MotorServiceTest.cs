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
    public class MotorServiceTests
    {
        private async Task<AppDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB per test
                .Options;

            var context = new AppDbContext(options);

            // Seed motors and customers
            if (!await context.Motors.AnyAsync())
            {
                context.Motors.AddRange(new List<MotorModel>
                {
                    new MotorModel { Id = 1, Brand = "Honda", PricePerDay = 100, IsAvailable = true },
                    new MotorModel { Id = 2, Brand = "Yamaha", PricePerDay = 150, IsAvailable = true }
                });
            }

            if (!await context.Customers.AnyAsync())
            {
                context.Customers.AddRange(new List<CustomerModel>
                {
                    new CustomerModel { Id = 1, Email = "test1@test.com", Password = "pass1" },
                    new CustomerModel { Id = 2, Email = "test2@test.com", Password = "pass2" }
                });
            }

            await context.SaveChangesAsync();
            return context;
        }

        [Fact]
        public async Task GetAllMotorsAsync_ShouldReturnAllMotors()
        {
            // Arrange
            var context = await GetInMemoryDbContext();
            var service = new MotorService(context);

            // Act
            var motors = await service.GetAllMotorsAsync();

            // Assert
            motors.Should().HaveCount(2);
            motors.Select(m => m.Brand).Should().Contain(new[] { "Honda", "Yamaha" });
        }

        [Fact]
        public async Task RentMotorAsync_ValidRequest_ShouldReturnTransaction()
        {
            // Arrange
            var context = await GetInMemoryDbContext();
            var service = new MotorService(context);

            // Act
            var transaction = await service.RentMotorAsync(1, 1, 3); // Customer 1 rents Motor 1 for 3 days

            // Assert
            transaction.Should().NotBeNull();
            transaction!.CustomerId.Should().Be(1);
            transaction.MotorId.Should().Be(1);
            transaction.DaysRented.Should().Be(3);
            transaction.TotalAmount.Should().Be(300); // 100 * 3
            transaction.Customer!.Email.Should().Be("test1@test.com");
            transaction.Motor!.IsAvailable.Should().BeFalse();

            // Database should have updated motor availability
            var motorInDb = await context.Motors.FindAsync(1);
            motorInDb!.IsAvailable.Should().BeFalse();
        }

        [Fact]
        public async Task RentMotorAsync_InvalidDays_ShouldReturnNull()
        {
            // Arrange
            var context = await GetInMemoryDbContext();
            var service = new MotorService(context);

            // Act
            var transaction = await service.RentMotorAsync(1, 1, 0); // invalid days

            // Assert
            transaction.Should().BeNull();
        }

        [Fact]
        public async Task RentMotorAsync_UnavailableMotor_ShouldReturnNull()
        {
            // Arrange
            var context = await GetInMemoryDbContext();
            var service = new MotorService(context);

            // Make motor unavailable
            var motor = await context.Motors.FindAsync(1);
            motor!.IsAvailable = false;
            await context.SaveChangesAsync();

            // Act
            var transaction = await service.RentMotorAsync(1, 1, 2);

            // Assert
            transaction.Should().BeNull();
        }

        [Fact]
        public async Task RentMotorAsync_NonExistingCustomerOrMotor_ShouldReturnNull()
        {
            // Arrange
            var context = await GetInMemoryDbContext();
            var service = new MotorService(context);

            // Non-existing customer
            var tx1 = await service.RentMotorAsync(999, 1, 2);

            // Non-existing motor
            var tx2 = await service.RentMotorAsync(1, 999, 2);

            // Assert
            tx1.Should().BeNull();
            tx2.Should().BeNull();
        }
    }
}