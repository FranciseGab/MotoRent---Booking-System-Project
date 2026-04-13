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
    public class CustomerControllerTests
    {
        private async Task<AppDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = new AppDbContext(options);

            // Seed some customers
            if (!await context.Customers.AnyAsync())
            {
                context.Customers.AddRange(new List<CustomerModel>
                {
                    new CustomerModel { Id = 1, Email = "test1@test.com", Password = "pass1" },
                    new CustomerModel { Id = 2, Email = "test2@test.com", Password = "pass2" }
                });
                await context.SaveChangesAsync();
            }

            return context;
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllCustomers()
        {
            // Arrange
            var context = await GetInMemoryDbContext();
            var service = new CustomerService(context);
            var controller = new CustomerController(service);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();

            var customers = okResult!.Value as List<CustomerResponseDto>;
            customers.Should().NotBeNull();
            customers!.Count.Should().Be(2);
            customers!.Select(c => c.Email).Should().Contain(new[] { "test1@test.com", "test2@test.com" });
        }

        [Fact]
        public async Task GetById_ExistingCustomer_ShouldReturnCustomerWithTransactions()
        {
            // Arrange
            var context = await GetInMemoryDbContext();
            var service = new CustomerService(context);
            var controller = new CustomerController(service);

            // Act
            var result = await controller.GetById(1);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();

            var customer = okResult!.Value as CustomerWithTransactionsDto;
            customer.Should().NotBeNull();
            customer!.Email.Should().Be("test1@test.com");
            customer.Transactions.Should().BeEmpty(); // No transactions seeded
        }

        [Fact]
        public async Task GetById_NonExistingCustomer_ShouldReturnNotFound()
        {
            // Arrange
            var context = await GetInMemoryDbContext();
            var service = new CustomerService(context);
            var controller = new CustomerController(service);

            // Act
            var result = await controller.GetById(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Login_ValidCredentials_ShouldReturnOk()
        {
            // Arrange
            var context = await GetInMemoryDbContext();
            var service = new CustomerService(context);
            var controller = new CustomerController(service);
            var request = new LoginRequest("test1@test.com", "pass1");

            // Act
            var result = await controller.Login(request);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();

            var response = okResult!.Value as CustomerResponseDto;
            response.Should().NotBeNull();
            response!.Id.Should().Be(1);
            response.Email.Should().Be("test1@test.com");
        }

        [Fact]
        public async Task Login_InvalidCredentials_ShouldReturnUnauthorized()
        {
            // Arrange
            var context = await GetInMemoryDbContext();
            var service = new CustomerService(context);
            var controller = new CustomerController(service);
            var request = new LoginRequest("wrong@test.com", "wrongpass");

            // Act
            var result = await controller.Login(request);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }
    }
}