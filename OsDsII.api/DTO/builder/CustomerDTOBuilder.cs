namespace OsDsII.DTOS.Builders
{
    public class CustomerDtoBuilder
    {
        private CustomerDTO _customerDto = new CustomerDTO();

        public CustomerDtoBuilder WithId(int id)
        {
            _customerDto.Id = id;
            return this;
        }
        public CustomerDtoBuilder WithName(string name)
        {
            _customerDto.Name = name;
            return this;
        }

        public CustomerDtoBuilder WithEmail(string email)
        {
            _customerDto.Email = email;
            return this;
        }

        public CustomerDtoBuilder WithPhone(string phone)
        {
            _customerDto.Phone = phone;
            return this;
        }

        public CustomerDTO Build()
        {
            return _customerDto;
        }
    }
}