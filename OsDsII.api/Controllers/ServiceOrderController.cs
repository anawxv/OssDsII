using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsDsII.Models;
using OsDsII.Data;
using OsDsII.DTOS;
using AutoMapper;

namespace OsDsII.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ServiceOrdersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<ServiceOrdersController> _logger;
        private readonly IMapper _mapper;

        public ServiceOrdersController(DataContext dataContext, ILogger<ServiceOrdersController> logger, IMapper mapper)
        {
            _dataContext = dataContext;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(ServiceOrderDTO))]
        public async Task<IActionResult> CreateServiceOrderAsync([FromBody] ServiceOrderInput serviceOrderInput)
        {
            ServiceOrder serviceOrder = _mapper.Map<ServiceOrder>(serviceOrderInput);
            ServiceOrder createdServiceOrder = await CreateAsync(serviceOrder);

            ServiceOrderDTO serviceOrderDto = _mapper.Map<ServiceOrderDTO>(createdServiceOrder);

            return Ok(serviceOrderDto);
        }

        private async Task<ServiceOrder> CreateAsync(ServiceOrder serviceOrder)
        {
            Customer customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Id == serviceOrder.Id);
            if (customer == null)
            {
                _logger.LogInformation("Customer not found");
                throw new Exception("Customer not found");
            }

            serviceOrder.Customer = customer;
            serviceOrder.Status = StatusServiceOrder.OPEN;
            serviceOrder.OpeningDate = DateTimeOffset.Now;

            await _dataContext.ServiceOrders.AddAsync(serviceOrder);
            await _dataContext.SaveChangesAsync();
            return serviceOrder;
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