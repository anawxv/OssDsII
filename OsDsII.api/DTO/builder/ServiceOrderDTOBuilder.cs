using OsDsII.Models;
namespace OsDsII.DTOS.Builders
{
    public class ServiceOrderDTOBuilder
    {
        private ServiceOrderDTO _serviceOrderDto = new ServiceOrderDTO();

        public ServiceOrderDTOBuilder WithId(int id)
        {
            _serviceOrderDto.Id = id;
            return this;
        }

        public ServiceOrderDTOBuilder WithDescription(string description)
        {
            _serviceOrderDto.Description = description;
            return this;
        }

        public ServiceOrderDTOBuilder WithCustomer(CustomerDTO customer)
        {
            _serviceOrderDto.Customer = customer;
            return this;
        }

        public ServiceOrderDTOBuilder WithPrice(double price)
        {
            _serviceOrderDto.Price = price;
            return this;
        }

        public ServiceOrderDTOBuilder WithStatus(StatusServiceOrder status)
        {
            _serviceOrderDto.Status = status;
            return this;
        }

        public ServiceOrderDTOBuilder WithOpeningDate(DateTimeOffset openingDate)
        {
            _serviceOrderDto.OpeningDate = openingDate;
            return this;
        }

        public ServiceOrderDTOBuilder WithFinishDate()
        {
            _serviceOrderDto.FinishDate = null;
            return this;
        }

        public ServiceOrderDTO Build()
        {
            return _serviceOrderDto;
        }

    }
}