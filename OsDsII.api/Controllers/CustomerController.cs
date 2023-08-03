using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsDsII.Models;
using OsDsII.Data;

namespace OsDsII.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<CustomersController> _logger;
        public CustomersController(DataContext dataContext, ILogger<CustomersController> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(Customer customer)
        {
            try
            {
                Customer customerExists = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Email == customer.Email);
                if (customerExists != null && customerExists.Equals(customer))
                {
                    throw new Exception("Customer already exists");
                }

                await _dataContext.Customers.AddAsync(customer);
                await _dataContext.SaveChangesAsync();

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Information, nameof(CustomersController), new {Message = ex.Message});
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            try
            {
                Customer customerExists = await _dataContext.Customers.FirstOrDefaultAsync<Customer>(customer => customer.Id == id);
                if(customerExists != null)
                {
                    throw new Exception("usuario nao encontrado");
                }
                _dataContext.Customers.Remove(customerExists);
                _dataContext.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
    }
}