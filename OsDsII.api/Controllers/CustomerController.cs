using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using OsDsII.Models;
using OsDsII.Data;
using OsDsII.DTOS;

namespace OsDsII.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<CustomersController> _logger;
        private readonly IMapper _mapper;
        public CustomersController(DataContext dataContext, ILogger<CustomersController> logger, IMapper mapper)
        {
            _dataContext = dataContext;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            List<Customer> customers = await _dataContext?.Customers.ToListAsync();
            List<CustomerDTO> customersDTO = _mapper.Map<List<CustomerDTO>>(customers);
            return Ok(customersDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Customer customer = await _dataContext?.Customers.FirstOrDefaultAsync(c => id == c.Id);
            if (customer is null)
            {
                return NotFound();
            }
            CustomerDTO customerDTO = _mapper.Map<CustomerDTO>(customer);
            return Ok(customerDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] Customer customer)
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
                _logger.Log(LogLevel.Information, nameof(CustomersController), new { Message = ex.Message });
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            try
            {
                Customer customerExists = await _dataContext.Customers.FirstOrDefaultAsync<Customer>(customer => customer.Id == id);
                if (customerExists != null)
                {
                    throw new Exception("usuario nao encontrado");
                }
                _dataContext.Customers.Remove(customerExists);
                await _dataContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerAsync(int id, [FromBody] Customer customer)
        {
            try {
                Customer customerExists = await _dataContext.Customers.FirstOrDefaultAsync(c => id == c.Id) ?? throw new Exception("Customer not found");
                customerExists.Id = id;
                _dataContext.Customers.Update(customer);
                await _dataContext.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
    }
}