using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.controllers;
using src.db;
using src.db.models;
using src.services;
using src.dtos;
using Xunit;

namespace src.Tests.Controllers
{
    public class TransactionControllerTests
    {
        private async Task<AppDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
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
        public async Task GetAll_ShouldReturnAllTransactions()
        {
            var context = await GetInMemoryDbContext();
            var service = new TransactionService(context);
            var controller = new TransactionController(service);

            var result = await controller.GetAll();

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();

            var list = okResult!.Value as List<TransactionResponseDto>;
            list.Should().NotBeNull();
            list!.Count.Should().Be(2);
        }

        [Fact]
        public async Task GetById_ExistingTransaction_ShouldReturnTransaction()
        {
            var context = await GetInMemoryDbContext();
            var service = new TransactionService(context);
            var controller = new TransactionController(service);

            var result = await controller.GetById(1);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();

            var dto = okResult!.Value as TransactionResponseDto;
            dto.Should().NotBeNull();
            dto!.CustomerId.Should().Be(1);
            dto.MotorId.Should().Be(1);
        }

        [Fact]
        public async Task GetById_NonExistingTransaction_ShouldReturnNotFound()
        {
            var context = await GetInMemoryDbContext();
            var service = new TransactionService(context);
            var controller = new TransactionController(service);

            var result = await controller.GetById(999);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetByCustomer_ShouldReturnCorrectTransactions()
        {
            var context = await GetInMemoryDbContext();
            var service = new TransactionService(context);
            var controller = new TransactionController(service);

            var result = await controller.GetByCustomer(1);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();

            var list = okResult!.Value as List<TransactionResponseDto>;
            list.Should().NotBeNull();
            list!.Count.Should().Be(1);
            list[0].CustomerId.Should().Be(1);
        }
    }
}