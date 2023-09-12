
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using Moq;
using OsDsII.Controllers;
using OsDsII.Services;
using OsDsII.Models;
using OsDsII.Exceptions;
using OsDsII.Http;
using OsDsII.DTOS;

namespace OsDsII.Tests
{
    public class CustomersControllerTests
    {
        private readonly Mock<ICustomersService> _mockService;
        private readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            _mockService = new Mock<ICustomersService>();
            _controller = new CustomersController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOk()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "John" },
                new Customer { Id = 2, Name = "Jane" },
            };

            _mockService.Setup(svc => svc.GetAllAsync()).ReturnsAsync(customers);

            // Act
            IActionResult result = await _controller.GetAllAsync();

            // Assert
            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);

            // If you want to further check the body content
            var body = Assert.IsType<HttpResponseApi<IEnumerable<CustomerDTO>>>(objectResult.Value);
            Assert.Equal(customers.Count, body.Data.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOk()
        {
            // Arrange
            Customer customer = new Customer { Id = 1, Name = "John" };

            _mockService.Setup(svc => svc.GetByIdAsync(1)).ReturnsAsync(customer);

            // Act
            IActionResult result = await _controller.GetByIdAsync(1);

            // Assert
            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);

            // If you want to further check the body content
            var body = Assert.IsType<HttpResponseApi<CustomerDTO>>(objectResult.Value);
            Assert.Equal(customer.Id, body.Data.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound()
        {
            var exception = new BaseException("NotFound", "Customer not found", HttpStatusCode.NotFound, 404, "/api/v1/Customers/1", DateTimeOffset.UtcNow, null);
            // Arrange
            _mockService.Setup(svc => svc.GetByIdAsync(1)).ThrowsAsync(exception);

            // Act
            IActionResult result = await _controller.GetByIdAsync(1);

            // Assert
            ContentResult contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, contentResult.StatusCode);

            // If you want to further check the body content
            var body = JsonConvert.DeserializeObject<HttpErrorResponse>(contentResult.Content);
            Assert.Equal("NotFound", body.ErrorCode);
        }
    }
}