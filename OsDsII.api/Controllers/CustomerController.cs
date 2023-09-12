using Microsoft.AspNetCore.Mvc;
using OsDsII.Models;
using OsDsII.DTOS;
using OsDsII.Services;
using OsDsII.Http;
using OsDsII.Exceptions;

namespace OsDsII.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersService _customersService;
        public CustomersController(ICustomersService customersService)
        {
            _customersService = customersService;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HttpResponseApi<IEnumerable<CustomerDTO>>))]
        public async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<Customer> customers = await _customersService.GetAllAsync();
            IEnumerable<CustomerDTO> customersDTO = customers.Select(customer => customer.ToCustomer());
            return HttpResponseApi<IEnumerable<CustomerDTO>>.Ok(customersDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HttpResponseApi<CustomerDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(HttpErrorResponse))]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                Customer customer = await _customersService.GetByIdAsync(id);
                CustomerDTO customerDto = customer.ToCustomer();
                return HttpResponseApi<CustomerDTO>.Ok(customerDto);
            }
            catch (BaseException ex)
            {
                return ex.GetResponse();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(HttpResponseApi<CustomerDTO>))]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] Customer customer)
        {
            try
            {
                Customer customerExists = await _customersService.CreateCustomerAsync(customer);
                CustomerDTO customerDto = customerExists.ToCustomer();
                return HttpResponseApi<CustomerDTO>.Created(customerDto);
            }
            catch (BaseException ex)
            {
                // _logger.Log(LogLevel.Information, nameof(CustomersController), new { Message = ex.Message });
                return ex.GetResponse();
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
            catch (BaseException ex)
            {
                return ex.GetResponse();
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
            catch (BaseException ex)
            {
                // _logger.LogError(ex.Message, new { Timestamp = DateTimeOffset.Now, ErrorCode = "ERROR_CODE", Message = "", UriPath = HttpContext.Request.Path });
                return ex.GetResponse();
            }
        }
    }
}