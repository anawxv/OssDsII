using Moq;
using OsDsII.Services;
using OsDsII.Repositories;
using OsDsII.Models;
using OsDsII.DAL;
using OsDsII.Exceptions;


namespace OsDsII.Tests;
public class CustomersServiceTests
{
    private readonly Mock<ICustomersRepository> _mockRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CustomersService _service;

    public CustomersServiceTests()
    {
        _mockRepository = new Mock<ICustomersRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _service = new CustomersService(_mockRepository.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCustomers()
    {
        List<Customer> expectedCustomers = new List<Customer>
        {
            new Customer {Id = 1, Name = "John Doe", Phone = "+5511999999999", Email = "johndoe@mail.com"},
            new Customer {Id = 1, Name = "Jolyne", Phone = "+5521988874665", Email = "jolyne@mail.com"}
        };

        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedCustomers);

        var result = await _service.GetAllAsync();

        Assert.Equal(expectedCustomers, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnsCustomerById()
    {
        Customer expectedCustomer = new Customer { Id = 1, Name = "John Doe", Phone = "+5511999999999", Email = "johndoe@mail.com" };

        _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(expectedCustomer);

        Customer result = await _service.GetByIdAsync(1);

        Assert.Equal(expectedCustomer, result);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsNotFoundException()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Customer)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(4942615));
    }
}