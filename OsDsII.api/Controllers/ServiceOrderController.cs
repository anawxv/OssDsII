using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsDsII.Models;
using OsDsII.Data;
using OsDsII.DTOS;
namespace OsDsII.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ServiceOrdersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<ServiceOrdersController> _logger;

        public ServiceOrdersController(DataContext dataContext, ILogger<ServiceOrdersController> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceOrderAsync([FromBody] ServiceOrderInput ordemServicoInput)
        {
            ServiceOrder createdServiceOrder = await Save(ordemServicoInput);
            ServiceOrderDTO serviceOrderDto = createdServiceOrder.ToServiceOrder();
            return Created("ServiceOrder", serviceOrderDto);
        }

        private async Task<ServiceOrder> Save(ServiceOrderInput serviceOrderInput)
        {
            // Get customer from database
            Customer customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Id == serviceOrderInput.Id);

            // Create a ServiceOrder object from the ServiceOrderInput
            ServiceOrder serviceOrder = ServiceOrder.FromServiceOrderInput(serviceOrderInput, customer);

            // Add to database
            var createdServiceOrder = _dataContext.ServiceOrders.Add(serviceOrder);
            await _dataContext.SaveChangesAsync();

            return serviceOrder;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServiceOrderAsync()
        {
            try
            {
                List<ServiceOrder> serviceOrderList = await _dataContext.ServiceOrders.Include(c => c.Customer).ToListAsync();
                return Ok(serviceOrderList);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceOrderById(int id)
        {
            try
            {
                ServiceOrder serviceOrder = await _dataContext.ServiceOrders
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(serviceOrder => serviceOrder.Id == id);
                if (serviceOrder is null)
                {
                    _logger.LogInformation("NOT FOUND");
                    throw new Exception();
                }
                return Ok(serviceOrder);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}/status/finish")]
        public async Task<IActionResult> FinishServiceOrderAsync(int id)
        {
            try
            {
                ServiceOrder serviceOrder = await _dataContext.ServiceOrders.FirstOrDefaultAsync(serviceOrder => serviceOrder.Id == id);
                if (serviceOrder is null)
                {
                    throw new Exception();
                }

                if (serviceOrder.CanFinish())
                {
                    serviceOrder.FinishOS();
                    _dataContext.ServiceOrders.Update(serviceOrder);
                    await _dataContext.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}