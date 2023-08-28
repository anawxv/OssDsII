using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using OsDsII.Models;
using OsDsII.DTOS;
using OsDsII.Services;
using OsDsII.Http;

namespace OsDsII.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController : ControllerBase
    {
        // private readonly DataContext _dataContext;
        private readonly ICustomersService _customersService;
        private readonly ILogger<CustomersController> _logger;
        private readonly IMapper _mapper;
        public CustomersController(ICustomersService customersService, ILogger<CustomersController> logger, IMapper mapper)
        {
            _customersService = customersService;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HttpResponseApi<IEnumerable<CustomerDTO>>))]
        public async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<Customer> customers = await _customersService.GetAllAsync();
            IEnumerable<CustomerDTO> customersDTO = _mapper.Map<List<CustomerDTO>>(customers);
            return HttpResponseApi<IEnumerable<CustomerDTO>>.Ok(customersDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HttpResponseApi<IEnumerable<CustomerDTO>>))]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                Customer customer = await _customersService.GetByIdAsync(id);
                CustomerDTO customerDTO = _mapper.Map<CustomerDTO>(customer);
                return HttpResponseApi<CustomerDTO>.Ok(customerDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] Customer customer)
        {
            try
            {
                Customer customerExists = await _customersService.CreateCustomerAsync(customer);
                return Created("Customer", customer);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Information, nameof(CustomersController), new { Message = ex.Message });
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            try
            {
                await _customersService.DeleteCustomerAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerAsync(int id, [FromBody] Customer customer)
        {
            try
            {
                await _customersService.UpdateCustomerAsync(id, customer);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, new { Timestamp = DateTimeOffset.Now, ErrorCode = "ERROR_CODE", Message = "", UriPath = HttpContext.Request.Path });
                return BadRequest(new { Timestamp = DateTimeOffset.Now, ErrorCode = "ERROR_CODE", ErrorMessage = ex.Message, UriPath = HttpContext.Request.Path, HttpStatusCode = StatusCodes.Status400BadRequest });
            }
        }
    }
}