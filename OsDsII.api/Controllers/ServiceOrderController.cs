using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsDsII.Models;
using OsDsII.Data;

namespace OsDsII.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public async Task<IActionResult> CreateServiceOrderAsync(ServiceOrder serviceOrder)
        {
            Customer customerExists = await _dataContext.Customers.FirstOrDefaultAsync(customer => customer.Id == serviceOrder.Customer.Id);
            if (customerExists is null)
            {
                throw new Exception();
            }

            serviceOrder.Customer = customerExists;
            serviceOrder.Status = StatusServiceOrder.OPEN;
            serviceOrder.OpeningDate = DateTimeOffset.Now;

            await _dataContext.AddAsync(serviceOrder);
            await _dataContext.SaveChangesAsync();

            return Ok(serviceOrder);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServiceOrderAsync()
        {
            try
            {
                List<ServiceOrder> serviceOrderList = await _dataContext.ServiceOrders.ToListAsync();
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
                ServiceOrder serviceOrder = await _dataContext.ServiceOrders.FirstOrDefaultAsync(serviceOrder => serviceOrder.Id == id);
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

        [HttpPut("{id}/status")]
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
                    serviceOrder.finishOS();
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