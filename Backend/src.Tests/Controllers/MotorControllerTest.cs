using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.controllers;
using src.db;
using src.db.models;
using src.dtos;
using src.services;
using Xunit;

namespace src.Tests.Controllers
{
    public class MotorControllerTests
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
                new MotorModel 
                { 
                    Id = 1, 
                    Brand = "Honda", 
                    PricePerDay = 100, 
                    IsAvailable = true,
                    ImageUrl = "honda.jpg"   
                },
                new MotorModel 
                { 
                    Id = 2, 
                    Brand = "Yamaha", 
                    PricePerDay = 150, 
                    IsAvailable = true,
                    ImageUrl = "yamaha.jpg" 
                }
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
        public async Task GetAll_ShouldReturnAllMotors()
        {
            // Arrange
            var context = await GetInMemoryDbContext();
            var service = new MotorService(context);
            var controller = new MotorController(service);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();

            var motors = okResult!.Value as List<MotorResponseDto>;
            motors.Should().NotBeNull();
            motors!.Count.Should().Be(2);
            motors.Select(m => m.Brand).Should().Contain(new[] { "Honda", "Yamaha" });
            motors.Select(m => m.ImageUrl)
            .Should()
            .Contain(new[] { "honda.jpg", "yamaha.jpg" });
        }

        [Fact]
        public async Task RentMotor_ValidRequest_ShouldReturnTransaction()
        {
            // Arrange
            var context = await GetInMemoryDbContext();
            var service = new MotorService(context);
            var controller = new MotorController(service);
            var request = new RentRequestDto(1, 1, 3); // Customer 1 rents Motor 1 for 3 days

            // Act
            var result = await controller.RentMotor(request);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();

            var transaction = okResult!.Value as TransactionResponseDto;
            transaction.Should().NotBeNull();
            transaction!.CustomerId.Should().Be(1);
            transaction.MotorId.Should().Be(1);
            transaction.DaysRented.Should().Be(3);
            transaction.TotalAmount.Should().Be(300); // 100 * 3
            transaction.CustomerEmail.Should().Be("test1@test.com");

            // Motor should now be unavailable
            var motor = await context.Motors.FindAsync(1);
            motor!.IsAvailable.Should().BeFalse();
        }

        [Fact]
        public async Task RentMotor_InvalidDays_ShouldReturnBadRequest()
        {
            // Arrange
            var context = await GetInMemoryDbContext();
            var service = new MotorService(context);
            var controller = new MotorController(service);
            var request = new RentRequestDto(1, 1, 0); // Invalid days

            // Act
            var result = await controller.RentMotor(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task RentMotor_UnavailableMotor_ShouldReturnBadRequest()
        {
            // Arrange
            var context = await GetInMemoryDbContext();
            var service = new MotorService(context);
            var controller = new MotorController(service);

            // Make Motor 1 unavailable
            var motor = await context.Motors.FindAsync(1);
            motor!.IsAvailable = false;
            await context.SaveChangesAsync();

            var request = new RentRequestDto(1, 1, 2);

            // Act
            var result = await controller.RentMotor(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task RentMotor_NonExistingCustomerOrMotor_ShouldReturnBadRequest()
        {
            // Arrange
            var context = await GetInMemoryDbContext();
            var service = new MotorService(context);
            var controller = new MotorController(service);

            var request1 = new RentRequestDto(999, 1, 2); // Non-existing customer
            var request2 = new RentRequestDto(1, 999, 2); // Non-existing motor

            // Act & Assert
            (await controller.RentMotor(request1)).Should().BeOfType<BadRequestObjectResult>();
            (await controller.RentMotor(request2)).Should().BeOfType<BadRequestObjectResult>();
        }
    }
}